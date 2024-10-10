using System.ComponentModel.DataAnnotations;

namespace Repository_DAL_.Model;

public class Label
{
    [Key]
    public int label_id { get; set; }

    public string labeld { get; set; }

    public string label_description { get; set; }

    public DateTime createdate { get; set; } = DateTime.UtcNow;

    public DateTime modifieddate { get; set; } = DateTime.UtcNow;
}