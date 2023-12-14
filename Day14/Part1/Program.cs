string[] lines = File.ReadAllLines("../input.txt");
List<List<char>> map = new List<List<char>>();

int index = 0;
foreach(string l in lines)
{
    map.Add(new List<char>());
    foreach(char c in l)
    {
        map[index].Add(c);
    }   
    index++;
}

for(int i = 0; i < map.Count; i++)
{
    for(int j = 0; j < map[i].Count; j++)
    {
        if(map[i][j] == 'O')
        {
            int test = i;
            for(int x = 1; test - x >= 0; x++)
            {
                if(map[i - x][j] == '.')
                {
                    map[i - x][j] = 'O';
                    map[i - x + 1][j] = '.';
                }
                else
                {
                    break;
                }
            }
        }
    }
}

int result = CalculateResult();
Console.WriteLine("Result: " + result);

int CalculateResult()
{
    int res = 0;
    int scoreCurentLine = map.Count;
    for(int i = 0; i < map.Count; i++)
    {
        for(int j = 0; j < map[i].Count; j++)
        {
            if(map[i][j] == 'O')
            {
                res += scoreCurentLine;
            }
        }
        scoreCurentLine--;
    }

    return res;
}