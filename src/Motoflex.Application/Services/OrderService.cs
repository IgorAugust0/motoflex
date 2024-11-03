using Motoflex.Domain.Entities;
using Motoflex.Domain.Interfaces.Messaging;
using Motoflex.Domain.Interfaces.Repositories;
using Motoflex.Domain.Utilities;
using Microsoft.Extensions.Logging;
using Motoflex.Domain.Handlers.Command;
using Motoflex.Application.Interfaces;
using Motoflex.Domain.Interfaces.Notifications;

namespace Motoflex.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _repository;
        private readonly IPublisher<NotifyOrderRentersCommand> _publisherNotification;
        private readonly IRenterService _renterService;
        private readonly INotificationContext _notificationContext;
        private readonly ILogger<OrderService> _logger;

        public OrderService(
            IOrderRepository repository,
            IPublisher<NotifyOrderRentersCommand> publisherNotification,
            IRenterService renterService,
            INotificationContext notificationContext,
            ILogger<OrderService> logger)
        {
            ArgumentNullException.ThrowIfNull(repository, nameof(repository));
            ArgumentNullException.ThrowIfNull(publisherNotification, nameof(publisherNotification));
            ArgumentNullException.ThrowIfNull(renterService, nameof(renterService));
            ArgumentNullException.ThrowIfNull(notificationContext, nameof(notificationContext));
            ArgumentNullException.ThrowIfNull(logger, nameof(logger));
            _repository = repository;
            _publisherNotification = publisherNotification;
            _renterService = renterService;
            _notificationContext = notificationContext;
            _logger = logger;
        }

        public async Task<Order?> GetNotifiedOrdersAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                _notificationContext.AddNotification("Invalid order ID");
                return null;
            }
            return await _repository.GetNotifiedOrdersAsync(id);
        }

        public IEnumerable<Order> Get()
        {
            return _repository.Get();
        }

        private async Task<Order?> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty) return null;
            return await _repository.GetByIdAsync(id);
        }

        public async Task<Order?> InsertOrderAsync(Order order)
        {
            if (order == null)
            {
                _logger.LogError("Order object is null");
                throw new ArgumentNullException(nameof(order));
            }

            try
            {
                await _repository.InsertAsync(order);
                
                var command = new NotifyOrderRentersCommand(order.Id);
                await _publisherNotification.PublishAsync(command);
                
                _logger.LogInformation($"Order created successfully. Id: {order.Id}");
                return order;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating order");
                _notificationContext.AddNotification("Unexpected error occurred while creating order");
                return null;
            }
        }

        private async Task UpdateOrderAsync(Order order)
        {
            ArgumentNullException.ThrowIfNull(order);
            await _repository.UpdateAsync(order);
        }

        public async Task<bool> AcceptOrderAsync(Guid id, Guid renterId)
        {
            if (id == Guid.Empty || renterId == Guid.Empty)
            {
                _notificationContext.AddNotification("Invalid input parameters");
                return false;
            }

            try
            {
                var (isValid, order, renter) = await ValidateOrderAcceptanceAsync(id, renterId);
                if (!isValid || order == null || renter == null) return false;

                order.Status = Status.Accepted;
                order.Renter = renter;
                await UpdateOrderAsync(order);

                _logger.LogInformation($"Order {id} accepted by renter {renterId}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error accepting order. OrderId: {id}, RenterId: {renterId}");
                _notificationContext.AddNotification("Unexpected error occurred while accepting order");
                return false;
            }
        }

        public async Task<bool> FinishOrderAsync(Guid id, Guid renterId)
        {
            if (id == Guid.Empty || renterId == Guid.Empty)
            {
                _notificationContext.AddNotification("Invalid input parameters");
                return false;
            }

            try
            {
                var order = await GetByIdAsync(id);
                if (!ValidateOrderDelivery(order, renterId)) return false;

                order.Status = Status.Delivered;
                await UpdateOrderAsync(order);

                _logger.LogInformation($"OrderId: {order.Id}. Delivered by renter: {renterId}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error finishing order. OrderId: {id}, RenterId: {renterId}");
                _notificationContext.AddNotification("Unexpected error occurred while finishing order");
                return false;
            }
        }

        private async Task<(bool isValid, Order? order, Renter? renter)> ValidateOrderAcceptanceAsync(Guid orderId, Guid renterId)
        {
            var renter = await _renterService.GetAsync(renterId);
            if (renter == null)
            {
                _notificationContext.AddNotification(ErrorNotifications.OrderNotFound);
                return (false, null, null);
            }

            var order = await GetNotifiedOrdersAsync(orderId);
            if (order == null)
            {
                _notificationContext.AddNotification(ErrorNotifications.OrderNotFound);
                return (false, null, null);
            }

            if (!order.Notifieds.Any(e => e.Id == renterId))
            {
                _notificationContext.AddNotification(ErrorNotifications.RenterNotNotified);
                return (false, null, null);
            }

            if (order.Status != Status.Available)
            {
                _notificationContext.AddNotification(ErrorNotifications.OrderNotAvailable);
                return (false, null, null);
            }

            return (true, order, renter);
        }

        private bool ValidateOrderDelivery(Order? order, Guid renterId)
        {
            if (order == null)
            {
                _notificationContext.AddNotification(ErrorNotifications.OrderNotFound);
                return false;
            }

            if (order.RenterId != renterId)
            {
                _notificationContext.AddNotification(ErrorNotifications.OrderNotOwnedByRenter);
                return false;
            }

            if (order.Status != Status.Accepted)
            {
                _notificationContext.AddNotification(ErrorNotifications.OrderStatusInvalid);
                return false;
            }

            return true;
        }
    }
}