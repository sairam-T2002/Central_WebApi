using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repository_DAL_.Model
{
    public class Orders
    {
        [Key]
        public int Srl { get; set; }

        public string Order_Id {  get; set; }

        [ForeignKey("Reservations")]
        public string Reservation_Id { get; set; }

        [ForeignKey("Users")]
        public int Id { get; set; }

        public double AmountPaid { get; set; }

        public virtual Reservation Reservation { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? CancellendAt { get; set; } = DateTime.UtcNow;
    }
}
