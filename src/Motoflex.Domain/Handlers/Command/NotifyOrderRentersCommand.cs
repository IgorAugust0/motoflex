namespace Motoflex.Domain.Handlers.Command
{
    public class NotifyOrderRentersCommand(Guid orderId)
    {
        public Guid OrderId { get; set; } = orderId;
    }
}
