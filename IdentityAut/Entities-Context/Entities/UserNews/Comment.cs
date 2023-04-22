using AspNetSamples.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities_Context.Entities.UserNews
{
    public class Comment : IBaseEntity
    {
        public Int32 Id { get; set; }
        public DateTime DateTime { get; set; }
        public Int32 PositiveRate { get; set; }
        public String Text { get; set; }

        public Int32 UserId { get; set; }
        public User User   { get; set; }

        public Int32 ArticleId { get; set; }
        public Article Article { get; set; }

    }
}
