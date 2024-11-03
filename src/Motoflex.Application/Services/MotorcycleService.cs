using Motoflex.Application.Interfaces;
using Motoflex.Domain.Interfaces.Repositories;
using Motoflex.Domain.Interfaces.Notifications;
using Microsoft.Extensions.Logging;
using Motoflex.Domain.Entities;
using Motoflex.Domain.Utilities;

namespace Motoflex.Application.Services
{
    public class MotorcycleService : IMotorcycleService
    {
        private readonly IMotoRepository _repository;
        private readonly INotificationContext _notificationContext;
        private readonly ILogger<MotorcycleService> _logger;

        public MotorcycleService(IMotoRepository repository, INotificationContext notificationContext, ILogger<MotorcycleService> logger)
        {
            ArgumentNullException.ThrowIfNull(repository, nameof(repository));
            ArgumentNullException.ThrowIfNull(notificationContext, nameof(notificationContext));
            ArgumentNullException.ThrowIfNull(logger, nameof(logger));
            _repository = repository;
            _notificationContext = notificationContext;
            _logger = logger;
        }

        public async Task<IEnumerable<Motorcycle>> GetByLicensePlateAsync(string licensePlate)
        {
            if (string.IsNullOrWhiteSpace(licensePlate))
            {
                _notificationContext.AddNotification("License plate cannot be empty");
                return [];
            }

            return await _repository.GetByLicensePlateAsync(licensePlate);
        }

        public async Task<IEnumerable<Motorcycle>> GetAvailableAsync()
        {
            var motorcycles = await _repository.GetAsync();
            return motorcycles.Where(m => m.Available);
        }

        public async Task<bool> InsertMotorcycleAsync(Motorcycle motorcycle)
        {
            if (motorcycle == null)
            {
                _logger.LogError("Motorcycle object is null");
                throw new ArgumentNullException(nameof(motorcycle));
            }

            try
            {
                if (await IsLicensePlateUsedAsync(motorcycle.LicensePlate))
                {
                    _notificationContext.AddNotification(ErrorNotifications.LicensePlateAlreadyExists);
                    return false;
                }

                await _repository.InsertAsync(motorcycle);
                _logger.LogInformation("Motorcycle inserted successfully. Id: {MotorcycleId}", motorcycle.Id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while inserting motorcycle");
                _notificationContext.AddNotification("Unexpected error occurred while inserting motorcycle");
                return false;
            }
        }

        public async Task<Motorcycle?> UpdateMotorcycleLicensePlateAsync(Guid id, string licensePlate)
        {
            if (id == Guid.Empty || string.IsNullOrWhiteSpace(licensePlate))
            {
                _notificationContext.AddNotification("Invalid input parameters");
                return null;
            }

            try
            {
                var motorcycle = await GetMotorcycleByIdAsync(id);
                if (motorcycle == null) return null;

                if (await IsLicensePlateUsedAsync(licensePlate))
                {
                    _notificationContext.AddNotification(ErrorNotifications.LicensePlateAlreadyExists);
                    return null;
                }

                _logger.LogInformation("MotorcycleId:{MotorcycleId}. LicensePlate from {OldLicensePlate} to {NewLicensePlate}", motorcycle.Id, motorcycle.LicensePlate, licensePlate);

                motorcycle.LicensePlate = licensePlate;
                await UpdateMotorcycleAsync(motorcycle);

                return motorcycle;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating motorcycle license plate. Id: {Id}", id);
                _notificationContext.AddNotification("Unexpected error occurred while updating motorcycle");
                return null;
            }
        }

        public async Task UpdateMotorcycleAsync(Motorcycle motorcycle)
        {
            ArgumentNullException.ThrowIfNull(motorcycle, nameof(motorcycle));
            await _repository.UpdateAsync(motorcycle);
        }

        public async Task<bool> DeleteMotorcycleAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                _notificationContext.AddNotification("Invalid motorcycle ID");
                return false;
            }

            try
            {
                // var rentals = await _repository.GetRentalsAsync();
                // var motorcycle = rentals.Where(m => m.Id == id).FirstOrDefault();
                var motorcycle = await _repository.GetWithRentalsAsync(id);
                if (motorcycle == null)
                {
                    _notificationContext.AddNotification(ErrorNotifications.MotorcycleNotFound);
                    return false;
                }

                if (motorcycle.Rentals.Count != 0)
                {
                    _notificationContext.AddNotification(ErrorNotifications.MotorcycleHasRentalHistory);
                    return false;
                }

                await _repository.DeleteAsync(id);
                _logger.LogInformation("MotorcycleId:{MotorcycleId}. Deleted.", motorcycle.Id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting motorcycle. Id: {id}");
                _notificationContext.AddNotification("Unexpected error occurred while deleting motorcycle");
                return false;
            }
        }

        private async Task<bool> IsLicensePlateUsedAsync(string licensePlate)
        {
            var existingMotorcycles = await GetByLicensePlateAsync(licensePlate);
            return existingMotorcycles.Any();
        }

        private async Task<Motorcycle> GetMotorcycleByIdAsync(Guid id)
        {
            var motorcycles = await _repository.GetByIdAsync(id);
            var motorcycle = motorcycles.FirstOrDefault();
            if (motorcycle == null)
            {
                _notificationContext.AddNotification(ErrorNotifications.MotorcycleNotFound);
                throw new KeyNotFoundException($"Motorcycle with id {id} not found");
            }
            return motorcycle;
        }
    }
}
