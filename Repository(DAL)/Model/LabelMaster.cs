using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository_DAL_.Model
{
    public class LabelMaster
    {
        [Key]
        public int Id { get; set; }
        public string Label { get; set; }
    }
}
