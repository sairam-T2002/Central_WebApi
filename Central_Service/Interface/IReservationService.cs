using Central_Service.DTO;

namespace Central_Service.Interface;

public interface IReservationService
{
    Task<ReserveCartDTO> ReserveCart( List<ProductDto> Cart, int userId );

    Task<OrderDTOOut> ConfirmReservation( OrderDTOInp input);
}