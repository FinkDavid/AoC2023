string[] lines = File.ReadAllLines("../input.txt");

int result = 0;

foreach (string l in lines)
{
    string[] gameRounds = l.Split(';');

    int minGreen = 0;
    int minRed = 0;
    int minBlue = 0;

    foreach(string s in gameRounds)
    {
        int greenAmount = getCountOfColor(s, "green");
        int redAmount = getCountOfColor(s, "red");
        int blueAmount = getCountOfColor(s, "blue");

        if(greenAmount > minGreen)
        {
            minGreen = greenAmount;
        }

        if(redAmount > minRed)
        {
            minRed = redAmount;
        }

        if(blueAmount > minBlue)
        {
            minBlue = blueAmount;
        }
    }

    result += minGreen * minRed * minBlue;
}

Console.WriteLine("Result: " + result);

int getCountOfColor(string line, string color)
{
    List<int> drops = new List<int>();

    for (int i = 0; i < line.Length - 1; i++)
    {
        if(line[i] == color[0] && line[i+1] == color[1] && line[i+2] == color[2])
        {
            if(char.IsDigit(line[i - 3]))
            {
                return int.Parse(line[i - 3].ToString() + line[i - 2].ToString());
            }
            else
            {
                return int.Parse(line[i - 2].ToString());
            }
        }
    }

    return 0;
}