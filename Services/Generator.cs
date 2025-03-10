using SendMessage.Models;
using System.Collections;
using System.Net.WebSockets;
using System.Text.Json;
using System.Text;

namespace SendMessage.Services
{
    public static class Generator
    {
        private static string[] accounts = { "acc1" , "acc2", "acc3", "acc4" };
        private static HttpBody[] GenerateMessages(int n)
        {

            long[] phones = { 3065023453, 6359999963, 1234567891 };
            HttpBody[] data = new HttpBody[n];
            Random random = new Random();
            while (--n >= 0)
            {
                data[n] = new HttpBody
                {
                    BusinessPhone = phones[random.Next(0, phones.Length)],
                    CustomerPhone = 1234,
                    Message = "hello"
                };

            }
            return data;
        }
        private static Account[] CreateMessagesForEachAccount(this int numberOfMessages)
        {           
            int n = accounts.Length;
            Account[] accountArray = new Account[n];
            Random random = new Random();
            while (--n >= 0)
            {
                accountArray[n] = new Account
                {
                    accountId = accounts[random.Next(0, n)],
                    httpMessages = GenerateMessages(numberOfMessages)
                };
            }
            return accountArray;
        }

        public static async Task GenerateTestDataAndGetUpdatesAsync(int messagesPerAccount)
        {
            Account[] accountArray = messagesPerAccount.CreateMessagesForEachAccount();

            foreach (Account account in accountArray)
            {

                foreach (HttpBody httpBody in account.httpMessages)
                {
                    account.accountId.sendMessageWithValidLimit(httpBody);
                }

                Object status = account.accountId.GetDetails();

                Console.WriteLine(status);

            }
        }
        public static async Task GenerateTestDataAndGetUpdatesAsync(this HttpContext context, int messagesPerAccount)
        {
            using WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();

            Account[] accountArray = messagesPerAccount.CreateMessagesForEachAccount();

            foreach (Account account in accountArray)
            {

                foreach (HttpBody httpBody in account.httpMessages)
                {
                    account.accountId.sendMessageWithValidLimit(httpBody);
                }

                Object status = account.accountId.GetDetails();

                Console.WriteLine(status);

                string jsonMessage = JsonSerializer.Serialize(status);

                var buffer = Encoding.UTF8.GetBytes(jsonMessage);

                await webSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);

            }
        }
    }
}
