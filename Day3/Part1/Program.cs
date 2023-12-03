string[] lines = File.ReadAllLines("../input.txt");

char[,] engine = new char[lines.GetLength(0), lines.GetLength(0)];
List<int> validNumbers = new List<int>();
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
            bool isValid = isNumberValid(x, y, length, engine);

            if(isValid)
            {
                validNumbers.Add(int.Parse(number));
            }
        }

        y += length;
    }
}

foreach(int n in validNumbers)
{
    result += n;
}

Console.WriteLine("Result: " + result);

bool isNumberValid(int indexX, int indexY, int length, char[,] engine)
{
    if(indexY != 0)
    {
        if(engine[indexX, indexY - 1] != '.')
        {
            return true;
        }
    }
    
    if(indexY + length < engine.GetLength(1))
    {
        if(engine[indexX, indexY + length] != '.')
        {
            return true;
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
            if(engine[indexX - 1, i] != '.')
            {
                return true;
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
            if(engine[indexX + 1, i] != '.')
            {
                return true;
            }
        }
    }

    return false;
}