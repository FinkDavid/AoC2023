string[] lines = File.ReadAllLines("../input.txt");

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

int startIndex = 0;
int finishIndex = 0;

for(int i = 2; i < lines.Length; i++)
{
    Console.WriteLine("Adding: " + lines[i].Remove(3) + "   with left: " + lines[i].Substring(7, 3) + "   and right: " + lines[i].Substring(12, 3));
    nodes.Add(new Node(lines[i].Remove(3), lines[i].Substring(7, 3), lines[i].Substring(12, 3)));

    if(lines[i].Remove(3) == "AAA")
    {
        startIndex =  i - 2;
    }
    else if(lines[i].Remove(3) == "ZZZ")
    {
        finishIndex = i - 2;
    }
}

bool targetFound = false;
int steps = 0;
int currentIndex = startIndex;

while(!targetFound)
{
    for(int i = 0; i < instructions.Count; i++)
    {
        Console.WriteLine("CurrentNode: " + nodes[currentIndex].currentNode + "    going: " + instructions[i]);
        if(instructions[i] == true)
        {
            currentIndex = findIndexWithNodeName(nodes[currentIndex].rightNode);
        }
        else
        {
            currentIndex = findIndexWithNodeName(nodes[currentIndex].leftNode);
        }

        if(currentIndex == finishIndex)
        {
            targetFound = true;
            steps++;
            break;
        }
        steps++;
    }
}

Console.WriteLine("Result: " + steps);

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