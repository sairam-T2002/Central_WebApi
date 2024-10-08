using Central_Service.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Central_Service.Interface
{
    public interface IReservationService
    {
        Task<bool> ReserveCart( List<ProductDto> Cart, string userName);
    }
}
