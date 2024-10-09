using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repository_DAL_.Model
{
    public class Reservation
    {
        [Key]
        public int Srl {  get; set; }

        public string Reservation_Id { get; set; } = string.Empty;

        [ForeignKey("Users")]
        public int Id { get; set; }

        public string Cart { get; set; } = string.Empty;

        public double CartPrice { get; set; }

        public DateTime CreatedTime { get; set; }

        public DateTime? ConfirmedTime { get; set; }

        public DateTime ExpireTime {  get; set; }

        public bool IsExpired {  get; set; }
    }
}
