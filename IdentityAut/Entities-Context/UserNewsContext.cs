using Entities_Context.Entities.UserNews;
using Microsoft.EntityFrameworkCore;


namespace Project.Domain.DBContext
{
    public class UserNewsContext : DbContext
    {
        public DbSet<News> News { get; set; }
        public DbSet<UserConfig> Users { get; set; }
        public DbSet<UserNews> UsersNews { get; set; }

        public UserNewsContext(DbContextOptions<UserNewsContext> options) : base(options)
        { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionBuilder)
        {
            base.OnConfiguring(optionBuilder);
        }
    }

}