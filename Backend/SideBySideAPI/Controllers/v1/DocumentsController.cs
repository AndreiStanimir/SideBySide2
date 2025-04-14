using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SideBySideAPI.Interfaces;
using SideBySideAPI.Models;
using SideBySideAPI.Models.DTOs;
using SideBySideAPI.Models.Responses;
using Asp.Versioning;

namespace SideBySideAPI.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    public class DocumentsController : ControllerBase
    {
        private readonly IDocumentService _documentService;
        private readonly IOcrService _ocrService;
        private readonly ILogger<DocumentsController> _logger;

        public DocumentsController(
            IDocumentService documentService,
            IOcrService ocrService,
            ILogger<DocumentsController> logger)
        {
            _documentService = documentService;
            _ocrService = ocrService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDocuments()
        {
            var userId = User.FindFirst("sub")?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new ErrorResponse
                {
                    Success = false,
                    Message = "User ID not found in token"
                });
            }

            var documents = await _documentService.GetDocumentsByUserIdAsync(userId);
            return Ok(ApiResponse<IEnumerable<DocumentDTO>>.SuccessResponse(documents));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDocument(string id)
        {
            var userId = User.FindFirst("sub")?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new ErrorResponse
                {
                    Success = false,
                    Message = "User ID not found in token"
                });
            }

            var document = await _documentService.GetDocumentByIdAsync(id);

            if (document == null)
            {
                return NotFound(new ErrorResponse
                {
                    Success = false,
                    Message = $"Document with ID {id} not found"
                });
            }

            if (document.UserId != userId)
            {
                return Forbid();
            }

            return Ok(ApiResponse<DocumentDTO>.SuccessResponse(document));
        }

        [HttpPost]
        public async Task<IActionResult> CreateDocument([FromForm] CreateDocumentDTO createDocumentDto)
        {
            if (createDocumentDto.File == null || createDocumentDto.File.Length == 0)
            {
                return BadRequest(new ErrorResponse
                {
                    Success = false,
                    Message = "No file was uploaded"
                });
            }

            var userId = User.FindFirst("sub")?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new ErrorResponse
                {
                    Success = false,
                    Message = "User ID not found in token"
                });
            }

            try
            {
                var document = await _documentService.CreateDocumentAsync(createDocumentDto, userId);
                
                // Process the document with OCR asynchronously
                _ = Task.Run(async () =>
                {
                    try
                    {
                        await _ocrService.ProcessDocumentAsync(document.Id);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error processing document with OCR: {DocumentId}", document.Id);
                    }
                });

                return CreatedAtAction(nameof(GetDocument), new { id = document.Id }, 
                    ApiResponse<DocumentDTO>.SuccessResponse(document, "Document created successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating document");
                return StatusCode(500, new ErrorResponse
                {
                    Success = false,
                    Message = "Error creating document"
                });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDocument(string id, [FromBody] UpdateDocumentDTO updateDocumentDto)
        {
            var userId = User.FindFirst("sub")?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new ErrorResponse
                {
                    Success = false,
                    Message = "User ID not found in token"
                });
            }

            var existingDocument = await _documentService.GetDocumentByIdAsync(id);

            if (existingDocument == null)
            {
                return NotFound(new ErrorResponse
                {
                    Success = false,
                    Message = $"Document with ID {id} not found"
                });
            }

            if (existingDocument.UserId != userId)
            {
                return Forbid();
            }

            try
            {
                var updatedDocument = await _documentService.UpdateDocumentAsync(id, updateDocumentDto);
                return Ok(ApiResponse<DocumentDTO>.SuccessResponse(updatedDocument, "Document updated successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating document: {DocumentId}", id);
                return StatusCode(500, new ErrorResponse
                {
                    Success = false,
                    Message = "Error updating document"
                });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDocument(string id)
        {
            var userId = User.FindFirst("sub")?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new ErrorResponse
                {
                    Success = false,
                    Message = "User ID not found in token"
                });
            }

            var existingDocument = await _documentService.GetDocumentByIdAsync(id);

            if (existingDocument == null)
            {
                return NotFound(new ErrorResponse
                {
                    Success = false,
                    Message = $"Document with ID {id} not found"
                });
            }

            if (existingDocument.UserId != userId)
            {
                return Forbid();
            }

            try
            {
                await _documentService.DeleteDocumentAsync(id);
                return Ok(ApiResponse.SuccessResponse("Document deleted successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting document: {DocumentId}", id);
                return StatusCode(500, new ErrorResponse
                {
                    Success = false,
                    Message = "Error deleting document"
                });
            }
        }

        [HttpGet("{id}/segments")]
        public async Task<IActionResult> GetDocumentSegments(string id)
        {
            var userId = User.FindFirst("sub")?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new ErrorResponse
                {
                    Success = false,
                    Message = "User ID not found in token"
                });
            }

            var document = await _documentService.GetDocumentByIdAsync(id);

            if (document == null)
            {
                return NotFound(new ErrorResponse
                {
                    Success = false,
                    Message = $"Document with ID {id} not found"
                });
            }

            if (document.UserId != userId)
            {
                return Forbid();
            }

            var segments = await _documentService.GetDocumentSegmentsAsync(id);
            return Ok(ApiResponse<IEnumerable<DocumentSegmentDTO>>.SuccessResponse(segments));
        }
    }
} 