year=$1

mkdir $year && cd $year
mkdir data

for i in $(seq -w 01 25); do
    mkdir "day${i}"
    cd "day${i}"
    echo > Solution.cs
    cd ..
done
dotnet new xunit --name "aoc_${year}" --output .
rm UnitTest1.cs

cd ..
dotnet sln add "${year}/aoc_${year}.csproj"
