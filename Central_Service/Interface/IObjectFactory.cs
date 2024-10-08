﻿using Central_Service.DTO;
using Repository_DAL_.Model;

namespace Central_Service.Interface;

public interface IObjectFactory
{
    ProductDto BuildProductDto( Product product, string imageUrl );
    CategoryDto BuildCategoryDto( Category category, string imageUrl );
    Reservation BuildReservationDomain( List<ProductDto> cart, bool isexpired, int userId );
    Orders BuildOrderDomain( OrderDTOInp order );
}