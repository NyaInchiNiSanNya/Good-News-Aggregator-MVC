﻿using Entities_Context.Entities.UserNews;
using Microsoft.EntityFrameworkCore;


namespace Entities_Context
{
    public class UserArticleContext : DbContext
    {
        public DbSet<Article> Articles { get; set; }
        public DbSet<ArticleTag> ArticlesTags { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<UserRole> Roles { get; set; }
        public DbSet<Source> Sources { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<SiteTheme> Themes { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UsersRoles> UsersRoles { get; set; }


        public UserArticleContext(DbContextOptions<UserArticleContext> options) : base(options)
        { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionBuilder)
        {
            base.OnConfiguring(optionBuilder);
        }
    }

}