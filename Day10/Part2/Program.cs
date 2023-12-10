using System.Numerics;

string[] lines = File.ReadAllLines("../input.txt");

Tile[,] map = new Tile[lines.Length, lines[0].Length];
char[,] mapDoubled = new char[lines.Length, lines[0].Length * 2];
Vector2 startPosition = new Vector2(20 * 2, 103 * 2);

for(int i = 0; i < lines.Length; i++)
{
    for(int j = 0; j < lines[0].Length; j++)
    {
        char symbol = lines[i][j];
        List<PipeOpenings> openings = new List<PipeOpenings>();
        map[i, j] = new Tile(symbol, openings);
    }
}

//Double map size horizontally
for(int i = 0; i < lines.Length; i++)
{
    for(int j = 0; j < lines[0].Length; j++)
    {
        char symbol = map[i, j].symbol;
        switch(symbol)
        {
            case '.': mapDoubled[i,j * 2] = '.';  mapDoubled[i, j * 2 + 1] = '.'; break;
            case '|': mapDoubled[i,j * 2] = '|';  mapDoubled[i, j * 2 + 1] = '.'; break;
            case '-': mapDoubled[i,j * 2] = '-';  mapDoubled[i, j * 2 + 1] = '-'; break;
            case 'F': mapDoubled[i,j * 2] = 'F';  mapDoubled[i, j * 2 + 1] = '-'; break;
            case '7': mapDoubled[i,j * 2] = '7';  mapDoubled[i, j * 2 + 1] = '.'; break;
            case 'J': mapDoubled[i,j * 2] = 'J';  mapDoubled[i, j * 2 + 1] = '.'; break;
            case 'L': mapDoubled[i,j * 2] = 'L';  mapDoubled[i, j * 2 + 1] = '-'; break;
            case 'S': mapDoubled[i,j * 2] = '7';  mapDoubled[i, j * 2 + 1] = '.'; break;
        }
    }
}

char[,] mapDoubled2 = new char[lines.Length * 2, lines[0].Length * 2];

//Double map size vertically
for(int i = 0; i < mapDoubled.GetLength(0); i++)
{
    for(int j = 0; j < mapDoubled.GetLength(1); j++)
    {
        char symbol = mapDoubled[i, j];
        switch(symbol)
        {
            case '.': mapDoubled2[i * 2,j] = '.';  mapDoubled2[i* 2 + 1, j ] = '.'; break;
            case '|': mapDoubled2[i * 2,j] = '|';  mapDoubled2[i* 2 + 1, j] = '|'; break;
            case '-': mapDoubled2[i * 2,j] = '-';  mapDoubled2[i* 2 + 1, j] = '.'; break;
            case 'F': mapDoubled2[i * 2,j] = 'F';  mapDoubled2[i* 2 + 1, j] = '|'; break;
            case '7': mapDoubled2[i * 2,j] = '7';  mapDoubled2[i* 2 + 1, j] = '|'; break;
            case 'J': mapDoubled2[i * 2,j] = 'J';  mapDoubled2[i* 2 + 1, j] = '.'; break;
            case 'L': mapDoubled2[i * 2,j] = 'L';  mapDoubled2[i* 2 + 1, j] = '.'; break;
            case 'S': mapDoubled2[i * 2,j] = '7';  mapDoubled2[i* 2 + 1, j] = '|'; break;
        }
    }
}

//Now do the same as in Part1
Tile[,] mapBigger = new Tile[mapDoubled2.GetLength(0), mapDoubled2.GetLength(1)];
for(int i = 0; i < mapDoubled2.GetLength(0); i++)
{
    for(int j = 0; j < mapDoubled2.GetLength(1); j++)
    {
        char symbol = mapDoubled2[i, j];
        List<PipeOpenings> openings = new List<PipeOpenings>();
        switch(symbol)
        {
            case '|': openings.Add(PipeOpenings.Up); openings.Add(PipeOpenings.Down); break;
            case '-': openings.Add(PipeOpenings.Left); openings.Add(PipeOpenings.Right); break;
            case 'F': openings.Add(PipeOpenings.Right); openings.Add(PipeOpenings.Down); break;
            case '7': openings.Add(PipeOpenings.Down); openings.Add(PipeOpenings.Left); break;
            case 'J': openings.Add(PipeOpenings.Left); openings.Add(PipeOpenings.Up); break;
            case 'L': openings.Add(PipeOpenings.Up); openings.Add(PipeOpenings.Right); break;
        }
        mapBigger[i, j] = new Tile(symbol, openings);
    }
}

//Go along the connected pipe starting at S
bool loopCompleted = false;
Vector2 currentPosition = startPosition;
PipeOpenings sideComingFrom = PipeOpenings.Down;
while(!loopCompleted)
{
    foreach(PipeOpenings opening in mapBigger[(int)currentPosition.X, (int)currentPosition.Y].pipeOpenings)
    {
        if(opening != sideComingFrom)
        {
            switch(opening)
            {
                case PipeOpenings.Up: currentPosition.X -= 1; break;
                case PipeOpenings.Right: currentPosition.Y += 1; break;
                case PipeOpenings.Down: currentPosition.X += 1; break;
                case PipeOpenings.Left: currentPosition.Y -= 1; break;
            }
            mapBigger[(int)currentPosition.X, (int)currentPosition.Y].symbol = '#';
            sideComingFrom = getOppositeSide(opening);
            break;
        }
    }

    if(currentPosition == startPosition)
    {
        loopCompleted = true;
    }
}

char[,] mapBiggerChar = new char[mapBigger.GetLength(0), mapBigger.GetLength(1)];
for(int i = 0; i < mapBiggerChar.GetLength(0); i++)
{
    for(int j = 0; j < mapBiggerChar.GetLength(1); j++)
    {
        mapBiggerChar[i, j] = mapBigger[i, j].symbol;
    }
}

//Replace all unnecessary pipes with empty space
for(int i = 0; i < mapBiggerChar.GetLength(0); i++)
{
    for(int j = 0; j < mapBiggerChar.GetLength(1); j++)
    {
        if(mapBiggerChar[i, j] != '#')
        {
            mapBiggerChar[i, j] = '.';
        }
    }
}

//Flood map with Water 'W' to discover all spaces that are fully covered by the remaining pipes aka: They are inside the pipe loop
FloodFill(mapBiggerChar);

//Now remove every 2nd row and every 2nd column again to go back to the normal sized map
char[,] finalMap = new char[lines.Length, lines[0].Length];
for(int i = 0; i < finalMap.GetLength(0); i++)
{
    for(int j = 0; j < finalMap.GetLength(1); j++)
    {
        finalMap[i, j] = mapBiggerChar[i * 2, j * 2];
    }
}

//Count all remaining empty spaces
int result = 0;
for(int i = 0; i < finalMap.GetLength(0); i++)
{
    for(int j = 0; j < finalMap.GetLength(1); j++)
    {
        if(finalMap[i, j] == 'V')
        {
            result++;
        }
    }
}

Console.WriteLine("Result: " + result);

static void FloodFill(char[,] map)
{
    int rows = map.GetLength(0);
    int cols = map.GetLength(1);

    for (int i = 0; i < rows; i++)
    {
        for (int j = 0; j < cols; j++)
        {
            if (map[i, j] == '.' && IsConnectedToBoundary(map, i, j))
            {
                Flood(map, i, j);
            }
        }
    }
}

static bool IsConnectedToBoundary(char[,] map, int row, int col)
{
    int rows = map.GetLength(0);
    int cols = map.GetLength(1);

    if (row == 0 || row == rows - 1 || col == 0 || col == cols - 1)
    {
        return true;
    }

    return IsConnectedToBoundaryHelper(map, row, col);
}

static bool IsConnectedToBoundaryHelper(char[,] map, int row, int col)
{
    int rows = map.GetLength(0);
    int cols = map.GetLength(1);

    if (row < 0 || col < 0 || row >= rows || col >= cols || map[row, col] != '.')
    {
        return false;
    }

    map[row, col] = 'V';

    bool connected = IsConnectedToBoundaryHelper(map, row - 1, col) ||
                        IsConnectedToBoundaryHelper(map, row + 1, col) ||
                        IsConnectedToBoundaryHelper(map, row, col - 1) ||
                        IsConnectedToBoundaryHelper(map, row, col + 1);

    return connected;
}

static void Flood(char[,] map, int row, int col)
{
    int rows = map.GetLength(0);
    int cols = map.GetLength(1);

    if (row < 0 || col < 0 || row >= rows || col >= cols || map[row, col] != '.')
    {
        return;
    }

    map[row, col] = 'W';

    Flood(map, row - 1, col);
    Flood(map, row + 1, col);
    Flood(map, row, col - 1);
    Flood(map, row, col + 1);
}

PipeOpenings getOppositeSide(PipeOpenings opening)
{
    switch(opening)
    {
        case PipeOpenings.Up: return PipeOpenings.Down;
        case PipeOpenings.Right: return PipeOpenings.Left;
        case PipeOpenings.Down: return PipeOpenings.Up;
        case PipeOpenings.Left: return PipeOpenings.Right;
    }

    return PipeOpenings.Up;
}

class Tile
{
    public char symbol;
    public List<PipeOpenings> pipeOpenings = new List<PipeOpenings>();

    public Tile(char symb, List<PipeOpenings> openings)
    {
        symbol = symb;
        pipeOpenings = openings;
    }
}

enum PipeOpenings
{
    Up,
    Right,
    Down,
    Left
}