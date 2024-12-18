namespace FindDuplicateValues
{
    public class Duplicates
    {
        public static IEnumerable<(string, string)> Find(IEnumerable<string> values)
        {
            var keyBuilder = new Phonix.Metaphone();

            // find a phonetical key to associate to the value to group values by how they sound
            foreach (var group in values.Select(x => new { Name = x, Key = keyBuilder.BuildKey(x) })
                .GroupBy(a => a.Key)
                .Where(g => g.Count() > 1)
                .Select(g => g.Select(a => a.Name)))
            {
                // within the phonetical group, compare all values to find best matches
                var matches = GetAllPairs(group.ToList())
                    .Select(p => new
                    {
                        X = p.Item1,
                        Y = p.Item2,
                        Similarity = JaroWinkler.Compute(p.Item1, p.Item2)
                    }).Where(a => a.Similarity > 0.95);

                foreach (var match in matches)
                {
                    yield return new (match.X, match.Y);
                }
            }
        }

        private static List<(T, T)> GetAllPairs<T>(List<T> items)
        {
            var result = new List<(T, T)>();

            for (var i = 0; i < items.Count; i++)
            {
                for (int j = i + 1; j < items.Count; j++)
                {
                    result.Add((items[i], items[j]));
                }
            }

            return result;
        }
    }
}
