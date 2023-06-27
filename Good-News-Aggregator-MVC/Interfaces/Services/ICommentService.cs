using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DTOs.Article;

namespace IServices.Services
{
    public interface ICommentService
    {
        public Task<List<CommentDto>> GetArticleCommentsByArticleId(Int32 id);
        public Task AddNewComment(Int32 articleId, String textComment, String Email);
    }
}
