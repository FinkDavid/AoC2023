string[] lines = File.ReadAllLines("../input.txt");
int result = 0;

Dictionary<string, int> numberMapping = new Dictionary<string, int>
{
    { "zero", 0 },
    { "one", 1 },
    { "two", 2 },
    { "three", 3 },
    { "four", 4 },
    { "five", 5 },
    { "six", 6 },
    { "seven", 7 },
    { "eight", 8 },
    { "nine", 9 },
    { "1", 1 },
    { "2", 2 },
    { "3", 3 },
    { "4", 4 },
    { "5", 5 },
    { "6", 6 },
    { "7", 7 },
    { "8", 8 },
    { "9", 9 }
};

foreach (string l in lines)
{
    int firstIndex = l.Length;
    int lastIndex = -1;
    int lastNumber = 0;
    int firstNumber = 0;

    foreach(KeyValuePair<string, int> entry in numberMapping)
    {
        int x = l.LastIndexOf(entry.Key);
        int y = l.IndexOf(entry.Key);

        if(x > lastIndex)
        {
            lastIndex = x;
            lastNumber = entry.Value;
        }

        if(y < firstIndex && y >= 0)
        {
            firstIndex = y;
            firstNumber = entry.Value;
        }
    }

    result += int.Parse(firstNumber.ToString() + lastNumber.ToString());
}

Console.WriteLine("Result: " + result);