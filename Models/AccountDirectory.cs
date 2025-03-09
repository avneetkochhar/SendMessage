using System.Runtime.InteropServices;

namespace SendMessage.Models
{
    public class AccountDirectory
    {
        private const int phoneMaxLimit = 3;
        private readonly string accountID;
        public PhoneDictionary<long, int> dictionary = new();

        public AccountDirectory(string accountID, long businessPhone)
        {
            this.accountID = accountID;
            this.SetBusinessPhone(businessPhone);
        }

        public void SetBusinessPhone(long businessPhone)
        {
            int limit = dictionary.GetLimit(businessPhone);

            if (limit > -1 & limit < phoneMaxLimit)
            {
                this.dictionary.SendMessagedAndSetLimit(businessPhone, limit);
            }
            else if (limit >= phoneMaxLimit)
            {
                Console.WriteLine($"Phone max limit {phoneMaxLimit} reached: message cannot be sent");
            }

        }

        public int GetLimit(string accountId)
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
