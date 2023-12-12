string[] lines = File.ReadAllLines("../input.txt");

int result = 0;

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

    int amountOfUnknown = 0;
    foreach(char c in extendedRecords)
    {
        if(c == '?')
        {   
            amountOfUnknown++;
        }
    }
}

Console.WriteLine("Result: " + result);
