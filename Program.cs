using FindDuplicateValues;

var values = File.ReadAllLines(args[0]);

foreach (var duplicate in Duplicates.Find(values).OrderBy(x => x))
{
    Console.WriteLine(duplicate.Item1 + " matches " + duplicate.Item2);
}