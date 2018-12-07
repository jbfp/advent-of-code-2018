var frequency = File
    .ReadLines("./input.txt")
    .Select(int.Parse)
    .Sum();

Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("Resulting frequency: {0}", frequency);
Console.ResetColor();
