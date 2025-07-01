using System.ComponentModel.DataAnnotations;

namespace eContentApp.Domain.Entities
{
    public partial class Post
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string? Title { get; set; }

        public string? Content { get; set; }

        [MaxLength(500)]
        public string? Thumnail_url { get; set; }

        public DateTime PublishedAt { get; set; } = DateTime.UtcNow;

        public PostStatus Status { get; set; } = PostStatus.Draft;

        public virtual ICollection<Category> Categories { get; set; } = [];
    }
}
