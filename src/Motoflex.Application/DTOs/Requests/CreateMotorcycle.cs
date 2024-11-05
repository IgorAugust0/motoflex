using System.ComponentModel.DataAnnotations;

namespace Motoflex.Application.DTOs.Requests
{
    public sealed class CreateMotorcycle
    {
        [Required(ErrorMessage = "O ano é obrigatório")]
        public int Year { get; init; }

        [Required(ErrorMessage = "O modelo é obrigatório")]
        public string? Model { get; init; }

        [Required(ErrorMessage = "A placa é obrigatória")]
        [Length(7, 7, ErrorMessage = "A placa deve ter 7 caracteres")]
        public string? LicensePlate { get; init; }
    }
}
