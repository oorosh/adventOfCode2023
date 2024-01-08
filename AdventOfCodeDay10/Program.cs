using System;
using System.IO;
using System.Collections.Generic;

public class PipeGrid
{
    private const int gridSize = 140;
    private char[,] grid = new char[gridSize, gridSize];
    private bool[,] visited = new bool[gridSize, gridSize];
    private int tileCount = 0;

    public PipeGrid(char[,] initialGrid)
    {
        grid = initialGrid;
    }

    public (bool, int) HasClosedLoop(int startX, int startY)
    {
        bool hasLoop = Dfs(startX, startY);
        int count = hasLoop ? tileCount : 0;
        tileCount = 0; // Reset for future calls
        return (hasLoop, count);
    }

    private bool Dfs(int startX, int startY)
    {
        Stack<(int x, int y, int fromX, int fromY)> stack = new Stack<(int x, int y, int fromX, int fromY)>();
        stack.Push((startX, startY, -1, -1));

        while (stack.Count > 0)
        {
            var (x, y, fromX, fromY) = stack.Pop();

            if (x < 0 || y < 0 || x >= gridSize || y >= gridSize || grid[x, y] == '.' || visited[x, y])
                continue;

            visited[x, y] = true;
            tileCount++;

            List<(int, int)> nextPositions = GetNextPositions(x, y);

            foreach (var (nextX, nextY) in nextPositions)
            {
                if (nextX == fromX && nextY == fromY) continue;

                if (visited[nextX, nextY])
                {
                    // If a visited node is encountered (other than the one we came from), a loop is found
                    return true;
                }
                else
                {
                    stack.Push((nextX, nextY, x, y));
                }
            }
        }

        // Reset tile count if no loop is found
        tileCount = 0;
        return false;
    }

    private List<(int, int)> GetNextPositions(int x, int y)
    {
        var positions = new List<(int, int)>();
        char pipe = grid[x, y];

        switch (pipe)
        {
            case '|':
                positions.Add((x - 1, y));
                positions.Add((x + 1, y));
                break;
            case '-':
                positions.Add((x, y - 1));
                positions.Add((x, y + 1));
                break;
            case 'L':
                positions.Add((x - 1, y));
                positions.Add((x, y + 1));
                break;
            case 'J':
                positions.Add((x - 1, y));
                positions.Add((x, y - 1));
                break;
            case '7':
                positions.Add((x + 1, y));
                positions.Add((x, y - 1));
                break;
            case 'F':
                positions.Add((x + 1, y));
                positions.Add((x, y + 1));
                break;
            case 'S': // Assuming 'S' can connect in all directions
                positions.Add((x - 1, y));
                positions.Add((x + 1, y));
                positions.Add((x, y - 1));
                positions.Add((x, y + 1));
                break;
        }

        return positions;
    }
}

public class PipeGridLoader
{
    private const int gridSize = 140;

    public static char[,] LoadGridFromFile(string filePath)
    {
        char[,] grid = new char[gridSize, gridSize];
        string[] lines = File.ReadAllLines(filePath);

        for (int i = 0; i < lines.Length; i++)
        {
            for (int j = 0; j < lines[i].Length; j++)
            {
                grid[i, j] = lines[i][j];
            }
        }

        return grid;
    }

    public static (int, int) FindStartPosition(char[,] grid)
    {
        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                if (grid[i, j] == 'S')
                {
                    return (i, j);
                }
            }
        }

        throw new InvalidOperationException("Start position 'S' not found in the grid.");
    }
}

public class Program
{
    public static void Main()
    {
        string filePath = "input.txt"; // Replace with the path to your text file
        char[,] grid = PipeGridLoader.LoadGridFromFile(filePath);

        var (startX, startY) = PipeGridLoader.FindStartPosition(grid);

        PipeGrid pipeGrid = new PipeGrid(grid);

        var (hasLoop, loopSize) = pipeGrid.HasClosedLoop(startX, startY);
        Console.WriteLine($"Closed loop exists: {hasLoop}");
        if (hasLoop)
        {
            Console.WriteLine($"Number of tiles in loop: {loopSize}");
        }
    }
}