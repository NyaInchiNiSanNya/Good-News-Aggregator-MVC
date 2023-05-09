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
using Microsoft.AspNetCore.Authorization;
using MVC.ControllerFactory;
using NUnit.Framework;

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


        public async Task<IActionResult> GetArticlesNames(String searchLineRequest = "")
        {
            List<AutoCompleteDataDto> list =
                await _serviceFactory.createArticlesService()
                    .GetArticlesNamesByPartNameAsync(searchLineRequest);
            return Ok(list);
        }

        [HttpGet]
        public async Task<IActionResult> GetArticlesByPage(Int32 page = 1, String tag="",String searchLineRequest ="")
        {
            Int32 articlesCount = 0;

            if (!String.IsNullOrEmpty(searchLineRequest))
            {
                articlesCount = await _serviceFactory
                    .createArticlesService()
                    .GetArticleCountWithPartNameAsync(searchLineRequest);
                
            }
            else
            {
                if (!String.IsNullOrEmpty(tag))
                {
                    articlesCount = await _serviceFactory
                        .createArticlesService()
                        .GetArticleCountWithTagAsync(tag);
                }
                else
                {
                    articlesCount = await _serviceFactory
                        .createArticlesService()
                        .GetTotalArticleCountAsync();
                }
            }



            if (Int32.TryParse(_serviceFactory
                    .createConfigurationService()
                    ["Pagination:Articles:DefaultPageSize"], out var pageSize))
            {
                List<string> articleBlocks = new List<string>();

                var pageInfo = new PageInfo()
                {
                    PageSize = pageSize,
                    PageNumber = page,
                    TotalItems = articlesCount
                };

                List<ArticleDTO> articles = new List<ArticleDTO>();


                if (!String.IsNullOrEmpty(searchLineRequest))
                {
                    articles = await _serviceFactory.createArticlesService()
                        .GetArticlesByPartNameAsync(page, pageSize,searchLineRequest);
                }
                else
                {

                    if (!String.IsNullOrEmpty(tag))
                    {
                        articles = await _serviceFactory
                            .createArticlesService().GetArticlesWithSourceByTagByPageAsync(page, pageSize, tag);
                    }
                    else
                    {
                        articles = await _serviceFactory
                            .createArticlesService()
                            .GetShortArticlesWithSourceByPageAsync(page, pageSize);
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
                return StatusCode(500, new { Message = "Can't read configuration data" });
            }
        }

        [HttpGet]
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
        
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteArticlesById(Int32 ArticleId)
        {
            await _serviceFactory.createArticlesService().DeleteArticleById(ArticleId);

            return RedirectToAction("GetArticlesByPage");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetArticleNamesBySearchStringPart(Int32 ArticleId)
        {
            await _serviceFactory.createArticlesService().DeleteArticleById(ArticleId);

            return RedirectToAction("GetArticlesByPage");
        }

    }
}