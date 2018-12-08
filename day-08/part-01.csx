static (int numNodes, int numMetadata) ParseHeader(IList<int> parts, ref int index)
{
    var numNodes = parts[index++];
    var numMetadata = parts[index++];
    return (numNodes, numMetadata);
}

static int Sum(IList<int> parts, ref int index, (int numNodes, int numMetada) header)
{
    int sum = 0;

    for (int i = 0; i < header.numNodes; i++)
    {
        sum += Sum(parts, ref index, ParseHeader(parts, ref index));
    }

    for (int i = 0; i < header.numMetada; i++)
    {
        sum += parts[index++];
    }

    return sum;
}

var licenseFileParts = File
    .ReadAllText("./input.txt")
    .Split(' ')
    .Select(int.Parse)
    .ToList();

int index = 0;
int sum = Sum(licenseFileParts, ref index, ParseHeader(licenseFileParts, ref index));

Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("Metadata sum: {0}", sum);
Console.ResetColor();
