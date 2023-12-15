string[] lines = File.ReadAllLines("../input.txt");
int result = 0;
string line = lines[0] + ',';
for(int i = 0; i < line.Length; i++)
{
    int resultPerInput = 0;
    while(line[i] != ',')
    {
        resultPerInput = (resultPerInput + line[i]) * 17 % 256;
        i++;
    }
    result += resultPerInput;
}
Console.WriteLine("Result: " + result);