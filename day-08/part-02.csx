static (int numNodes, int numMetadata) ParseHeader(IList<int> parts, ref int index)
{
    var numNodes = parts[index++];
    var numMetadata = parts[index++];
    return (numNodes, numMetadata);
}

static int CalculateScore(IList<int> parts, ref int index, (int numNodes, int numMetada) header)
{
    int score = 0;

    if (header.numNodes > 0)
    {
        var scores = new List<int>(header.numNodes);

        // Calculate the scores of child nodes.
        for (int i = 0; i < header.numNodes; i++)
        {
            scores.Add(CalculateScore(parts, ref index, ParseHeader(parts, ref index)));
        }

        // Use the metadata values as indexes into the node scores.
        for (int i = 0; i < header.numMetada; i++)
        {
            var nodeIndex = parts[index++];

            if (nodeIndex > 0 && nodeIndex <= scores.Count)
            {
                score += scores[nodeIndex - 1];
            }
        }
    }
    else
    {
        // The value of a node is the sum of its metadata nodes.
        for (int i = 0; i < header.numMetada; i++)
        {
            score += parts[index++];
        }
    }

    return score;
}

var licenseFileParts = File
    .ReadAllText("./input.txt")
    .Split(' ')
    .Select(int.Parse)
    .ToList();

int index = 0;
int sum = CalculateScore(licenseFileParts, ref index, ParseHeader(licenseFileParts, ref index));

Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("Metadata sum: {0}", sum);
Console.ResetColor();
