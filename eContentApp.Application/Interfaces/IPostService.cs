using eContentApp.Application.DTOs.Post;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eContentApp.Application.Interfaces
{
    public interface IPostService
    {
        Task<IEnumerable<PostListDto>> GetAllPostsAsync();
        Task<PostDetailDto?> GetPostDetailAsync(Guid id);
        Task<Guid> CreatePostAsync(CreatePostDto postDto);
        Task UpdatePostAsync(UpdatePostDto postDto);
        Task DeletePostAsync(Guid id);
    }
}
