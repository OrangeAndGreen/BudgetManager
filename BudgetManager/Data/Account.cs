namespace BudgetManager.Data
{
    public class Account
    {
        //TODO: Not using this yet
        public int Id { get; set; }
        public AccountType Type { get; set; }
        public string Label { get; set; }

        public Account(int id, AccountType accountType, string label)
        {
            Id = id;
            Type = accountType;
            Label = label;
        }
    }
}
