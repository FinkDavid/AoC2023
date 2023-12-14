string[] lines = File.ReadAllLines("../input.txt");
char[,] map = new char[lines.Length, lines[0].Length];

for (int i = 0; i < lines.Length; i++)
{
    for (int j = 0; j < lines[i].Length; j++)
    {
        map[i, j] = lines[i][j];
    }
}

Dictionary<string, int> seenArrangements = new Dictionary<string, int>();
string state = ConvertCharArrayToString(map);

int cycleEnd = 0;
int cycles = 1000000000;
for(int i = 0; i < cycles; i++)
{
    seenArrangements.Add(state, i);
    RollStones('N');
    RollStones('W');
    RollStones('S');
    RollStones('E');
    state = ConvertCharArrayToString(map);
    if(seenArrangements.ContainsKey(state))
    {
        cycleEnd = i + 1;
        break;
    }
}

int cycleStart = seenArrangements[state];
int remainingCycles = (cycles - cycleStart) % (cycleEnd - cycleStart);

Console.WriteLine("Cycle found: " + cycleStart + " - " + cycleEnd);
Console.WriteLine("Remaining Cycles: " + remainingCycles);

for (int i = 0; i < remainingCycles; i++) 
{
    RollStones('N');
    RollStones('W');
    RollStones('S');
    RollStones('E');
}

long result = CalculateResult();
Console.WriteLine("Result: " + result);

long CalculateResult()
{
    int res = 0;
    int scoreCurentLine = map.GetLength(0);
    for(int i = 0; i < map.GetLength(0); i++)
    {
        for(int j = 0; j < map.GetLength(1); j++)
        {
            if(map[i, j] == 'O')
            {
                res += scoreCurentLine;
            }
        }
        scoreCurentLine--;
    }

    return res;
}

string ConvertCharArrayToString(char[,] charArray)
{
    int rows = charArray.GetLength(0);
    int cols = charArray.GetLength(1);

    System.Text.StringBuilder result = new System.Text.StringBuilder();

    for(int i = 0; i < rows; i++)
    {
        for(int j = 0; j < cols; j++)
        {
            result.Append(charArray[i, j]);
        }
    }

    return result.ToString();
}

void RollStones(char direction)
{
    switch(direction)
    {
        case 'N': RollStonesNorthWest(direction); break;
        case 'W': RollStonesNorthWest(direction); break;
        case 'S': RollStonesSouthEast(direction); break;
        case 'E': RollStonesSouthEast(direction); break;
    }
}

void RollStonesNorthWest(char direction)
{
    for(int i = 0; i < map.GetLength(0); i++)
    {
        for(int j = 0; j < map.GetLength(1); j++)
        {
            if(map[i, j] == 'O')
            {
                if(direction == 'N')
                {
                    for(int x = 1; i - x >= 0; x++)
                    {
                        if(map[i - x, j] == '.')
                        {
                            map[i - x, j] = 'O';
                            map[i - x + 1, j] = '.';
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                else if(direction == 'W')
                {
                    for(int x = 1; j - x >= 0; x++)
                    {
                        if(map[i, j - x] == '.')
                        {
                            map[i, j - x] = 'O';
                            map[i, j - x + 1] = '.';
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
        }
    }
}

void RollStonesSouthEast(char direction)
{
    for(int i = map.GetLength(0) - 1; i >= 0; i--)
    {
        for(int j = map.GetLength(1) - 1; j >= 0; j--)
        {
            if(map[i, j] == 'O')
            {
                if(direction == 'S')
                {
                    for(int x = 1; i + x < map.GetLength(0); x++)
                    {
                        if(map[i + x, j] == '.')
                        {
                            map[i + x, j] = 'O';
                            map[i + x - 1, j] = '.';
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                else if(direction == 'E')
                {
                    for(int x = 1; j + x < map.GetLength(1); x++)
                    {
                        if(map[i, j + x] == '.')
                        {
                            map[i, j + x] = 'O';
                            map[i, j + x - 1] = '.';
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
        }
    }
}