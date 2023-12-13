string[] lines = File.ReadAllLines("../input.txt");
List<List<string>> linesPerMap = new List<List<string>>
{
    new List<string>()
};

int currentMap = 0;
foreach(string l in lines)
{
    if(string.IsNullOrWhiteSpace(l))
    {
        linesPerMap.Add(new List<string>());
        currentMap++;
    }
    else
    {
        linesPerMap[currentMap].Add(l);
    }
}


int result = 0;
for(int i = 0; i < linesPerMap.Count; i++)
{
    int reflection = CheckHorizontal(linesPerMap[i]);
    int reflectionSmudges = 0;

    if(reflection == -1)
    {
        reflectionSmudges = CheckHorizontalSmudges(linesPerMap[i], 50000);
    }   
    else
    {
        reflectionSmudges = CheckHorizontalSmudges(linesPerMap[i], reflection);
    }
    

    if(reflectionSmudges == -1)
    {
        reflection = CheckVertical(linesPerMap[i]);
        reflectionSmudges = CheckVerticalSmudges(linesPerMap[i], reflection);
        result += reflectionSmudges;
    }
    else
    {
        result += reflectionSmudges * 100;
    }
}

Console.WriteLine("Result: " + result);

int CheckHorizontalSmudges(List<string> map, int oldReflection)
{
    for(int i = 0; i < map.Count - 1; i++)
    {
        bool areRowsSame = true;
        bool smudgeUsed = false;
        for(int x = 0; x < map[i].Length; x++)
        {
            if(map[i][x] != map[i + 1][x])
            {
                if(smudgeUsed)
                {
                    areRowsSame = false;
                    break;
                }  
                smudgeUsed = true;
            }
        }

        if(areRowsSame)
        {
            int smallestDistance;
            int lowerDistance = i;
            int upperDistance = map.Count - 1 - i - 1;

            smallestDistance = lowerDistance;

            if(upperDistance < lowerDistance)
            {
                smallestDistance = upperDistance;
            }

            bool foundHorizontal = true;
            for(int j = 1; j <= smallestDistance; j++)
            {
                for(int x = 0; x < map[i-j].Length; x++)
                {
                    if(map[i-j][x] != map[i + j + 1][x])
                    {
                        if(smudgeUsed == true)
                        {
                            foundHorizontal = false;
                            break;
                        }  
                        smudgeUsed = true;
                    }
                }
            }

            if(foundHorizontal)
            {
                if(i + 1 != oldReflection)
                {
                    return i + 1;
                }
            }
        }
    }
    return -1;
}

int CheckVerticalSmudges(List<string> map, int oldReflection)
{
    List<string> flippedMap = FlipMap(map);
    return CheckHorizontalSmudges(flippedMap, oldReflection);
}

int CheckHorizontal(List<string> map)
{
    for(int i = 0; i < map.Count - 1; i++)
    {
        if(map[i] == map[i+1])
        {
            int smallestDistance;
            int lowerDistance = i;
            int upperDistance = map.Count - 1 - i - 1;

            smallestDistance = lowerDistance;

            if(upperDistance < lowerDistance)
            {
                smallestDistance = upperDistance;
            }

            bool foundHorizontal = true;

            for(int j = 0; j <= smallestDistance; j++)
            {
                if(map[i - j] != map[i + j + 1])
                {
                    
                    foundHorizontal = false;
                    break;
                }
            }

            if(foundHorizontal)
            {
                return i + 1;
            }
        }
    }

    return -1;
}

int CheckVertical(List<string> map)
{
    List<string> flippedMap = FlipMap(map);
    return CheckHorizontal(flippedMap);
}

List<string> FlipMap(List<string> map)
{
    int numRows = map.Count;
    int numCols = map[0].Length;

    List<string> flippedMap = new List<string>();

    for (int col = 0; col < numCols; col++)
    {
        string newRow = "";
        for (int row = numRows - 1; row >= 0; row--)
        {
            newRow += map[row][col];
        }
        flippedMap.Add(newRow);
    }

    return flippedMap;
}

//37416