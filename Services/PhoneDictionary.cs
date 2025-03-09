namespace SendMessage.Services
{
    public class PhoneDictionary<Long, Integer>
    {
        private readonly TimeSpan phoneExpiry = TimeSpan.FromSeconds(2);
        private Dictionary<Long, (int count, DateTime Expiry)> dictionary = new();
        public void SendMessagedAndSetLimit(Long phone, int count)
        {
            Console.WriteLine("Message sent...");
            dictionary[phone] = (count + 1, dictionary[phone].Expiry);
        }

        public int GetLimit(Long phone)
        {
            if (dictionary.ContainsKey(phone))
            {
                if (DateTime.Now > dictionary[phone].Expiry)
                {
                    Console.WriteLine("Phone session expired: message cannot be sent");
                    dictionary.Remove(phone);
                    return -1;
                }
            }
            else
            {
                dictionary[phone] = (0, DateTime.Now.Add(phoneExpiry));
            }

            return dictionary[phone].count;
        }

        public IEnumerable<KeyValuePair<Long, int>> GetAllValidEntries()
        {
            return dictionary.Select(kv => new KeyValuePair<Long, int>(kv.Key, kv.Value.count));
        }
    }
}
