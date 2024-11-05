using Motoflex.Domain.Entities;
using Motoflex.Domain.Interfaces.Repositories;
using Motoflex.Domain.Interfaces.Storage;
using Motoflex.Domain.Utilities;
using Microsoft.Extensions.Logging;
using Motoflex.Application.Interfaces;
using Motoflex.Domain.Interfaces.Notifications;
using File = Motoflex.Domain.Utilities.File;

namespace Motoflex.Application.Services
{
    public class RenterService : IRenterService
    {
        private readonly IRenterRepository _repository;
        private readonly INotificationContext _notificationContext;
        private readonly ILogger<RenterService> _logger;
        private readonly IStorage _storage;

        public RenterService(
            IRenterRepository repository,
            INotificationContext notificationContext,
            ILogger<RenterService> logger,
            IStorage storage)
        {
            ArgumentNullException.ThrowIfNull(repository, nameof(repository));
            ArgumentNullException.ThrowIfNull(notificationContext, nameof(notificationContext));
            ArgumentNullException.ThrowIfNull(logger, nameof(logger));
            ArgumentNullException.ThrowIfNull(storage, nameof(storage));

            _repository = repository;
            _notificationContext = notificationContext;
            _logger = logger;
            _storage = storage;
        }

        public async Task<IEnumerable<Renter>> GetAsync()
        {
            return await _repository.GetAsync();
        }

        public async Task<Renter?> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                _notificationContext.AddNotification("Invalid renter ID");
                return null;
            }
            var renter = await _repository.GetByIdAsync(id);
            return renter.SingleOrDefault();
        }

        public async Task<Renter?> GetRentalsAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                _notificationContext.AddNotification("Invalid renter ID");
                return null;
            }
            var rentals = await _repository.GetAllRentalsAsync();
            return rentals.FirstOrDefault(e => e.Id == id);
        }

        public async Task<bool> InsertRenterAsync(Renter renter)
        {
            if (renter == null)
            {
                _logger.LogError("Renter object is null");
                throw new ArgumentNullException(nameof(renter));
            }

            try
            {
                if (!await ValidateNewRenterAsync(renter)) return false;

                await _repository.InsertAsync(renter);
                _logger.LogInformation("Renter created successfully. Id: {RenterId}", renter.Id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating renter");
                _notificationContext.AddNotification("Unexpected error occurred while creating renter");
                return false;
            }
        }

        public async Task<Renter?> UpdateRenterCnhImageAsync(Guid id, File image)
        {
            if (id == Guid.Empty || image == null)
            {
                _notificationContext.AddNotification("Invalid input parameters");
                return null;
            }

            try
            {
                var renter = await ValidateRenterImageUpdateAsync(id, image);
                if (renter == null) return null;

                var imageUrl = await UploadRenterImageAsync(renter, image);
                if (string.IsNullOrEmpty(imageUrl)) return null;

                renter.CnhImage = imageUrl;
                await _repository.UpdateAsync(renter);

                _logger.LogInformation("Image updated for renter {RenterId}. New URL: {ImageUrl}", id, imageUrl);
                return renter;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating renter CNH image. RenterId: {RenterId}", id);
                _notificationContext.AddNotification("Unexpected error occurred while updating renter image");
                return null;
            }
        }

        private async Task<bool> ValidateNewRenterAsync(Renter renter)
        {
            var existingRenters = await GetAsync();

            var cnhUsed = existingRenters.Any(x => x.Cnh == renter.Cnh);
            var cnpjUsed = existingRenters.Any(x => x.Cnpj == renter.Cnpj);

            if (cnhUsed)
            {
                _notificationContext.AddNotification(ErrorNotifications.DriversLicenseAlreadyExists);
            }

            if (cnpjUsed)
            {
                _notificationContext.AddNotification(ErrorNotifications.TaxIdAlreadyExists);
            }

            return !_notificationContext.HasNotifications;
        }

        private async Task<Renter?> ValidateRenterImageUpdateAsync(Guid id, File image)
        {
            var renter = await GetByIdAsync(id);
            if (renter == null)
            {
                _notificationContext.AddNotification(ErrorNotifications.RenterNotFound);
                return null;
            }

            if (!IsValidImageType(image))
            {
                _notificationContext.AddNotification(ErrorNotifications.InvalidImageFormat);
                return null;
            }

            return renter;
        }

        private async Task<string?> UploadRenterImageAsync(Renter renter, File image)
        {
            try
            {
                var fileName = $"CNH-{renter.Id}.{image.Type}";
                var imageUrl = await _storage.UploadFile(image.Stream, fileName);

                _logger.LogInformation("RenterId:{RenterId} - CNH image updated from {OldImageUrl} to {NewImageUrl}", renter.Id, renter.CnhImage, imageUrl);
                return imageUrl;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading renter CNH image");
                _notificationContext.AddNotification("Error uploading image");
                return null;
            }
        }

        private static bool IsValidImageType(File image)
        {
            return image.Type.Contains("png") || image.Type.Contains("bmp");
        }
    }
}
