namespace BudgetManager.Data
{
    public class TransactionType
    {
        public int Id { get; set; }
        public string Identifier { get; set; }
        public string Vendor { get; set; }
        public string Category { get; set; }
        public bool IdentifierMustStart { get; set; }
        public bool IdentifierCaseSensitive { get; set; }


        public bool Matches(string identifier)
        {
            return DoesIdentifierMatch(Identifier, identifier, IdentifierCaseSensitive, IdentifierMustStart);
        }

        public static bool DoesIdentifierMatch(string pattern, string description, bool caseSensitive, bool startsWith)
        {
            if (description == null)
            {
                return false;
            }

            if (!caseSensitive)
            {
                description = description.ToLower();
                pattern = pattern.ToLower();
            }

            if (startsWith)
            {
                return description.StartsWith(pattern);
            }

            return description.Contains(pattern);
        }
    }
}
