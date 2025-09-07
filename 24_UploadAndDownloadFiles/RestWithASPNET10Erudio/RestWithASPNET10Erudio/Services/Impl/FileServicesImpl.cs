using RestWithASPNET10Erudio.Data.DTO.V1;

namespace RestWithASPNET10Erudio.Services.Impl
{
    public class FileServicesImpl : IFileServices
    {
        private readonly string _basePath;
        private readonly IHttpContextAccessor _context;

        private static readonly HashSet<string> _allowedExtensions =
            new() { ".txt", ".pdf", ".png", ".jpg", ".jpeg", ".docx", ".mp3" };

        public FileServicesImpl(IHttpContextAccessor context)
        {
            _context = context;
            _basePath = Path.Combine(
                Directory.GetCurrentDirectory(), "UploadDir");
            if (!Directory.Exists(_basePath))
            {
                Directory.CreateDirectory(_basePath);
            }
        }

        public byte[] GetFile(string fileName)
        {
            var filePath = Path.Combine(_basePath, fileName);
            if (!File.Exists(filePath)) return null;
            return File.ReadAllBytes(filePath);
        }

        public async Task<FileDetailDTO> SaveFileToDisk(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new Exception("File is empty");
            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!_allowedExtensions.Contains(fileExtension))
                throw new Exception("File extension is not allowed");

            var documentName = Path.GetFileName(file.FileName);
            var destination = Path.Combine(_basePath, documentName);

            var baseUrl = $"{_context.HttpContext.Request.Scheme}" +
                $"://{_context.HttpContext.Request.Host}";    

            var fileDetail = new FileDetailDTO
            {
                DocumentName = documentName,
                DocType = file.ContentType,
                DocUrl = $"{baseUrl}/api/file/v1/downloadFile/{documentName}"
            };

            using var stream = new FileStream(destination, FileMode.Create);

            await file.CopyToAsync(stream);
            return fileDetail;
        }

        public async Task<List<FileDetailDTO>> SaveFilesToDisk(List<IFormFile> files)
        {
            var results = new List<FileDetailDTO>();
            foreach (var file in files)
            {
                var detail = await SaveFileToDisk(file);
                if (!string.IsNullOrEmpty(detail.DocumentName))
                    results.Add(detail);
            }
            return results;
        }
    }
}
