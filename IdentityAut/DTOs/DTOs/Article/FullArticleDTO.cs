using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.Article
{
    public class FullArticleDTO
    {
        public Int32 Id { get; set; }
        public String Title { get; set; }
        public String ShortDescription { get; set; }
        public String FullText { get; set; }
        public Int32 PositiveRate { get; set; }
        public String URL { get; set; }
        public String ArticlePicture { get; set; }
        public DateTime DateTime { get; set; }
        public String Source { get; set; }
        public Int32 SourceId { get; set; }
    }
}
