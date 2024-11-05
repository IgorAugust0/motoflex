using Motoflex.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Motoflex.Application.DTOs.Requests
{
    public sealed class CreateRental
    {
        [Required(ErrorMessage = "O tipo de plano é obrigatório")]
        public required Plan Plan { get; init; }
    }
}
