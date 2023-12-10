using System.Numerics;

string[] lines = File.ReadAllLines("../input.txt");

Tile[,] map = new Tile[lines.Length, lines[0].Length];

Vector2 startPosition = new Vector2(0, 0);

for(int i = 0; i < lines.Length; i++)
{
    for(int j = 0; j < lines[0].Length; j++)
    {
        char symbol = lines[i][j];
        List<PipeOpenings> openings = new List<PipeOpenings>();
        switch(symbol)
        {
            case '|': openings.Add(PipeOpenings.Up); openings.Add(PipeOpenings.Down); break;
            case '-': openings.Add(PipeOpenings.Left); openings.Add(PipeOpenings.Right); break;
            case 'F': openings.Add(PipeOpenings.Right); openings.Add(PipeOpenings.Down); break;
            case '7': openings.Add(PipeOpenings.Down); openings.Add(PipeOpenings.Left); break;
            case 'J': openings.Add(PipeOpenings.Left); openings.Add(PipeOpenings.Up); break;
            case 'L': openings.Add(PipeOpenings.Up); openings.Add(PipeOpenings.Right); break;
            case 'S': symbol = '7'; openings.Add(PipeOpenings.Down); openings.Add(PipeOpenings.Left); startPosition = new Vector2(i, j); break;
        }
        map[i, j] = new Tile(symbol, openings);
    }
}

bool loopCompleted = false;
int steps = 0;
Vector2 currentPosition = startPosition;
PipeOpenings sideComingFrom = PipeOpenings.Down;

while(!loopCompleted)
{
    foreach(PipeOpenings opening in map[(int)currentPosition.X, (int)currentPosition.Y].pipeOpenings)
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
            map[(int)currentPosition.X, (int)currentPosition.Y].symbol = '#';
            sideComingFrom = getOppositeSide(opening);
            break;
        }
    }

    if(currentPosition == startPosition)
    {
        loopCompleted = true;
    }
    steps++;
}

float furthestPipeAway = steps / 2;

Console.WriteLine("Steps: " + furthestPipeAway);

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