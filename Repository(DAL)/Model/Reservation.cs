using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repository_DAL_.Model;

public class Reservation
{
    [Key]
    public int srl { get; set; }

    public string reservation_id { get; set; } = string.Empty;

    [ForeignKey("users")]
    public int id { get; set; }

    public string cart { get; set; } = string.Empty;

    public double cartprice { get; set; }

    public DateTime createdtime { get; set; }

    public DateTime? confirmedtime { get; set; }

    public DateTime expiretime { get; set; }

    public bool isexpired { get; set; }
}