using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repository_DAL_.Model;

public class Category
{
    [Key]
    public int category_id { get; set; }

    public string categoryname { get; set; } = string.Empty;

    public DateTime createdate { get; set; } = DateTime.UtcNow;

    public DateTime modifieddate { get; set; } = DateTime.UtcNow;

    [ForeignKey("image")]
    public int image_srl { get; set; }

    public virtual Image? image { get; set; }

    public virtual ICollection<Product>? products { get; set; }
}