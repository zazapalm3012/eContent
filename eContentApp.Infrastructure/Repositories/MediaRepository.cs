using eContentApp.Application.Interfaces;
using eContentApp.Domain.Entities;
using eContentApp.Infrastructure.Data;

namespace eContentApp.Infrastructure.Repositories
{
    public class MediaRepository : GenericRepository<Media, Guid>, IMediaRepository
    {
        public MediaRepository(AppDbContext context) : base(context)
        {
        }
    }
}
