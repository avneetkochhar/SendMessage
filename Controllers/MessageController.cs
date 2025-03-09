using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace SendMessage
{
    [ApiController]
    [Route("api/message")]

    public class MessageController : ControllerBase
    {
        private static string accountNumber ;

        [HttpPost("send")]
        public IActionResult SendMessage([FromHeader(Name = "Account-Number")] string accountId, HttpBody httpBody)
        {
            accountNumber = accountId;

            accountId.sendMessageWithValidLimit(httpBody);

            return Ok(new
            {
                Status = "Success " + JsonSerializer.Serialize(httpBody),

            });
        }

        public Object GetDataForWebSocket()
        {
            return accountNumber.GetDetails();
        }
    }
}
