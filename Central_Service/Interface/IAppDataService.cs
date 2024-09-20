using Central_Service.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Central_Service.Interface
{
    public interface IAppDataService
    {
        Task<AppDataModel> HomePageData(string webroot);
        Task<SearchModel> SeachResult( string baseUrl, string category, string searchQuery);
        Task<string> AppConfig();
        Task<int> SaveCart(List<ProductDto> Cart, string username);
    }
}
