string[] lines = File.ReadAllLines("../input.txt");

long result = 0;

foreach(string l in lines)
{
    List<List<long>> sequences = new List<List<long>>();
    List<long> history = new List<long>();
    string[] numbers = l.Split(' ');
    for(int i = 0; i < numbers.Length; i++)
    {
        history.Add(long.Parse(numbers[i]));
    }

    sequences.Add(history);
    List<long> currentSequence = history;
    while(!isSequenceAllZeros(currentSequence))
    {
        currentSequence = getNextSequence(currentSequence);
        sequences.Add(currentSequence);
    }

    sequences[sequences.Count - 1].Insert(0, 0);
    for(int i = sequences.Count - 2; i >= 0; i--)
    {
        long numberToAdd = sequences[i][0] - sequences[i + 1][0];
        sequences[i].Insert(0, numberToAdd);
    }

    result += sequences[0][0];
}


Console.WriteLine("Result: " + result);

List<long> getNextSequence(List<long> currentSequence)
{
    List<long> nextSequence = new List<long>();
    int sequenceLength = currentSequence.Count - 1;

    for(int i = currentSequence.Count - 1; i > 0; i--)
    {
        nextSequence.Insert(0, currentSequence[i] - currentSequence[i - 1]);
    }

    return nextSequence;
}

bool isSequenceAllZeros(List<long> currentSequence)
{
    foreach(long number in currentSequence)
    {
        if(number != 0)
        {
            return false;
        }
    }

    return true;
}