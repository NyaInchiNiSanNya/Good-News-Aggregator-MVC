﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.Article
{
    public class FullArticleDto
    {
        public Int32 Id { get; set; }
        public String HashUrlId { get; set; }
        public String Title { get; set; }
        public String ShortDescription { get; set; }
        public String FullText { get; set; }
        public Double PositiveRate { get; set; }
        public Double FirstRate { get; set; }
        public Double SecondRate { get; set; }
        public List<String> ArticleTags { get; set; }
        public String ArticleSourceUrl { get; set; }
        public String SourceUrl { get; set; }
        public String SourceName { get; set; }
        public String ArticlePicture { get; set; }
        public DateTime DateTime { get; set; }
        public Int32 SourceId { get; set; }
    }
}
