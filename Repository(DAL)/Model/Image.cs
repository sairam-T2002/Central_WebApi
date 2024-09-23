using System.ComponentModel.DataAnnotations;

namespace Repository_DAL_.Model
{
    public class Image
    {
        [Key]
        public int Image_Srl { get; set; }

        public string? Image_Description { get; set; }

        public string Image_Type { get; set; } = string.Empty;

        public bool IsCarousel { get; set; }

        public DateTime CreateDate { get; set; } = DateTime.Now;

        public DateTime ModifiedDate { get; set; } = DateTime.Now;
    }
}
