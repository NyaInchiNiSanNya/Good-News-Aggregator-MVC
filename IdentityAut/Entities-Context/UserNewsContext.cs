using Entities_Context.Entities.UserNews;
using Microsoft.EntityFrameworkCore;


namespace Entities_Context
{
    public class UserNewsContext : DbContext
    {
        public DbSet<Artincle> News { get; set; }
        public DbSet<UserConfig> UserConfiguration { get; set; }
        public DbSet<UserInformation> UserInformation { get; set; }
        public DbSet<UserNews> UsersNews { get; set; }

        public UserNewsContext(DbContextOptions<UserNewsContext> options) : base(options)
        { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionBuilder)
        {
            base.OnConfiguring(optionBuilder);
        }
    }

}