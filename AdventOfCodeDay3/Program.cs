using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Program
{
    static List<string> LoadSchematic(string filePath)
    {
        return File.ReadAllLines(filePath).ToList();
    }

    static void Main()
    {
        string filePath = "input.txt"; // Update with the actual file path
        List<string> schematic = LoadSchematic(filePath);

        long sumOfPartNumbers = 0;
        long sumOfProducts = 0;
        HashSet<string> usedNumbersForSum = new HashSet<string>();

        for (int i = 0; i < schematic.Count; i++)
        {
            for (int j = 0; j < schematic[i].Length; j++)
            {
                if (char.IsDigit(schematic[i][j]) && (j == 0 || !char.IsDigit(schematic[i][j - 1])))
                {
                    string numberStr = BuildNumber(schematic, i, j);
                    if (!usedNumbersForSum.Contains(numberStr) && IsAdjacentToSymbol(schematic, i, j, numberStr.Length))
                    {
                        sumOfPartNumbers += int.Parse(numberStr);
                        usedNumbersForSum.Add(numberStr);
                    }
                }
            }
        }

        sumOfProducts = CalculateSumOfProducts(schematic);

        Console.WriteLine("Sum of part numbers: " + sumOfPartNumbers);
        Console.WriteLine("Sum of products of numbers adjacent to each '*': " + sumOfProducts);
    }

    static long CalculateSumOfProducts(List<string> schematic)
    {
        long sumOfProducts = 0;
        HashSet<string> processedMultiDigitNumbers = new HashSet<string>();

        for (int i = 0; i < schematic.Count; i++)
        {
            for (int j = 0; j < schematic[i].Length; j++)
            {
                if (schematic[i][j] == '*')
                {
                    var adjacentNumbers = FindAdjacentMultiDigitNumbers(schematic, i, j, processedMultiDigitNumbers);
                    if (adjacentNumbers.Count > 1)
                    {
                        sumOfProducts += adjacentNumbers.Aggregate(1L, (a, b) => a * b);
                    }
                }
            }
        }

        return sumOfProducts;
    }

    static string BuildNumber(List<string> schematic, int x, int y)
    {
        string numberStr = "";
        while (y < schematic[x].Length && char.IsDigit(schematic[x][y]))
        {
            numberStr += schematic[x][y];
            y++;
        }
        return numberStr;
    }

    static bool IsAdjacentToSymbol(List<string> schematic, int x, int y, int length)
    {
        for (int i = 0; i < length; i++)
        {
            if (CheckAdjacent(schematic, x, y + i, c => !char.IsDigit(c) && c != '.'))
            {
                return true;
            }
        }
        return false;
    }

    static bool CheckAdjacent(List<string> schematic, int x, int y, Func<char, bool> condition)
    {
        int[] dx = { -1, -1, -1, 0, 0, 1, 1, 1 };
        int[] dy = { -1, 0, 1, -1, 1, -1, 0, 1 };

        for (int i = 0; i < 8; i++)
        {
            int newX = x + dx[i];
            int newY = y + dy[i];

            if (newX >= 0 && newX < schematic.Count && newY >= 0 && newY < schematic[newX].Length)
            {
                if (condition(schematic[newX][newY]))
                {
                    return true;
                }
            }
        }
        return false;
    }

    static List<long> FindAdjacentMultiDigitNumbers(List<string> schematic, int x, int y, HashSet<string> processedMultiDigitNumbers)
    {
        var adjacentNumbers = new HashSet<long>();
        Action<int, int> checkCell = (dx, dy) =>
        {
            int newX = x + dx, newY = y + dy;
            if (newX >= 0 && newX < schematic.Count && newY >= 0 && newY < schematic[newX].Length && char.IsDigit(schematic[newX][newY]))
            {
                string numberStr = BuildNumber(schematic, newX, newY);
                if (!processedMultiDigitNumbers.Contains(numberStr))
                {
                    adjacentNumbers.Add(long.Parse(numberStr));
                    processedMultiDigitNumbers.Add(numberStr);
                }
            }
        };

        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                if (dx != 0 || dy != 0) checkCell(dx, dy);
            }
        }

        return adjacentNumbers.ToList();
    }
}
