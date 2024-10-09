using Central_Service.DTO;

namespace Central_Service.Interface;

public interface IReservationService
{
    Task<bool> ReserveCart( List<ProductDto> Cart, int userId );
}