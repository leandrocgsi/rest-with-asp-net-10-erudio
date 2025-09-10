using System.ComponentModel.DataAnnotations;

namespace RestWithASPNET10Erudio.Data.DTO.V1
{
    public class FileUploadDTO
    {
        [Required]
        public IFormFile File { get; set; }
    }
}
