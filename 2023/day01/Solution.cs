/*

--- Day 1: Trebuchet?! ---
Something is wrong with global snow production, and you've been selected to take a look. The Elves have even given you a
map; on it, they've used stars to mark the top fifty locations that are likely to be having problems.

You've been doing this long enough to know that to restore snow operations, you need to check all fifty stars by
December 25th.

Collect stars by solving puzzles. Two puzzles will be made available on each day in the Advent calendar; the second
puzzle is unlocked when you complete the first. Each puzzle grants one star. Good luck!

You try to ask why they can't just use a weather machine ("not powerful enough") and where they're even sending you
("the sky") and why your map looks mostly blank ("you sure ask a lot of questions") and hang on did you just say the
sky ("of course, where do you think snow comes from") when you realize that the Elves are already loading you into a
trebuchet ("please hold still, we need to strap you in").

As they're making the final adjustments, they discover that their calibration document (your puzzle input) has been
amended by a very young Elf who was apparently just excited to show off her art skills. Consequently, the Elves are
having trouble reading the values on the document.

*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace AdventOfCode.Year2023.Day01
{
    /*

    The newly-improved calibration document consists of lines of text; each line originally contained a specific
    calibration value that the Elves now need to recover. On each line, the calibration value can be found by combining
    the first digit and the last digit (in that order) to form a single two-digit number.

    For example:

    1abc2
    pqr3stu8vwx
    a1b2c3d4e5f
    treb7uchet

    In this example, the calibration values of these four lines are 12, 38, 15, and 77. Adding these together produces
    142.

    Consider your entire calibration document. What is the sum of all of the calibration values?

    */
    public class Problem1
    {
        const int SOLUTION = 56049;

        [Fact]
        public static void Solution1()
        {
            string[] lines = File.ReadAllLines(Path.Combine("data", "day01.txt"));

            int totalSum = 0;
            foreach (string line in lines)
            {
                char firstDigit = line.First(x => char.IsDigit(x));
                char lastDigit = line.Reverse().First(x => char.IsDigit(x));
                totalSum += Convert.ToInt32(new string(new char[] { firstDigit, lastDigit }));
            }

            Assert.Equal(SOLUTION, totalSum);
        }
    }

    /*

    Your calculation isn't quite right. It looks like some of the digits are actually spelled out with letters: one,
    two, three, four, five, six, seven, eight, and nine also count as valid "digits".

    Equipped with this new information, you now need to find the real first and last digit on each line. For example:

    two1nine
    eightwothree
    abcone2threexyz
    xtwone3four
    4nineeightseven2
    zoneight234
    7pqrstsixteen

    In this example, the calibration values are 29, 83, 13, 24, 42, 14, and 76. Adding these together produces 281.

    What is the sum of all of the calibration values?

    */
    public class Problem2
    {
        const int SOLUTION = 54530;

        [Fact]
        public static void Solution1()
        {
            static int? SearchForDigit(string text)
            {
                Dictionary<string, int> digitMap = new()
                {
                    {"1", 1}, { "one", 1 },
                    {"2", 2}, { "two", 2 },
                    {"3", 3}, { "three", 3 },
                    {"4", 4}, { "four", 4 },
                    {"5", 5}, { "five", 5 },
                    {"6", 6}, { "six", 6 },
                    {"7", 7}, { "seven", 7 },
                    {"8", 8}, { "eight", 8 },
                    {"9", 9}, { "nine", 9 },
                };

                foreach ((string digit, int value) in digitMap)
                    if (text.Contains(digit))
                        return value;
                return null;
            }

            string[] lines = File.ReadAllLines(Path.Combine("data", "day01.txt"));

            int totalSum = 0;
            foreach (string line in lines)
            {
                int? firstDigit = null;
                int? lastDigit = null;
                for (int i = 0; i <= line.Length; i++)
                {
                    firstDigit ??= SearchForDigit(line[..i]);
                    lastDigit ??= SearchForDigit(line[^i..]);
                }
                if (firstDigit is not null && lastDigit is not null)
                    totalSum += firstDigit.Value * 10 + lastDigit.Value;
            }

            Assert.Equal(SOLUTION, totalSum);
        }
    }
}
