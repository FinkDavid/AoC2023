string[] lines = File.ReadAllLines("../input.txt");

int result = 0;

foreach(string l in lines)
{
    string[] split = l.Split(' ');

    string records = split[0];
    string[] num = split[1].Split(',');
    List<int> numbers = new List<int>();
    for(int i = 0; i < num.Length; i++)
    {
        numbers.Add(int.Parse(num[i]));
    }

    int amountOfUnknown = 0;
    foreach(char c in records)
    {
        if(c == '?')
        {   
            amountOfUnknown++;
        }
    }

    int numberOfPossibilities = (int)Math.Pow(2, amountOfUnknown);
    for(int i = 0; i < numberOfPossibilities; i++)
    {
        string combination = GetCombination(records, i, amountOfUnknown);
        List<int> sizes = GetSegmentSizes(combination);
        
        
        if(sizes.Count == numbers.Count)
        {
            bool isValidArragnment = true;
            for(int j = 0; j < numbers.Count; j++)
            {
                if(numbers[j] != sizes[j])
                {
                    isValidArragnment = false;
                    break;
                }
            }

            if(isValidArragnment)
            {
                result++;
            }
        }
    }
}

Console.WriteLine("Result: " + result);

string GetCombination(string str, int value, int amountOfUnknown)
{
    char[] combination = str.ToCharArray();
    int mask = 1;

    for(int i = 0; i < str.Length; i++)
    {
        if(combination[i] == '?')
        {
            combination[i] = (value & mask) == 0 ? '#' : '.';
            mask <<= 1;

            if(amountOfUnknown-- == 0)
            {
                break;
            }
        }
    }

    return new string(combination);
}

List<int> GetSegmentSizes(string combination)
{
    List<int> segmentSizes = new List<int>();
    int currentSegmentSize = 0;

    foreach(char c in combination)
    {
        if(c == '#')
        {
            currentSegmentSize++;
        }
        else if(currentSegmentSize > 0)
        {
            segmentSizes.Add(currentSegmentSize);
            currentSegmentSize = 0;
        }
    }

    if(currentSegmentSize > 0)
    {
        segmentSizes.Add(currentSegmentSize);
    }

    return segmentSizes;
}
