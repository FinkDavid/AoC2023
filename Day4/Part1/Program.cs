string[] lines = File.ReadAllLines("../input.txt");

int result = 0;

foreach (string l in lines)
{
    string[] split = l.Split('|');
    string[] splitFirstPart = split[0].Split(':');

    string firstPart = splitFirstPart[1];
    string secondPart = split[1];

    string[] firstNumbers = firstPart.Split(' ');
    string[] secondNumbers = secondPart.Split(' ');

    List<int> drawnNumbers = getNumberList(firstNumbers);
    List<int> winningNumbers = getNumberList(secondNumbers);

    int resultForLine = 0;

    foreach(int drawnNumber in drawnNumbers)
    {
        if(winningNumbers.Contains(drawnNumber))
        {
            if(resultForLine == 0)
            {
                resultForLine = 1;
            }
            else
            {
                resultForLine *= 2;
            }
        }
    }

    result += resultForLine;
}

Console.WriteLine("Result: " + result);

List<int> getNumberList(string[] numbers)
{
    List<int> numberList = new List<int>();

    foreach(string number in numbers)
    {
        int i = 0;
        if(int.TryParse(number, out i))
        {
            numberList.Add(i);
        }
    }

    return numberList;
}