using Central_Service.DTO;

namespace Central_Service.Interface
{
    public interface IAppDataService
    {
        Task<AppDataDTO> HomePageData(string webroot);
        Task<SearchModel> SeachResult( string baseUrl, string category, string searchQuery);
        Task<string> AppConfig();
        Task<int> SaveCart(List<ProductDto> Cart, int userId);
    }
}
