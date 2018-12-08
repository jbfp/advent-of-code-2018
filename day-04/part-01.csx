using System.Globalization;

const string fallsAsleepMsg = "falls asleep";
const string timestampFormat = "[yyyy-MM-dd HH:mm]";
const string wakesUpMsg = "wakes up";

static (DateTimeOffset, string) ParseTimestamp(string line)
{
    var timestamp = DateTimeOffset.ParseExact(
        line.Substring(0, 18),
        timestampFormat,
        null,
        DateTimeStyles.AssumeUniversal);

    var rest = line.Substring(19);

    return (timestamp, rest);
}

class TimeSlept
{
    public TimeSpan Total;
    public readonly int[] Minutes = new int[60];

    public void Deconstruct(out TimeSpan total, out int[] minutes)
    {
        total = Total;
        minutes = Minutes;
    }
}

var events = File
    .ReadLines("./input.txt")
    .Select(ParseTimestamp)
    .OrderBy(t => t.Item1);

var guards = new Dictionary<int, TimeSlept>();
var currentGuard = 0;
var asleepFrom = DateTimeOffset.MinValue;

foreach (var (timestamp, rest) in events)
{
    if (rest == fallsAsleepMsg)
    {
        asleepFrom = timestamp;
    }
    else if (rest == wakesUpMsg)
    {
        if (!guards.TryGetValue(currentGuard, out var timeSlept))
        {
            guards.Add(currentGuard, timeSlept = new TimeSlept());
        }

        for (int minute = asleepFrom.Minute; minute < timestamp.Minute; minute++)
        {
            timeSlept.Total += TimeSpan.FromMinutes(1);
            timeSlept.Minutes[minute]++;
        }
    }
    else
    {
        // "Guard #___ begins shift".
        currentGuard = int.Parse(rest.Split(' ')[1].Substring(1));
    }
}

foreach (var (guardId, timeSlept) in guards)
{
    Console.WriteLine("Guard #{0} slept for {1}.", guardId, timeSlept.Total);
}

var (guardId, (total, minutes)) = guards
    .OrderByDescending(kvp => kvp.Value.Total)
    .FirstOrDefault();

var mostFrequently = minutes
    .Select((n, i) => (n, i))
    .OrderByDescending(t => t.n)
    .ThenByDescending(t => t.i)
    .Select(t => t.i)
    .First();

Console.WriteLine("-------------------------");
Console.WriteLine("Guard #{0} slept the most at {1} minutes! Most frequently at minute {2}.", guardId, total.TotalMinutes, mostFrequently);
Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("Result: {0}", guardId * mostFrequently);
Console.ResetColor();
