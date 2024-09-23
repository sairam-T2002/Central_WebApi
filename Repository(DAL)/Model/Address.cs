using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository_DAL_.Model
{
    public class Address
    {
        [Key]
        public int Id { get; set; }
        public int User_Id { get; set; }
        public int Address_Type {  get; set; }
        public string Address_lane { get; set; }
        public string Pincode { get; set; }
        public string City { get; set; }
        public string Landmark { get; set; }
    }
}
