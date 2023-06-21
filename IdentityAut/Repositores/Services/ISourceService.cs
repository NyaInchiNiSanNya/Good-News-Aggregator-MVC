using Core.DTOs.Article;

namespace IServices.Services
{
    public interface ISourceService
    {
        public Task<String> GetSourceNameByIdAsync( Int32 Id);
        public Task<List<SourceDto>> GetAllSourcesDtoAsync();
    }
}
