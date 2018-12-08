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

var events = File
    .ReadLines("./input.txt")
    .Select(ParseTimestamp)
    .OrderBy(t => t.Item1);

var guards = new Dictionary<int, int[]>();
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
            guards.Add(currentGuard, timeSlept = new int[60]);
        }

        for (int minute = asleepFrom.Minute; minute < timestamp.Minute; minute++)
        {
            timeSlept[minute]++;
        }
    }
    else
    {
        // "Guard #___ begins shift".
        currentGuard = int.Parse(rest.Split(' ')[1].Substring(1));
    }
}

int? sleepiestGuard = null;
int sleepiestMinute = 0;

for (int minute = 0; minute < 60; minute++)
{
    foreach (var (guardId, minutes) in guards)
    {
        if (sleepiestGuard == null || minutes[minute] >= guards[sleepiestGuard.Value][sleepiestMinute])
        {
            sleepiestGuard = guardId;
            sleepiestMinute = minute;
        }
    }
}

Console.WriteLine("The sleepiest guard was #{0} and was most frequently asleep at minute {1}.", sleepiestGuard, sleepiestMinute);
Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("Result: {0}", sleepiestGuard * sleepiestMinute);
Console.ResetColor();
