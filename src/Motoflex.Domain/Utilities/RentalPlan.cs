namespace Motoflex.Domain.Utilities
{
    public interface IRentalPlan
    {
        decimal DailyRate { get; }
        decimal EarlyTerminationRate { get; }
    }

    public abstract class RentalPlanBase(decimal dailyRate, decimal terminationRate) : IRentalPlan
    {
        public decimal DailyRate { get; } = dailyRate;
        public decimal EarlyTerminationRate { get; } = terminationRate;
    }

    public class SevenDayPlan : RentalPlanBase
    {
        public SevenDayPlan() : base(30m, 0.2m) { }
    }

    public class FifteenDayPlan : RentalPlanBase
    {
        public FifteenDayPlan() : base(28m, 0.4m) { }
    }

    public class ThirtyDayPlan : RentalPlanBase
    {
        public ThirtyDayPlan() : base(22m, 0.6m) { }
    }

    public class FortyFiveDayPlan : RentalPlanBase
    {
        public FortyFiveDayPlan() : base(20m, 0.8m) { }
    }

    public class FiftyDayPlan : RentalPlanBase
    {
        public FiftyDayPlan() : base(18m, 1m) { }
    }
}
