namespace SendMessage.Services
{
    public class AccountDirectory
    {
        private const int phoneMaxLimit = 3;
        public PhoneDictionary<long, int> dictionary = new();

        public AccountDirectory(string accountID, long businessPhone)
        {
            SetBusinessPhone(businessPhone);
        }

        public void SetBusinessPhone(long businessPhone)
        {
            int limit = dictionary.GetLimit(businessPhone);

            if (limit > -1 & limit < phoneMaxLimit)
            {
                dictionary.SendMessagedAndSetLimit(businessPhone, limit);
            }
            else if (limit >= phoneMaxLimit)
            {
                Console.WriteLine($"Phone max limit {phoneMaxLimit} reached: message cannot be sent");
            }

        }

        public int GetLimit()
        {
            int limit = 0;

            foreach (var kvp in dictionary.GetAllValidEntries())
            {
                limit += kvp.Value;
            }

            return limit;
        }
    }
}
