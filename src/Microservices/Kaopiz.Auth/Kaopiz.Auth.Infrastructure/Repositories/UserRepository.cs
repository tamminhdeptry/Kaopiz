using Kaopiz.Auth.Application;
using Microsoft.EntityFrameworkCore;

namespace Kaopiz.Auth.Infrastructure
{
    internal class UserRepository : IUserRepository
    {
        private readonly AppDbContext _appDbContext;

        public UserRepository(
            AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<UserEntity?> GetUserByUserId(Guid id, bool disableTracking = false)
        {
            IQueryable<UserEntity> query = _appDbContext.Users;
            if (disableTracking)
            {
                query = query.AsNoTracking();
            }
            UserEntity? user = await query.FirstOrDefaultAsync(user => user.Id == id);
            return user;
        }

        public async Task<UserEntity?> GetUserByUserName(string userName, bool disableTracking = false)
        {
            IQueryable<UserEntity> query = _appDbContext.Users;
            if (disableTracking)
            {
                query = query.AsNoTracking();
            }
            UserEntity? user = await query.FirstOrDefaultAsync(user => user.UserName == userName);
            return user;
        }

        public async Task<UserEntity> AddAsync(UserEntity user, bool needSaveChange = true,
            CancellationToken cancellationToken = default)
        {
            var entity = await _appDbContext.Users.AddAsync(user, cancellationToken: cancellationToken);
            if (needSaveChange)
            {
                await _appDbContext.SaveChangesAsync();
            }
            return entity.Entity;
        }
    }
}
