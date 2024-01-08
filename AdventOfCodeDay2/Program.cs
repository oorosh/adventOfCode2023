using System.Text.RegularExpressions;

class Program
{
    class CubeSet
    {
        public int Red;
        public int Green;
        public int Blue;
    }

    class Game
    {
        public int ID;
        public List<CubeSet> Sets = new List<CubeSet>();
    }

    static void Main()
    {
        var games = LoadGamesFromFile("input.txt");

        int sumOfPossibleGameIDs = 0;

        int sumOfPower = 0;

        foreach (var game in games)
        {
            if (IsGamePossible(game))
            {
                sumOfPossibleGameIDs += game.ID;
            }

            var red = game.Sets.OrderByDescending(x => x.Red).FirstOrDefault().Red;
            var green = game.Sets.OrderByDescending(x => x.Green).FirstOrDefault().Green;            
            var blue = game.Sets.OrderByDescending(x => x.Blue).FirstOrDefault().Blue;

            sumOfPower += red * green * blue;
        }

        Console.WriteLine("Sum of possible game IDs: " + sumOfPossibleGameIDs);
        Console.WriteLine("Sum of the power of these sets: " + sumOfPower);
    }

    static List<Game> LoadGamesFromFile(string filePath)
    {
        var games = new List<Game>();
        var lines = File.ReadAllLines(filePath);

        foreach (var line in lines)
        {
            var gameParts = Regex.Match(line, @"Game (\d+): (.*)").Groups;
            if (gameParts.Count != 3) continue;

            var game = new Game { ID = int.Parse(gameParts[1].Value.Trim()) };
            var sets = gameParts[2].Value.Split(';');

            foreach (var set in sets)
            {
                var cubeSet = new CubeSet();
                var matches = Regex.Matches(set, @"(\d+) (\w+)");

                foreach (Match match in matches)
                {
                    var count = int.Parse(match.Groups[1].Value);
                    var color = match.Groups[2].Value;

                    switch (color.ToLower())
                    {
                        case "red":
                            cubeSet.Red = count;
                            break;
                        case "green":
                            cubeSet.Green = count;
                            break;
                        case "blue":
                            cubeSet.Blue = count;
                            break;
                    }
                }

                game.Sets.Add(cubeSet);
            }

            games.Add(game);
        }

        return games;
    }

    static bool IsGamePossible(Game game)
    {
        foreach (var set in game.Sets)
        {
            if (set.Red > 12 || set.Green > 13 || set.Blue > 14)
            {
                return false;
            }
        }
        return true;
    }
}
