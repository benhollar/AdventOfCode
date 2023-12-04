/*

--- Day 3: Gear Ratios ---

You and the Elf eventually reach a gondola lift station; he says the gondola lift will take you up to the water source,
but this is as far as he can bring you. You go inside.

It doesn't take long to find the gondolas, but there seems to be a problem: they're not moving.

"Aaah!"

You turn around to see a slightly-greasy Elf with a wrench and a look of surprise. "Sorry, I wasn't expecting anyone!
The gondola lift isn't working right now; it'll still be a while before I can fix it." You offer to help.

The engineer explains that an engine part seems to be missing from the engine, but nobody can figure out which one. If
you can add up all the part numbers in the engine schematic, it should be easy to work out which part is missing.

*/

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using Xunit;

namespace AdventOfCode.Year2023.Day03
{
    /*

    The engine schematic (your puzzle input) consists of a visual representation of the engine. There are lots of
    numbers and symbols you don't really understand, but apparently any number adjacent to a symbol, even diagonally, is
    a "part number" and should be included in your sum. (Periods (.) do not count as a symbol.)

    Here is an example engine schematic:

    467..114..
    ...*......
    ..35..633.
    ......#...
    617*......
    .....+.58.
    ..592.....
    ......755.
    ...$.*....
    .664.598..

    In this schematic, two numbers are not part numbers because they are not adjacent to a symbol: 114 (top right) and
    58 (middle right). Every other number is adjacent to a symbol and so is a part number; their sum is 4361.

    Of course, the actual engine schematic is much larger. What is the sum of all of the part numbers in the engine
    schematic?

    */
    public class Problem1
    {
        const int SOLUTION = 531932;

        public static Range[] FindNumberRanges(string line)
        {
            List<Range> numberRanges = new();
            int? start = null;
            for (int iChar = 0; iChar < line.Length; iChar++)
            {
                bool isDigit = char.IsDigit(line[iChar]);
                if (isDigit)
                {
                    start ??= iChar;
                }
                else if (start is not null)
                {
                    numberRanges.Add(new(start.Value, iChar));
                    start = null;
                }
                if (start is not null && iChar == line.Length - 1)
                {
                    numberRanges.Add(new(start.Value, iChar + 1));
                    start = null;
                }
            }
            return numberRanges.ToArray();
        }

        [Fact]
        public static void Solution1()
        {
            int[] FindSymbolIndices(string line)
            {
                return Enumerable.Range(0, line.Length)
                    .Where(x => !char.IsDigit(line[x]) && line[x] != '.')
                    .ToArray();
            }

            string[] lines = File.ReadAllLines(Path.Combine("data", "day03.txt"));

            List<int> schematicNumbers = new();
            for (int iLine = 0; iLine < lines.Length; iLine++)
            {
                string currentLine = lines[iLine];
                string previousLine = iLine != 0
                    ? lines[iLine - 1]
                    : string.Concat(Enumerable.Repeat(".", currentLine.Length));
                string nextLine = iLine != lines.Length - 1
                    ? lines[iLine + 1]
                    : string.Concat(Enumerable.Repeat(".", currentLine.Length));

                // Find the numbers in the current line
                Range[] numberRanges = FindNumberRanges(currentLine);

                // Find the symbols in each of the candidate neighbor lines
                int[] previousLineSymbolIndices = FindSymbolIndices(previousLine);
                int[] currentLineSymbolIndices = FindSymbolIndices(currentLine);
                int[] nextLineSymbolIndices = FindSymbolIndices(nextLine);

                // Identify numbers adjacent to symbols (i.e. range of number +/- 1 includes symbol index in any line)
                foreach (Range numberRange in numberRanges)
                {
                    // Pad the count by 2 (where we'd ordinarily pad by 1) to accommodate for the modified start index
                    int rangeLength = numberRange.End.Value - numberRange.Start.Value + 2;
                    int[] searchRange = Enumerable.Range(numberRange.Start.Value - 1, rangeLength).ToArray();

                    bool isSchematic = false;
                    isSchematic |= searchRange.Any(index => previousLineSymbolIndices.Contains(index));
                    isSchematic |= searchRange.Any(index => currentLineSymbolIndices.Contains(index));
                    isSchematic |= searchRange.Any(index => nextLineSymbolIndices.Contains(index));

                    if (isSchematic)
                        schematicNumbers.Add(Convert.ToInt32(currentLine[numberRange]));
                }
            }

            Assert.Equal(SOLUTION, schematicNumbers.Sum());
        }
    }

    /*

    The engineer finds the missing part and installs it in the engine! As the engine springs to life, you jump in the
    closest gondola, finally ready to ascend to the water source.

    You don't seem to be going very fast, though. Maybe something is still wrong? Fortunately, the gondola has a phone
    labeled "help", so you pick it up and the engineer answers.

    Before you can explain the situation, she suggests that you look out the window. There stands the engineer, holding
    a phone in one hand and waving with the other. You're going so slowly that you haven't even left the station. You
    exit the gondola.

    The missing part wasn't the only issue - one of the gears in the engine is wrong. A gear is any * symbol that is
    adjacent to exactly two part numbers. Its gear ratio is the result of multiplying those two numbers together.

    This time, you need to find the gear ratio of every gear and add them all up so that the engineer can figure out
    which gear needs to be replaced.

    Consider the same engine schematic again:

    467..114..
    ...*......
    ..35..633.
    ......#...
    617*......
    .....+.58.
    ..592.....
    ......755.
    ...$.*....
    .664.598..

    In this schematic, there are two gears. The first is in the top left; it has part numbers 467 and 35, so its gear
    ratio is 16345. The second gear is in the lower right; its gear ratio is 451490. (The * adjacent to 617 is not a
    gear because it is only adjacent to one part number.) Adding up all of the gear ratios produces 467835.

    What is the sum of all of the gear ratios in your engine schematic?

    */
    public class Problem2
    {
        const int SOLUTION = 73646890;

        static bool FindCandidateGearNumbers(string line,
                                             int gearIndex, 
                                             Range[] numberIndices, 
                                             [NotNullWhen(true)] out List<int>? gearNumbers)
        {
            gearNumbers = null;
            foreach (Range numberRange in numberIndices)
            {
                // Pad the count by 2 (where we'd ordinarily pad by 1) to accommodate for the modified start index
                int rangeLength = numberRange.End.Value - numberRange.Start.Value + 2;
                int[] searchRange = Enumerable.Range(numberRange.Start.Value - 1, rangeLength).ToArray();
                if (searchRange.Contains(gearIndex))
                {
                    int number = Convert.ToInt32(line[numberRange]);
                    if (gearNumbers is null)
                        gearNumbers = new() { number };
                    else
                        gearNumbers.Add(number);
                }
            }
            return gearNumbers is not null;
        }

        [Fact]
        public static void Solution1()
        {
            string[] lines = File.ReadAllLines(Path.Combine("data", "day03.txt"));

            int gearRatioSum = 0;
            for (int iLine = 0; iLine < lines.Length; iLine++)
            {
                string currentLine = lines[iLine];
                string previousLine = iLine != 0
                    ? lines[iLine - 1]
                    : string.Concat(Enumerable.Repeat(".", currentLine.Length));
                string nextLine = iLine != lines.Length - 1
                    ? lines[iLine + 1]
                    : string.Concat(Enumerable.Repeat(".", currentLine.Length));

                // Find the gears in the current line
                int[] gearIndices = Enumerable.Range(0, currentLine.Length)
                    .Where(x => currentLine[x] == '*')
                    .ToArray();

                // Find the numbers in each of the candidate neighbor lines
                Range[] previousLineNumberIndices = Problem1.FindNumberRanges(previousLine);
                Range[] currentLineNumberIndices = Problem1.FindNumberRanges(currentLine);
                Range[] nextLineNumberIndices = Problem1.FindNumberRanges(nextLine);

                List<int>? neighbor = null;
                foreach (int gearIndex in gearIndices)
                {
                    List<int> neighborNumbers = new();
                    if (FindCandidateGearNumbers(previousLine, gearIndex, previousLineNumberIndices, out neighbor))
                        neighborNumbers.AddRange(neighbor);
                    if (FindCandidateGearNumbers(currentLine, gearIndex, currentLineNumberIndices, out neighbor))
                        neighborNumbers.AddRange(neighbor);
                    if (FindCandidateGearNumbers(nextLine, gearIndex, nextLineNumberIndices, out neighbor))
                        neighborNumbers.AddRange(neighbor);

                    if (neighborNumbers.Count == 2)
                        gearRatioSum += neighborNumbers[0] * neighborNumbers[1];
                }
            }

            Assert.Equal(SOLUTION, gearRatioSum);
        }
    }
}
