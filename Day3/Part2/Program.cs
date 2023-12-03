using System.Numerics;

string[] lines = File.ReadAllLines("../input.txt");

char[,] engine = new char[lines.GetLength(0), lines.GetLength(0)];
List<KeyValuePair<int, Vector2>> digitPairings = new List<KeyValuePair<int, Vector2>>();
int result = 0;

int i = 0;
foreach (string l in lines)
{
    int j = 0;
    foreach(char c in l)
    {
        engine[i, j] = c;
        j++;
    }

    i++;
}

for(int x = 0; x < engine.GetLength(0); x++)
{
    for(int y = 0; y < engine.GetLength(1); y++)
    {
        int length = 0;
        string number = "";
        if(char.IsDigit(engine[x,y]))
        {
            number += engine[x, y].ToString();
            length = 1;
            if(y + 1 < engine.GetLength(1))
            {
                if(char.IsDigit(engine[x, y + 1]))
                {
                    number += engine[x, y + 1].ToString();
                    length = 2;
                    if(y + 2 < engine.GetLength(1))
                    {
                        if(char.IsDigit(engine[x, y + 2]))
                        {
                            number += engine[x, y + 2].ToString();
                            length = 3;
                        }
                    }
                }
            }
        }

        if(number != "")
        {
            Vector2 starPosition = isNumberValid(x, y, length, engine);

            if(starPosition.X != -1 && starPosition.Y != -1)
            {
                digitPairings.Add(new KeyValuePair<int, Vector2>(int.Parse(number), starPosition));
            }
        }

        y += length;
    }
}

var groupedPairs = digitPairings.GroupBy(pair => pair.Value);
List<List<int>> separatedLists = groupedPairs.Select(group => group.Select(pair => pair.Key).ToList()).ToList();

for (int j = 0; j < separatedLists.Count; j++)
{
    if(separatedLists[j].Count == 2)
    {
        result += separatedLists[j][0] * separatedLists[j][1];
    }
}

Console.WriteLine("Result: " + result);

Vector2 isNumberValid(int indexX, int indexY, int length, char[,] engine)
{
    Vector2 starPosition = new Vector2(-1, -1);
    if(indexY != 0)
    {
        if(engine[indexX, indexY - 1] == '*')
        {
            starPosition.X = indexX;
            starPosition.Y = indexY - 1;
            return starPosition;
        }
    }
    
    if(indexY + length < engine.GetLength(1))
    {
        if(engine[indexX, indexY + length] == '*')
        {
            starPosition.X = indexX;
            starPosition.Y = indexY + length;
            return starPosition;
        }
    }
    
    if(indexX != 0)
    {
        int startLeft = indexY - 1;

        if(startLeft < 0)
        {
            startLeft = 0;
        }

        int endRight = indexY + length + 1;

        if(endRight > engine.GetLength(1))
        {
            endRight = engine.GetLength(1);
        }

        for(int i = startLeft; i < endRight; i++)
        {
            if(engine[indexX - 1, i] == '*')
            {
                starPosition.X = indexX - 1;
                starPosition.Y = i;
                return starPosition;
            }
        }
    }

    if(indexX < engine.GetLength(1) - 1)
    {
        int startLeft = indexY - 1;

        if(startLeft < 0)
        {
            startLeft = 0;
        }

        int endRight = indexY + length + 1;

        if(endRight > engine.GetLength(1))
        {
            endRight = engine.GetLength(1);
        }
        
        for(int i = startLeft; i < endRight; i++)
        {
            if(engine[indexX + 1, i] == '*')
            {
                starPosition.X = indexX + 1;
                starPosition.Y = i;
                return starPosition;
            }
        }
    }

    return starPosition;
}