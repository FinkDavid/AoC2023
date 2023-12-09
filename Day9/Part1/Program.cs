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

    sequences[sequences.Count - 1].Add(0);
    for(int i = sequences.Count - 2; i >= 0; i--)
    {
        long numberToAdd = sequences[i + 1][sequences[i+1].Count - 1] +  sequences[i][sequences[i+1].Count - 1];
        sequences[i].Add(numberToAdd);
        if(i == 0)
        {
            result += numberToAdd;
        }
    }
}

Console.WriteLine("Result: " + result);

List<long> getNextSequence(List<long> currentSequence)
{
    List<long> nextSequence = new List<long>();
    int sequenceLength = currentSequence.Count - 1;

    for(int i = 0; i < sequenceLength; i++)
    {
        nextSequence.Add(currentSequence[i + 1] - currentSequence[i]);
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