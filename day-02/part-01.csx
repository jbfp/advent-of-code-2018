var lines = File.ReadLines("./input.txt");
int numTwos = 0;
int numThrees = 0;

foreach (var line in lines)
{
    var chars = line
        .GroupBy(c => c)
        .Select(group => group.Count())
        .ToHashSet();

    if (chars.Contains(2))
    {
        numTwos++;
    }

    if (chars.Contains(3))
    {
        numThrees++;
    }
}

var checksum = numThrees * numTwos;

Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("Checksum: {0}", checksum);
Console.ResetColor();
