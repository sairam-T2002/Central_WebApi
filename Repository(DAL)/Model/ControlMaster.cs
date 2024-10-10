using System.ComponentModel.DataAnnotations;

namespace Repository_DAL_.Model;

public class ControlMaster
{
    [Key]
    public int id { get; set; }

    public string devurl { get; set; } = string.Empty;

    public string produrl { get; set; } = string.Empty;

    public string? gmapkey { get; set; } = string.Empty;

    public int defaultsearchimg { get; set; }

    public DateTime createdate { get; set; } = DateTime.UtcNow;

    public DateTime modifieddate { get; set; } = DateTime.UtcNow;
}