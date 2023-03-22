namespace Entities_Context.Entities.UserNews
{
    public class Article
    {
        public Int32 Id { get; set; }
        public String Title { get; set; }
        public String ShortDescription { get; set; }
        public String FullText { get; set; }
        public Int32 PositiveRate { get; set; }
        public String URL { get; set; }
        public DateTime DateTime { get; set; }
        
        public Int32 SourceId { get; set; }
        public Source Source { get; set; }

        public List<ArticleTag> Tags{ get; set; }

        public List<Comment> Comments { get; set; }
    }
}