﻿using System.Runtime.InteropServices;

namespace SendMessage.Services
{
    public class AccountDirectory
    {
        private const int phoneMaxLimit = 50;
        public PhoneDictionary<long, int> dictionary = new();

        public AccountDirectory(string accountID, long businessPhone)
        {
            SetBusinessPhone(businessPhone);
        }

        public void SetBusinessPhone(long businessPhone)
        {
            int limit = dictionary.GetPhoneLimit(businessPhone);

            if (limit > -1 & limit < phoneMaxLimit)
            {
                dictionary.SendMessagedAndSetLimit(businessPhone, limit);
            }
            else if (limit >= phoneMaxLimit)
            {
                Console.WriteLine($"Phone {businessPhone} max limit {phoneMaxLimit} reached: message cannot be sent");
            }

        }

        public int GetAccountLimit()
        {
            int limit = 0;

            foreach (var kvp in dictionary.GetAllValidEntries())
            {
                limit += kvp.Value;
            }
            return limit;
        }

        //public Dictionary<Long, (int count, DateTime Expiry)> getPhoneDictionary()
        //{
        //    return dictionary;
        //}
    }
}
