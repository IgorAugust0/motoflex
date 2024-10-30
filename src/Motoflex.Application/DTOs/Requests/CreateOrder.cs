using System.ComponentModel.DataAnnotations;

namespace Motoflex.Application.DTOs.Requests
{
    public class CreateOrder
    {
        [Required(ErrorMessage = "Valor da corrida é obrigatório")]
        public required decimal DeliveryFee { get; set; }
    }
}
