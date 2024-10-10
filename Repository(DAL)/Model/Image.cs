using System.ComponentModel.DataAnnotations;

namespace Repository_DAL_.Model;

public class Image
{
    [Key]
    public int image_srl { get; set; }

    public string? image_description { get; set; }

    public string image_type { get; set; } = string.Empty;

    public bool iscarousel { get; set; }

    public DateTime createdate { get; set; } = DateTime.UtcNow;

    public DateTime modifieddate { get; set; } = DateTime.UtcNow;
}