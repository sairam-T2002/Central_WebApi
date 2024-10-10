using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repository_DAL_.Model;

public class Orders
{
    [Key]
    public int srl { get; set; }

    public string order_id { get; set; }

    [ForeignKey("reservations")]
    public string reservation_id { get; set; }

    [ForeignKey("users")]
    public int id { get; set; }

    public string paymentmethod {  get; set; }

    public string transactionref {  get; set; }

    public double amountpaid { get; set; }

    public virtual Reservation reservation { get; set; }

    public DateTime createdate { get; set; } = DateTime.UtcNow;

    public DateTime? cancellendate { get; set; } = DateTime.UtcNow;

    public string confirmationstatus { get; set; } = string.Empty;
}