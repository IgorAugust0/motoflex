using Motoflex.Domain.Entities;

namespace Motoflex.Domain.Utilities
{
    public static class RentalPriceCalculator
    {
        // Fixed daily rate applied for days after the rental finish date
        private const decimal _dailyPriceAfterFinish = 50;

        public static decimal Calculate(Rental rental)
        {
            // Retrieve the rate plan details based on the rental's selected plan
            var plan = GetRentalPlan(rental.Plan);
            var returnDate = rental.ReturnAt.Date;
            var beginDate = rental.BeginAt.Date;
            var finishDate = rental.FinishAt.Date;

            // Calculate total rental days, days rented before finish, and days after finish
            int rentalDays = (finishDate - beginDate).Days + 1;
            int daysBeforeFinish = (returnDate - beginDate).Days + 1;
            int daysAfterFinish = (returnDate - finishDate).Days;

            // Determine price based on whether returned before, on, or after the finish date
            return returnDate < finishDate
                ? CalculatePriceBeforeFinishDate()
                : returnDate > finishDate
                    ? CalculatePriceAfterFinishDate()
                    : CalculatePriceOnFinishDate();

            // Calculates rental cost if returned on the finish date
            decimal CalculatePriceOnFinishDate() =>
                rentalDays * plan.DailyRate;

            // Calculates cost if returned before the finish date, with early termination rate
            decimal CalculatePriceBeforeFinishDate() =>
                daysBeforeFinish * plan.DailyRate +
                (rentalDays - daysBeforeFinish) * plan.DailyRate * plan.EarlyTerminationRate;

            // Calculates cost if returned after the finish date, with additional late fees
            decimal CalculatePriceAfterFinishDate() =>
                rentalDays * plan.DailyRate +
                daysAfterFinish * _dailyPriceAfterFinish;
        }

        // Retrieves the appropriate rental plan object based on the rental's selected plan type
        private static IRentalPlan GetRentalPlan(Plan plan) => plan switch
        {
            Plan.SevenDays => new SevenDayPlan(),
            Plan.FifteenDays => new FifteenDayPlan(),
            Plan.ThirtyDays => new ThirtyDayPlan(),
            Plan.FortyFiveDays => new FortyFiveDayPlan(),
            Plan.FiftyDays => new FiftyDayPlan(),
            _ => throw new ArgumentException("Unknown plan type.", nameof(plan))
        };
    }
}
