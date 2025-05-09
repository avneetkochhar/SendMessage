﻿using SendMessage.Models;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

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

        private static Account[] CreateMessagesForAllAccount(this int numberOfMessages)
        {           
            int n = accounts.Length;
            Account[] accountArray = new Account[n];
            Random random = new Random();
            while (--n >= 0)
            {
                accountArray[n] = new Account
                {
                    accountId = accounts[random.Next(0, accounts.Length)],
                    httpMessages = GenerateMessages(numberOfMessages)
                };
            }
            return accountArray;
        }

        public static async Task GenerateTestDataAndSendSMS(int messagesPerAccount)
        {
            WebSocket webSocket = null;

            await CreateAndSendMessages(messagesPerAccount, webSocket);
        }       

        public static async Task GenerateTestDataToSendSMSAndGetUpdatesWithWebSocket(this HttpContext context, int messagesPerAccount)
        {
            using WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();

            await CreateAndSendMessages(messagesPerAccount, webSocket);

        }
        
        private static async Task CreateAndSendMessages(int messagesPerAccount, WebSocket webSocket)
        {
            Account[] accountArray = messagesPerAccount.CreateMessagesForAllAccount();

            DateTime startTime = DateTime.Now;

            foreach (Account account in accountArray)
            {
                for (int i = 0, batchSize = 100; i < account.httpMessages.Count(); i += batchSize)
                {
                  var batchParameters = account.httpMessages.Skip(i).Take(batchSize).ToList();

                    var tasks = batchParameters.Select(async httpBody =>
                    {                       
                            account.accountId.sendMessageWithValidLimit(httpBody);                        
                    });

                     await Task.WhenAll(tasks);
                }

                Object status = account.accountId.GetDetails();

                Console.WriteLine(status);

                await SendUpdatesToWebSocket(webSocket, status);
            }

            TimeSpan difference = DateTime.Now - startTime;

            Console.WriteLine($"Time taken {(int)difference.TotalMinutes} minute {(int)difference.TotalSeconds} seconds ");
        }
        
        private static async Task SendUpdatesToWebSocket(WebSocket? webSocket, object status)
        {
            if ( !( webSocket==null))
            {
                string jsonMessage = JsonSerializer.Serialize(status);

                var buffer = Encoding.UTF8.GetBytes(jsonMessage);

                await webSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }
    }
}
