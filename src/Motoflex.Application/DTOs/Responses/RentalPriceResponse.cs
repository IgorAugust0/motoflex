namespace Motoflex.Application.DTOs.Responses
{
    public class RentalPriceResponse(decimal amount)
    {
        public decimal Amount { get; } = amount;
    }
}
