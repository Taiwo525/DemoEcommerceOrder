using System.ComponentModel.DataAnnotations;

namespace OrderApi.Application.DTOs
{
     public record OrderDetailsDto
        (
        [Required] int OrderId,
        [Required] int ProductId,
        [Required] int UserId,
        [Required, EmailAddress] string Email,
        [Required, EmailAddress] string Address,
        [Required] string PhoneNumber,
        [Required] string ProductName,
        [Required] string UserName,
        [Required, Range(1, int.MaxValue)] int PurchaseQuantity,
        [Required, DataType(DataType.Currency)] decimal UnitPrice,
        [Required, DataType(DataType.Currency)] decimal TotalPrice,
        DateTime OreredDate
        );
}
