using Microsoft.AspNetCore.Identity;

namespace CustomIdentityApp.Models
{
    public sealed class User : IdentityUser
    {
        public String DisplayName { get; set; }
    }
}