using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestWithASPNET10Erudio.Data.DTO.V1;
using RestWithASPNET10Erudio.Services;

namespace RestWithASPNET10Erudio.Controllers.V1
{

    [ApiController]
    [Route("api/[controller]/v1")]
    [Authorize("Bearer")]
    public class FileController(
        IFileServices fileServices,
        ILogger<FileController> logger)
    : Controller
    {
        private IFileServices _fileServices = fileServices;
        private readonly ILogger<FileController> _logger = logger;

        [HttpGet("downloadFile/{fileName}")]
        [ProducesResponseType(200, Type = typeof(byte[]))]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [Produces("application/octet-stream")]
        public IActionResult DownloadFile(string fileName)
        {
            var buffer = _fileServices.GetFile(fileName);
            if (buffer == null || buffer.Length == 0)
                return NoContent();

            var contentType = $"application/{Path.GetExtension(fileName).TrimStart('.')}";
            return File(buffer, contentType, fileName);
        }

        [HttpPost("uploadFile")]
        [ProducesResponseType(200, Type = typeof(FileDetailDTO))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [Produces("application/json", "application/xml")]
        //public async Task<IActionResult> UploadFile(IFormFile file)
        public async Task<IActionResult> UploadFile([FromForm] FileUploadDTO input)
        {
            var fileDetail = await _fileServices.SaveFileToDisk(input.File);
            _logger.LogInformation("File {fileName} uploaded successfully.", fileDetail.DocumentName);
            return Ok(fileDetail);
        }

        [HttpPost("uploadMultipleFiles")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(200, Type = typeof(List<FileDetailDTO>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [Produces("application/json", "application/xml")]
        public async Task<IActionResult> UploadMultipleFiles(
            [FromForm] MultipleFilesUploadDTO input
        )
        {
            var details = await _fileServices
                .SaveFilesToDisk(input.Files);
            return Ok(details);
        }
    }
}
