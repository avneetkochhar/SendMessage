namespace SendMessage.Services
{
    public class PhoneDirectory<Long, Integer>
    {
        private readonly TimeSpan phoneExpiry = TimeSpan.FromSeconds(1);// time limit of 1 second for each phoneNumber to send sms

        private Dictionary<Long, (int numberOfMessages, DateTime Expiry)> phoneDirectory = new();
        public void SendMessagedAndSetLimit(Long phone, int count)
        {
            Console.WriteLine($"Message sent from {phone}...");

            phoneDirectory[phone] = (count + 1, phoneDirectory[phone].Expiry);
        }

        public int GetNumberOfMessages(Long phone)
        {
            if (phoneDirectory.ContainsKey(phone))
            {
                if (DateTime.Now > phoneDirectory[phone].Expiry)
                {
                    Console.WriteLine($"Phone# {phone} session expired: message cannot be sent");

                    phoneDirectory.Remove(phone);

                    return -1;
                }
            }
            else
            {
                phoneDirectory[phone] = (0, DateTime.Now.Add(phoneExpiry));
            }

            return phoneDirectory[phone].numberOfMessages;
        }

        public IEnumerable<KeyValuePair<Long, int>> GetAllValidEntries()
        {
            return phoneDirectory.Select(kv => new KeyValuePair<Long, int>(kv.Key, kv.Value.numberOfMessages));
        }

       
    }
}
