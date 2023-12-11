using System.Numerics;

string[] lines = File.ReadAllLines("../input.txt");

List<List<bool>> galaxy = new List<List<bool>>();
for(int i = 0; i < lines.Length; i++)
{
    galaxy.Add(new List<bool>());
    for(int j = 0; j < lines[0].Length; j++)
    {
        if(lines[i][j] == '#')
        {
            galaxy[i].Add(true);
        }
        else
        {
            galaxy[i].Add(false);
        }
    }
}

List<int> rowIndexes = new List<int>();
List<int> columnIndexes = new List<int>();
ExtendGalaxy();

Dictionary<int, Vector2> galaxyPositions = new Dictionary<int, Vector2>();
int galaxyID = 1;
for(int i = 0; i < galaxy.Count; i++)
{
    for(int j = 0; j < galaxy[i].Count; j++)
    {
        if(galaxy[i][j])
        {
            galaxyPositions.Add(galaxyID, new Vector2(i, j));
            galaxyID++;
        }
    }
}

int rowsToAdd = 1000000 - 1;
long result = 0;
int id = 2;
for (int i = 1; i <= galaxyPositions.Count; i++)
{
    for (int j = id; j <= galaxyPositions.Count; j++)
    {
        int amount = 0;
        foreach(int row in rowIndexes)
        {
            if((row > galaxyPositions[i].X && row < galaxyPositions[j].X) || (row > galaxyPositions[j].X && row < galaxyPositions[i].X))
            {
                amount++;
            }
        }

        foreach(int column in columnIndexes)
        {
            if((column > galaxyPositions[i].Y && column < galaxyPositions[j].Y) || (column > galaxyPositions[j].Y && column < galaxyPositions[i].Y))
            {
                amount++;
            }
        }

        float normalDistance = Math.Abs(galaxyPositions[i].X - galaxyPositions[j].X) + Math.Abs(galaxyPositions[i].Y - galaxyPositions[j].Y);
        float finalDistance = rowsToAdd * amount + normalDistance;
        result += (long)finalDistance;
    }
    id++;
}

Console.WriteLine("Result: " + result);

void ExtendGalaxy()
{
    //Extend horizontally
    for(int i = galaxy.Count - 1; i >= 0; i--)
    {
        bool rowHasGalaxy = false;
        for(int j = 0; j < galaxy[i].Count; j++)
        {
            if(galaxy[i][j])
            {
                rowHasGalaxy = true;
                break;
            }
        }

        if(!rowHasGalaxy)
        {
            rowIndexes.Add(i);
        }
    }

    //Extend vertically
    for (int j = galaxy[0].Count - 1; j >= 0; j--)
    {
        bool columnHasGalaxy = false;
        for (int i = 0; i < galaxy.Count; i++)
        {
            if (galaxy[i][j])
            {
                columnHasGalaxy = true;
                break;
            }
        }

        if (!columnHasGalaxy)
        {
            columnIndexes.Add(j);
        }
    }
}