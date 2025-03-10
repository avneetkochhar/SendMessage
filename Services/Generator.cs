using SendMessage.Models;
using System.Collections;

namespace SendMessage.Services
{
    public static class Generator
    {
        private static string[] accounts = { "acc1", "acc2", "acc3", "acc4" };
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

        public static Account[] GenerateData(this int numberOfMessages)
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

    }
}
