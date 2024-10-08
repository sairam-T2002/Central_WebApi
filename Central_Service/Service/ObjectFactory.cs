using Central_Service.Interface;
using Central_Service.DTO;
using Repository_DAL_.Model;

namespace Central_Service.Service
{
    public class ObjectFactory: IObjectFactory
    {
        public ProductDto BuildProductDto( Product product, string imageUrl)
        {
            return new ProductDto
            {
                Product_Id = product.Product_Id,
                Product_Name = product.Product_Name,
                Category_Id = product.Category_Id,
                Image_Url = imageUrl,
                IsVeg = product.IsVeg,
                IsBestSeller = product.IsBestSeller,
                StockCount = product.StockCount,
                Rating = product.Rating,
                RatingCount = product.RatingCount,
                IsFeatured = product.IsFeatured,
                Price = product.Price,
            };
        }
        public CategoryDto BuildCategoryDto( Category category, string imageUrl )
        {
            return new CategoryDto
            {
                Category_Id = category.Category_Id,
                Category_Name = category.CategoryName,
                Image_Url = imageUrl
            };
        }
    }
}
