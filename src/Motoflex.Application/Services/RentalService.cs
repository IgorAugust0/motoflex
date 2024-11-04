using Microsoft.Extensions.Logging;
using Motoflex.Application.Interfaces;
using Motoflex.Domain.Entities;
using Motoflex.Domain.Interfaces.Notifications;
using Motoflex.Domain.Interfaces.Repositories;
using Motoflex.Domain.Utilities;

namespace Motoflex.Application.Services
{
    public class RentalService : IRentalService
    {
        private readonly IRentalRepository _repository;
        private readonly IRenterService _renterService;
        private readonly IMotorcycleService _motorcycleService;
        private readonly INotificationContext _notificationContext;
        private readonly ILogger<RentalService> _logger;

        public RentalService(
            IRentalRepository repository,
            IRenterService renterService,
            IMotorcycleService motorcycleService,
            INotificationContext notificationContext,
            ILogger<RentalService> logger)
        {
            ArgumentNullException.ThrowIfNull(repository, nameof(repository));
            ArgumentNullException.ThrowIfNull(renterService, nameof(renterService));
            ArgumentNullException.ThrowIfNull(motorcycleService, nameof(motorcycleService));
            ArgumentNullException.ThrowIfNull(notificationContext, nameof(notificationContext));
            ArgumentNullException.ThrowIfNull(logger, nameof(logger));
            _repository = repository;
            _renterService = renterService;
            _motorcycleService = motorcycleService;
            _notificationContext = notificationContext;
            _logger = logger;
        }

        public async Task<Rental?> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                _notificationContext.AddNotification("Invalid rental ID");
                return null;
            }

            try
            {
                var rental = (await _repository.GetByIdAsync(id)).FirstOrDefault();

                if (rental == null)
                {
                    _notificationContext.AddNotification(ErrorNotifications.RentalNotFound);
                }

                return rental;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving rental. RentalId: {RentalId}", id);
                _notificationContext.AddNotification("Unexpected error occurred while retrieving rental");
                return null;
            }
        }

        public async Task<bool> InsertRentalAsync(Rental rental)
        {
            if (rental == null)
            {
                _logger.LogError("Rental object is null");
                throw new ArgumentNullException(nameof(rental));
            }

            try
            {
                if (!await ValidateRentalRequestAsync(rental)) return false;

                var motorcycle = await AssignMotorcycleAsync();
                if (motorcycle == null) return false;

                await CompleteRentalAssignmentAsync(rental, motorcycle);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while inserting rental");
                _notificationContext.AddNotification("Unexpected error occurred while processing rental");
                return false;
            }
        }

        public async Task<decimal> ReportReturnAsync(Guid id, DateTime returnDate, Guid renterId)
        {
            if (id == Guid.Empty || renterId == Guid.Empty)
            {
                _notificationContext.AddNotification("Invalid ID provided");
                return 0;
            }

            try
            {
                var rental = (await _repository.GetByIdAsync(id)).FirstOrDefault();
                if (rental == null || !ValidateReturn(rental, renterId)) return 0;

                return await ProcessReturnAsync(rental, returnDate);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while processing return");
                _notificationContext.AddNotification("Unexpected error occurred while processing return");
                return 0;
            }
        }

        private async Task<bool> ValidateRentalRequestAsync(Rental rental)
        {
            var renter = await _renterService.GetRentalsAsync(rental.RenterId);
            if (renter == null)
            {
                _notificationContext.AddNotification(ErrorNotifications.RenterNotFound);
                return false;
            }

            if (renter.Rentals.Any(l => l.Active))
            {
                _notificationContext.AddNotification(ErrorNotifications.RenterHasActiveRental);
                return false;
            }

            if (renter.CnhType == CnhType.B)
            {
                _notificationContext.AddNotification(ErrorNotifications.RenterLacksMotorcycleLicense);
                return false;
            }

            return true;
        }

        private async Task<Motorcycle?> AssignMotorcycleAsync()
        {
            var motorcycle = (await _motorcycleService.GetAvailableAsync()).FirstOrDefault();
            // var motorcycle = motorcycles.FirstOrDefault();
            if (motorcycle == null)
            {
                _notificationContext.AddNotification(ErrorNotifications.NoMotorcyclesAvailable);
                _logger.LogInformation(ErrorNotifications.NoMotorcyclesAvailable);
                return null;
            }
            return motorcycle;
        }

        private async Task CompleteRentalAssignmentAsync(Rental rental, Motorcycle motorcycle)
        {
            motorcycle.Available = false;
            await _motorcycleService.UpdateMotorcycleAsync(motorcycle);

            rental.Motorcycle = motorcycle;
            await _repository.InsertAsync(rental);
        }

        private bool ValidateReturn(Rental rental, Guid renterId)
        {
            if (rental == null)
            {
                _notificationContext.AddNotification(ErrorNotifications.RentalNotFound);
                return false;
            }
            if (renterId != rental.RenterId)
            {
                _notificationContext.AddNotification(ErrorNotifications.RentalNotOwnedByRenter);
                return false;
            }
            if (!rental.Active)
            {
                _notificationContext.AddNotification(ErrorNotifications.RentalAlreadyInactive);
                return false;
            }
            return true;
        }

        private async Task<decimal> ProcessReturnAsync(Rental rental, DateTime returnDate)
        {
            rental.ReturnAt = returnDate;
            rental.Active = false;
            rental.Motorcycle.Available = true;

            await _repository.UpdateAsync(rental);
            return RentalPriceCalculator.Calculate(rental);
        }
    }
}
