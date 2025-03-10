using Microsoft.Extensions.Caching.Memory;
using SendMessage.Models;
using System.Text.Json;

namespace SendMessage.Services
{
    public static class AccountDirectory
    {
        private const int accountMaxLimit = 50; //maximum limit a account allow to send messages

        private static readonly TimeSpan accountExpiry = TimeSpan.FromSeconds(1);// // time limit of 1 second for each accountId to live in memoryCache

        private static MemoryCache accountDirectory = new(new MemoryCacheOptions());

        public static void sendMessageWithValidLimit(this string accountId, HttpBody httpBody)
        {

            if (accountDirectory.TryGetValue(accountId, out AccountReference accounReference))
            {
                if (accounReference.GetAccountLimit() < accountMaxLimit)
                {
                    accounReference.SetBusinessPhone(httpBody.BusinessPhone);
                }
                else
                {
                    Console.WriteLine($"Account {accountId} max limit {accountMaxLimit} reached: message cannot be sent");
                }
            }
            else
            {
                Console.WriteLine($"Starting new accountId {accountId} session");

                AccountReference newAccountReference = new AccountReference( httpBody.BusinessPhone);

                accountDirectory.Set(accountId, newAccountReference, accountExpiry);
            }
        }

        public static Object GetDetails(this string accountId)
        {  
            Status res = new(accountId);

            if (accountDirectory.TryGetValue(accountId, out AccountReference accountReference)) {

                res.accountLimit = accountReference.GetAccountLimit();

                foreach (KeyValuePair<long,int> kvp in accountReference.phoneDirectory.GetAllValidEntries()) { 

                    res.list.Add(kvp.Key +" : " + accountReference.phoneDirectory.GetNumberOfMessages(kvp.Key));
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
