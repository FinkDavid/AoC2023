﻿using System.Numerics;

string[] lines = File.ReadAllLines("../input.txt");

List<List<char>> map = new List<List<char>>
{
    new List<char>()
};
map[0].Add('#');
Vector2 currentPosition = new Vector2(0, 0);

foreach(string l in lines)
{
    string[] split = l.Split(' ');
    string direction = split[0];
    int amount = int.Parse(split[1]);

    switch(direction)
    {
        case "R": DigRight(amount); break;
        case "D": DigDown(amount); break;
        case "L": DigLeft(amount); break;
        case "U": DigUp(amount); break;
    }
}

//Add a border so the flood works correctly and then apply the flood algorithm
AddRow(true);
AddRow(false);
AddColumn(true);
AddColumn(false);
FloodFill(map);

for(int i = 0; i < map.Count; i++)
{
    for(int j = 0; j < map[i].Count; j++)
    {
        if(map[i][j] == 'V')
        {
            map[i][j] = '#';
        }
        else if(map[i][j] == 'W')
        {
            map[i][j] = '.';
        }
    }
}

int result = 0;

for(int i = 0; i < map.Count; i++)
{
    for(int j = 0; j < map[i].Count; j++)
    {
        if(map[i][j] == '#')
        {
            result++;
        }
    }
}

Console.WriteLine("Result: " + result);

string filePath = "output.txt";
WriteListToTextFile(map, filePath);

void WriteListToTextFile(List<List<char>> listOfLists, string filePath)
{
    using (StreamWriter writer = new StreamWriter(filePath))
    {
        foreach (List<char> charList in listOfLists)
        {
            foreach (char character in charList)
            {
                writer.Write(character);
            }
            writer.WriteLine();
        }
    }
}

void DigRight(int amount)
{
    int maxRight = (int)currentPosition.X + amount;
    if(maxRight >= map[(int)currentPosition.Y].Count)
    {
        int difference = maxRight - map[(int)currentPosition.Y].Count;
        for(int i = 0; i < difference + 1; i++)
        {
            AddColumn(false);
        }
    }

    for(int i = 0; i < amount; i++)
    {
        currentPosition.X++;
        map[(int)currentPosition.Y][(int)currentPosition.X] = '#';
    }
}

void DigDown(int amount)
{
    int maxDown = (int)currentPosition.Y + amount;
    if(maxDown > map.Count)
    {
        int difference = maxDown - map.Count;
        for(int i = 0; i < difference + 1; i++)
        {
            AddRow(false);
        }
    }

    for(int i = 0; i < amount; i++)
    {
        currentPosition.Y++;
        map[(int)currentPosition.Y][(int)currentPosition.X] = '#';
    }
}

void DigLeft(int amount)
{
    int maxLeft = (int)currentPosition.X - amount;
    if(maxLeft < 0)
    {
        int difference = maxLeft * -1;
        currentPosition.X += difference;
        for(int i = 0; i < difference; i++)
        {
            AddColumn(true);
        }
    }

    for(int i = 0; i < amount; i++)
    {
        currentPosition.X--;
        map[(int)currentPosition.Y][(int)currentPosition.X] = '#';
    }
}

void DigUp(int amount)
{
    int maxUp = (int)currentPosition.Y - amount;
    if(maxUp < 0)
    {
        int difference = maxUp * -1;
        currentPosition.Y += difference;
        for(int i = 0; i < difference; i++)
        {
            AddRow(true);
        }
    }

    for(int i = 0; i < amount; i++)
    {
        currentPosition.Y--;
        map[(int)currentPosition.Y][(int)currentPosition.X] = '#';
    }
}

void AddColumn(bool atStart)
{
    if(atStart)
    {
        foreach(List<char> m in map)
        {
            m.Insert(0, '.');
        }
    }
    else
    {
        foreach(List<char> m in map)
        {
            m.Add('.');
        }
    }
}

void AddRow(bool atStart)
{
    if(atStart)
    {
        map.Insert(0, new List<char>());
        for(int i = 0; i < map[1].Count; i++)
        {
            map[0].Add('.');
        }
    }
    else
    {
        map.Add(new List<char>());
        for(int i = 0; i < map[0].Count; i++)
        {
            map[map.Count - 1].Add('.');
        }
    }
}

void FloodFill(List<List<char>> map)
{
    int rows = map.Count;
    int cols = map[0].Count;

    for (int i = 0; i < rows; i++)
    {
        for (int j = 0; j < cols; j++)
        {
            if (map[i][j] == '.' && IsConnectedToBoundary(map, i, j))
            {
                Flood(map, i, j);
            }
        }
    }
}

bool IsConnectedToBoundary(List<List<char>> map, int row, int col)
{
    int rows = map.Count;
    int cols = map[0].Count;

    if (row == 0 || row == rows - 1 || col == 0 || col == cols - 1)
    {
        return true;
    }

    Queue<(int, int)> queue = new Queue<(int, int)>();
    queue.Enqueue((row, col));

    while (queue.Count > 0)
    {
        var (r, c) = queue.Dequeue();

        if (r < 0 || c < 0 || r >= rows || c >= cols || map[r][c] != '.')
        {
            continue;
        }

        map[r][c] = 'V';

        queue.Enqueue((r - 1, c));
        queue.Enqueue((r + 1, c));
        queue.Enqueue((r, c - 1));
        queue.Enqueue((r, c + 1));
    }

    return false;
}

void Flood(List<List<char>> map, int startRow, int startCol)
{
    int rows = map.Count;
    int cols = map[0].Count;

    Stack<(int, int)> stack = new Stack<(int, int)>();
    stack.Push((startRow, startCol));

    while (stack.Count > 0)
    {
        var (row, col) = stack.Pop();

        if (row < 0 || col < 0 || row >= rows || col >= cols || map[row][col] != '.')
        {
            continue;
        }

        map[row][col] = 'W';

        stack.Push((row - 1, col));
        stack.Push((row + 1, col));
        stack.Push((row, col - 1));
        stack.Push((row, col + 1));
    }
}