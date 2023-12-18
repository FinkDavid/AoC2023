string[] lines = File.ReadAllLines("../input.txt");

List<(double X, double Y)> vertices = new List<(double X, double Y)>();
(double X, double Y) currentPosition = (0, 0);
vertices.Add(currentPosition);

double length = 0;
foreach(string l in lines)
{
    string[] split = l.Split(' ');
    string direction = split[2].Substring(7, 1);
    double amount = Convert.ToInt64(split[2].Substring(2, 5), 16);
    length += amount;
    
    switch(direction)
    {
        case "0": currentPosition.X += amount; break;
        case "1": currentPosition.Y += amount; break;
        case "2": currentPosition.X -= amount; break;
        case "3": currentPosition.Y -= amount; break;
    }
    vertices.Add(currentPosition);
}

double area = ShoelaceFormula(vertices);
double result = area + length / 2 + 1;
Console.WriteLine("Result: " + result);

double ShoelaceFormula(List<(double X, double Y)> vertices)
{
    int n = vertices.Count;
    double area = 0;

    for (int i = 0; i < n - 1; i++)
    {
        area += vertices[i].X * vertices[i + 1].Y - vertices[i + 1].X * vertices[i].Y;
    }

    area += vertices[n - 1].X * vertices[0].Y - vertices[0].X * vertices[n - 1].Y;
    area = Math.Abs(area) / 2;

    return area;
}