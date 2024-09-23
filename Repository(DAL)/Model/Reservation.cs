using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository_DAL_.Model
{
    public class Reservation
    {
        [Key]
        public int Srl {  get; set; }

        
        public string Reservation_Id { get; set; } = string.Empty;

        public string Cart { get; set; } = string.Empty;

        public DateTime CreatedTime { get; set; }

        public DateTime ExpireTime {  get; set; }
    }
}
