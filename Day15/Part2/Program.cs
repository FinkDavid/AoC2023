string[] lines = File.ReadAllLines("../input.txt");
int result = 0;
string line = lines[0] + ',';

List<Box> boxes = new List<Box>();
for(int i = 0; i < 256; i++)
{
    boxes.Add(new Box(i));
}

for(int i = 0; i < line.Length; i++)
{
    string input = "";
    while(line[i] != ',')
    {
        input += line[i];
        i++;
    }

    string label;
    int focalLength;

    if(input[input.Length - 1] == '-')
    {
        label = input.Substring(0, input.Length - 1);
        int boxNumber = getBoxNumber(label);
        int index = boxes[boxNumber].content.IndexOf(label);
        if(index != -1)
        {
            boxes[boxNumber].content.RemoveAt(index);
            boxes[boxNumber].focalLengths.RemoveAt(index);
        }
    }
    else
    {
        label = input.Substring(0, input.Length - 2);
        focalLength = int.Parse(input[input.Length - 1].ToString());

        int boxNumber = getBoxNumber(label);
        if(boxes[boxNumber].content.Contains(label))
        {
            int index = boxes[boxNumber].content.IndexOf(label);
            boxes[boxNumber].content.RemoveAt(index);
            boxes[boxNumber].focalLengths.RemoveAt(index);

            boxes[boxNumber].content.Insert(index, label);
            boxes[boxNumber].focalLengths.Insert(index, focalLength);
        }
        else
        {
            boxes[boxNumber].content.Add(label);
            boxes[boxNumber].focalLengths.Add(focalLength);
        }
    }
}

for(int i = 0; i < boxes.Count; i++)
{
    for(int j = 0; j < boxes[i].content.Count; j++)
    {
        result += (1 + i) * (j + 1) * boxes[i].focalLengths[j];
    }
}

Console.WriteLine("Result: " + result);

int getBoxNumber(string label)
{
    int res = 0;
    for(int i = 0; i < label.Length; i++)
    {
        res = (res + label[i]) * 17 % 256;
    }

    return res;
}

class Box
{
    int id;
    public List<string> content;
    public List<int> focalLengths;

    public Box(int id)
    {
        content = new List<string>();
        focalLengths = new List<int>();
        this.id = id;
    }
}