using System.Text.RegularExpressions;

class Claim
{
    public int Left;
    public int Top;
    public int Width;
    public int Height;

    public int Right => Left + Width;
    public int Bottom => Top + Height;
}

static Claim MatchToClaim(Match match)
{
    return new Claim
    {
        Left = int.Parse(match.Groups[2].Value),
        Top = int.Parse(match.Groups[3].Value),
        Width = int.Parse(match.Groups[4].Value),
        Height = int.Parse(match.Groups[5].Value),
    };
}

var regex = new Regex(@"^#(\d+).@.(\d+),(\d+):.(\d+)x(\d+)$", RegexOptions.Compiled);

var claims = File
    .ReadLines("./input.txt")
    .Select(line => regex.Match(line))
    .Select(MatchToClaim)
    .ToList();

var maxX = claims.Max(claim => claim.Right);
var maxY = claims.Max(claim => claim.Bottom);
var fabric = new int[maxX, maxY];

foreach (var claim in claims)
{
    for (int x = claim.Left; x < claim.Right; x++)
    {
        for (int y = claim.Top; y < claim.Bottom; y++)
        {
            fabric[x, y]++;
        }
    }
}

int numOverlappingSquareInches = 0;

for (int x = 0; x < maxX; x++)
{
    for (int y = 0; y < maxY; y++)
    {
        if (fabric[x, y] > 1)
        {
            numOverlappingSquareInches++;
        }
    }
}

Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("# of overlapping square inches: {0}", numOverlappingSquareInches);
Console.ResetColor();
