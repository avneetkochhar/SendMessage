using Microsoft.Extensions.Caching.Memory;
using SendMessage.Models;
using System.Collections.Generic;
using System.Text.Json;

namespace SendMessage.Services
{
    public static class Message
    {
        private const int accountMaxLimit = 50;

        private static readonly TimeSpan accountExpiry = TimeSpan.FromSeconds(1);

        private static MemoryCache cache = new(new MemoryCacheOptions());

        public static void sendMessageWithValidLimit(this string accountId, HttpBody httpBody)
        {

            if (cache.TryGetValue(accountId, out AccountDirectory account))
            {
                if (account.GetAccountLimit() < accountMaxLimit)
                {
                    account.SetBusinessPhone(httpBody.BusinessPhone);
                }
                else
                {
                    Console.WriteLine($"Account {accountId} max limit {accountMaxLimit} reached: message cannot be sent");
                }
            }
            else
            {
                Console.WriteLine($"Starting new accountId {accountId} session");

                AccountDirectory accountDirectory = new AccountDirectory(accountId, httpBody.BusinessPhone);

                cache.Set(accountId, accountDirectory, accountExpiry);
            }
        }

        public static Object GetDetails(this string accountId)
        {
            int accountLimit = 0;
            Status res = new(accountId);

            if (cache.TryGetValue(accountId, out AccountDirectory account)) {

                res.accountLimit = account.GetAccountLimit();

                foreach (KeyValuePair<long,int> kvp in account.dictionary.GetAllValidEntries()) { 

                    res.list.Add(kvp.Key +" : " + account.dictionary.GetPhoneLimit(kvp.Key));
                }
            }
            return new
            {
                accountID = res.accountId,
                accountLimit = res.accountLimit,
                array = JsonSerializer.Serialize( res.list)
            };
        }
    }
}
