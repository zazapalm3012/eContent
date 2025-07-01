using eContentApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eContentApp.Application.Interfaces
{
    public interface IPostRepository : IGenericRepository<Post, Guid>
    {
        Task<Post?> GetPostByIdWithCategoriesAsync(Guid id);
        Task<IEnumerable<Post>> GetAllPostsWithCategoriesAsync();
    }
}
