﻿
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
        public const string ImageRequired = "É necessário enviar uma imagem";
        public const string InvalidId = "ID inválido";
        public const string AuthenticationError = "Erro ao processar autenticação";

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
    }
}

