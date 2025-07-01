using eContentApp.Application.Interfaces;
using eContentApp.Domain.Entities;
using eContentApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace eContentApp.Infrastructure.Repositories
{
    public class CategoryRepository : GenericRepository<Category, Guid>, ICategoryRepository
    {
        public CategoryRepository(AppDbContext context) : base(context)
        {

        }

        public async Task<IEnumerable<Category>> GetCategoriesWithPostsAsync()
        {
            return await _dbSet
                         .Include(c => c.Posts)
                         .ToListAsync();
        }
    }
}