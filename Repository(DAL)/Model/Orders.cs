using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository_DAL_.Model
{
    public class Orders
    {
        [Key]
        public int Srl { get; set; }

        public string Order_Id {  get; set; }

        [ForeignKey("Image")]
        public string Reservation_Id { get; set; }

        public virtual Reservation Reservation { get; set; }
    }
}
