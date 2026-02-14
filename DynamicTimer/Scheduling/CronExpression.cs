namespace DynamicTimer.Scheduling;

public class CronExpression
{
    private HashSet<int> _minutes = new();
    private HashSet<int> _hours = new();
    private HashSet<int> _daysOfMonth = new();
    private HashSet<int> _months = new();
    private HashSet<int> _daysOfWeek = new();

    public string Expression { get; }

    public CronExpression(string cronExpression)
    {
        Expression = cronExpression;
        Parse(cronExpression);
    }

    private void Parse(string cronExpression)
    {
        var parts = cronExpression.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length != 5)
        {
            throw new ArgumentException("Cron expression must have 5 fields: minute hour day month weekday");
        }

        _minutes = ParseField(parts[0], 0, 59);
        _hours = ParseField(parts[1], 0, 23);
        _daysOfMonth = ParseField(parts[2], 1, 31);
        _months = ParseField(parts[3], 1, 12);
        _daysOfWeek = ParseField(parts[4], 0, 6);
    }

    private HashSet<int> ParseField(string field, int min, int max)
    {
        var result = new HashSet<int>();

        // Wildcard - all values
        if (field == "*")
        {
            for (int i = min; i <= max; i++)
            {
                result.Add(i);
            }
            return result;
        }

        // Handle comma-separated values
        var parts = field.Split(',');
        foreach (var part in parts)
        {
            // Step values: */5 or 10-20/2
            if (part.Contains('/'))
            {
                var stepParts = part.Split('/');
                var step = int.Parse(stepParts[1]);

                int rangeMin = min;
                int rangeMax = max;

                // Check if there's a range before the step
                if (stepParts[0] != "*")
                {
                    if (stepParts[0].Contains('-'))
                    {
                        var rangeParts = stepParts[0].Split('-');
                        rangeMin = int.Parse(rangeParts[0]);
                        rangeMax = int.Parse(rangeParts[1]);
                    }
                    else
                    {
                        rangeMin = int.Parse(stepParts[0]);
                    }
                }

                for (int i = rangeMin; i <= rangeMax; i += step)
                {
                    if (i >= min && i <= max)
                    {
                        result.Add(i);
                    }
                }
            }
            // Range: 1-5
            else if (part.Contains('-'))
            {
                var rangeParts = part.Split('-');
                var rangeMin = int.Parse(rangeParts[0]);
                var rangeMax = int.Parse(rangeParts[1]);

                for (int i = rangeMin; i <= rangeMax; i++)
                {
                    if (i >= min && i <= max)
                    {
                        result.Add(i);
                    }
                }
            }
            // Specific value: 5
            else
            {
                var value = int.Parse(part);
                if (value >= min && value <= max)
                {
                    result.Add(value);
                }
            }
        }

        return result;
    }

    public DateTime? GetNextOccurrence(DateTime from)
    {
        // Start from the next minute
        var candidate = new DateTime(from.Year, from.Month, from.Day, from.Hour, from.Minute, 0)
            .AddMinutes(1);

        // Search for up to 4 years to prevent infinite loops
        var maxIterations = 4 * 365 * 24 * 60; // 4 years in minutes
        var iterations = 0;

        while (iterations < maxIterations)
        {
            if (Matches(candidate))
            {
                return candidate;
            }

            candidate = candidate.AddMinutes(1);
            iterations++;
        }

        return null; // No match found
    }

    private bool Matches(DateTime dateTime)
    {
        // Check if the datetime matches all cron fields
        if (!_minutes.Contains(dateTime.Minute))
            return false;

        if (!_hours.Contains(dateTime.Hour))
            return false;

        if (!_months.Contains(dateTime.Month))
            return false;

        // Day of month and day of week logic:
        // In cron, if both are specified (not *), it matches if EITHER condition is true
        bool dayOfMonthMatch = _daysOfMonth.Contains(dateTime.Day);
        bool dayOfWeekMatch = _daysOfWeek.Contains((int)dateTime.DayOfWeek);

        // If both are wildcards (all values), match
        if (_daysOfMonth.Count == 31 && _daysOfWeek.Count == 7)
            return true;

        // If only day of month is specified
        if (_daysOfMonth.Count < 31 && _daysOfWeek.Count == 7)
            return dayOfMonthMatch;

        // If only day of week is specified
        if (_daysOfWeek.Count < 7 && _daysOfMonth.Count == 31)
            return dayOfWeekMatch;

        // Both are specified - match if either is true
        return dayOfMonthMatch || dayOfWeekMatch;
    }

    public static bool TryParse(string cronExpression, out CronExpression? result)
    {
        try
        {
            result = new CronExpression(cronExpression);
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }
}
