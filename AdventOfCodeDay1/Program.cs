var total = 0;
var numbers = new[] { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };

var lines = File.ReadAllLines("input.txt");

foreach (var line in lines)
{
    var digits = "";

    for (var index = 0; index < line.Length; index++)
    {
        if (char.IsDigit(line[index]))
            digits += line[index];

        //uncomment for part 2
        //for (var i = 0; i < numbers.Length; i++)
        //    if (line[index..].StartsWith(numbers[i]))
        //        digits += i.ToString();
    }

    total += Convert.ToInt32(digits[0] + "" + digits[^1]);
}

Console.WriteLine(total);