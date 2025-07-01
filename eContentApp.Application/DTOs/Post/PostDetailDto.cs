using eContentApp.Application.DTOs.Category;

namespace eContentApp.Application.DTOs.Post
{
    public class PostDetailDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string? ThumbnailUrl { get; set; }
        public DateTime PublishedAt { get; set; }
        public string Status { get; set; } = string.Empty;
        public List<CategoryDto> Categories { get; set; } = [];
    }
}
