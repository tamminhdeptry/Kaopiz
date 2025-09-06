using CET.Infrastructure.Entity.Users;
using CET.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace CET.Infrastructure.Repositories
{
    internal class UserRepository : IUserRepository
    {
        private readonly AppDbContext _appDbContext;
        public UserRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<UserEntity?> GetUserByUserId(Guid id)
        {
            var user = await _appDbContext.Users.FindAsync(id);
            return user;
        }

        public async Task<UserEntity?> GetUserByUserName(string userName)
        {
            var user = await _appDbContext.Users.FirstOrDefaultAsync(x => x.UserName == userName);
            return user;
        }

        public async Task<UserEntity?> AddAsync(UserEntity user)
        {
            var entity = await _appDbContext.Users.AddAsync(user);
            return entity.Entity;
        }
    }
}
