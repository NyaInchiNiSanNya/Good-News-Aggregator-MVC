using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Domain.Entities
{
    public class UserNews
    {

        public Int32 Id { get; set; }
        public Int32 UserId { get; set; }
        public User.User User { get; set; }
        public Int32 NewsId { get; set; }
        public News News { get; set; }
    }
}
