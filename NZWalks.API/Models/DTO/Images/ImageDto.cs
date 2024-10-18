using System.ComponentModel.DataAnnotations.Schema;

namespace NZWalks.API.Models.DTO.Images {
    public class ImageDto {
        public string FileName { get; set; }
        public string? FileDescription { get; set; }
        public string FileExtension { get; set; }
        public long FileSiezeInBytes { get; set; }
        public string FilePath { get; set; }
    }
}