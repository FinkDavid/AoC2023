﻿string[] lines = File.ReadAllLines("../input.txt");

List<bool> instructions = new List<bool>();
List<Node> nodes = new List<Node>();

foreach (char c in lines[0])
{
    if(c == 'L')
    {
        instructions.Add(false);
    }
    else if(c == 'R')
    {
        instructions.Add(true);
    }
}

List<int> startIndexes = new List<int>();
List<int> finishIndexes = new List<int>();

for(int i = 2; i < lines.Length; i++)
{
    nodes.Add(new Node(lines[i].Remove(3), lines[i].Substring(7, 3), lines[i].Substring(12, 3)));

    if(lines[i][2] == 'A')
    {
        startIndexes.Add(i - 2);
    }
    else if(lines[i][2] == 'Z')
    {
        finishIndexes.Add(i - 2);
    }
}

bool targetFound = false;
int steps = 0;
List<int> currentIndexes = new List<int>();

List<long> cycleSizes = new List<long>();

for(int i = 0; i < startIndexes.Count; i++)
{
    currentIndexes.Add(startIndexes[i]);
}

while(!targetFound)
{
    for(int i = 0; i < instructions.Count; i++)
    {
        for(int j = 0; j < currentIndexes.Count; j++)
        {
            if(instructions[i] == true)
            {
                currentIndexes[j] = findIndexWithNodeName(nodes[currentIndexes[j]].rightNode);
            }
            else
            {
                currentIndexes[j] = findIndexWithNodeName(nodes[currentIndexes[j]].leftNode);
            }
        }

        steps++;

        for (int x = currentIndexes.Count - 1; x >= 0; x--)
        {
            if (finishIndexes.Contains(currentIndexes[x]))
            {
                cycleSizes.Add(steps);
                currentIndexes.Remove(currentIndexes[x]);
            }
        }

        if(currentIndexes.Count == 0)
        {
            targetFound = true;
            steps++;
            break;
        }
    }
}

Console.WriteLine("Result: " + CalculateLCM(cycleSizes));

long CalculateLCM(List<long> numbers)
{
    long lcm = numbers[0];

    for (int i = 1; i < numbers.Count; i++)
    {
        lcm = CalculateLCM2(lcm, numbers[i]);
    }

    return lcm;
}

long CalculateLCM2(long a, long b)
{
    return Math.Abs(a * b) / CalculateGCD(a, b);
}

long CalculateGCD(long a, long b)
{
    while (b != 0)
    {
        long temp = b;
        b = a % b;
        a = temp;
    }
    return a;
}

int findIndexWithNodeName(string name)
{
    for(int i = 0; i < nodes.Count; i++)
    {
        if(nodes[i].currentNode == name)
        {
            return i;
        }
    }
    
    return -1;
}

class Node
{
    public string currentNode;
    public string leftNode;
    public string rightNode;

    public Node(string cN, string lN, string rN)
    {
        currentNode = cN;
        leftNode = lN;
        rightNode = rN;
    }
}