using Core.DTOs.Article;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC.ControllerFactory;
using MVC.Models.TegHelperModels;
using Serilog;

namespace MVC.Controllers
{
    public class ArticleController : Controller
    {
        private readonly IServiceFactory _serviceFactory;
        public ArticleController(IServiceFactory serviceFactory)
        {
            if (serviceFactory is null)
            {
                throw new NullReferenceException(nameof(serviceFactory));
            }
            _serviceFactory = serviceFactory;

        }


        public async Task<IActionResult> GetArticlesNames(String searchLineRequest = "")
        {
            List<AutoCompleteDataDto> list =
                await _serviceFactory.CreateArticlesService()
                    .GetArticlesNamesByPartNameAsync(searchLineRequest);
            
            return Ok(list);
        }

        [HttpGet]
        public async Task<IActionResult> GetArticlesByPage(Int32 page = 1, String tag = "", 
            String searchLineRequest = "")
        {
            Int32 articlesCount = 0;
            Int32 userRateFilter = 0;

            if (HttpContext.User.Identity.IsAuthenticated)
            {
                userRateFilter = await _serviceFactory
                    .CreateUserConfigService()
                    .GetUserArticleRateFilter(HttpContext.User.Identity.Name);
            }


            articlesCount = await _serviceFactory
                .CreateArticlesService()
                .GetArticleCount(tag,userRateFilter,searchLineRequest);




            if (Int32.TryParse(_serviceFactory
                    .CreateConfigurationService()
                    ["Pagination:Articles:DefaultPageSize"], out var pageSize))
            {

                var pageInfo = new PageInfo()
                {
                    PageSize = pageSize,
                    PageNumber = page,
                    TotalItems = articlesCount
                };

                var articles = new List<ShortArticleDto>();


                if (!String.IsNullOrEmpty(searchLineRequest))
                {
                    articles = await _serviceFactory.CreateArticlesService()
                        .GetArticlesByPartNameAsync(page, pageSize, searchLineRequest);
                }
                else
                {

                    if (!String.IsNullOrEmpty(tag))
                    {
                        articles = await _serviceFactory
                            .CreateArticlesService()
                            .GetArticlesWithSourceByTagByPageAsync(page, pageSize, tag, userRateFilter);
                    }
                    else
                    {
                        articles = await _serviceFactory
                            .CreateArticlesService()
                            .GetShortArticlesWithSourceByPageAsync(page, pageSize, userRateFilter);
                    }
                }

                return View("Articles", new ObjectListModel()
                {
                    ArticlePreviews = articles,
                    PageInfo = pageInfo
                });
            }
            else
            {
                Log.Error("Can't read configuration data: articles page info");
                return BadRequest();
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetSelectedArticle(Int32 articleId)
        {
            FullArticleDto? fullArticleDto = await _serviceFactory
                .CreateArticlesService()
                .GetFullArticleByIdAsync(articleId);

            if (fullArticleDto is not null)
            {
                return View("FullArticle", fullArticleDto);
            }

            return NotFound();

        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteArticlesById(Int32 articleId)
        {
            await _serviceFactory.CreateArticlesService().DeleteArticleById(articleId);

            return RedirectToAction("GetArticlesByPage");
        }

    }
}