using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repository_DAL_.Model;

public class Product
{
    [Key]
    public int product_id { get; set; }

    public string product_name { get; set; }

    public int price { get; set; }

    public bool isveg { get; set; }

    [ForeignKey("image")]
    public int image_srl { get; set; }

    public virtual Image image { get; set; }

    [ForeignKey("category")]
    public int category_id { get; set; }

    public virtual Category category { get; set; }

    public bool isfeatured { get; set; }

    [Range(0, 5, ErrorMessage = "Reviews must be between 0 and 5.")]
    [RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessage = "Reviews can have a maximum of 2 decimal places.")]
    public double rating { get; set; }

    public int ratingcount { get; set; }

    public int stockcount { get; set; }

    public bool isbestseller { get; set; }

    public DateTime createdate { get; set; } = DateTime.UtcNow;

    public DateTime modifieddate { get; set; } = DateTime.UtcNow;
}