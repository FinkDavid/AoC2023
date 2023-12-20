string[] lines = File.ReadAllLines("../input.txt");

Dictionary<string, List<(string condition, string destination)>> workflows = new Dictionary<string, List<(string, string)>>();

foreach(string l in lines)
{
    if(string.IsNullOrEmpty(l))
    {
        break;
    }
    else
    {
        string[] split = l.Split('{');
        string workflowName = split[0];

        int current = 1;
        if(workflowName != "in")
        {
            workflowName += current.ToString();
        }
        

        workflows.Add(workflowName, new List<(string condition, string destination)>());

        string conditions = split[1].Remove(split[1].Length - 1);
        string[] conditionsSplit = conditions.Split(',');

        for(int i = 0; i < conditionsSplit.Length - 1; i++)
        {
            string[] conditionDestinationSplit = conditionsSplit[i].Split(':');
            string conditionDestinationName = conditionDestinationSplit[1];

            if(conditionDestinationSplit[1] != "A" && conditionDestinationSplit[1] != "R")
            {
                conditionDestinationName += "1";
            }

            workflows[workflowName].Add((conditionDestinationSplit[0], conditionDestinationName));
        }

        string test = conditionsSplit[conditionsSplit.Length - 1];

        if(test != "A" && test != "R")
        {
            test += "1";
        }

        workflows[workflowName].Add(("m>0", test));

        current++;
        if(workflows[workflowName].Count > 2)
        {
            string newName = workflowName.Substring(0, workflowName.Length - 1) + current;
            workflows[workflowName].Insert(1, ("m>0", newName));
            workflows.Add(newName, new List<(string condition, string destination)>());

            for(int i = 2; i < workflows[workflowName].Count; i++)
            {
                workflows[newName].Add(workflows[workflowName][i]);
            }

            int x = 2;
            int count = workflows[workflowName].Count;
            for(int i = 2; i < count; i++)
            {
                workflows[workflowName].RemoveAt(x);
            }

            if(workflows[newName].Count > 2)
            {
                current++;
                string newName2 = newName.Substring(0, newName.Length - 1) + current;
                workflows[newName].Insert(1, ("m>0", newName2));
                workflows.Add(newName2, new List<(string condition, string destination)>());

                for(int i = 2; i < workflows[newName].Count; i++)
                {
                    workflows[newName2].Add(workflows[newName][i]);
                }

                count = workflows[newName].Count;
                for(int i = 2; i < count; i++)
                {
                    workflows[newName].RemoveAt(x);
                }
            }
        }
    }
}
long result = 0;

Next("in", new Dictionary<char, long>() { {'x', 0}, {'m', 0}, {'a', 0}, {'s', 0} }, new Dictionary<char, long>() { {'x', 4000}, {'m', 4000}, {'a', 4000}, {'s', 4000} }, ref result);

Console.WriteLine("Result: " + result);

void Next(string current, Dictionary<char, long> startingNumbers, Dictionary<char, long> endingNumbers, ref long result)
{
    if(current != "A" && current != "R")
    {
        char cond = workflows[current][0].condition[1];
        int num = int.Parse(workflows[current][0].condition.Substring(2));
        char letter = workflows[current][0].condition[0];
        long oldNumber;

        Dictionary<char, long> startingNumbersCopy = new Dictionary<char, long>(startingNumbers);
        Dictionary<char, long> endingNumbersCopy = new Dictionary<char, long>(endingNumbers);

        switch(cond)
        {
            case '<':
                oldNumber = endingNumbersCopy[letter];
                endingNumbersCopy[letter] = num - 1;
                Next(workflows[current][0].destination, startingNumbersCopy, endingNumbersCopy, ref result);
                endingNumbers[letter] = oldNumber;
                startingNumbers[letter] = num - 1;
                Next(workflows[current][1].destination, startingNumbers, endingNumbers, ref result);
                break;
            case '>':
                oldNumber = startingNumbersCopy[letter];
                startingNumbersCopy[letter] = num;
                Next(workflows[current][0].destination, startingNumbersCopy, endingNumbersCopy, ref result);
                startingNumbers[letter] = oldNumber;
                endingNumbers[letter] = num;
                Next(workflows[current][1].destination, startingNumbers, endingNumbers, ref result);
                break;
        }
    }
    else
    {
        if(current == "A")
        {
            result += (endingNumbers['x'] - startingNumbers['x']) * (endingNumbers['m'] - startingNumbers['m']) * (endingNumbers['a'] - startingNumbers['a']) * (endingNumbers['s'] - startingNumbers['s']);
        }
    }
}
