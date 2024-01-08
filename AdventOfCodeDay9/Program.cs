
var inputFilePath = "input.txt";
var inputLines = File.ReadAllLines(inputFilePath);
int sumNext = 0;
int sumPrev = 0;

foreach (var line in inputLines)
{
    var numbers = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
    sumNext += CalculateNextValue(numbers);
    sumPrev += CalculatePreviousValue(numbers);
}

Console.WriteLine("The sum of these extrapolated for next values is: " + sumNext);
Console.WriteLine("The sum of these extrapolated for previous values is: " + sumPrev);
static int CalculateNextValue(List<int> sequence)
{
    List<List<int>> differenceLevels = new List<List<int>>();

    // Initialize the first level of differences.
    differenceLevels.Add(CalculateDifferences(sequence));

    // Continue calculating difference levels until all differences are zero.
    while (differenceLevels.Last().Any(d => d != 0))
    {
        differenceLevels.Add(CalculateDifferences(differenceLevels.Last()));
    }

    // Work back up the difference levels to calculate the next number in the sequence.
    for (int level = differenceLevels.Count - 2; level >= 0; level--)
    {
        List<int> currentLevel = differenceLevels[level];
        int lastDifference = currentLevel.Last();
        int lastValue = sequence.Last();
        sequence.Add(lastValue + lastDifference);
    }

    // The last number in the sequence is the next number to be added.
    return sequence.Last();
}

static int CalculatePreviousValue(List<int> sequence)
{
    List<List<int>> differenceLevels = new List<List<int>>();

    // Initialize the first level of differences.
    differenceLevels.Add(CalculateForPreviousDifferences(sequence));

    // Continue calculating difference levels until all differences are zero.
    while (differenceLevels.Last().Any(d => d != 0))
    {
        differenceLevels.Add(CalculateForPreviousDifferences(differenceLevels.Last()));
    }

    // Work back up the difference levels to calculate the previous number in the sequence.
    int previousValue = sequence[0];
    for (int level = differenceLevels.Count - 1; level > 0; level--)
    {
        List<int> currentLevel = differenceLevels[level];
        if (currentLevel.Count > 0) // Make sure we don't access an empty list
        {
            previousValue -= currentLevel[0]; // Subtract the first difference of the current level
        }
    }

    return previousValue;
}

static List<int> CalculateDifferences(List<int> numbers)
{
    List<int> differences = new List<int>();

    for (int i = 0; i < numbers.Count - 1; i++)
    {
        differences.Add(numbers[i + 1] - numbers[i]);
    }

    return differences;
}
static List<int> CalculateForPreviousDifferences(List<int> numbers)
{
    List<int> differences = new List<int>(numbers.Count);

    for (int i = 0; i < numbers.Count - 1; i++)
    {
        differences.Add(numbers[i + 1] - numbers[i]);
    }

    return differences;
}