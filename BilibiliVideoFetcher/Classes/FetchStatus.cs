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
