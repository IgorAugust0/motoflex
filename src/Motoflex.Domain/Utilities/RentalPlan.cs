namespace Motoflex.Domain.Utilities
{
    public interface IRentalPlan
    {
        decimal DailyRate { get; }
        decimal EarlyTerminationRate { get; }
    }

    public class SevenDayPlan : IRentalPlan
    {
        private const decimal DAILY_RATE = 30m;
        private const decimal TERMINATION_RATE = 0.2m;

        public decimal DailyRate => DAILY_RATE;
        public decimal EarlyTerminationRate => TERMINATION_RATE;
    }

    public class FifteenDayPlan : IRentalPlan
    {
        private const decimal DAILY_RATE = 28m;
        private const decimal TERMINATION_RATE = 0.4m;

        public decimal DailyRate => DAILY_RATE;
        public decimal EarlyTerminationRate => TERMINATION_RATE;
    }

    public class ThirtyDayPlan : IRentalPlan
    {
        private const decimal DAILY_RATE = 22m;
        private const decimal TERMINATION_RATE = 0.6m;

        public decimal DailyRate => DAILY_RATE;
        public decimal EarlyTerminationRate => TERMINATION_RATE;
    }

    public class FortyFiveDayPlan : IRentalPlan
    {
        private const decimal DAILY_RATE = 20m;
        private const decimal TERMINATION_RATE = 0.8m;

        public decimal DailyRate => DAILY_RATE;
        public decimal EarlyTerminationRate => TERMINATION_RATE;
    }

    public class FiftyDayPlan : IRentalPlan
    {
        private const decimal DAILY_RATE = 18m;
        private const decimal TERMINATION_RATE = 1m;

        public decimal DailyRate => DAILY_RATE;
        public decimal EarlyTerminationRate => TERMINATION_RATE;
    }
}
