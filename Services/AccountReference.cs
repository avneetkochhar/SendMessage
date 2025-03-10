using System.Runtime.InteropServices;

namespace SendMessage.Services
{
    public class AccountReference
    {
        private const int messagesMaxLimit = 50; //maximum limit a phone allow to send messages

        public PhoneDirectory<long, int> phoneDirectory = new();

        public AccountReference(long businessPhone)
        {
            SetBusinessPhone(businessPhone);
        }

        public void SetBusinessPhone(long businessPhone)
        {
            int limit = phoneDirectory.GetNumberOfMessages(businessPhone);

            if (limit > -1 & limit < messagesMaxLimit)
            {
                phoneDirectory.SendMessagedAndSetLimit(businessPhone, limit);
            }
            else if (limit >= messagesMaxLimit)
            {
                Console.WriteLine($"Phone {businessPhone} max limit {messagesMaxLimit} reached: message cannot be sent");
            }

        }

        public int GetAccountLimit()
        {
            int limit = 0;

            foreach (var kvp in phoneDirectory.GetAllValidEntries())
            {
                limit += kvp.Value;
            }
            return limit;
        }
    }
}
