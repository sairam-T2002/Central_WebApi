using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Central_Service.Model
{
    public class Category_DTO
    {
        public string name { get; set; }
        public int Image_Srl { get; set; }
    }
    public class Product_DTO
    {
        public int prd_id { get; set; }
        public string name { get; set; }
        public bool isVeg { get; set; }
        public int price { get; set; }
        public int Image_Srl { get; set; }
        public int Category_Id { get; set; }
        public string Img_url {  get; set; }
    }
    public class StaticDataRet_DTO
    {
        public Dictionary<string, Temp_DTO> Contents { get; set; } = new Dictionary<string, Temp_DTO>();
    }
    public class Temp_DTO
    {
        public List<Product_DTO> products { get; set; } = new List<Product_DTO>();
        public string name {  set; get; }
        public int Image_Srl { set; get; }
        public string Img_Url {  set; get; }
    }
}
