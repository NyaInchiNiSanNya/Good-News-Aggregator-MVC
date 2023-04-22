using AspNetSamples.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Entities_Context.Entities.UserNews
{
    public class Tag : IBaseEntity
    {
        public Int32 Id { get; set; }
        public string Name { get; set; }

        public List<ArticleTag> Articles { get; set; }
    }
}
