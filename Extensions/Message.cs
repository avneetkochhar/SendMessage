using Microsoft.Extensions.Caching.Memory;
using SendMessage.Services;
using System.Text.Json;

namespace SendMessage
{
    public static class Message
    {
        private const int accountMaxLimit = 2;
        private static readonly TimeSpan accountExpiry = TimeSpan.FromSeconds(5);
        private static MemoryCache cache = new(new MemoryCacheOptions());
        private static string accountNumber;
        public static void sendMessageWithValidLimit(this string accountId, HttpBody httpBody) {

            if (cache.TryGetValue(accountId, out AccountDirectory account))
            {
                if (account.GetLimit() < accountMaxLimit)
                {
                    account.SetBusinessPhone(httpBody.BusinessPhone);
                }
                else
                {
                    Console.WriteLine($"Account max limit {accountMaxLimit} reached: message cannot be sent");
                }
            }
            else
            {
                Console.WriteLine("Starting new accountId session");

                AccountDirectory accountDirectory = new AccountDirectory(accountId, httpBody.BusinessPhone);

                cache.Set(accountId, accountDirectory, accountExpiry);
            }
        }

        public static Object GetDetails(this string accountId)
        {
            return new
            {
                account = accountNumber,
                limit = JsonSerializer.Serialize(cache.Get(accountId)) ,

            };
        }
    }
}
