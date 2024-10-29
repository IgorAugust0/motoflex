using System.ComponentModel.DataAnnotations;

namespace Motoflex.Application.DTOs.Requests
{
    public class CreateMotorcycle
    {
        [Required(ErrorMessage = "O ano é obrigatório")]
        public int Year { get; set; }

        [Required(ErrorMessage = "O modelo é obrigatório")]
        public string? Model { get; set; }

        [Required(ErrorMessage = "A placa é obrigatória")]
        [Length(7, 7, ErrorMessage = "A placa deve ter 7 caracteres")]
        public string? LicensePlate { get; set; }
    }
}
