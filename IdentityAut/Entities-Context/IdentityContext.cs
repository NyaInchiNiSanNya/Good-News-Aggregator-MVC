using Entities_Context.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace IdentityContext
{
    public class IdentityContext : IdentityDbContext<User>
    {
        public IdentityContext(DbContextOptions<IdentityContext> options)
            : base(options) { }
    }
}