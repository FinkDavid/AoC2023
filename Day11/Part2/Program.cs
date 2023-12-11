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



foreach(int row in rowIndexes)
{
    Console.WriteLine(row);
}

foreach(int column in columnIndexes)
{
    Console.WriteLine(column);
}

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

char[,] map = new char[galaxy.Count, galaxy[0].Count];
galaxyID = 1;
for(int i = 0; i < galaxy.Count; i++)
{
    for(int j = 0; j < galaxy[0].Count; j++)
    {
        if(galaxy[i][j])
        {
            map[i, j] = Convert.ToChar(galaxyID.ToString());
            galaxyID++;
        }
        else
        {
            map[i, j] = '.';
        }
    }
}

string filePath = "output.txt";

// Save the char[,] array to a text file
SaveCharArrayToFile(map, filePath);

int rowsToAdd = 100 - 1;
float result = 0;
int id = 2;
for (int i = 1; i <= galaxyPositions.Count; i++)
{
    for (int j = id; j <= galaxyPositions.Count; j++)
    {
        int amount = 0;
        foreach(int row in rowIndexes)
        {
            if((row >= galaxyPositions[i].X && row <= galaxyPositions[j].X) || (row >= galaxyPositions[j].X && row <= galaxyPositions[i].X))
            {
                amount++;
            }
        }

        foreach(int column in columnIndexes)
        {
            if((column >= galaxyPositions[i].Y && column <= galaxyPositions[j].Y) || (column >= galaxyPositions[j].Y && column <= galaxyPositions[i].Y))
            {
                amount++;
            }
        }

        Console.WriteLine("Amount: " + amount + " between: " +   i + " and " + j);
        result += Math.Abs((long)galaxyPositions[i].X - (long)galaxyPositions[j].X) + Math.Abs((long)galaxyPositions[i].Y - (long)galaxyPositions[j].Y) + (rowsToAdd * amount);
    }
    id++;
}

Console.WriteLine("Result: " + result);

static void SaveCharArrayToFile(char[,] charArray, string filePath)
{
    // Get the dimensions of the char array
    int rows = charArray.GetLength(0);
    int cols = charArray.GetLength(1);

    // Create a StreamWriter to write to the file
    using (StreamWriter writer = new StreamWriter(filePath))
    {
        // Iterate through each element in the char array
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                // Write each character to the file
                writer.Write(charArray[i, j]);
            }

            // Write a new line after each row
            writer.WriteLine();
        }
    }
}

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
            /*
            galaxy.Insert(i + 1, new List<bool>());
            for (int k = 0; k < galaxy[i].Count; k++)
            {
                galaxy[i + 1].Add(false);
            }*/
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

//Correct: 632003913611

//Correct with 10: 14233571
//Correct with 100: 71113211