// Copyright = Karl Buhariwala
// ServiceMe App
// FileName = TradingHoursChecker.cs

namespace StockApp.ServiceLayer
{
    using System;

    public class TradingHoursChecker
    {
        private TimeZoneInfo est = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
        
        public bool IsTradingHours()
        {
            var easternNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, est);

            return (TimeSpan.FromHours(9).Add(TimeSpan.FromMinutes(30)) < easternNow.TimeOfDay
                && easternNow.TimeOfDay < TimeSpan.FromHours(16) 
                && easternNow.DayOfWeek != DayOfWeek.Saturday 
                && easternNow.DayOfWeek != DayOfWeek.Sunday);
        }
    }
}
