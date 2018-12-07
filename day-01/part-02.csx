static int FindFirstRepeatingFrequency(IEnumerable<int> frequencyChanges)
{
    var seen = new HashSet<int>();
    var frequency = 0;

    while (true) // The important part is to keep looping.
    {
        foreach (var frequencyChange in frequencyChanges)
        {
            frequency += frequencyChange;

            if (!seen.Add(frequency))
            {
                return frequency;
            }
        }
    }
}

var frequencyChanges = File
    .ReadLines("./input.txt")
    .Select(int.Parse);

var firstRepeatingFrequency = FindFirstRepeatingFrequency(frequencyChanges);

Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("Resulting frequency: {0}", firstRepeatingFrequency);
Console.ResetColor();
