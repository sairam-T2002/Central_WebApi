using System.ComponentModel.DataAnnotations;

namespace Repository_DAL_.Model
{
    public class ApiLog
    {
        [Key]
        public int srl { get; set; }

        public string log_origin {  get; set; }
        
        public string log {  get; set; }

        public string Exception {  get; set; }

        public DateTime DateTime { get; set; }
    }
}
