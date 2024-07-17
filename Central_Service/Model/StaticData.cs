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
        public List<int> price { get; set; } = new List<int>();
        public List<string> qunatityList { get; set; }
        public int Image_Srl { get; set; }
        public int Category_Id { get; set; }
        public string Img_url {  get; set; }
    }
    public class StaticDataRet_DTO
    {
        public Dictionary<string, Dictionary<string,List<Product_DTO>>> Catlog { get; set;} = new Dictionary<string, Dictionary<string, List<Product_DTO>>>();
    }
}
