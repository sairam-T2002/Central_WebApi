using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repository_DAL_.Model
{
    public class Category
    {
        [Key]
        public int Category_Id { get; set; }

        public string CategoryName { get; set; } = string.Empty;

        public DateTime CreateDate { get; set; } = DateTime.Now;

        public DateTime ModifiedDate { get; set; } = DateTime.Now;

        [ForeignKey("Image")]
        public int Image_Srl { get; set; }

        public virtual Image? Image { get; set; }

        public virtual ICollection<Product>? Products { get; set; }
    }
}
