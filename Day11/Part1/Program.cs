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

float result = 0;
int id = 2;
for (int i = 1; i <= galaxyPositions.Count; i++)
{
    for (int j = id; j <= galaxyPositions.Count; j++)
    {
        float distance = Math.Abs(galaxyPositions[i].X - galaxyPositions[j].X) + Math.Abs(galaxyPositions[i].Y - galaxyPositions[j].Y);
        Console.WriteLine("Distance between " + i + " and " + j + ": " + distance);
        result += distance;
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
            galaxy.Insert(i + 1, new List<bool>());
            for (int k = 0; k < galaxy[i].Count; k++)
            {
                galaxy[i + 1].Add(false);
            }
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
            for(int i = 0; i < galaxy.Count; i++)
            {
                galaxy[i].Insert(j + 1, false);
            }
        }
    }
}