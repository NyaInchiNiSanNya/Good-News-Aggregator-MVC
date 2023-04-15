using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.Account
{
    public class UserDTO
    {
        public Int32 Id { get; set; }
        public String Name { get; set; }
        public String Email { get; set; }
        public DateTime Created { get; set; }
        public Int32 PositiveRate { get; set; }
        //public byte[]? ProfilePicture { get; set; }
        public List<String>? Roles { get; set; }
    }
}
