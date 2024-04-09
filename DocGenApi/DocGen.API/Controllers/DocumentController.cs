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
    public class UploadDocumentController : ControllerBase
    {
        private readonly IS3Service _s3Service;
        private readonly IOpenAIService _openAIService;
        private readonly ApplicationDbContext _dbContext;

        public UploadDocumentController(IS3Service s3Service, IOpenAIService openAIService, ApplicationDbContext dbContext)
        {
            _s3Service = s3Service;
            _openAIService = openAIService;
            _dbContext = dbContext;
        }

        [HttpPost("document")]
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

                // Upload original document to S3
                string bucketName = "docgen-documents-20240403";
                string documentUrl = await _s3Service.UploadFileContentAsync(request.Content, bucketName, request.FileName);

                // Store original document info in DB
                var uploadedDocument = new UploadDocument
                {
                    UserID = user.UserId,
                    UpDocName = request.FileName,
                    UpDocURL = documentUrl
                };
                _dbContext.UploadDocument.Add(uploadedDocument);
                await _dbContext.SaveChangesAsync();

                // Generate summary with GPT Service
                var summary = await _openAIService.GenerateSummaryAsync(request.FileName, request.Content);

                // Upload generated summary to S3
                string summaryFileName = $"{request.FileName}_Summary.txt";
                string summaryDocumentUrl = await _s3Service.UploadFileContentAsync(summary, bucketName, summaryFileName);

                // Store generated document info in DB
                var generatedDocument = new GeneratedDocument
                {
                    UpDocID = uploadedDocument.UpDocID,
                    DocTypeID = 1,
                    GenDocName = summaryFileName,
                    GenDocURL = summaryDocumentUrl
                };
                _dbContext.GeneratedDocument.Add(generatedDocument);
                await _dbContext.SaveChangesAsync();

                // Return response to user
                return Ok(new { Content = summary });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }

    public class UploadRequest
    {
        public string FileName { get; set; }
        public string Content { get; set; }
    }
}
