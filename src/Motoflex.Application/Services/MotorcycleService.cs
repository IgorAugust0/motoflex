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

        public IEnumerable<Motorcycle> GetByLicensePlate(string licensePlate)
        {
            if (string.IsNullOrWhiteSpace(licensePlate))
            {
                _notificationContext.AddNotification("License plate cannot be empty");
                return [];
            }

            return _repository.GetByLicensePlate(licensePlate);
        }

        public IEnumerable<Motorcycle> GetAvailable()
        {
            var motorcycles = _repository.Get();
            return motorcycles.Where(m => m.Available);
        }

        public async Task InsertMotorcycleAsync(Motorcycle motorcycle)
        {
            if (motorcycle == null)
            {
                _logger.LogError("Motorcycle object is null");
                throw new ArgumentNullException(nameof(motorcycle));
            }

            try
            {
                if (IsLicensePlateUsed(motorcycle.LicensePlate))
                {
                    _notificationContext.AddNotification(ErrorNotifications.LicensePlateAlreadyExists);
                    return;
                }

                await _repository.InsertAsync(motorcycle);
                _logger.LogInformation($"Motorcycle inserted successfully. Id: {motorcycle.Id}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while inserting motorcycle");
                _notificationContext.AddNotification("Unexpected error occurred while inserting motorcycle");
            }
        }

        // TODO: fix CS8603 / CA2254
        public Motorcycle? UpdateMotorcycleLicensePlate(Guid id, string licensePlate)
        {
            if (id == Guid.Empty || string.IsNullOrWhiteSpace(licensePlate))
            {
                _notificationContext.AddNotification("Invalid input parameters");
                return null;
            }

            try
            {
                var motorcycle = GetMotorcycleById(id);
                if (motorcycle == null) return null;

                if (IsLicensePlateUsed(licensePlate))
                {
                    _notificationContext.AddNotification(ErrorNotifications.LicensePlateAlreadyExists);
                    return null;
                }

                _logger.LogInformation("MotorcycleId:{MotorcycleId}. LicensePlate from {OldLicensePlate} to {NewLicensePlate}", motorcycle.Id, motorcycle.LicensePlate, licensePlate);

                motorcycle.LicensePlate = licensePlate;
                UpdateMotorcycle(motorcycle);

                return motorcycle;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating motorcycle license plate. Id: {Id}", id);
                _notificationContext.AddNotification("Unexpected error occurred while updating motorcycle");
                return null;
            }
        }

        public void UpdateMotorcycle(Motorcycle motorcycle)
        {
            ArgumentNullException.ThrowIfNull(motorcycle, nameof(motorcycle));
            _repository.UpdateAsync(motorcycle);
        }

        public void DeleteMotorcycle(Guid id)
        {
            if (id == Guid.Empty)
            {
                _notificationContext.AddNotification("Invalid motorcycle ID");
                return;
            }

            try
            {
                var motorcycle = _repository.GetRentals().Where(m => m.Id == id).FirstOrDefault();
                if (motorcycle == null)
                {
                    _notificationContext.AddNotification(ErrorNotifications.MotorcycleNotFound);
                    return;
                }

                if (motorcycle.Rentals.Count != 0)
                {
                    _notificationContext.AddNotification(ErrorNotifications.MotorcycleHasRentalHistory);
                    return;
                }

                _repository.DeleteAsync(id);
                _logger.LogInformation("MotorcycleId:{MotorcycleId}. Deleted.", motorcycle.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting motorcycle. Id: {id}");
                _notificationContext.AddNotification("Unexpected error occurred while deleting motorcycle");
            }
        }

        private bool IsLicensePlateUsed(string licensePlate)
        {
            var existingMotorcycles = GetByLicensePlate(licensePlate);
            return existingMotorcycles.Any();
        }

        private Motorcycle GetMotorcycleById(Guid id)
        {
            var motorcycle = _repository.Get(id).FirstOrDefault();
            if (motorcycle == null)
            {
                _notificationContext.AddNotification(ErrorNotifications.MotorcycleNotFound);
                throw new KeyNotFoundException($"Motorcycle with id {id} not found");
            }
            return motorcycle;
        }
    }
}
