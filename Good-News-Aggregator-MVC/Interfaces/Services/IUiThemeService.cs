namespace IServices.Services
{
    public interface IUiThemeService
    {
        public Task<String> GetThemeNameByIdAsync(Int32 id);

        public Task InitiateThemeAsync();

        public Task<Int32> GetIdThemeByStringAsync(string theme);

        public Task<Int32> GetIdDefaultThemeAsync();

        public Task<Boolean> IsThemeExistByNameAsync(string theme);
        
        public Task<List<String>> GetAllThemesAsync();
    }
}
