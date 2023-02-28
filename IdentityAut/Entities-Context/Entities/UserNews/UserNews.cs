using System.ComponentModel.DataAnnotations.Schema;
using Entities_Context.Entities.Identity;

namespace Entities_Context.Entities.UserNews
{
    public class UserNews
    {

        public int Id { get; set; }
        public int UserId { get; set; }
        public UserConfig User { get; set; }
        public int NewsId { get; set; }
        public News News { get; set; }
    }
}