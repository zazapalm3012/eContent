using System;
using System.Collections.Generic;
using eContentApp.Application.DTOs.Category;

using System.Text.Json.Serialization;

namespace eContentApp.Application.DTOs.Post
{
    public class PostListDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        [JsonPropertyName("thumbnailUrl")]
        public string? ThumbnailUrl { get; set; }
        public DateTime PublishedAt { get; set; }
        //public List<CategoryDto> Categories { get; set; } = [];
    }
}
