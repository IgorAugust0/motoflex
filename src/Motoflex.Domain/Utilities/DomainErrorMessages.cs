
namespace Motoflex.Domain.Utilities
{
    public static class ErrorNotifications
    {
        // Resource Not Found Messages
        public const string MotorcycleNotFound = "Moto não encontrada";
        public const string OrderNotFound = "Pedido não encontrado";
        public const string RenterNotFound = "Entregador não encontrado";
        public const string RentalNotFound = "Locação não encontrada";
        public const string NoMotorcyclesAvailable = "Nenhuma moto disponivel";

        // Resource Already Exists Messages
        public const string LicensePlateAlreadyExists = "Placa já utilizada";
        public const string DriversLicenseAlreadyExists = "CNH já utilizada";
        public const string TaxIdAlreadyExists = "CNPJ já utilizado";

        // Validation Messages
        public const string InvalidImageFormat = "A imagem deve ser do tipo png ou bmp";
        public const string ImageRequired = "Necessário enviar uma imagem";

        // Business Rule Violation Messages
        public const string MotorcycleHasRentalHistory = "Moto já alugada anteriormente";
        public const string RenterHasActiveRental = "Entregador já possui uma locação ativa";
        public const string RenterLacksMotorcycleLicense = "Entregador não possui categoria A";
        public const string RentalNotOwnedByRenter = "Locação não pertence ao entregador";
        public const string RenterNotNotified = "Entregador não recebeu notificação";
        public const string OrderNotAvailable = "Pedido não está com status Disponivel";
        public const string OrderNotOwnedByRenter = "Pedido não pertence ao entregador";
        public const string OrderStatusInvalid = "Pedido ainda não foi aceito ou já foi entregue";
        public const string RentalAlreadyInactive = "A locação já foi finalizada";


        // public const string MotorcycleNotFound = "Motorcycle not found";
        // public const string OrderNotFound = "Order not found";
        // public const string RenterNotFound = "Renter not found";
        // public const string RentalNotFound = "Rental not found";
        // public const string NoMotorcyclesAvailable = "No motorcycles available";

        // public const string LicensePlateAlreadyExists = "License plate already exists";
        // public const string DriversLicenseAlreadyExists = "Driver's license already exists";
        // public const string TaxIdAlreadyExists = "Tax ID already exists";

        // public const string InvalidImageFormat = "The image must be in png or bmp format";
        // public const string ImageRequired = "An image is required";

        // public const string MotorcycleHasRentalHistory = "Motorcycle has rental history";
        // public const string RenterHasActiveRental = "Renter already has an active rental";
        // public const string RenterLacksMotorcycleLicense = "Renter does not have a motorcycle license";
        // public const string RentalNotOwnedByRenter = "Rental is not owned by the renter";
        // public const string RenterNotNotified = "Renter was not notified";
        // public const string OrderNotAvailable = "Order is not available";
        // public const string OrderNotOwnedByRenter = "Order is not owned by the renter";
        // public const string OrderStatusInvalid = "Order has not been accepted or has already been delivered";
        // public const string RentalAlreadyInactive = "The rental has already been completed";
    }
}

