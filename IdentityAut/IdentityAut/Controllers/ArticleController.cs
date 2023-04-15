using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business_Logic.Models.TegHelperModels;
using Business_Logic.Models.TegHelperModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using IServices;

namespace Business_Logic.Controllers
{
    public class ArticleController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IArticleService _articlesService;

        public ArticleController(IConfiguration configuration, IArticleService articlesService)
        {
            if (configuration is null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }
            _configuration = configuration;

            if (articlesService is null)
            {
                throw new ArgumentNullException(nameof(articlesService));
            }
            _articlesService = articlesService;

        }

        public async Task<IActionResult> GetArticlesByPage(Int32 page = 1)
        {
            var totalArticlesCount = _articlesService.GetTotalArticleCount();

            if (Int32.TryParse(_configuration["Pagination:Articles:DefaultPageSize"], out var pageSize))
            {
                List<string> ArticleBlocks = new List<string>();

                var pageInfo = new PageInfo()
                {
                    PageSize = pageSize,
                    PageNumber = page,
                    TotalItems = totalArticlesCount
                };

                var articles = await _articlesService.GetShortArticlesWithSource(page,pageSize);
                

                return View("Article", new ObjectListModel()
                {
                    ArticlePreviews = articles,
                    PageInfo = pageInfo
                });
            }
            else
            {
                return StatusCode(500, new { Message = "Can't read configuration data" });
            }
        }


    }
}