# Advent of Code

My solutions to [Advent of Code](https://adventofcode.com/), organized by year. I began participating in Advent of Code
in 2023. Solutions are written in C# and implemented as unit tests using the `Xunit` framework, such that calculating
all the solutions can be done by simply calling `dotnet test` from the base of the repository.

## Scaffolding New Years

To keep things organized without cumbersome copy/paste file IO, I've written a simple Bash script to scaffold out a full
directory structure for a given year.

```bash
./scaffold_year.sh <YEAR>
```
