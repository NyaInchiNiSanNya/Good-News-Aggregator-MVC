using Core.DTOs.Article;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC.ControllerFactory;
using MVC.Models.comment;
using Serilog;

namespace MVC.Controllers
{
    public class CommentController:Controller
    {
        private readonly IServiceFactory _serviceFactory;

        public CommentController
        (IServiceFactory serviceFactory
        )
        {
            if (serviceFactory is null)
            {
                throw new NullReferenceException(nameof(serviceFactory));
            }
            _serviceFactory = serviceFactory;
        }

        [HttpGet]
        public async Task<IActionResult> GetComments(Int32 id)
        {
            List<CommentDto> list =
                await _serviceFactory
                    .CreateCommentService()
                    .GetArticleCommentsByArticleId(id);

            return Ok(list);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> PostComment([FromBody] CommentModel comment)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                var userEmail = HttpContext.User.Identity.Name;
                
                if (userEmail != null)
                {
                    await _serviceFactory.CreateCommentService().AddNewComment(comment.id, comment.text, userEmail);
                    return Ok();
                }
            }
            Log.Warning("attempt to publish a comment without authorization");
            return BadRequest("Пользователь не установлен");
        }
    }
}
