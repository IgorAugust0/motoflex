using System.ComponentModel.DataAnnotations;

namespace Motoflex.Application.DTOs.Requests
{
    public sealed class CreateOrder
    {
        [Required(ErrorMessage = "Valor da corrida é obrigatório")]
        public required decimal DeliveryFee { get; init; }
    }
}
