package groups.common.utils;

import java.time.ZonedDateTime;

public class TimeUtils {

    public record TimeDiff(int year, int month, int day, int hour, int minute, int second) {

        @Override
        public boolean equals(Object obj) {

            if (obj instanceof TimeDiff timeDiff) {

                if (year != timeDiff.year) return false;
                if (month != timeDiff.month) return false;
                if (day != timeDiff.day) return false;
                if (hour != timeDiff.hour) return false;
                if (minute != timeDiff.minute) return false;
                return second == timeDiff.second;
            }

            return false;
        }
    }

    public static TimeDiff calculateTimeDiff(ZonedDateTime time1, ZonedDateTime time2) {

       int yearDiff = time2.getYear() - time1.getYear();
       int monthDiff = time2.getMonthValue() - time1.getMonthValue();
       int dayDiff = time2.getDayOfMonth() - time1.getDayOfMonth();
       int hourDiff = time2.getHour() - time1.getHour();
       int minuteDiff = time2.getMinute() - time1.getMinute();
       int secondDiff = time2.getSecond() - time1.getSecond();

       return new TimeDiff(yearDiff, monthDiff, dayDiff, hourDiff, minuteDiff, secondDiff);
    }

    public static ZonedDateTime addTimeDiff(ZonedDateTime zonedDateTime, TimeDiff diff) {

        return zonedDateTime
                .plusYears(diff.year)
                .plusMonths(diff.month)
                .plusDays(diff.day)
                .plusHours(diff.hour)
                .plusMinutes(diff.minute)
                .plusSeconds(diff.second);
    }

}
