using System.Diagnostics.CodeAnalysis;

string[] lines = File.ReadAllLines("../input.txt");

string[] splitString = lines[0].Split(':');
string[] seedsString = splitString[1].Split(' ');
List<uint> seedsuint = getNumberList(seedsString);

List<Seed> seeds = new List<Seed>();

foreach(uint seed in seedsuint)
{
    seeds.Add(new Seed(seed));
}

CurrentCategory currentCategory = CurrentCategory.None;
List<Map> soilMapping = new List<Map>();
List<Map> fertilizerMapping = new List<Map>();
List<Map> waterMapping = new List<Map>();
List<Map> lightMapping = new List<Map>();
List<Map> temperatureMapping = new List<Map>();
List<Map> humidityMapping = new List<Map>();
List<Map> locationMapping = new List<Map>();

foreach (string l in lines)
{
    switch(currentCategory)
    {
        case CurrentCategory.seedToSoil: addLineToList(l, ref soilMapping); break;
        case CurrentCategory.soilToFertilizer: addLineToList(l, ref fertilizerMapping); break;
        case CurrentCategory.fertilizerToWater: addLineToList(l, ref waterMapping); break;
        case CurrentCategory.waterToLight: addLineToList(l, ref lightMapping); break;
        case CurrentCategory.lightToTemperature: addLineToList(l, ref temperatureMapping); break;
        case CurrentCategory.temperatureToHumidity: addLineToList(l, ref humidityMapping); break;
        case CurrentCategory.humidityToLocation: addLineToList(l, ref locationMapping); break;
    }

    if(l.Contains("seed-to-soil map"))
    {
        currentCategory = CurrentCategory.seedToSoil;
    }
    else if(l.Contains("soil-to-fertilizer map"))
    {
        currentCategory = CurrentCategory.soilToFertilizer;
    }
    else if(l.Contains("fertilizer-to-water map"))
    {
        currentCategory = CurrentCategory.fertilizerToWater;
    }
    else if(l.Contains("water-to-light map"))
    {
        currentCategory = CurrentCategory.waterToLight;
    }
    else if(l.Contains("light-to-temperature map"))
    {
        currentCategory = CurrentCategory.lightToTemperature;
    }
    else if(l.Contains("temperature-to-humidity map"))
    {
        currentCategory = CurrentCategory.temperatureToHumidity;
    }
    else if(l.Contains("humidity-to-location map"))
    {
        currentCategory = CurrentCategory.humidityToLocation;
    }
}

uint minLocation = uint.MaxValue;
foreach(Seed seed in seeds)
{
    seed.soil = getMappingWithGivenList(seed.seed, soilMapping);
    seed.fertilizer = getMappingWithGivenList(seed.soil, fertilizerMapping);
    seed.water = getMappingWithGivenList(seed.fertilizer, waterMapping);
    seed.light = getMappingWithGivenList(seed.water, lightMapping);
    seed.temperature = getMappingWithGivenList(seed.light, temperatureMapping);
    seed.humidity = getMappingWithGivenList(seed.temperature, humidityMapping);
    seed.location = getMappingWithGivenList(seed.humidity, locationMapping);
    if(seed.location < minLocation)
    {
        minLocation = seed.location;
    }
}

Console.WriteLine("Result: " + minLocation);

List<uint> getNumberList(string[] numbers)
{
    List<uint> numberList = new List<uint>();

    foreach(string number in numbers)
    {
        uint i = 0;
        if(uint.TryParse(number, out i))
        {
            numberList.Add(i);
        }
    }

    return numberList;
}

void addLineToList(string line, ref List<Map> list)
{
    if(string.IsNullOrEmpty(line) == false)
    {
        if(char.IsDigit(line[0]))
        {
            string[] lineSplit = line.Split(' ');
            list.Add(new Map(uint.Parse(lineSplit[0]), uint.Parse(lineSplit[1]), uint.Parse(lineSplit[2])));
        }
    }
}

uint getMappingWithGivenList(uint seed, List<Map> mappingList)
{
    uint mappingNumber = seed;
    foreach(Map m in mappingList)
    {
        if(seed <= m.startSource + m.length && seed >= m.startSource)
        {
            //Seed has to get a new mappingNumber cause he is between start and end of mapping
            return m.getMapping(seed);
        }
    }
    return mappingNumber;
}

enum CurrentCategory
{
    None,
    seedToSoil,
    soilToFertilizer,
    fertilizerToWater,
    waterToLight,
    lightToTemperature,
    temperatureToHumidity,
    humidityToLocation
}

class Seed
{
    public uint seed;
    public uint soil;
    public uint fertilizer;
    public uint water;
    public uint light;
    public uint temperature;
    public uint humidity;
    public uint location;
    
    public Seed(uint seed)
    {
        this.seed = seed;
    }
}

class Map
{
    public uint destinationSource;
    public uint startSource;
    public uint length;

    public Map(uint dS, uint sS, uint l)
    {
        destinationSource = dS;
        startSource = sS;
        length = l;
    }

    public uint getMapping(uint seed)
    {
        uint offset = seed - startSource;
        return destinationSource + offset;
    }
}