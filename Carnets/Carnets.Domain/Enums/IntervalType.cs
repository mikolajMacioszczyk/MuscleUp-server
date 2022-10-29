namespace Carnets.Domain.Enums
{
    // The same interval as defined on Stripe
    // https://stripe.com/docs/api/prices/object#price_object-recurring-interval
    public enum IntervalType
    {
        Day,
        Week,
        Month,
        Year
    }

    public static class IntervalTypeExtensions
    {
        public static DateTime AddToDate(this IntervalType interval, DateTime baseDate, int intervalCount)
        {
            switch (interval)
            {
                case IntervalType.Day:
                    return baseDate.AddDays(intervalCount);
                case IntervalType.Week:
                    return baseDate.AddDays(7 * intervalCount);
                case IntervalType.Month:
                    return baseDate.AddMonths(intervalCount);
                case IntervalType.Year:
                    return baseDate.AddYears(intervalCount);
                default:
                    throw new NotImplementedException(interval.ToString());
            }
        }
    }
}
