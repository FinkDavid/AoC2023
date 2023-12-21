string[] lines = File.ReadAllLines("../input.txt");

Dictionary<string, Module> modules = new Dictionary<string, Module>();
Queue<(string from, string targetModule, bool signal)> queue = new Queue<(string, string, bool)>();

foreach(string l in lines)
{
    string line = l.Replace(">", string.Empty).Replace(" ", string.Empty);
    string[] split = line.Split('-');
    string origin = split[0];
    string[] targets = split[1].Split(',');
    List<string> moduleTargets = new List<string>();
    for(int i = 0; i < targets.Length; i++)
    {
        moduleTargets.Add(targets[i]);
    }

    if(origin.Contains('%'))
    {
        origin = origin.Replace("%", "");
        modules.Add(origin, new Module(origin, '%', moduleTargets));
    }
    else if(origin.Contains('&'))
    {
        origin = origin.Replace("&", "");
        modules.Add(origin, new Module(origin, '&', moduleTargets));
    }
    else
    {
        modules.Add(origin, new Module(origin, ' ', moduleTargets));
    }
}


foreach(var kvp in modules)
{
    if(kvp.Value.moduleType == Type.Conjunction)
    {
        string name = kvp.Key;
        foreach(var kvp2 in modules)
        {
            if(kvp2.Value.moduleTargets.Contains(name))
            {
                kvp.Value.gettingSignalsFrom.Add(kvp2.Value.moduleName);
            }
        }

        for(int i = 0; i < kvp.Value.gettingSignalsFrom.Count; i++)
        {
            kvp.Value.status.Add(false);
        }
    }
}



int totalHighSignals = 0;
int totalLowSignals = 0;
bool found = false;
int result = 0;
for(int i = 0; i < 1000; i++)
{
    totalLowSignals++;
    queue.Enqueue(("button", "broadcaster", false));
    while(queue.Count > 0)
    {
        var line = queue.Dequeue();
        //Console.WriteLine(line.targetModule);
        if(line.targetModule != "rx")
        {
            modules[line.targetModule].HandleSignal(line.from, line.signal, ref queue, ref totalHighSignals, ref totalLowSignals);
        }
        else
        {
            if(line.signal == false)
            {
                Console.WriteLine("NOW");
                found = true;
                break;
            }
        }
    }
    if(found)
    {
        break;
    }
    result++;
    //Console.WriteLine();
    //Console.WriteLine();
}

Console.WriteLine("TotalHighSignals: " + totalHighSignals);
Console.WriteLine("TotalLowSignals: " + totalLowSignals);
Console.WriteLine("Result: " + result);

class Module
{
    public string moduleName;
    public Type moduleType;
    public List<bool> status = new List<bool>();
    public List<string> moduleTargets;
    public List<string> gettingSignalsFrom = new List<string>();

    public Module(string name, char type, List<string> targets)
    {
        moduleName = name;
        moduleTargets = targets;
        switch(type)
        {
            case ' ': moduleType = Type.Default; status.Add(false); break;
            case '%': moduleType = Type.FlipFlop; status.Add(false); break;
            case '&': moduleType = Type.Conjunction; break;
        }
    }

    public void HandleSignal(string from, bool signal, ref Queue<(string from, string targetModule, bool signal)> queue, ref int totalHighSignals, ref int totalLowSignals)
    {
        switch(moduleType)
        {
            case Type.Default: 
                foreach(string target in moduleTargets)
                {
                    if(signal)
                    {
                        totalHighSignals++;
                    }
                    else
                    {
                        totalLowSignals++;
                    }
                        
                    //Console.WriteLine("Sending " + signal + " from " + moduleName + " to " + target);
                    queue.Enqueue((moduleName, target, signal));
                }
            break;
            case Type.FlipFlop:
                if(signal == false)
                {
                    status[0] = !status[0];
                    foreach(string target in moduleTargets)
                    {
                        //Console.WriteLine("Sending " + status[0] + " from " + moduleName + " to " + target);
                        if(status[0])
                        {
                            totalHighSignals++;
                        }
                        else
                        {
                            totalLowSignals++;
                        }
                        queue.Enqueue((moduleName, target, status[0]));
                    }
                }
                break;
            case Type.Conjunction:
                bool allHigh = true;

                for(int i = 0; i < gettingSignalsFrom.Count; i++)
                {
                    //Console.WriteLine("if " + gettingSignalsFrom[i] + "==" + from);
                    if(gettingSignalsFrom[i] == from)
                    {
                        status[i] = signal;
                        break;
                    }
                }

                foreach(bool s in status)
                {
                    if(s == false)
                    {
                        allHigh = false;
                        break;
                    }
                }

                if(allHigh)
                {
                    foreach(string target in moduleTargets)
                    {
                        //Console.WriteLine("Sending " + false + " from " + moduleName + " to " + target);
                        totalLowSignals++;
                        queue.Enqueue((moduleName, target, false));
                    }
                }
                else
                {
                    foreach(string target in moduleTargets)
                    {
                        //Console.WriteLine("Sending " + true + " from " + moduleName + " to " + target);
                        totalHighSignals++;
                        queue.Enqueue((moduleName, target, true));
                    }
                }
                break;
        }
    }
}

enum Type
{
    Default,
    FlipFlop,
    Conjunction 
}