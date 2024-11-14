using System.ComponentModel.DataAnnotations;

namespace OrderApi.Application.DTOs
{
    public record ProductDto
    (
        int Id,
        [Required] string Name,
        [Required] string Category,
        [Required] string Description,
        [Required, Range(1, int.MaxValue)] int Quantity,
        [Required, DataType(DataType.Currency)] decimal Price
        );
}
