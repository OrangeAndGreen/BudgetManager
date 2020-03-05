namespace BudgetManager
{
    public static class Constants
    {
        public const string CacheDirectory = "Cache";
        public const string TypesFilename = "TransactionTypes.txt";
    }

    public enum AccountType
    {
        Unknown = 0,
        Checking = 1,
        Credit = 2,
        Savings = 3,
        Amazon = 4
    }
}
