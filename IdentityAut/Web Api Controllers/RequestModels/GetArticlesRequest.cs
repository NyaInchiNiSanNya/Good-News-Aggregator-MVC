namespace Web_Api_Controllers.RequestModels
{
    public class GetArticlesRequest
    {
        public Int32 Page { get; set; }
        public Int32 PageSize { get; set; }
        public Int32 UserFilter { get; set; }
    }
}
