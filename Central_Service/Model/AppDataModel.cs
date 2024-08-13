using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Central_Service.Model
{
    public class AppDataModel
    {
        public List<string> CarouselUrls { get; set; } = new List<string>();
        public List<CategoryDto> Categories { get; set; } = new List<CategoryDto>();
        public List<ProductDto> FeaturedProducts { get; set; } = new List<ProductDto>();
    }
    public class CategoryDto
    {
        public int Category_Id { get; set; }
        public string Category_Name { get; set; } = string.Empty;
        public string Image_Url { get; set; } = string.Empty;
    }
    public class ProductDto
    {
        public int Product_Id { get; set; }
        public string Product_Name { get; set; } = string.Empty;
        public int Category_Id { get; set; }
        public string Image_Url { get; set; } = string.Empty;
    }
}
