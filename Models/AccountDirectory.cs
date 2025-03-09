using System.Runtime.InteropServices;

namespace SendMessage.Models
{
    public class AccountDirectory
    {
      //  public readonly TimeSpan TTL = TimeSpan.FromSeconds(7);
        private const int MaxLimit = 12;
        private readonly string accountID;
        public PhoneDictionary<long, int> dictionary=new();

        public AccountDirectory(string accountID, long businessPhone) { 
                this.accountID = accountID;
                this.setBusinessPhone(businessPhone);
        }

        public void setBusinessPhone(long businessPhone) {

        this.dictionary.setCount(businessPhone, dictionary.getLimit(businessPhone) + 1);

        }

        public void setCount(string accountId, int count)
        {
            //dictionary[phone] = dictionary.ContainsKey(phone) ? (count, dictionary[phone].Expiry) : (count, DateTime.Now.Add(TTL));
        }

        public int getLimit(string accountId)
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
