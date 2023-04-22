using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using AspNetSamples.Core;

namespace Entities_Context.Entities.UserNews
{
    public class ArticleTag : IBaseEntity
    {
        public Int32 Id { get; set; }

        public Int32 ArticleId { get; set; }
        public Article Article { get; set; }

        public Int32 TegId { get; set; }
        public Tag Tag  { get; set; }
    }
}
