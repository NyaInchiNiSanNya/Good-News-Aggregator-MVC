using Microsoft.EntityFrameworkCore;
using Project.Domain.Entities;
using Project.Domain.Entities.User;


namespace Project.Domain.DBContext
{
    public class CharacterContext : DbContext
    {
        public DbSet<News> News { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserNews> UsersNews { get; set;}

        public CharacterContext(DbContextOptions<CharacterContext> options):base(options)
        {}

        protected override void OnConfiguring(DbContextOptionsBuilder optionBuilder)
        {
            base.OnConfiguring(optionBuilder);
        }
    }

}