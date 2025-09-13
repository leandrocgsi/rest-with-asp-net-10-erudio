using System.ComponentModel.DataAnnotations;

namespace RestWithASPNET10Erudio.Data.DTO.V1
{
    public class MultipleFilesUploadDTO
    {
        [Required]
        public List<IFormFile> Files { get; set; }
    }
}
