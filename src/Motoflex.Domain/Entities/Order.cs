using System.Text.Json.Serialization;

namespace Motoflex.Domain.Entities
{
    public class Order(decimal deliveryFee) : BaseEntity
    {
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public decimal DeliveryFee { get; set; } = deliveryFee;
        public Status Status { get; set; } = Status.Available;
        public Guid? RenterId { get; set; }

        [JsonIgnore]
        public virtual Renter Renter { get; set; } = null!;
        [JsonIgnore]
        public virtual ICollection<Renter> NotifiedRenters { get; set; } = []; // new List<Renter>(); 
    }

    public enum Status
    {
        Available,
        Accepted,
        Delivered,
    }
}
