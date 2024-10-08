using Central_Service.DTO;
using Repository_DAL_.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Central_Service.Interface
{
    public interface IObjectFactory
    {
        ProductDto BuildProductDto( Product product, string imageUrl );
        CategoryDto BuildCategoryDto( Category category, string imageUrl );
    }
}
