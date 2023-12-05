string[] lines = File.ReadAllLines("../input.txt");

string[] splitString = lines[0].Split(':');
string[] seedsString = splitString[1].Split(' ');

List<uint> seedsuint = new List<uint>();
foreach (string number in seedsString)
{
    if (uint.TryParse(number, out uint i))
    {
        seedsuint.Add(i);
    }
}

Dictionary<CurrentCategory, List<Map>> mappings = new Dictionary<CurrentCategory, List<Map>>
{
    { CurrentCategory.seedToSoil, new List<Map>() },
    { CurrentCategory.soilToFertilizer, new List<Map>() },
    { CurrentCategory.fertilizerToWater, new List<Map>() },
    { CurrentCategory.waterToLight, new List<Map>() },
    { CurrentCategory.lightToTemperature, new List<Map>() },
    { CurrentCategory.temperatureToHumidity, new List<Map>() },
    { CurrentCategory.humidityToLocation, new List<Map>() }
};

CurrentCategory currentCategory = CurrentCategory.None;

foreach (string l in lines)
{
    switch (currentCategory)
    {
        case CurrentCategory.seedToSoil: addLineToList(l, mappings[CurrentCategory.seedToSoil]); break;
        case CurrentCategory.soilToFertilizer: addLineToList(l, mappings[CurrentCategory.soilToFertilizer]); break;
        case CurrentCategory.fertilizerToWater: addLineToList(l, mappings[CurrentCategory.fertilizerToWater]); break;
        case CurrentCategory.waterToLight: addLineToList(l, mappings[CurrentCategory.waterToLight]); break;
        case CurrentCategory.lightToTemperature: addLineToList(l, mappings[CurrentCategory.lightToTemperature]); break;
        case CurrentCategory.temperatureToHumidity: addLineToList(l, mappings[CurrentCategory.temperatureToHumidity]); break;
        case CurrentCategory.humidityToLocation: addLineToList(l, mappings[CurrentCategory.humidityToLocation]); break;
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
Seed seed = new Seed(0); // Reuse the Seed object
for (int j = 0; j < seedsuint.Count; j += 2)
{
    uint seedStart = seedsuint[j];
    uint seedEnd = seedStart + seedsuint[j + 1];

    Console.WriteLine(j + "   " + seedStart);
    for (uint i = seedStart; i < seedEnd; i++)
    {
        seed.seed = i;

        seed.soil = getMappingWithGivenList(seed.seed, mappings[CurrentCategory.seedToSoil]);
        seed.fertilizer = getMappingWithGivenList(seed.soil, mappings[CurrentCategory.soilToFertilizer]);
        seed.water = getMappingWithGivenList(seed.fertilizer, mappings[CurrentCategory.fertilizerToWater]);
        seed.light = getMappingWithGivenList(seed.water, mappings[CurrentCategory.waterToLight]);
        seed.temperature = getMappingWithGivenList(seed.light, mappings[CurrentCategory.lightToTemperature]);
        seed.humidity = getMappingWithGivenList(seed.temperature, mappings[CurrentCategory.temperatureToHumidity]);
        seed.location = getMappingWithGivenList(seed.humidity, mappings[CurrentCategory.humidityToLocation]);

        if (seed.location < minLocation)
        {
            minLocation = seed.location;
        }
    }
}

Console.WriteLine("Result: " + minLocation);

void addLineToList(string line, List<Map> list)
{
    if (!string.IsNullOrEmpty(line) && char.IsDigit(line[0]))
    {
        string[] lineSplit = line.Split(' ');
        list.Add(new Map(uint.Parse(lineSplit[0]), uint.Parse(lineSplit[1]), uint.Parse(lineSplit[2])));
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