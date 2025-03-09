using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using SendMessage.Models;
using System.Security.Principal;
using System.Text.Json;

namespace SendMessage
{
    [ApiController]
    [Route("api/message")]

    public class MessageController : ControllerBase
    {
        private const int accountMaxLimit = 2;
        private readonly TimeSpan accountExpiry = TimeSpan.FromSeconds(5);
        private static MemoryCache cache = new(new MemoryCacheOptions());
        private static string accountId ;
        private static int curentLimit ;

        [HttpPost("send")]
        public IActionResult SendMessage([FromHeader(Name = "Account-Number")] string accountId, HttpBody httpBody)
        {
            accountId = accountId;

            if (cache.TryGetValue(accountId, out AccountDirectory account))
            {               
                if (account.GetLimit(accountId) < accountMaxLimit)
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

            return Ok(new
            {
                Status = "Success " + JsonSerializer.Serialize(httpBody),

            });
        }

        public Object GetBusiness()
        {
            return new {
                account = accountId,
              

            };
        }
    }
}
