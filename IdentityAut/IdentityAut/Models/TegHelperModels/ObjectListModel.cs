using System;
using System.Collections.Generic;
using Core.DTOs.Article;

namespace Business_Logic.Models.TegHelperModels
{
    public class ObjectListModel
    {
        public IEnumerable<ArticleDTO> ArticlePreviews { get; set; }
        public PageInfo PageInfo { get; set; }
    }
}
