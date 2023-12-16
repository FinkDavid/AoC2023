using System.Numerics;

string[] lines = File.ReadAllLines("../input.txt");

char[,] map = new char[lines.Length, lines[0].Length];
char[,] energizedMap = new char[lines.Length, lines[0].Length];
List<Beam> aliveBeams = new List<Beam>
{
    new Beam(new Vector2(0, 0), Direction.Right)
};

for(int i = 0; i < lines.Length; i++)
{
    for(int j = 0; j < lines[0].Length; j++)
    {
        map[i, j] = lines[i][j];
        energizedMap[i, j] = '.';
    }
}

energizedMap[0, 0] = '#';

while(aliveBeams.Count > 0)
{
    for(int i = aliveBeams.Count - 1; i >= 0; i--)
    {
        bool beamRemoved = false;

        switch(map[(int)aliveBeams[i].position.X, (int)aliveBeams[i].position.Y])
        {
            case '\\': aliveBeams[i].ChangeDirection('\\'); break;
            case '/': aliveBeams[i].ChangeDirection('/'); break;
            case '|': HandleImpact('|', i); break;
            case '-': HandleImpact('-', i); break;
        }

        switch(aliveBeams[i].movingDirection)
        {
            case Direction.Up: beamRemoved = MoveUp(i); break;
            case Direction.Right: beamRemoved = MoveRight(i); break;
            case Direction.Down: beamRemoved = MoveDown(i); break;
            case Direction.Left: beamRemoved = MoveLeft(i); break;
        }

        energizedMap[(int)aliveBeams[i].position.X, (int)aliveBeams[i].position.Y] = '#';

        if(beamRemoved)
        {
            aliveBeams.RemoveAt(i);
        }
        else
        {
            if(!aliveBeams[i].AddToPastPositions(aliveBeams[i].position, aliveBeams[i].movingDirection))
            {
                aliveBeams.RemoveAt(i);
            }
        }
    }
}

int result = 0;
for(int i = 0; i < energizedMap.GetLength(0); i++)
{
    for(int j = 0; j < energizedMap.GetLength(1); j++)
    {
        if(energizedMap[i, j] == '#')
        {
            result++;
        }
    }
}
Console.WriteLine("Result: " + result);

void HandleImpact(char symbol, int i)
{
    if(symbol == '|')
    {
        if(aliveBeams[i].movingDirection == Direction.Right || aliveBeams[i].movingDirection == Direction.Left)
        {
            aliveBeams[i].movingDirection = Direction.Up;
            Beam b = new Beam(new Vector2(aliveBeams[i].position.X, aliveBeams[i].position.Y), Direction.Down);
            b.pastPositions = aliveBeams[i].pastPositions;
            aliveBeams.Add(b);
        }
    }
    else if(symbol == '-')
    {
        if(aliveBeams[i].movingDirection == Direction.Up || aliveBeams[i].movingDirection == Direction.Down)
        {
            aliveBeams[i].movingDirection = Direction.Left;
            Beam b = new Beam(new Vector2(aliveBeams[i].position.X, aliveBeams[i].position.Y), Direction.Right);
            b.pastPositions = aliveBeams[i].pastPositions;
            aliveBeams.Add(b);
        }
    }
}

bool MoveUp(int i)
{
    if(aliveBeams[i].position.X == 0)
    {
        return true;
    }
    else
    {
        aliveBeams[i].position.X--;
    }
    return false;
}

bool MoveRight(int i)
{
    if(aliveBeams[i].position.Y == map.GetLength(1) - 1)
    {
        return true;
    }
    else
    {
        aliveBeams[i].position.Y++;
    }
    return false;
}

bool MoveDown(int i)
{
    if(aliveBeams[i].position.X == map.GetLength(0) - 1)
    {
        return true;
    }
    else
    {
        aliveBeams[i].position.X++;
    }
    return false;
}

bool MoveLeft(int i)
{
    if(aliveBeams[i].position.Y == 0)
    {
        return true;
    }
    else
    {
        aliveBeams[i].position.Y--;
    }
    return false;
}

class Beam
{
    public Vector2 position;
    public Direction movingDirection;
    public List<PastPosition> pastPositions = new List<PastPosition>();

    public struct PastPosition
    {
        public Vector2 position;
        public Direction movingDirection;

        public PastPosition(Vector2 pos, Direction direction)
        {
            position = pos;
            movingDirection = direction;
        }
    }

    public Beam(Vector2 pos, Direction direction)
    {
        position = pos;
        movingDirection = direction;
    }

    public bool AddToPastPositions(Vector2 pos, Direction direction)
    {
        for(int i = 0; i < pastPositions.Count; i++)
        {
            if(pastPositions[i].position == pos && pastPositions[i].movingDirection == direction)
            {
                return false;
            }
        }
        pastPositions.Add(new PastPosition(pos, direction));
        return true;
    }

    public void ChangeDirection(char symbol)
    {
        if(symbol == '\\')
        {
            switch(movingDirection)
            {
                case Direction.Up: movingDirection = Direction.Left; break;
                case Direction.Right: movingDirection = Direction.Down; break;
                case Direction.Down: movingDirection = Direction.Right; break;
                case Direction.Left: movingDirection = Direction.Up; break;
            }
        }
        else if(symbol == '/')
        {
            switch(movingDirection)
            {
                case Direction.Up: movingDirection = Direction.Right; break;
                case Direction.Right: movingDirection = Direction.Up; break;
                case Direction.Down: movingDirection = Direction.Left; break;
                case Direction.Left: movingDirection = Direction.Down; break;
            }
        }
    }
}

enum Direction
{
    Up,
    Right,
    Down,
    Left
}