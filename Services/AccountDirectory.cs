using Microsoft.Extensions.Caching.Memory;
using SendMessage.Models;
using System.Text.Json;

namespace SendMessage.Services
{
    public static class AccountDirectory
    {
        private const int accountMaxLimit = 50; //maximum limit a account allow sending sms to provider

        private static readonly TimeSpan accountExpiry = TimeSpan.FromSeconds(1);// // time limit of 1 second a account sending sms to provider

        private static MemoryCache accountDirectory = new(new MemoryCacheOptions());

        public static void sendMessageWithValidLimit(this string accountId, HttpBody httpBody)
        {

            if (accountDirectory.TryGetValue(accountId, out PhoneDirectoryReference phoneNumbersReference))
            {
                if (phoneNumbersReference.GetTotalMessagesLimit() < accountMaxLimit)
                {
                    phoneNumbersReference.SetBusinessPhone(httpBody.BusinessPhone);
                }
                else
                {
                    Console.WriteLine($"Account {accountId} max limit {accountMaxLimit} reached: message cannot be sent");
                }
            }
            else
            {
                Console.WriteLine($"Starting new accountId {accountId} session");

                PhoneDirectoryReference newPhoneNumbersReference = new PhoneDirectoryReference( httpBody.BusinessPhone);

                accountDirectory.Set(accountId, newPhoneNumbersReference, accountExpiry);
            }
        }

        public static Object GetDetails(this string accountId)
        {  
            Status res = new(accountId);

            if (accountDirectory.TryGetValue(accountId, out PhoneDirectoryReference phoneNumbersRef)) {

                res.accountLimit = phoneNumbersRef.GetTotalMessagesLimit();

                foreach (KeyValuePair<long,int> kvp in phoneNumbersRef.phoneDirectory.GetAllValidEntries()) { 

                    res.list.Add(kvp.Key +" : " + phoneNumbersRef.phoneDirectory.GetNumberOfMessages(kvp.Key));
                }
            }
            return new
            {
                accountId = res.accountId,
                accountLimit = res.accountLimit,
                list = JsonSerializer.Serialize( res.list)
            };
        }
    }
}
