namespace Motoflex.Domain.Entities
{
    public class Order(decimal deliveryFee) : BaseEntity
    {
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public decimal DeliveryFee { get; set; } = deliveryFee;
        public Status Status { get; set; } = Status.Available;
        public Guid? CourierId { get; set; }
    }

    public enum Status
    {
        Available,
        Accepted,
        Delivered,
    }
}
