using Microsoft.AspNetCore.DataProtection.KeyManagement;
using System.Collections.Concurrent;
using System.Linq;
using static System.Formats.Asn1.AsnWriter;

namespace SendMessage.Models
{
    public class PhoneDictionary<Long, Integer>
    {
        public readonly TimeSpan TTL = TimeSpan.FromSeconds(4);
        private const int MaxLimit = 6;
        private Dictionary<Long, (int count, DateTime Expiry)> dictionary = new();

        public void setCount(Long phone, int count)
        {
            dictionary[phone] = dictionary.ContainsKey(phone) ? (count, dictionary[phone].Expiry) : (count, DateTime.Now.Add(TTL));
        }

        public int getLimit(Long phone)
        {
            if (dictionary.ContainsKey(phone))
            {
                if (DateTime.Now > dictionary[phone].Expiry || dictionary[phone].count > MaxLimit)
                {
                    dictionary.Remove(phone);
                }
                else
                {
                    return dictionary[phone].count;
                }
            }
            return default;
        }

        public IEnumerable<KeyValuePair<Long, int>> GetAllValidEntries()
        {
            return dictionary.Where(kv => DateTime.UtcNow < kv.Value.Expiry )
                        .Select(kv => new KeyValuePair<Long, int>(kv.Key,kv.Value.count));
        }
    }
}
