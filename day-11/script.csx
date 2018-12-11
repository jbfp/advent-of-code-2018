// Key to solving this puzzle in a timely fashion:
// https://en.wikipedia.org/wiki/Summed-area_table

const int serialNumber = 7165; // Input.

static int GetPowerLevel(int x, int y)
{
    var rackId = x + 10;
    var powerLevel = rackId * y;
    powerLevel += serialNumber;
    powerLevel = powerLevel * rackId;

    if (powerLevel < 100)
    {
        powerLevel = 0;
    }
    else
    {
        powerLevel = (powerLevel % 1000) / 100; // Get the 'hundreds' digit.
    }

    powerLevel -= 5;
    return powerLevel;
}

struct Square
{
    public int X;
    public int Y;
    public int Size;
    public int Level;
}

static Square GetLargestPowerLevelSquare(int[,] grid, int squareSize)
{
    if (squareSize < 1 || squareSize > gridSize)
    {
        throw new ArgumentOutOfRangeException();
    }

    var maxCoord = (-1, -1);
    var max = int.MinValue;

    for (int x = squareSize; x < grid.GetLength(0); x++)
    {
        for (int y = squareSize; y < grid.GetLength(1); y++)
        {
            var a = grid[x - squareSize, y - squareSize];
            var b = grid[x, y - squareSize];
            var c = grid[x - squareSize, y];
            var d = grid[x, y];
            var sum = d - b - c + a;

            if (sum >= max)
            {
                max = sum;
                maxCoord = (x - squareSize + 2, y - squareSize + 2); // Top-left corner of rectangle.
            }
        }
    }

    return new Square
    {
        X  = maxCoord.Item1,
        Y = maxCoord.Item2,
        Size = squareSize,
        Level = max,
    };
}

const int gridSize = 300;

// Create summed-area table.
var grid = new int[gridSize, gridSize];

for (int x = 0; x < gridSize; x++)
{
    for (int y = 0; y < gridSize; y++)
    {
        var powerLevel = GetPowerLevel(x + 1, y + 1); // i(x, y)
        var above = y == 0 ? 0 : grid[x, y- 1]; // I(x, y - 1)
        var left = x == 0 ? 0 : grid[x - 1, y]; // I(x - 1, y)
        var aboveLeft = y == 0 || x == 0 ? 0 : grid[x - 1, y - 1]; // I(x - 1, y - 1)
        grid[x, y] = powerLevel + above + left - aboveLeft;
    }
}

// Part 1:
var squareForSize3 = GetLargestPowerLevelSquare(grid, 3);
Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("Largest power level square coord for {0}x{0} is {1} with a power level of {2:N0}.", squareForSize3.Size, (squareForSize3.X, squareForSize3.Y), squareForSize3.Level);
Console.ResetColor();

// Part 2:
var squares = new List<Square>(gridSize);

for (int n = 1; n <= gridSize; n++)
{
    squares.Add(GetLargestPowerLevelSquare(grid, n));
}

var max = squares.OrderByDescending(s => s.Level).First();
Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("Square at {0}x{0} @ {1} has the largest power level of {2:N0}.", max.Size, (max.X, max.Y), max.Level);
Console.ResetColor();
