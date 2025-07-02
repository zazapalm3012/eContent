using System.ComponentModel.DataAnnotations;

namespace eContentApp.Application.DTOs.Post
{
    public class CreatePostDto
    {
        [Required]
        [MaxLength(255)]
        public string Title { get; set; } = string.Empty;
        public string? Content { get; set; }
        [MaxLength(500)]
        public string? ThumbnailUrl { get; set; }
        public List<Guid>? CategoryIds { get; set; } = [];
        public string? Status { get; set; }
    }
}
