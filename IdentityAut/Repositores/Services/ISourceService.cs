﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DTOs.Article;
using Entities_Context.Entities.UserNews;

namespace IServices
{
    public interface ISourceService
    {
        public Task<String> GetSourceNameByIdAsync( Int32 Id);
        public Task<List<SourceDTO>> GetAllSourcesDTOAsync();
    }
}
