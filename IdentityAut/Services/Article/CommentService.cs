using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Core.DTOs.Article;
using Entities_Context.Entities.UserNews;
using IServices;
using IServices.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using Serilog;

namespace Services.Article
{
    public class CommentService : ICommentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CommentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        
        public async Task<List<CommentDto>> GetArticleCommentsByArticleId(int id)
        {
            if (id < 1)
            {
                throw new ArgumentException(nameof(id));
            }

            List<CommentDto> dto= await _unitOfWork.Comment
                .GetAsQueryable()
                .AsNoTracking()
                .Where(x=>x.ArticleId==id)
                .Include(x=>x.Article)
                .Include(x=>x.User)
                .Select(x=>_mapper.Map<CommentDto>(x))
                .ToListAsync();

            return dto;
        }

        public async Task AddNewComment(Int32 articleId, String textComment, String email)
        {
            if (articleId < 1 || textComment.IsNullOrEmpty() || email.IsNullOrEmpty() || textComment.Length>50)
            {
                Log.Error("Invalid comments parameters: id={0}, textComment={1}, email={2}",
                    articleId, textComment,email);
                
                throw new ArgumentException();
            }

            if (await _unitOfWork.Articles.GetAsQueryable().Where(x=>x.Id== articleId).AnyAsync())
            {
                var userId = await _unitOfWork.Users
                    .GetAsQueryable()
                    .AsNoTracking()
                    .Where(x=>x.Email.Equals(email))
                    .Select(x=>x.Id)
                    .FirstOrDefaultAsync();
                
                if (userId != 0)
                {
                    await _unitOfWork.Comment.AddAsync(new Comment()
                    {
                        DateTime = DateTime.Now,
                        Text = textComment,
                        UserId = userId,
                        ArticleId = articleId

                    });

                    await _unitOfWork.SaveChangesAsync();
                    return;
                }

                Log.Error("user does not exist:{0}", email);
                
                throw new ArgumentException();
            }
            else
            {
                Log.Error("article does not exist: id={0}", articleId);
                
                throw new ArgumentException();
            }
        }
    }
}
