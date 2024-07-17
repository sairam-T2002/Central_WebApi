using Central_Service.Interface;
using Central_Service.Model;
using Repository_DAL_.Model;
using Repository_DAL_;

namespace Central_Service.Service
{
    public class StaticDataService : IStaticDataService
    {
        private readonly IRepository<Category> _categories;
        private readonly IRepository<Product> _products;
        private readonly IRepository<Image> _images;

        public StaticDataService( IRepository<Category> categories, IRepository<Product> products, IRepository<Image> images )
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
                var temp2 = new Dictionary<string, List<Product_DTO>>();
                string CatImg = "";
                foreach (Category category in categories)
                {
                    List<Image> Images = await _images.Find(x => x.Image_Srl == category.Image_Srl);
                    Image img = Images.FirstOrDefault();
                    var imagePath = Path.Combine(WebRootPath, $"{img.Image_Srl + img.Image_Type}");
                    if (File.Exists(imagePath))
                    {
                        var imageUrl = $"/Images/{img.Image_Srl + img.Image_Type}";
                        CatImg = imageUrl;
                    }

                    List<Product_DTO> tempProdnew = new List<Product_DTO>();
                    string ProdImg = "";
                    foreach (Product product in products)
                    {
                        if (category.Category_Id == product.Category_Id)
                        {
                            Images = await _images.Find(x => x.Image_Srl == product.Image_Srl);
                            img = Images.FirstOrDefault();
                            imagePath = Path.Combine(WebRootPath, $"{img.Image_Srl + img.Image_Type}");
                            if (File.Exists(imagePath))
                            {
                                var imageUrl = $"/Images/{img.Image_Srl + img.Image_Type}";
                                ProdImg = imageUrl;
                            }

                            Product_DTO temp = new Product_DTO
                            {
                                prd_id = product.Product_Id,
                                name = product.Product_Name,
                                Image_Srl = product.Image_Srl,
                                price = product.Price.Split(',').Select(int.Parse).ToList(),
                                qunatityList = product.Quantity_List.Split(',').ToList(),
                                Category_Id = category.Category_Id,
                                isVeg = product.IsVeg == "true",
                                Img_url = ProdImg
                            };
                            tempProdnew.Add(temp);
                        }
                    }
                    temp2.Add(CatImg, tempProdnew);
                    output.Catlog.Add(category.CategoryName, temp2);
                }
            }
            catch (Exception ex)
            {

            }
            return output;
        }

    }
}
