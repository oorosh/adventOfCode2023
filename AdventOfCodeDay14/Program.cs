using System;
using System.IO;

class RockPuzzle
{
    private const char RoundedRock = 'O';
    private const char CubeRock = '#';
    private const char EmptySpace = '.';
    private char[,] platform;

    public RockPuzzle(string[] input)
    {
        int rows = input.Length;
        int cols = input[0].Length;
        platform = new char[rows, cols];

        for (int i = 0; i < rows; i++)
        {
            if (input[i].Length != cols)
            {
                throw new ArgumentException("All rows must have the same length");
            }

            for (int j = 0; j < cols; j++)
            {
                platform[i, j] = input[i][j];
            }
        }
    }

    public void TiltNorth()
    {
        for (int col = 0; col < platform.GetLength(1); col++)
        {
            for (int row = 0; row < platform.GetLength(0); row++)
            {
                if (platform[row, col] == RoundedRock)
                {
                    int targetRow = row;
                    while (targetRow > 0 && platform[targetRow - 1, col] == EmptySpace)
                    {
                        targetRow--; // Move up if the space above is empty
                    }

                    if (targetRow != row)
                    {
                        platform[targetRow, col] = RoundedRock;
                        platform[row, col] = EmptySpace;
                    }
                }
            }
        }
    }

    public int CalculateLoad()
    {
        int load = 0;
        int rows = platform.GetLength(0);

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < platform.GetLength(1); col++)
            {
                if (platform[row, col] == RoundedRock)
                {
                    load += (rows - row);
                }
            }
        }

        return load;
    }

    public void PrintPlatform()
    {
        for (int i = 0; i < platform.GetLength(0); i++)
        {
            for (int j = 0; j < platform.GetLength(1); j++)
            {
                Console.Write(platform[i, j]);
            }
            Console.WriteLine();
        }
    }

    static void Main()
    {
        try
        {
            // Update the file path to the location of your input file
            string[] input = File.ReadAllLines("input.txt");
            RockPuzzle puzzle = new RockPuzzle(input);
            puzzle.TiltNorth();
            Console.WriteLine("Platform after tilting north:");
            puzzle.PrintPlatform();

            int totalLoad = puzzle.CalculateLoad();
            Console.WriteLine($"Total Load: {totalLoad}");
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred: " + ex.Message);
        }
    }
}
