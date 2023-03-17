using System.ComponentModel.DataAnnotations.Schema;
using Entities_Context.Entities.Identity;

namespace Entities_Context.Entities.UserNews
{ 
    //Never used
    public class UserNews
    {

        public int Id { get; set; }
        public int UserId { get; set; }
        public int ArtincleId { get; set; }
        public Artincle Artincle { get; set; }
    }
}