using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business_Logic.Models.TegHelperModels;
using Business_Logic.Models.TegHelperModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Business_Logic.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;

        public HomeController(IConfiguration configuration)
        {
            if (configuration is null)
            {
                throw new ArgumentNullException();
            }
            _configuration = configuration;

        }

        public async Task<IActionResult> Start(int page = 1)
        {
            var totalArticlesCount = 100;

            if (int.TryParse(_configuration["Pagination:Articles:DefaultPageSize"], out var pageSize))
            {
                List<string> SimpleBlocks = new List<string>();
                for (int i = 0; i < 100; i++)
                {
                    SimpleBlocks.Add("Ну как там с деньгами" + i);
                }

                var pageInfo = new PageInfo()
                {
                    PageSize = pageSize,
                    PageNumber = page,
                    TotalItems = totalArticlesCount
                };

                var articles = SimpleBlocks
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize);


                return View("Index", new ObjectListModel()
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