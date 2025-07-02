using System.Collections.Generic;
using eContentApp.Application.DTOs.Post;

namespace eContentApp.Web.Models
{
    public class KnowledgeBaseViewModel
    {
        public List<CategoryWithPostsViewModel> Categories { get; set; } = new List<CategoryWithPostsViewModel>();
        public PostDetailDto? SelectedPost { get; set; }
    }

    public class CategoryWithPostsViewModel
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public List<PostListDto> Posts { get; set; } = new List<PostListDto>();
    }
}
