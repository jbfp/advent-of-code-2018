bool AreSameTypeWithDifferentPolarization(char a, char b)
{
    return a != b && char.ToLowerInvariant(a) == char.ToLowerInvariant(b);
}

var input = File.ReadAllText("./input.txt").Trim();
var list = new LinkedList<char>(input);

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

Console.WriteLine("Input length: {0}", input.Length);
Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("Final length: {0}", list.Count);
Console.ResetColor();
