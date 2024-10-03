using Central_Service.Core;
using Central_Service.Interface;
using Central_Service.Model;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Repository_DAL_.Model;
using Repository_DAL_;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExtensionMethods;
using Microsoft.EntityFrameworkCore;

namespace Central_Service.Service
{
    public class AppDataService : ServiceBase, IAppDataService
    {
        private readonly IRepository<Category> _categories;
        private readonly IRepository<Product> _products;
        private readonly IRepository<Image> _images;
        private readonly IRepository<Label> _labels;
        private readonly EFContext _dbcontext;
        private readonly IObjectFactory _factory;

        public AppDataService( EFContext dbcontext,ILogger<AppDataService> logger, IRepository<Category> categories, IRepository<Product> products, IRepository<Image> images, IRepository<Label> labels, IServiceProvider serviceProvider, IObjectFactory factory ) : base(logger, serviceProvider)
        {
            _categories = categories;
            _products = products;
            _images = images;
            _labels = labels;
            _dbcontext = dbcontext;
            _factory = factory;
        }

        public async Task<string> AppConfig()
        {  
            var repo = GetService<IRepository<ControlMaster>>();

            string? mapkey = (await repo.Find(x=>x.id == 1))[0].gmapkey ;
            return mapkey ?? "";
        }

        public async Task<AppDataModel> HomePageData( string baseUrl )
        {
            var output = new AppDataModel();
            var dbLogger = GetService<IApiLog>();
            List<ApiLog> log = new List<ApiLog>();
            try
            {
                Uri baseUri = new Uri(baseUrl);
                var images = await _images.GetAll();
                var carouselImages = images.Where(img => img.IsCarousel);
                var labels = (await _labels.GetAll()).OrderBy(ex=>ex.Label_Id);
                var repo = GetService<IRepository<ControlMaster>>();
                var control = (await repo.Find(x => x.id == 1))[0];
                var defaultImage = images.Find(img=> img.Image_Srl == control.defaultSearchImg);

                output.DefaultSearchBanner = new Uri(baseUri,$"/Images/{defaultImage?.Image_Srl}{defaultImage?.Image_Type}").ToString();
                output.Label.AddRange(labels.Select(lb=>lb.Labeld));
                output.CarouselUrls.AddRange(carouselImages.Select(img=> new Uri(baseUri, $"/Images/{img.Image_Srl}{img.Image_Type}").ToString()));
                var categories = (await _categories.GetAll()).OrderBy(cat=>cat.CategoryName);
                foreach (var cat in categories)
                {
                    var categoryImage = images.FirstOrDefault(img => img.Image_Srl == cat.Image_Srl);
                    if (categoryImage != null)
                    {
                        var imageUrl = new Uri(baseUri, $"/Images/{cat.Image_Srl}{categoryImage.Image_Type}").ToString();
                        output.Categories.Add(_factory.BuildCategoryDto(cat,imageUrl));
                    }
                }
                var featuredProducts = (await _products.Find(prd=>prd.IsFeatured)).OrderBy(prd=>prd.Product_Name);
                foreach (var product in featuredProducts)
                {
                    var productImage = images.FirstOrDefault(img => img.Image_Srl == product.Image_Srl);
                    if (productImage != null)
                    {
                        var imageUrl = new Uri(baseUri, $"/Images/{product.Image_Srl}{productImage.Image_Type}").ToString();
                        output.FeaturedProducts.Add(_factory.BuildProductDto(product,imageUrl));
                    }
                }
                log.Add(new ApiLog
                {
                    log_origin = $"AppDataService.{nameof(HomePageData)}-{LogUtil.Response}",
                    log = output.JSONStringify<AppDataModel>(),
                    Exception = string.Empty,
                    DateTime = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                Logger.LogInformation($"Method name: {nameof(HomePageData)}, Exception Message: {ex.Message}, Exception: {ex.JSONStringify<Exception>()}");
                log.Add(new ApiLog
                {
                    log_origin = $"AppDataService.{nameof(HomePageData)}-{LogUtil.Response}",
                    log = ex.JSONStringify<Exception>(),
                    Exception = ex.Message,
                    DateTime = DateTime.UtcNow
                });
                output =  null;
            }
            finally
            {
                dbLogger.AddDbLog(log);
            }
            return output;
        }

        public async Task<SearchModel> SeachResult( string baseUrl, string category, string searchQuery )
        {
            var output = new SearchModel();
            var dbLogger = GetService<IApiLog>();
            List<ApiLog> log = new List<ApiLog>();
            try
            {
                var images = await _images.GetAll();
                var baseUri = new Uri(baseUrl);
                var selectedCategory = (await _categories.Find(cat => cat.CategoryName == category)).FirstOrDefault();
                if (selectedCategory != null)
                {
                    var products = (await _products.Find(prd => prd.Category_Id == selectedCategory.Category_Id && prd.Product_Name.ToLower().Contains(searchQuery.Trim().ToLower()))).OrderBy(prd => prd.Product_Name);
                    output.CategoryId = selectedCategory.Category_Id;
                    output.CategoryName = selectedCategory.CategoryName;
                    output.CategoryImageUrl = new Uri(baseUri, $"/Images/{selectedCategory.Image_Srl}{images.Where(img => img.Image_Srl == selectedCategory.Image_Srl).FirstOrDefault()?.Image_Type ?? ""}").ToString();

                    output.Result = products.Select(ex => _factory.BuildProductDto(ex, new Uri(baseUri, $"/Images/{ex.Image_Srl}{images.Where(img => img.Image_Srl == ex.Image_Srl).FirstOrDefault()?.Image_Type ?? ""}").ToString())).ToList<ProductDto>();
                }
                else if (category.ToLower() == "all")
                {
                    var products = await _products.Find(prd => prd.Product_Name.ToLower().Contains(searchQuery.Trim().ToLower()));
                    output.CategoryId = 0;
                    output.CategoryName = "All";
                    output.Result = products.Select(ex => _factory.BuildProductDto(ex, new Uri(new Uri(baseUrl), $"/Images/{ex.Image_Srl}{images.Where(img => img.Image_Srl == ex.Image_Srl).FirstOrDefault()?.Image_Type ?? ""}").ToString())).ToList<ProductDto>();
                }
                else
                {
                    output = null;
                }
                log.Add(new ApiLog
                {
                    log_origin = $"AppDataService.{nameof(SeachResult)}-{LogUtil.Response}",
                    log = output.JSONStringify<SearchModel>(),
                    Exception = string.Empty,
                    DateTime = DateTime.UtcNow
                });
                log.Add(new ApiLog
                {
                    log_origin = $"AppDataService.{nameof(SeachResult)}-{LogUtil.Request}",
                    log = $"GetSearchResults/{category}?searchquery={searchQuery}",
                    Exception = string.Empty,
                    DateTime = DateTime.UtcNow
                });
            }
            catch(Exception ex)
            {
                Logger.LogInformation($"Method name: {nameof(SeachResult)}, Exception Message: {ex.Message}, Exception: {ex.JSONStringify<Exception>()}");
                log.Add(new ApiLog
                {
                    log_origin = $"AppDataService.{nameof(SeachResult)}-{LogUtil.Exception}",
                    log = ex.JSONStringify<Exception>(),
                    Exception = ex.Message,
                    DateTime = DateTime.UtcNow
                });
            }
            finally
            {
                dbLogger.AddDbLog(log);
            }
            return output;
        }

        public async Task<int> SaveCart( List<ProductDto> Cart, string username )
        {
            try
            {
                var repo = GetService<IRepository<User>>();
                if (repo == null) return -1;

                User? user = (await repo.Find(usr=>usr.Usr_Nam == username)).FirstOrDefault();
                if(user == null) return -1;

                user.Cart = Cart.JSONStringify<List<ProductDto>>();
                await repo.Update(user);
                return 1;
            }
            catch(Exception ex)
            {
                Logger.LogInformation($"Method name: {nameof(SaveCart)}, Exception Message: {ex.Message}, Exception: {ex.JSONStringify<Exception>()}");
            }
            return 0;
        }
    }
}
