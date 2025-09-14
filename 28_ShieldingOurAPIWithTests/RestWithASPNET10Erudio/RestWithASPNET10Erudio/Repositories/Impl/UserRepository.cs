using RestWithASPNET10Erudio.Model;
using RestWithASPNET10Erudio.Model.Context;

namespace RestWithASPNET10Erudio.Repositories.Impl
{
    public class UserRepository(MSSQLContext context)
        : GenericRepository<User>(context), IUserRepository
    {
        public User FindByUsername(string username)
        {
            return _context.Users.SingleOrDefault(
                u => u.Username == username);
        }
    }
}
