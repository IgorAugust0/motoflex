using Motoflex.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Motoflex.Application.DTOs.Requests
{
    public sealed class CreateRenter
    {
        [Required(ErrorMessage = "O nome do usuário é obrigatório")]
        public required string Name { get; init; }

        [Required(ErrorMessage = "O CNPJ é obrigatório")]
        [RegularExpression(@"^\d{14}$", ErrorMessage = "CNPJ deve ter 14 digitos")]
        public required string Cnpj { get; init; }

        [Required(ErrorMessage = "A data de nascimento é obrigatória")]
        public required DateTime BirthDate { get; init; }

        [Required(ErrorMessage = "A CNH é obrigatória")]
        [RegularExpression(@"^\d{12}$", ErrorMessage = "CNH deve ter 12 digitos")]
        public required string Cnh { get; init; }

        [Required(ErrorMessage = "O tipo de CNH é obrigatório")]
        [RegularExpression(@"^[a-eA-E]{1,5}$", ErrorMessage = "Tipo de CNH pode ter no máximo 5 caracteres de 'A' a 'E'")]
        public required CnhType CnhType { get; init; }
    }
}
