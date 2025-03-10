using System.Runtime.InteropServices;

namespace SendMessage.Services
{
    public class PhoneDirectoryReference
    {
        private const int phoneSmsMaxLimit = 50; //maximum limit a phoneNumber allow sending sms to provider

        public PhoneDirectory<long, int> phoneDirectory = new();

        public PhoneDirectoryReference(long businessPhone)
        {
            UpdatePhoneNumberAndSendMessage(businessPhone);
        }

        public void UpdatePhoneNumberAndSendMessage(long businessPhone)
        {
            int limit = phoneDirectory.GetNumberOfMessages(businessPhone);

            if (limit > -1 & limit < phoneSmsMaxLimit)
            {
                phoneDirectory.SendMessagedAndSetLimit(businessPhone, limit);
            }
            else if (limit >= phoneSmsMaxLimit)
            {
                Console.WriteLine($"Phone {businessPhone} max limit {phoneSmsMaxLimit} reached: message cannot be sent");
            }

        }

        public int GetTotalMessagesSent()
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
