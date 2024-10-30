using System.ComponentModel.DataAnnotations;

namespace Motoflex.Application.DTOs.Requests
{
    public class UpdateReturnDate
    {
        [Required(ErrorMessage = "Data de devolução é obrigatória")]
        public required DateTime ReturnDate { get; set; }
    }
}
