using System.Collections;
using System.Runtime.InteropServices;

namespace SendMessage.Models
{
    public class Status
    {
        public string accountId;
        public int accountLimit;
        public ArrayList list ;

        public Status(string account) { 
        this.accountId=account;
            list=new ArrayList();
        }
    }
}
