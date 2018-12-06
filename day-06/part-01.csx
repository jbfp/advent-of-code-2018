var coordinates = File
   .ReadLines("./input.txt")
   .Select(line => line
       .Split(',')
       .Select(int.Parse)
       .ToArray())
   .Select(split => (x: split[0], y: split[1]))
   .ToList()
   .AsReadOnly();

var smallestX = coordinates.Min(t => t.x);
var smallestY = coordinates.Min(t => t.y);

var largestX = coordinates.Max(t => t.x);
var largestY = coordinates.Max(t => t.y);

int ManhattanDistance((int p1, int p2) p, (int q1, int q2) q)
{
    return Math.Abs(p.p1 - q.q1) + Math.Abs(p.p2 - q.q2);
}

(int x, int y)? ClosestCoordinateToPoint((int, int) p)
{
    var closest = coordinates
        .Select(q => (q, d: ManhattanDistance(p, q)))
        .GroupBy(t => t.d)
        .OrderBy(g => g.Key)
        .First();

    if (closest.Count() > 1)
    {
        return null;
    }

    return closest.Single().q;
}

Dictionary<(int, int), int> ScoreTheBoard(int sz)
{
    var scores = new Dictionary<(int, int), int>(coordinates.ToDictionary(c => c, _ => 0));

    for (int x = smallestX - sz; x < largestX + sz; x++)
    {
        for (int y = smallestY - sz; y < largestY + sz; y++)
        {
            var closest = ClosestCoordinateToPoint((x, y));

            if (closest == null)
            {
                continue;
            }

            scores[closest.Value]++;
        }
    }

    return scores;
}

// If the area for a coordinate changes with the size of the board, it must be infinite.
var sc0 = ScoreTheBoard(0);
var sc1 = ScoreTheBoard(1);

var largestArea = coordinates
    .Select(c => (s1: sc0[c], s2: sc1[c]))
    .Where(t => t.s1 == t.s2)
    .Select(t => t.s1)
    .Max();

Console.Write("Largest area: ");
Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine(largestArea);
Console.ResetColor();
