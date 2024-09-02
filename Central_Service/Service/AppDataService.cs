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
        private readonly EFContext _dbcontext;

        public AppDataService( EFContext dbcontext,ILogger<AppDataService> logger, IRepository<Category> categories, IRepository<Product> products, IRepository<Image> images, IServiceProvider serviceProvider ) : base(logger, serviceProvider)
        {
            _categories = categories;
            _products = products;
            _images = images;
            _dbcontext = dbcontext;
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
            try
            {
                var images = await _images.GetAll();
                var carouselImages = images.Where(img => img.IsCarousel);

                foreach (var img in carouselImages)
                {
                    var imageUrl = new Uri(new Uri(baseUrl), $"/Images/{img.Image_Srl}{img.Image_Type}").ToString();
                    output.CarouselUrls.Add(imageUrl);
                }

                var categories = await _categories.GetAll();
                foreach (var cat in categories)
                {
                    var categoryImage = images.FirstOrDefault(img => img.Image_Srl == cat.Image_Srl);
                    if (categoryImage != null)
                    {
                        var imageUrl = new Uri(new Uri(baseUrl), $"/Images/{cat.Image_Srl}{categoryImage.Image_Type}").ToString();
                        output.Categories.Add(new CategoryDto
                        {
                            Category_Id = cat.Category_Id,
                            Category_Name = cat.CategoryName,
                            Image_Url = imageUrl
                        });
                    }
                }

                var featuredProducts = (await _products.GetAll()).Where(prd => prd.IsFeatured);
                foreach (var product in featuredProducts)
                {
                    var productImage = images.FirstOrDefault(img => img.Image_Srl == product.Image_Srl);
                    if (productImage != null)
                    {
                        var imageUrl = new Uri(new Uri(baseUrl), $"/Images/{product.Image_Srl}{productImage.Image_Type}").ToString();
                        output.FeaturedProducts.Add(new ProductDto
                        {
                            Product_Id = product.Product_Id,
                            Product_Name = product.Product_Name,
                            Category_Id = product.Category_Id,
                            Image_Url = imageUrl,
                            IsVeg = product.IsVeg,
                            IsBestSeller = product.IsBestSeller,
                            StockCount = product.StockCount,
                            Rating = product.Reviews,
                            IsFeatured = product.IsFeatured,
                            Price = product.Price,

                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogInformation($"Method name: {nameof(HomePageData)}, Exception Message: {ex.Message}, Exception: {ex.JSONStringify<Exception>()}");
                return null;
            }
            return output;
        }
    }
}
