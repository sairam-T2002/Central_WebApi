using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository_DAL_.Model
{
    public class ControlMaster
    {
        [Key]
        public int id { get; set; }
        public string devUrl { get; set; } = string.Empty;
        public string prodUrl { get; set; } = string.Empty;
        public string? gmapkey {  get; set; } = string.Empty;
        public int defaultSearchImg {  get; set; }
    }
}
