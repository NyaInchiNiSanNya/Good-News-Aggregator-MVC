﻿using IServices.Services;
using Services.Account;
using Services.Article;
using Services.Article.ArticleRate;

namespace MVC.Extensions
{
    public static class GoodNewsAggregatorServicesExtension
    {
        public static IServiceCollection AddGoodNewsAggregatorServices
            (this IServiceCollection services)
        {
            services.AddScoped<ISourceService, SourceService>();
            services.AddScoped<IArticleSentimentAnalyzer, ArticleSentimentAnalyzer>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IArticleTagService, ArticleTagService>();
            services.AddScoped<IArticleService, ArticleService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ISettingsService, SettingsService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IUiThemeService, UiThemeService>();
            
            return services;
        } 
    }
}
