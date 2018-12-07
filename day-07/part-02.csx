// 1. 'A' is 65.
// 2. It takes 60 + 1 seconds to complete A
// So the time it takes can also be written as the ASCII value - 4.
const int shift = -4;
const int numWorkers = 5;

class Task
{
    public char Step;
    public int TimeRemaining;
}

class Worker
{
    private readonly string _name;

    private Task _currentTask;

    public Worker(int i)
    {
        _name = $"W{i}";
    }

    public char WorkingOn => _currentTask?.Step ?? '.';

    public override string ToString() => _name;

    public void Assign(char task)
    {
        if (_currentTask != null)
        {
            throw new InvalidOperationException("Worker is working");
        }

        _currentTask = new Task
        {
            Step = task,
            TimeRemaining = task + shift,
        };
    }

    public char? Work()
    {
        if (_currentTask == null)
        {
            return null;
        }

        _currentTask.TimeRemaining--;

        if (_currentTask.TimeRemaining == 0)
        {
            var step = _currentTask.Step;
            _currentTask = null;
            return step;
        }

        return null;
    }
}

bool IsReady(char task)
{
    return previousByTask[task].All(completed.Contains);
}

var edges = File
    .ReadLines("./input.txt")
    .Select(line => new { From = line.Substring(5, 1)[0], To = line.Substring(36, 1)[0] })
    .ToList();

// Store all nodes that point to a given node.
var previousByTask = edges
    .ToLookup(edge => edge.To, edge => edge.From);

// Store all nodes that a given node points to.
var nextByTask = edges
    .ToLookup(edge => edge.From, edge => edge.To);

var output = 0;
var tasks = new Queue<char>(new[] { 'A', 'B', 'L' });
var inProgress = new HashSet<char>();
var completed = new HashSet<char>();
var workers = Enumerable.Range(0, numWorkers).Select(i => new Worker(i + 1)).ToList();
var availableWorkers = new Queue<Worker>(workers);

while (true)
{
    foreach (var worker in workers)
    {
        var work = worker.Work();

        if (work.HasValue)
        {
            char task = work.Value;
            inProgress.Remove(task);
            completed.Add(task);
            availableWorkers.Enqueue(worker);

            foreach (var next in nextByTask[task])
            {
                if (inProgress.Contains(next) || completed.Contains(next))
                {
                    continue;
                }

                if (IsReady(next))
                {
                    tasks.Enqueue(next);
                }
            }
        }
    }

    while (tasks.Count > 0 && availableWorkers.Count > 0)
    {
        var task = tasks.Dequeue();
        var worker = availableWorkers.Dequeue();
        worker.Assign(task);
        inProgress.Add(task);
    }

    if (tasks.Count == 0 && inProgress.Count == 0)
    {
        break;
    }

    output++;
}

Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("{0} seconds", output);
Console.ResetColor();
