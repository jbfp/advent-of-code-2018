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

static (int Id, Claim Claim) MatchToClaim(Match match)
{
    var id = int.Parse(match.Groups[1].Value);

    var claim = new Claim
    {
        Left = int.Parse(match.Groups[2].Value),
        Top = int.Parse(match.Groups[3].Value),
        Width = int.Parse(match.Groups[4].Value),
        Height = int.Parse(match.Groups[5].Value),
    };

    return (id, claim);
}

var regex = new Regex(@"^#(\d+).@.(\d+),(\d+):.(\d+)x(\d+)$", RegexOptions.Compiled);

var claimsById = File
    .ReadLines("./input.txt")
    .Select(line => regex.Match(line))
    .Select(MatchToClaim)
    .ToDictionary(t => t.Id, t => t.Claim);

var maxX = claimsById.Values.Max(claim => claim.Right);
var maxY = claimsById.Values.Max(claim => claim.Bottom);
var fabric = new int[maxX, maxY];
var claimIds = new HashSet<int>(claimsById.Keys);

foreach (var claimById in claimsById)
{
    var id = claimById.Key;
    var claim = claimById.Value;

    for (int x = claim.Left; x < claim.Right; x++)
    {
        for (int y = claim.Top; y < claim.Bottom; y++)
        {
            var existingClaimId = fabric[x, y];

            if (existingClaimId == 0)
            {
                // Unclaimed; claim it!
                fabric[x, y] = id;
            }
            else
            {
                // Overlapping; remove both IDs from possible candidates and overwrite with -1.
                claimIds.Remove(existingClaimId);
                claimIds.Remove(id);
                fabric[x, y] = -1;
            }
        }
    }
}

Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("ID of non-overlapping claim: {0}", claimIds.Single());
Console.ResetColor();
