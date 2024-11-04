using Motoflex.Domain.Entities;

namespace Motoflex.Application.Interfaces
{
    public interface IMotorcycleService
    {
        Task<IEnumerable<Motorcycle>> GetByLicensePlateAsync(string licensePlate);
        Task<IEnumerable<Motorcycle>> GetAvailableAsync();
        Task<Motorcycle> GetMotorcycleByIdAsync(Guid id);
        Task<bool> InsertMotorcycleAsync(Motorcycle motorcycle);
        Task<Motorcycle?> UpdateMotorcycleLicensePlateAsync(Guid id, string licensePlate);
        Task UpdateMotorcycleAsync(Motorcycle motorcycle);
        Task<bool> DeleteMotorcycleAsync(Guid id);
    }
}
