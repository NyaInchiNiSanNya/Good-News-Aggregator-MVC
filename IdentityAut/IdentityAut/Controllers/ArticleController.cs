using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business_Logic.Models.TegHelperModels;
using Business_Logic.Models.TegHelperModels;
using Core.DTOs.Article;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using IServices;
using MVC.ControllerFactory;

namespace Business_Logic.Controllers
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

        public async Task<IActionResult> GetArticlesByPage(Int32 page = 1)
        {
            var totalArticlesCount = await _serviceFactory
                .createArticlesService()
                .GetTotalArticleCountAsync();


            if (Int32.TryParse(_serviceFactory
                    .createConfigurationService()
                    ["Pagination:Articles:DefaultPageSize"], out var pageSize))
            {
                List<string> articleBlocks = new List<string>();

                var pageInfo = new PageInfo()
                {
                    PageSize = pageSize,
                    PageNumber = page,
                    TotalItems = totalArticlesCount
                };

                var articles = await _serviceFactory
                    .createArticlesService()
                    .GetShortArticlesWithSourceByPageAsync(page,pageSize);
                

                return View("Articles", new ObjectListModel()
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
        public async Task<IActionResult> GetSelectedArticle(Int32 ArticleId)
        {
            FullArticleDTO fullArticleDto = await _serviceFactory
                .createArticlesService()
                .GetFullArticleByIdAsync(ArticleId);

            if (fullArticleDto is not null)
            {
                return View("FullArticle", fullArticleDto);
            }

            return NotFound();

        }

    }
}