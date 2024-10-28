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
                Plan.A => BeginAt.AddDays(6),
                Plan.B => BeginAt.AddDays(14),
                Plan.C => BeginAt.AddDays(29),
                Plan.D => BeginAt.AddDays(44),
                Plan.E => BeginAt.AddDays(49),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }

    public enum Plan
    {
        A,
        B,
        C,
        D,
        E
    }
}
