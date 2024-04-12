using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DocGen.Core.Interfaces;
using DocGen.Infrastructure.Data;
using System.Security.Claims;
using DocGen.Core.Entities;
using Microsoft.EntityFrameworkCore;
using DocGen.Core.Models;

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

                User user = await _dbContext.User.FirstOrDefaultAsync(u => u.UserSub == userSub);
                if (user == null)
                {
                    return NotFound("User not registered. Please register before attempting this.");
                }

                // Handle Original Document
                string bucketName = "docgen-documents-20240403";
                string documentUrl = await _s3Service.UploadFileContentAsync(request.Content, bucketName, request.FileName);

                
                UploadDocument uploadedDocument = new UploadDocument
                {
                    UserID = user.UserId,
                    UpDocName = request.FileName,
                    UpDocURL = documentUrl
                };
                _dbContext.UploadDocument.Add(uploadedDocument);
                await _dbContext.SaveChangesAsync();
                
                // Handle Generated Document
                string summary = await _openAIService.GenerateSummaryAsync(request.FileName, request.Content);
                
                string summaryFileName = $"{request.FileName}_Summary.txt";
                string summaryDocumentUrl = await _s3Service.UploadFileContentAsync(summary, bucketName, summaryFileName);
                
                GeneratedDocument generatedDocument = new GeneratedDocument
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
            string userSub = User.FindFirst(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userSub == null)
            {
                return Unauthorized("User identity not found.");
            }

            User user = await _dbContext.User.FirstOrDefaultAsync(user => user.UserSub == userSub);
            if (user == null)
            {
                return NotFound("User not registered.");
            }

            if (id.HasValue)
            {
                UploadDocument document = await _dbContext.UploadDocument
                    .FirstOrDefaultAsync(uploadDocument => uploadDocument.UserID == user.UserId && uploadDocument.UpDocID == id.Value);
                if (document == null)
                {
                    return NotFound("Uploaded document not found.");
                }
                return Ok(document);
            }
            else
            {
                List<UploadDocument> documents = await _dbContext.UploadDocument
                    .Where(uploadDocument => uploadDocument.UserID == user.UserId)
                    .ToListAsync();
                return Ok(documents);
            }
        }
        
        // GENERATED DOCUMENTS
        //Fetching Generated Docs
        [HttpGet("generated-documents/{id?}")]
        public async Task<IActionResult> GetGeneratedDocuments(int? id)
        {
            string userSub = User.FindFirst(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userSub == null)
            {
                return Unauthorized("User identity not found.");
            }

           
            User user = await _dbContext.User.FirstOrDefaultAsync(user => user.UserSub == userSub);
            if (user == null)
            {
                return NotFound("User not registered.");
            }

            if (id.HasValue)
            {

                var document = await _dbContext.GeneratedDocument
                    .Include(generatedDocument => generatedDocument.UploadDocument)
                    .Where(generatedDocument => generatedDocument.GenDocID == id.Value && generatedDocument.UploadDocument.UserID == user.UserId)
                    .Select(generatedDocument => new { generatedDocument.GenDocID, generatedDocument.GenDocName, generatedDocument.GenDocURL })
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
                    .Include(generatedDocument => generatedDocument.UploadDocument) 
                    .Where(generatedDocument => generatedDocument.UploadDocument.UserID == user.UserId)
                    .Select(generatedDocument => new { generatedDocument.GenDocID, generatedDocument.GenDocName, generatedDocument.GenDocURL })
                    .ToListAsync();

                return Ok(documents);
            }
        }
    }
}
