using Microsoft.EntityFrameworkCore;
using SchoolApp.Data;
using SchoolApp.Security;

namespace SchoolApp.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(SchoolAppDbContext context) : base(context)
        {
        }

        public async Task<User?> GetUserAsync(string username, string password)
        {
            //return await context.Users.FirstOrDefaultAsync(u => 
            //(u.Username == username || u.Email == username) && 
            //EncryptionUtil.IsValidPassword(password, u.Password)); 
            var user = await context.Users.FirstOrDefaultAsync(x => x.Username == username
                   || x.Email == username);
            if (user == null)
            {
                return null;
            }
            if (!EncryptionUtil.IsValidPassword(password, user.Password!))
            {
                return null;
            }
            return user;
        }

        public async Task<List<User>> GetAllUsersFilteredPaginatedAsync(int pageNumber, int pageSize, 
            List<Func<User, bool>> predicates)
        {
            int skip = (pageNumber - 1) * pageSize;
            IQueryable<User> query = context.Users.Skip(skip).Take(pageSize);

            if (predicates != null && predicates.Any())
            {
                query = query.Where(u => predicates.All(predicate => predicate(u)));
            }
            return await query.ToListAsync();
        }

        public async Task<User?> GetByUsernameAsync(string username) => 
            await context.Users.FirstOrDefaultAsync(u => u.Username == username);

        public async Task<User?> UpdateUserAsync(int id, User user)
        {
            var existingUser = await context.Users.FirstOrDefaultAsync(u => u.Id == user.Id);
            if (existingUser is null) return null;
            if (existingUser.Id != id) return null;

            context.Users.Attach(user);
            context.Entry(user).State = EntityState.Modified;
            return existingUser;
        }


    }
}
