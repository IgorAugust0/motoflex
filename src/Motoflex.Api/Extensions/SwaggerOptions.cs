namespace Motoflex.Api.Extensions
{
    /// <summary>
    /// Configuration options for Swagger documentation.
    /// </summary>
    /// <remarks>
    /// This class contains configuration settings used to customize the Swagger/OpenAPI documentation.
    /// </remarks>
    public sealed class SwaggerOptions
    {
        public const string SectionName = "Swagger";
        public bool EnableAuthentication { get; init; } = true;
        public string Title { get; init; } = "Motoflex API";
        public string Version { get; init; } = "v1";
        public string Description { get; init; } = "API para gerenciar aluguel de motos e entregadores";
    }
}
