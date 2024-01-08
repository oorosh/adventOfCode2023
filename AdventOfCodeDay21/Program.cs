using System;
using System.Collections.Generic;
using System.IO;

class ElfGarden
{
    static void Main(string[] args)
    {
        string[] mapLines = File.ReadAllLines("input.txt");
        List<string> map = new List<string>(mapLines);

        int steps = 64; // Number of steps the Elf can take
        var (newMap, countO) = MoveElf(map, steps);
        Console.WriteLine($"Map after {steps} steps:");
        foreach (var row in newMap)
        {
            Console.WriteLine(row);
        }
        Console.WriteLine($"Total reachable garden plots: {countO}");
    }

    static (List<string>, int) MoveElf(List<string> map, int steps)
    {
        int rows = map.Count;
        int cols = map[0].Length;
        var reach = new bool[rows, cols];

        // Find starting position and initialize reachability map
        (int row, int col) start = (0, 0);
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                if (map[r][c] == 'S')
                {
                    start = (r, c);
                    reach[r, c] = true;
                }
            }
        }

        // Perform movements
        for (int step = 0; step < steps; step++)
        {
            var newReach = new bool[rows, cols];
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    if (reach[r, c])
                    {
                        MarkReachable(newReach, r, c, rows, cols, map);
                    }
                }
            }
            reach = newReach;
        }

        // Update map with reachable positions and count 'O's
        int countO = 0;
        var newMap = new List<string>();
        for (int r = 0; r < rows; r++)
        {
            char[] row = map[r].ToCharArray();
            for (int c = 0; c < cols; c++)
            {
                if (reach[r, c])
                {
                    row[c] = 'O';
                    countO++;
                }
            }
            newMap.Add(new string(row));
        }

        return (newMap, countO);
    }

    static void MarkReachable(bool[,] reach, int r, int c, int rows, int cols, List<string> map)
    {
        int[] dr = { -1, 1, 0, 0 };
        int[] dc = { 0, 0, -1, 1 };

        for (int i = 0; i < 4; i++)
        {
            int nr = r + dr[i];
            int nc = c + dc[i];

            if (nr >= 0 && nr < rows && nc >= 0 && nc < cols && (map[nr][nc] == '.' || map[nr][nc] == 'S'))
            {
                reach[nr, nc] = true;
            }
        }
    }
}
