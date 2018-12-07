static string TestPair(string a, string b, int length)
{
    var common = new List<char>(length);
    var hasAtLeastOneDiff = false;

    for (int i = 0; i < length; i++)
    {
        if (a[i] == b[i])
        {
            common.Add(a[i]);
        }
        else if (hasAtLeastOneDiff)
        {
            // Has more than one different character. Try the next pair.
            return null;
        }
        else
        {
            hasAtLeastOneDiff = true;
        }
    }

    if (hasAtLeastOneDiff)
    {
        return string.Join("", common);
    }

    return null;
}

static string TestLines(IReadOnlyList<string> lines)
{
    for (int i = 0; i < lines.Count - 1; i++)
    {
        var a = lines[i];
        var b = lines[i + 1];
        var length = a.Length;
        var result = TestPair(a, b, length);

        if (result != null)
        {
            return result;
        }
    }

    return null;
}

var lines = File
    .ReadLines("./input.txt")
    .OrderBy(line => line)
    .ToList()
    .AsReadOnly();

Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("Common chars: {0}", TestLines(lines));
Console.ResetColor();
