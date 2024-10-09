using System.ComponentModel.DataAnnotations;

namespace Repository_DAL_.Model
{
    public class LabelMaster
    {
        [Key]
        public int Id { get; set; }

        public string Label { get; set; }

        public DateTime CreateDate { get; set; } = DateTime.UtcNow;

        public DateTime ModifiedDate { get; set; } = DateTime.UtcNow;
    }
}
