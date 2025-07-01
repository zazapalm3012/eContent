using System;
using System.Collections.Generic;
using eContentApp.Application.DTOs.Category;

namespace eContentApp.Application.DTOs.Post
{
    public class PostListDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? ThumbnailUrl { get; set; }
        public DateTime PublishedAt { get; set; }
        public List<CategoryDto> Categories { get; set; } = [];
    }
}
