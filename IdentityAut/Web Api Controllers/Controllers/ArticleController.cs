using Core.DTOs.Article;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web_Api_Controllers.ControllerFactory;
using Web_Api_Controllers.Filters.Errors;
using Web_Api_Controllers.RequestModels;
using Web_Api_Controllers.ResponseModels;

namespace Web_Api_Controllers.Controllers
{
    [ApiController]
    [CustomExceptionFilter]
    [Route("article")]
    public class ArticleController : ControllerBase
    {
        private readonly IServiceFactory _serviceFactory;

        public ArticleController(IServiceFactory serviceFactory)
        {
            _serviceFactory = serviceFactory ?? throw new NullReferenceException(nameof(serviceFactory));
        }

        [HttpGet]
        public async Task<IActionResult> GetArticlesByPage([FromQuery] GetArticlesRequest request)
        {
            var articleList = await _serviceFactory
                .CreateArticlesService()
                .GetShortArticlesWithSourceByPageAsync(request.Page, request.PageSize, request.UserFilter);
            
            return Ok(articleList);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetSelectedArticle(Int32 id)
        {
            var fullArticleDto = await _serviceFactory
                .CreateArticlesService()
                .GetFullArticleByIdAsync(id);

            if (fullArticleDto == null)
            {
                return NotFound();
            }

            return Ok(_serviceFactory.CreateMapperService().Map<GetArticleResponse>(fullArticleDto));
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteArticlesById(Int32 id)
        {
            Boolean response=await _serviceFactory.CreateArticlesService().DeleteArticleById(id);

            if (response)
            {
                return Ok(StatusCode(204));
            }

            return NotFound();
        }


    }
}