using System.ComponentModel.DataAnnotations;

namespace Repository_DAL_.Model;

public class Address
{
    [Key]
    public int id { get; set; }
    public int user_id { get; set; }
    public int address_type { get; set; }
    public string address_lane { get; set; }
    public string pincode { get; set; }
    public string city { get; set; }
    public string landmark { get; set; }
}