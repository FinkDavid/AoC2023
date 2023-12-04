string[] lines = File.ReadAllLines("../input.txt");

int result = 0;

List<int> amountOfCardsWon = new List<int>();

for(int i = 0; i < lines.Length; i++)
{
    amountOfCardsWon.Add(1);
}

int currentCard = 0;
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

    for(int i = 0; i < amountOfCardsWon[currentCard]; i++)
    {
        int resultForLine = 0;

        foreach(int drawnNumber in drawnNumbers)
        {
            if(winningNumbers.Contains(drawnNumber))
            {
                resultForLine++;
            }
        }

        for(int j = currentCard + 1; j < currentCard + 1 + resultForLine; j++)
        {
            if(j <= amountOfCardsWon.Count)
            {
                amountOfCardsWon[j]++;
            }
        }
    }

    currentCard++;
}

foreach(int cards in amountOfCardsWon)
{
    result += cards;
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