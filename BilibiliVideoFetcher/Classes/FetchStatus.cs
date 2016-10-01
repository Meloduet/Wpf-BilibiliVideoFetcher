using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BilibiliVideoFetcher.Classes
{
    public struct FetchStatus
    {
        public int Status;
        public string Message;
        public FetchStatus(int status, string message)
        {
            Status = status;
            Message = message;
        }
    }
}
