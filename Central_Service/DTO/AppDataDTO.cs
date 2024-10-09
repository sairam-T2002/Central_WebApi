namespace Central_Service.DTO
{
    public class AppDataDTO
    {
        public List<string> CarouselUrls { get; set; } = new List<string>();
        public List<CategoryDto> Categories { get; set; } = new List<CategoryDto>();
        public List<ProductDto> FeaturedProducts { get; set; } = new List<ProductDto>();
        public List<string> Label { get; set; } = new List<string>();
        public string DefaultSearchBanner {  get; set; } = string.Empty;
    }
    public class SearchModel
    {
        public string CategoryName { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public string CategoryImageUrl { get; set; } = string.Empty;
        public List<ProductDto> Result { get; set; } = new List<ProductDto>();
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
        public bool IsVeg { get; set; }
        public bool IsBestSeller { get; set; }
        public int StockCount { get; set; }
        public double Rating { get; set; }
        public int  RatingCount { get; set; }
        public bool IsFeatured { get; set; }
        public int Price { get; set; }
        public int Quantity {  get; set; }
    }
}
