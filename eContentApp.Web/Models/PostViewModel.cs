
using System;

namespace eContentApp.Web.Models
{
    public class PostViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? ThumbnailUrl { get; set; }
        public DateTime PublishedAt { get; set; }
    }
}
