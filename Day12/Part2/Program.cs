string[] lines = File.ReadAllLines("../input.txt");

int result = 0;
int possiblePaths = 0;

foreach(string l in lines)
{
    string[] split = l.Split(' ');

    string records = split[0];
    string extendedRecords = records;
    string[] num = split[1].Split(',');
    List<int> numbers = new List<int>();
    for(int j = 0; j < 5; j++)
    {
        for(int i = 0; i < num.Length; i++)
        {
            numbers.Add(int.Parse(num[i]));
        }
    }

    for(int j = 0; j < 4; j++)
    {
        extendedRecords = extendedRecords + "?" + records;
    }

    Console.Write(extendedRecords + " - ");

    foreach(int size in numbers)
    {
        Console.Write(size + ",");
    }
    Console.WriteLine();

    string currentRecord = "";
    
    getNext(0, extendedRecords, currentRecord, numbers);
}

Console.WriteLine("Result: " + possiblePaths);

void getNext(int depth, string input, string current, List<int> numbers)
{
    //Console.WriteLine(depth + "   current: " + current);
    if(depth <= input.Length)
    {
        while(true)
        {
            if(input[depth] == '?')
            {
                if(CheckIfStillValid(numbers, current + "#"))
                {
                    getNext(depth + 1, input, current + "#", numbers);
                }
                else
                {
                    Console.WriteLine("Found unvalid path:  " + current + "#");
                    break;
                }

                if(CheckIfStillValid(numbers, current + "."))
                {
                    getNext(depth + 1, input, current + ".", numbers);
                }
                else
                {
                    Console.WriteLine("Found unvalid path:  " + current + ".");
                    break;
                }
            }
            else
            {
                current = current + input[depth];
                depth++;
            }

            if(depth >= input.Length)
            {
                break;
            }
        }
    }

    if(current.Length == input.Length)
    {
        possiblePaths++;
        Console.WriteLine("Finished path    " + current);
    }
}

bool CheckIfStillValid(List<int> numbers, string current)
{
    string test = "";
    foreach(char c in current)
    {
        if(c == '?')
        {
            test = test + '.';
        }
        else
        {
            test = test + c;
        }
    }
    List<int> n = GetSegmentSizes(test);
    /*
    foreach(int size in n)
    {
        Console.Write(size + ",");
    }
    Console.WriteLine();
    */

    bool isValidArragnment = true;
    for(int j = 0; j < n.Count; j++)
    {
        if(numbers[j] != n[j])
        {
            isValidArragnment = false;
            break;
        }
    }
    return isValidArragnment;
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