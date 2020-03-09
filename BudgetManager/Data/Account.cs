namespace BudgetManager.Data
{
    public class Account
    {
        public AccountType Type { get; set; }
        public string Label { get; set; }

        public Account(AccountType accountType, string label)
        {
            Type = accountType;
            Label = label;
        }
    }
}
