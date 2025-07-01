using System.ComponentModel.DataAnnotations;

namespace eContentApp.Domain.Entities
{
    public class Media
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string FileName { get; set; } = string.Empty;

        public string FilePath { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string FileType { get; set; } = string.Empty;
        public long FileSize { get; set; }
        public DateTime UploadedAt { get; set; }
        [MaxLength(500)]
        public string? Caption { get; set; }

        public Media()
        {
            Id = Guid.NewGuid();
            UploadedAt = DateTime.UtcNow;
        }
    }
}
