var edges = File
    .ReadLines("./input.txt")
    .Select(line => new { From = line.Substring(5, 1)[0], To = line.Substring(36, 1)[0] })
    .ToList();

// Store all nodes that point to a given node.
var previousByNode = edges
    .ToLookup(edge => edge.To, edge => edge.From);

// Store all nodes that a given node points to.
var nextByNode = edges
    .ToLookup(edge => edge.From, edge => edge.To);

var nodes = edges
    .SelectMany(edge => new[] { edge.From, edge.To })
    .Distinct()
    .ToList();

// The "first" nodes are the ones that have no other nodes point to them.
var firsts = nodes
    .Except(nextByNode
        .SelectMany(group => group));

// My first thought was to construct a graph and traverse it in the correct way,
// but when that didn't work, I re-read the problem statement, and realized a priority queue
// was the correct data structure here.
var heap = new SortedSet<char>(firsts);
var completed = new HashSet<char>(firsts);
var output = new List<char>();

bool IsCompleted(char node)
{
    return previousByNode[node].All(completed.Contains);
}

while (heap.Count > 0)
{
    var current = heap.ElementAt(0);
    heap.Remove(current);

    if (IsCompleted(current))
    {
        completed.Add(current);
        output.Add(current);
    }

    foreach (var edge in nextByNode[current])
    {
        if (!completed.Contains(edge))
        {
            heap.Add(edge);
        }
    }
}

Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine(string.Join("", output));
Console.ResetColor();
