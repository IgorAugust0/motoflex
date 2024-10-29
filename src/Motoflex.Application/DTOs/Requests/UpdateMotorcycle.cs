using System.ComponentModel.DataAnnotations;

namespace Motoflex.Application.DTOs.Requests
{
    public class UpdateMotorcycle
    {
        [Required(ErrorMessage = "A placa é obrigatória")]
        [Length(7, 7, ErrorMessage = "A placa deve ter 7 caracteres")]
        public string? LicensePlate { get; set; }
    }
}
