using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using SendMessage.Models;
using System.Text.Json;

namespace SendMessage
{
    [ApiController]
    [Route("api/message")]

    public class MessageController : ControllerBase
    {
        //private static PhoneDictionary<long, int> dictionary = new();
        private static MemoryCache cache = new(new MemoryCacheOptions());
        private static AccountDirectory accountDirectory;
        private static HttpBody b=new();

        [HttpPost("send")]
        public IActionResult SendMessage([FromHeader(Name= "Account-Number")] string accountId, HttpBody httpBody)
        {    
           b=httpBody;



            if (cache.TryGetValue(accountId, out AccountDirectory account)) {
                account.setBusinessPhone(httpBody.BusinessPhone);
            }
            else
            {               
                accountDirectory = new AccountDirectory(accountId, httpBody.BusinessPhone);
                cache.Set(accountId, accountDirectory, TimeSpan.FromSeconds(5));
            }



            //dictionary.setCount(httpBody.BusinessPhone, dictionary.getCount(httpBody.BusinessPhone)+1);


            return Ok(new
            {
                Status = "Success "+ JsonSerializer.Serialize(httpBody),
              
            });
        }

        public Object GetBusiness() {
            return new  { 
              phone = b.BusinessPhone,
              count = accountDirectory.dictionary.getLimit(b.BusinessPhone),
              ttl =5,
            };
        }
    }
}
