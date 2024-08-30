using Central_Service.Interface;
using Central_Service.Model;
using Repository_DAL_.Model;
using Repository_DAL_;
using System.Xml.Linq;
using System.Diagnostics;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using ExtensionMethods;
using Central_Service.Core;

namespace Central_Service.Service
{
    public class StaticDataService : ServiceBase,IStaticDataService
    {
        private readonly IRepository<Category> _categories;
        private readonly IRepository<Product> _products;
        private readonly IRepository<Image> _images;

        public StaticDataService( IRepository<Category> categories, IRepository<Product> products, IRepository<Image> images, ILogger<StaticDataService> logger, IServiceProvider serviceProvider ):base(logger, serviceProvider)
        {
            _categories = categories;
            _products = products;
            _images = images;
        }

        public async Task<StaticDataRet_DTO> ServeStaticData( string WebRootPath )
        {
            var output = new StaticDataRet_DTO();
            try
            {
                List<Category> categories = await _categories.GetAll();
                List<Product> products = await _products.GetAll();
                List<Image> images = await _images.GetAll();
                foreach (Category category in categories)
                {
                    Temp_DTO tempProds = new Temp_DTO { name = category.CategoryName,Image_Srl = category.Image_Srl };
                    Image? typ = images.Find(ex=>ex.Image_Srl == category.Image_Srl);
                    var imagePath = Path.Combine(WebRootPath, category.Image_Srl+ typ?.Image_Type);
                    if (File.Exists(imagePath))
                    {
                        string imageUrl = $"/Images/{category.Image_Srl + typ?.Image_Type}";
                        tempProds.Img_Url = imageUrl;
                    }
                    foreach (Product product in products)
                    {
                        if (product.Category_Id == category.Category_Id)
                        {
                            typ = images.Find(ex => ex.Image_Srl == product.Image_Srl);
                            imagePath = Path.Combine(WebRootPath, product.Image_Srl + typ?.Image_Type);
                            string imageUrl = "";
                            if (File.Exists(imagePath))
                            {
                                imageUrl = $"/Images/{product.Image_Srl + typ?.Image_Type}";
                            }
                            Product_DTO temp = new Product_DTO { 
                                prd_id = product.Product_Id,
                                name = product.Product_Name,
                                price = product.Price,
                                isVeg = product.IsVeg,
                                Img_url = imageUrl,
                                Image_Srl = product.Image_Srl,
                                Category_Id = product.Category_Id
                            };
                            tempProds.products.Add(temp);
                        }
                    }
                    output.Contents.Add(category.CategoryName, tempProds);
                }
            }
            catch (Exception ex)
            {
                Logger.LogInformation($"{nameof(ServeStaticData)} exception, exception message: '{ex.Message}' and exception object {ex.JSONStringify<Exception>()}");
            }
            return output;
        }

    }
}
