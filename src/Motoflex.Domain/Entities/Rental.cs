using System.Text.Json.Serialization;

namespace Motoflex.Domain.Entities
{
    public class Rental : BaseEntity
    {
        public Plan Plan { get; set; }
        public Guid RenterId { get; set; }
        public Guid MotorcycleId { get; set; }
        public DateTime BeginAt { get; set; }
        public DateTime FinishAt { get; set; }
        public DateTime ReturnAt { get; set; }
        public bool Active { get; set; }

        [JsonIgnore]
        public virtual Motorcycle Motorcycle { get; set; } = null!;
        [JsonIgnore]
        public virtual Renter Renter { get; set; } = null!;

        public Rental(Plan plan, Guid renterId) : base()
        {
            Plan = plan;
            RenterId = renterId;
            BeginAt = DateTime.Now.AddDays(1);
            Active = true;
            SetFinishAt();
        }

        private void SetFinishAt()
        {
            FinishAt = Plan switch
            {
                Plan.SevenDays => BeginAt.AddDays(6),
                Plan.FifteenDays => BeginAt.AddDays(14),
                Plan.ThirtyDays => BeginAt.AddDays(29),
                Plan.FortyFiveDays => BeginAt.AddDays(44),
                Plan.FiftyDays => BeginAt.AddDays(49),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }

    public enum Plan
    {
        SevenDays,
        FifteenDays,
        ThirtyDays,
        FortyFiveDays,
        FiftyDays
    }
}
