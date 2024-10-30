namespace Motoflex.Application.DTOs.Responses
{
    public class AuthenticationToken(string token)
    {
        public string Token { get; init; } = token; // immutable after initialization
    }
}
