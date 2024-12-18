namespace FindDuplicateValues
{
    /// <summary>
    /// This class computes the Jaro-Winkler similarity of two strings
    /// </summary>
    public static class JaroWinkler
    {
        public static double Compute(string s1, string s2)
        {
            if (s1 == s2)
            {
                return 1.0;
            }

            if (string.IsNullOrEmpty(s1) || string.IsNullOrEmpty(s2))
            {
                return 0.0;
            }

            var matchRange = Math.Max(s1.Length, s2.Length) / 2 - 1;

            var s1Matches = new bool[s1.Length];
            var s2Matches = new bool[s2.Length];
            var matchCount = 0;

            for (var i = 0; i < s1.Length; i++)
            {
                var start = Math.Max(0, i - matchRange);
                var end = Math.Min(s2.Length - 1, i + matchRange);

                for (var j = start; j <= end; j++)
                {
                    if (!s2Matches[j] && s1[i] == s2[j])
                    {
                        s1Matches[i] = true;
                        s2Matches[j] = true;
                        matchCount++;
                        break;
                    }
                }
            }

            if (matchCount == 0)
            {
                return 0.0;
            }

            var transpositions = 0;
            var s2Index = 0;
            for (var i = 0; i < s1.Length; i++)
            {
                if (s1Matches[i])
                {
                    while (!s2Matches[s2Index])
                    {
                        s2Index++;
                    }

                    if (s1[i] != s2[s2Index])
                    {
                        transpositions++;
                    }

                    s2Index++;
                }
            }
            transpositions /= 2;

            var jaro = (1.0 / 3.0) * (
                (double)matchCount / s1.Length +
                (double)matchCount / s2.Length +
                (double)(matchCount - transpositions) / matchCount
            );

            var commonPrefix = 0;
            var prefixLimit = Math.Min(4, Math.Min(s1.Length, s2.Length));
            for (var i = 0; i < prefixLimit; i++)
            {
                if (s1[i] == s2[i])
                {
                    commonPrefix++;
                }
                else
                {
                    break;
                }
            }

            return jaro + (commonPrefix * 0.1 * (1 - jaro));
        }
    }
}
