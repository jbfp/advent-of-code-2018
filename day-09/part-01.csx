using System.Text.RegularExpressions;

var input = File.ReadAllText("./input.txt");
var regex = new Regex(@"^(\d+) players; last marble is worth (\d+) points$", RegexOptions.Compiled);
var match = regex.Match(input);
var (numPlayers, lastMarblePoints) = (int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value));
var scores = new int[numPlayers];
var circle = new LinkedList<int>(new[] { 0 });
var currentMarble = circle.First;
var currentPlayer = 0;

LinkedListNode<T> Clockwise<T>(LinkedListNode<T> node, int n)
{
    var current = node;

    for (int i = 0; i < n; i++)
    {
        current = current.Next;

        if (current == null)
        {
            current = node.List.First;
        }
    }

    return current;
}

LinkedListNode<T> CounterClockwise<T>(LinkedListNode<T> node, int n)
{
    var current = node;

    for (int i = 0; i < n; i++)
    {
        current = current.Previous;

        if (current == null)
        {
            current = node.List.Last;
        }
    }

    return current;
}

for (int marble = 1; marble < lastMarblePoints; marble++)
{
    if (marble % 23 == 0)
    {
        scores[currentPlayer] += marble; // First, the current player keeps the marble they would have placed, adding it to their score.
        var marbleToRemove = CounterClockwise(currentMarble, 7); // In addition, the marble 7 marbles counter-clockwise from the current marble...
        var marbleToRemoveValue = marbleToRemove.Value;
        var nextMarble = Clockwise(marbleToRemove, 1);
        circle.Remove(marbleToRemove); // ... is removed from the circle...
        scores[currentPlayer] += marbleToRemoveValue; // ... and also added to the current player's score.
        currentMarble = nextMarble; // The marble located immediately clockwise of the marble that was removed becomes the new current marble.
    }
    else
    {
        var nextMarble = Clockwise(currentMarble, 1);
        currentMarble = circle.AddAfter(nextMarble, marble);
    }

    // Next player.
    currentPlayer = (currentPlayer + 1) % numPlayers;
}


Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("High score is {0}", scores.Max());
Console.ResetColor();
