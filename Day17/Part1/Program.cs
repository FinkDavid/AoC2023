string[] lines = File.ReadAllLines("../input.txt");
int[,] map = new int[lines.Length, lines[0].Length];

for(int i = 0; i < lines.Length; i++)
{
    for(int j = 0; j < lines[i].Length; j++)
    {
        map[i, j] = int.Parse(lines[i][j].ToString());
    }
}

DijkstraAlgorithm graph = new DijkstraAlgorithm(map);
List<(int x, int y)> shortestPathCost = graph.FindShortestPath();

int result = 0;
foreach((int x, int y) node in shortestPathCost)
{
    result += map[node.x, node.y];
    Console.WriteLine(node.x + "/" + node.y + ":    " + map[node.x, node.y]);
}
Console.WriteLine("Result: " + result);

class DijkstraAlgorithm
{
    private int[,] map;
    private int rows;
    private int cols;

    public DijkstraAlgorithm(int[,] map)
    {
        this.map = map;
        this.rows = map.GetLength(0);
        this.cols = map.GetLength(1);
    }

    public List<(int, int)> FindShortestPath()
    {
        int[,] distance = new int[rows, cols];
        bool[,] visited = new bool[rows, cols];
        (int, int)[,] predecessors = new (int, int)[rows, cols];

        // Initialize distances with a large value (representing infinity)
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                distance[i, j] = int.MaxValue;
            }
        }

        // Dijkstra's algorithm
        distance[0, 0] = map[0, 0];

        for (int count = 0; count < rows * cols - 1; count++)
        {
            int minDistance = int.MaxValue;
            int minI = -1, minJ = -1;

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (!visited[i, j] && distance[i, j] < minDistance)
                    {
                        minDistance = distance[i, j];
                        minI = i;
                        minJ = j;
                    }
                }
            }

            if (minI == -1 || minJ == -1)
            {
                // No path found
                return new List<(int, int)>();
            }

            visited[minI, minJ] = true;

            // Update distances of adjacent cells
            // Assuming movements are allowed in 4 directions: up, down, left, right
            int[] rowMoves = { -1, 0, 1, 0 };
            int[] colMoves = { 0, 1, 0, -1 };

            for (int k = 0; k < 4; k++)
            {
                int newRow = minI + rowMoves[k];
                int newCol = minJ + colMoves[k];

                if (IsValid(newRow, newCol) && !visited[newRow, newCol] &&
                    distance[minI, minJ] + map[newRow, newCol] < distance[newRow, newCol])
                {
                    distance[newRow, newCol] = distance[minI, minJ] + map[newRow, newCol];
                    predecessors[newRow, newCol] = (minI, minJ);
                }
            }
        }

        // Reconstruct the path from finish to start
        List<(int, int)> path = new List<(int, int)>();
        int currentRow = rows - 1;
        int currentCol = cols - 1;

        while (currentRow != 0 || currentCol != 0)
        {
            path.Add((currentRow, currentCol));
            (int predRow, int predCol) = predecessors[currentRow, currentCol];
            currentRow = predRow;
            currentCol = predCol;
        }

        // Add the start node
        path.Add((0, 0));

        // Reverse the path to get it from start to finish
        path.Reverse();

        return path;
    }

    private bool IsValid(int row, int col)
    {
        return (row >= 0 && row < rows && col >= 0 && col < cols);
    }
}