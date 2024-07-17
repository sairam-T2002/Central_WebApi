using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repository_DAL_.Model
{
    public class Product
    {
        [Key]
        public int Product_Id { get; set; }
        public string Product_Name { get; set; }
        public string Price { get; set; }
        public string Quantity_List { get; set; }
        public string IsVeg { get; set; }

        [ForeignKey("Image")]
        public int Image_Srl { get; set; }
        public virtual Image Image { get; set; }

        [ForeignKey("Category")]
        public int Category_Id { get; set; }
        public virtual Category Category { get; set; }

        public bool IsFeatured { get; set; }
    }
}
