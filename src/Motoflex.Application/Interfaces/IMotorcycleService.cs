using Motoflex.Domain.Entities;

namespace Motoflex.Application.Interfaces
{
    public interface IMotorcycleService
    {
        Task<IEnumerable<Motorcycle>> GetByLicensePlateAsync(string licensePlate);
        Task<IEnumerable<Motorcycle>> GetAvailableAsync();
        Task<bool> InsertMotorcycleAsync(Motorcycle motorcycle);
        Task<Motorcycle?> UpdateMotorcycleLicensePlateAsync(Guid id, string licensePlate);
        Task UpdateMotorcycleAsync(Motorcycle motorcycle);
        Task<bool> DeleteMotorcycleAsync(Guid id);

        // IEnumerable<Motorcycle> GetByLicensePlate(string licensePlate);
        // IEnumerable<Motorcycle> GetAvailable();
        // Task InsertMotorcycleAsync(Motorcycle motorcycle);
        // Motorcycle? UpdateMotorcycleLicensePlate(Guid id, string licensePlate);
        // void UpdateMotorcycle(Motorcycle moto);
        // void DeleteMotorcycle(Guid id);
    }
}
