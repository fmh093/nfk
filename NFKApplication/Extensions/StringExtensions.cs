namespace NFKApplication.Extensions
{
    public static class StringExtensions
    {
        public static bool IsValidString (this string str)
        {
            string[] illegalKeywords = { "SELECT", "DROP", "WHERE", "INSERT", "DELETE" };

            foreach (var illegalKeyword in illegalKeywords)
            {
                if (str.Contains(illegalKeyword))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
