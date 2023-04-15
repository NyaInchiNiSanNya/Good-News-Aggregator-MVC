﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DTOs.Article;

namespace IServices
{
    public interface IArticleService
    {
        public Task<List<ArticleDTO>> GetShortArticlesWithSource(Int32 Page,Int32 PageSize);
        public Int32 GetTotalArticleCount();
    }
}
