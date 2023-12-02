string[] lines = File.ReadAllLines("../input.txt");

int round = 1;
int result = 0;

foreach (string l in lines)
{
    string[] gameRounds = l.Split(';');

    bool roundCorrect = true;
    foreach(string s in gameRounds)
    {
        int greenAmount = getCountOfColor(s, "green");
        int redAmount = getCountOfColor(s, "red");
        int blueAmount = getCountOfColor(s, "blue");

        if(greenAmount > 13 || redAmount > 12 || blueAmount > 14)
        {
            roundCorrect = false;
            break;
        }
    }

    if(roundCorrect)
    {
        result += round;
    }    
    round++;
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