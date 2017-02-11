using System;

namespace BilibiliVideoFetcher.Process
{
    public class FetchException : Exception
    {
        public FetchException()
        {
        }

        public FetchException(string message) : base(message)
        {
        }
    }

}
