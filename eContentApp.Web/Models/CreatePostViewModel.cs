using System.ComponentModel.DataAnnotations;

namespace eContentApp.Web.Models
{
    public class CreatePostViewModel
    {
        [Required]
        public required string Title { get; set; }

        public required string Content { get; set; }

        public required string ThumbnailUrl { get; set; }

        [Required]
        public List<Guid> CategoryIds { get; set; } = new List<Guid>();
    }
}
