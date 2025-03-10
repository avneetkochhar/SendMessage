using Microsoft.Extensions.Caching.Memory;
using SendMessage.Models;
using System.Text.Json;

namespace SendMessage.Services
{
    public static class AccountDirectory
    {
        private const int accountMaxLimit = 50;

        private static readonly TimeSpan accountExpiry = TimeSpan.FromSeconds(1);// 

        private static MemoryCache accoutDirectory = new(new MemoryCacheOptions());

        public static void sendMessageWithValidLimit(this string accountId, HttpBody httpBody)
        {

            if (accoutDirectory.TryGetValue(accountId, out AccountReference accounReference))
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

                accoutDirectory.Set(accountId, newAccountReference, accountExpiry);
            }
        }

        public static Object GetDetails(this string accountId)
        {  
            Status res = new(accountId);

            if (accoutDirectory.TryGetValue(accountId, out AccountReference accountDirectory)) {

                res.accountLimit = accountDirectory.GetAccountLimit();

                foreach (KeyValuePair<long,int> kvp in accountDirectory.phoneDirectory.GetAllValidEntries()) { 

                    res.list.Add(kvp.Key +" : " + accountDirectory.phoneDirectory.GetNumberOfMessages(kvp.Key));
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
