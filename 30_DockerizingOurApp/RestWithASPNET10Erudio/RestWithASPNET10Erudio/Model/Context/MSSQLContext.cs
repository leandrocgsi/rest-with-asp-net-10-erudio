using Microsoft.EntityFrameworkCore;

namespace RestWithASPNET10Erudio.Model.Context
{
    public class MSSQLContext(DbContextOptions<MSSQLContext> options) : DbContext(options)
    {
        public DbSet<Person> Persons { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<User> Users { get; set; }
    }
}