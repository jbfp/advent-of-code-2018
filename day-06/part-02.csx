var coordinates = File
    .ReadLines("./input.txt")
    .Select(line => line
        .Split(',')
        .Select(int.Parse)
        .ToArray())
    .Select(split => (x: split[0], y: split[1]))
    .ToList()
    .AsReadOnly();

const int maxScore = 10000;

var smallestX = coordinates.Min(t => t.x);
var smallestY = coordinates.Min(t => t.y);

var largestX = coordinates.Max(t => t.x);
var largestY = coordinates.Max(t => t.y);

var width = largestX - smallestX;
var height = largestY - smallestY;
var size = 28; // This was the lowest value of 'size' with the correct result. There must be something I'm not understanding here.

var map = new Dictionary<(int, int), int>();

int ManhattanDistance((int p1, int p2) p, (int q1, int q2) q)
{
    return Math.Abs(p.p1 - q.q1) + Math.Abs(p.p2 - q.q2);
}

for (int x = smallestX - size; x < width + size; x++)
{
    for (int y = smallestY - size; y < height + size; y++)
    {
        var p = (x, y);

        map[p] = coordinates
            .Select(q => ManhattanDistance(p, q))
            .Sum();
    }
}

var regionSize = map
    .Values
    .Where(score => score < maxScore)
    .Count();

Console.Write("Region size: ");
Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine(regionSize);
Console.ResetColor();
