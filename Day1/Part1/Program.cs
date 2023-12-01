string[] lines = File.ReadAllLines("../input.txt");

List<char> numbers = new List<char>();
int result = 0;

foreach (string l in lines)
{
    foreach (char c in l)
    {
        if (char.IsDigit(c))
        {
            numbers.Add(c);
        }
    }
    result += int.Parse(numbers[0].ToString() + numbers[numbers.Count - 1].ToString());
    numbers.Clear();
}

Console.WriteLine("Result: " + result);