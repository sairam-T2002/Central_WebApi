using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repository_DAL_.Model;

public class Product
{
    [Key]
    public int Product_Id { get; set; }

    public string Product_Name { get; set; }

    public int Price { get; set; }

    public bool IsVeg { get; set; }

    [ForeignKey("Image")]
    public int Image_Srl { get; set; }

    public virtual Image Image { get; set; }

    [ForeignKey("Category")]
    public int Category_Id { get; set; }

    public virtual Category Category { get; set; }

    public bool IsFeatured { get; set; }

    [Range(0, 5, ErrorMessage = "Reviews must be between 0 and 5.")]
    [RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessage = "Reviews can have a maximum of 2 decimal places.")]
    public double Rating { get; set; }

    public int RatingCount { get; set; }

    public int StockCount { get; set; }

    public bool IsBestSeller { get; set; }

    public DateTime CreateDate { get; set; } = DateTime.UtcNow;

    public DateTime ModifiedDate { get; set; } = DateTime.UtcNow;
}