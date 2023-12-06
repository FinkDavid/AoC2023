string[] lines = File.ReadAllLines("../input.txt");

List<int> times = new List<int>();
List<int> distances = new List<int>();

Dictionary<int, int> records = new Dictionary<int, int>();

string[] splitTimesString = lines[0].Split(':');
string[] timesString = splitTimesString[1].Split(' ');
times = GetNumberList(timesString);

string[] splitDistancesString = lines[1].Split(':');
string[] distancesString = splitDistancesString[1].Split(' ');
distances = GetNumberList(distancesString);

for(int i = 0; i < times.Count; i++)
{
    records.Add(times[i], distances[i]);
}

List<int> timesBeatenPerRun = new List<int>();

foreach(KeyValuePair<int, int> record in records)
{
    int timesBeaten = 0;
    for(int i = 0; i < record.Key; i++)
    {
        int distance = i * (record.Key - i);
        if(distance > record.Value)
        {
            timesBeaten++;
        }
    }
    timesBeatenPerRun.Add(timesBeaten);
}

int result = timesBeatenPerRun[0];
for(int i = 1; i < timesBeatenPerRun.Count; i++)
{
    result *= timesBeatenPerRun[i];
}

Console.WriteLine("Result: " + result);

List<int> GetNumberList(string[] numbers)
{
    List<int> numberList = new List<int>();

    foreach(string number in numbers)
    {
        int i = 0;
        if(int.TryParse(number, out i))
        {
            numberList.Add(i);
        }
    }

    return numberList;
}