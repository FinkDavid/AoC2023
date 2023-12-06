string[] lines = File.ReadAllLines("../input.txt");

string[] splitTimesString = lines[0].Split(':');
long time = long.Parse(splitTimesString[1].Replace(" ", ""));

string[] splitDistancesString = lines[1].Split(':');
long distance = long.Parse(splitDistancesString[1].Replace(" ", ""));

int timesBeaten = 0;
for(long i = 0; i < time; i++)
{
    long currentDistance = i * (time - i);
    if(currentDistance > distance)
    {
        timesBeaten++;
    }
}

Console.WriteLine("Result: " + timesBeaten);