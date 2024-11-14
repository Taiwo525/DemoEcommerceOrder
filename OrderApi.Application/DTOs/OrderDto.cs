using System.ComponentModel.DataAnnotations;

namespace OrderApi.Application.DTOs
{
    public record OrderDto(int id,
        [Required, Range(1, int.MaxValue)] int ProductId,
        [Required, Range(1, int.MaxValue)] int UserId,
        [Required, Range(1, int.MaxValue)] int PurchaseQuantity,
        DateTime OrderedDate
        );
}
