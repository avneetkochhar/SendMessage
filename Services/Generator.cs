using SendMessage.Models;

namespace SendMessage.Services
{
    public static class Generator
    {
        private static HttpBody[] generateMessages(int n)
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

        public static Account[] generateData(this int numberOfMessages)
        {
            string[] accounts = { "acc1", "acc2", "acc3", "acc4" };
            int n = accounts.Length;
            Account[] accountArray = new Account[n];
            Random random = new Random();
            while (--n >= 0)
            {
                accountArray[n] = new Account
                {
                    accountId = accounts[random.Next(0, n)],
                    httpMessages = generateMessages(numberOfMessages)
                };
            }
            return accountArray;
        }
    }
}
