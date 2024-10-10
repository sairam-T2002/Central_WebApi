using Central_Service.Interface;
using Central_Service.DTO;
using Repository_DAL_.Model;
using ExtensionMethods;

namespace Central_Service.Service;

public class ObjectFactory : IObjectFactory
{
    public ProductDto BuildProductDto( Product product, string imageUrl )
    {
        return new ProductDto
        {
            Product_Id = product.product_id,
            Product_Name = product.product_name,
            Category_Id = product.category_id,
            Image_Url = imageUrl,
            IsVeg = product.isveg,
            IsBestSeller = product.isbestseller,
            StockCount = product.stockcount,
            Rating = product.rating,
            RatingCount = product.ratingcount,
            IsFeatured = product.isfeatured,
            Price = product.price,
        };
    }
    public CategoryDto BuildCategoryDto( Category category, string imageUrl )
    {
        return new CategoryDto
        {
            Category_Id = category.category_id,
            Category_Name = category.categoryname,
            Image_Url = imageUrl
        };
    }

    public Reservation BuildReservationDomain( List<ProductDto> cart, bool isexpired, int userId )
    {
        return new Reservation
        {
            reservation_id = $"Reservation_{Guid.NewGuid()}",
            cart = cart.JSONStringify(),
            createdtime = DateTime.UtcNow,
            expiretime = DateTime.UtcNow.AddMinutes(10),
            isexpired = isexpired,
            id = userId
        };
    }

    public Orders BuildOrderDomain( OrderDTOInp order)
    {
        return new Orders {
            order_id = $"Order_{Guid.NewGuid()}",
            reservation_id = order.ReservationId,
            id = order.UserId,
            paymentmethod = order.PaymentMethod,
            transactionref = order.TransactionRef,
            amountpaid = order.AmountPaid,
            createdate = DateTime.UtcNow,
            confirmationstatus = order.Status
        };
    }
}