string[] lines = File.ReadAllLines("../input.txt");

Dictionary<string, List<(string condition, string destination)>> workflows = new Dictionary<string, List<(string, string)>>();
List<Part> parts = new List<Part>();
List<Part> acceptedParts = new List<Part>();

bool isWorkflow = true;
foreach(string l in lines)
{
    if(string.IsNullOrEmpty(l))
    {
        isWorkflow = false;
    }
    else
    {
        if(isWorkflow)
        {
            string[] split = l.Split('{');
            string workflowName = split[0];

            workflows.Add(workflowName, new List<(string condition, string destination)>());

            string conditions = split[1].Remove(split[1].Length - 1);
            string[] conditionsSplit = conditions.Split(',');

            for(int i = 0; i < conditionsSplit.Length - 1; i++)
            {
                string[] conditionDestinationSplit = conditionsSplit[i].Split(':');
                workflows[workflowName].Add((conditionDestinationSplit[0], conditionDestinationSplit[1]));
            }

            //Adding "invisible" condition to the last destination that will always be true
            workflows[workflowName].Add(("m>0", conditionsSplit[conditionsSplit.Length - 1]));
        }
        else
        {
            string trimmedInput = l.Trim('{', '}');
            string[] split = trimmedInput.Split(',');
            parts.Add(new Part());
            foreach(string s in split)
            {
                string name = s[0].ToString();
                int number = int.Parse(s.Substring(2, s.Length - 2));
                parts[parts.Count - 1].ratings.Add(name, number);
            }
        }
    }
}

foreach(Part part in parts)
{
    string currentWorkflow = "in";
    bool partDone = false;
    while(!partDone)
    {
        //Loop over all conditions
        for(int i = 0; i < workflows[currentWorkflow].Count; i++)
        {
            bool nextCondition = false;
            string name = workflows[currentWorkflow][i].condition[0].ToString();
            char cond = workflows[currentWorkflow][i].condition[1];
            int num = int.Parse(workflows[currentWorkflow][i].condition.Substring(2));
            switch(cond)
            {
                case '<': 
                    if(part.ratings[name] < num)
                    {
                        string destination = workflows[currentWorkflow][i].destination;
                        if(destination == "A")
                        {
                            acceptedParts.Add(part);
                            partDone = true;
                        }
                        else if(destination == "R")
                        {
                            partDone = true;
                        }
                        else
                        {
                            currentWorkflow = destination;
                        }
                        nextCondition = true;
                    }
                    break;
                case '>': 
                    if(part.ratings[name] > num)
                    {
                        string destination = workflows[currentWorkflow][i].destination;
                        if(destination == "A")
                        {
                            acceptedParts.Add(part);
                            partDone = true;
                        }
                        else if(destination == "R")
                        {
                            partDone = true;
                        }
                        else
                        {
                            currentWorkflow = destination;
                        }
                        nextCondition = true;
                    }
                    break;
                default: break;
            }

            if(nextCondition)
            {
                break;
            }
        }
    }
}

int result = 0;
foreach(Part p in acceptedParts)
{
    result += p.getSumOfValues();
}   

Console.WriteLine("Result: " + result);

class Part
{
    public Dictionary<string, int> ratings = new Dictionary<string, int>();

    public int getSumOfValues()
    {
        int sum = 0;
        foreach (var kvp in ratings)
        {
            sum += kvp.Value;
        }

        return sum;
    }
}