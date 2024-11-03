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

        public IEnumerable<Renter> Get()
        {
            return _repository.Get();
        }

        public Renter? Get(Guid id)
        {
            if (id == Guid.Empty)
            {
                _notificationContext.AddNotification("Invalid renter ID");
                return null;
            }
            return _repository.Get(id).FirstOrDefault();
        }

        public Renter? GetRentals(Guid id)
        {
            if (id == Guid.Empty)
            {
                _notificationContext.AddNotification("Invalid renter ID");
                return null;
            }
            var rentals = _repository.GetAllRentals();
            return rentals.FirstOrDefault(e => e.Id == id);
        }

        public Task InsertRenterAsync(Renter renter)
        {
            if (renter == null)
            {
                _logger.LogError("Renter object is null");
                throw new ArgumentNullException(nameof(renter));
            }

            try
            {
                if (!ValidateNewRenter(renter))
                {
                    return Task.FromResult(false);
                }

                _repository.InsertAsync(renter);
                _logger.LogInformation("Renter created successfully. Id: {RenterId}", renter.Id);
                return Task.FromResult(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating renter");
                _notificationContext.AddNotification("Unexpected error occurred while creating renter");
                return Task.FromResult(false);
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
                var renter = ValidateRenterImageUpdate(id, image);
                if (renter == null) return null;

                var imageUrl = await UploadRenterImage(renter, image);
                if (string.IsNullOrEmpty(imageUrl))
                    return null;

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

        private bool ValidateNewRenter(Renter renter)
        {
            var existingRenters = Get();

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

        private Renter? ValidateRenterImageUpdate(Guid id, File image)
        {
            var renter = Get(id);
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

        private async Task<string?> UploadRenterImage(Renter renter, File image)
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
