using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DocGen.Core.Interfaces;
using DocGen.Infrastructure.Data;
using System.Security.Claims;
using DocGen.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace DesignDocGen.API.Controllers
{
    [ApiController]
    [Route("api/v1/")]
    [Authorize(Policy = "RegisteredUser")]
    public class DocumentController : ControllerBase
    {
        private readonly IS3Service _s3Service;
        private readonly IOpenAIService _openAIService;
        private readonly ApplicationDbContext _dbContext;

        public DocumentController(IS3Service s3Service, IOpenAIService openAIService, ApplicationDbContext dbContext)
        {
            _s3Service = s3Service;
            _openAIService = openAIService;
            _dbContext = dbContext;
        }

        [HttpPost("uploaded-documents")]
        public async Task<IActionResult> UploadDocument([FromBody] UploadRequest request)
        {
            try
            {
                string userSub = User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                if (userSub == null)
                {
                    return Unauthorized("User identity not found. Auth0 error.");
                }

                var user = await _dbContext.User.FirstOrDefaultAsync(u => u.UserSub == userSub);
                if (user == null)
                {
                    return NotFound("User not registered. Please register before attempting this.");
                }

                // Handle Original Document
                string bucketName = "docgen-documents-20240403";
                string documentUrl = await _s3Service.UploadFileContentAsync(request.Content, bucketName, request.FileName);

                
                var uploadedDocument = new UploadDocument
                {
                    UserID = user.UserId,
                    UpDocName = request.FileName,
                    UpDocURL = documentUrl
                };
                _dbContext.UploadDocument.Add(uploadedDocument);
                await _dbContext.SaveChangesAsync();
                
                // Handle Generated Document
                var summary = await _openAIService.GenerateSummaryAsync(request.FileName, request.Content);
                
                string summaryFileName = $"{request.FileName}_Summary.txt";
                string summaryDocumentUrl = await _s3Service.UploadFileContentAsync(summary, bucketName, summaryFileName);
                
                var generatedDocument = new GeneratedDocument
                {
                    UpDocID = uploadedDocument.UpDocID,
                    DocTypeID = 1,
                    GenDocName = summaryFileName,
                    GenDocURL = summaryDocumentUrl
                };
                _dbContext.GeneratedDocument.Add(generatedDocument);
                await _dbContext.SaveChangesAsync();
                
                return Ok(new { Content = summary });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
        
        [HttpGet("uploaded-documents/{id?}")]
        public async Task<IActionResult> GetUploadedDocuments(int? id)
        {
            var userSub = User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userSub == null)
            {
                return Unauthorized("User identity not found.");
            }

            var user = await _dbContext.User.FirstOrDefaultAsync(u => u.UserSub == userSub);
            if (user == null)
            {
                return NotFound("User not registered.");
            }

            if (id.HasValue)
            {
                var document = await _dbContext.UploadDocument
                    .FirstOrDefaultAsync(d => d.UserID == user.UserId && d.UpDocID == id.Value);
                if (document == null)
                {
                    return NotFound("Uploaded document not found.");
                }
                return Ok(document);
            }
            else
            {
                var documents = await _dbContext.UploadDocument
                    .Where(d => d.UserID == user.UserId)
                    .ToListAsync();
                return Ok(documents);
            }
        }
        
        // GENERATED DOCUMENTS
        
        //Fetching Generated Docs
        [HttpGet("generated-documents/{id?}")]
        public async Task<IActionResult> GetGeneratedDocuments(int? id)
        {
            var userSub = User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userSub == null)
            {
                return Unauthorized("User identity not found.");
            }

           
            var user = await _dbContext.User.FirstOrDefaultAsync(u => u.UserSub == userSub);
            if (user == null)
            {
                return NotFound("User not registered.");
            }

            if (id.HasValue)
            {

                var document = await _dbContext.GeneratedDocument
                    .Include(d => d.UploadDocument)
                    .Where(d => d.GenDocID == id.Value && d.UploadDocument.UserID == user.UserId)
                    .Select(d => new { d.GenDocID, d.GenDocName, d.GenDocURL })
                    .FirstOrDefaultAsync();

                if (document == null)
                {
                    return NotFound("Generated document not found.");
                }
                return Ok(document);
            }
            else
            {

                var documents = await _dbContext.GeneratedDocument
                    .Include(d => d.UploadDocument) 
                    .Where(d => d.UploadDocument.UserID == user.UserId)
                    .Select(d => new { d.GenDocID, d.GenDocName, d.GenDocURL })
                    .ToListAsync();

                return Ok(documents);
            }
        }
    }

    public class UploadRequest
    {
        public string FileName { get; set; }
        public string Content { get; set; }
    }
}
