using Microsoft.EntityFrameworkCore;
using eContentApp.Application.Interfaces;
using eContentApp.Domain.Entities;
using eContentApp.Infrastructure.Data;

namespace eContentApp.Infrastructure.Repositories
{
    public class PostRepository : GenericRepository<Post, Guid>, IPostRepository
    {
        public PostRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Post?> GetPostByIdWithCategoriesAsync(Guid id)
        {
            return await _dbSet
                         .Include(p => p.Categories)
                         .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Post>> GetAllPostsWithCategoriesAsync()
        {
            return await _dbSet
                         .Include(p => p.Categories)
                         .ToListAsync();
        }
        public async Task<IEnumerable<Post>> GetAllPostsSimpleAsync() 
        {
            return await _dbSet.ToListAsync();
        }
    }
}