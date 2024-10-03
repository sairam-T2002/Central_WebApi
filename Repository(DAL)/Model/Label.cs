using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository_DAL_.Model
{
    public class Label
    {
        [Key]
        public int Label_Id { get; set; }

        public string Labeld { get; set; }

        public string Label_Description { get; set; }

        public DateTime CreateDate { get; set; } = DateTime.UtcNow;

        public DateTime ModifiedDate { get; set; } = DateTime.UtcNow;
    }
}
