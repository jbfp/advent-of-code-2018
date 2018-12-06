bool AreSameTypeWithDifferentPolarization(char a, char b)
{
    return a != b && char.ToLowerInvariant(a) == char.ToLowerInvariant(b);
}

var input = File.ReadAllText("./input.txt").Trim();

var results = input
    .Select(char.ToLowerInvariant)
    .Distinct()
    .Select(type =>
    {
        var lower = type;
        var upper = char.ToUpperInvariant(lower);
        var inputWithoutType = input.Where(c => c != lower && c != upper);
        var list = new LinkedList<char>(inputWithoutType);

        var currentNode = list.First;

        while (currentNode != list.Last)
        {
            var nextNode = currentNode.Next;

            if (AreSameTypeWithDifferentPolarization(currentNode.Value, nextNode.Value))
            {
                // Same type, but different polarization!
                // ANNIHILATE!!!
                var currentNextNode = currentNode.Previous;
                list.Remove(currentNode);
                list.Remove(nextNode);
                currentNode = currentNextNode ?? list.First; // Go to the previous node, or the first node if currentNode used to be the first node.
            }
            else
            {
                currentNode = nextNode;
            }
        }

        return (Type: type, list.Count);
    })
    .OrderByDescending(t => t.Count)
    .ToList();

for (int i = 0; i < results.Count; i++)
{
    var (type, result) = results[i];

    if (i == results.Count - 1)
    {
        Console.ForegroundColor = ConsoleColor.Green;
    }

    Console.WriteLine("[{0}]: {1}", type, result);
    Console.ResetColor();
}
