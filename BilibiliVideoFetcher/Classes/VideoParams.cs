using BilibiliVideoFetcher.Data;
using BilibiliVideoFetcher.Helper;

namespace BilibiliVideoFetcher.Classes
{
    public class VideoParams
    {
        public string Player { get; set; }

        public int CutId { get; set; }

        public string Quality { get; set; }

        public VideoParams(string cid) : this(int.Parse(cid))
        {
        }

        public VideoParams(int cid)
        {
            this.Player = "1";
            this.CutId = cid;

            var appSettings = ApplicationSettings.GetInstance();

            if (appSettings.FetchingOption.Format == "flv")
            {
                this.Quality = "3";
            }
            else
            {
                if (appSettings.FetchingOption.Quality == "high")
                {
                    this.Quality = "2";
                }
                else
                {
                    this.Quality = "1";
                }
            }
        }

        public string ToQueryString()
        {

            return BiliBiliApiHelper.SignQueryParams($"cid={CutId}&player={Player}&quality={Quality}");
        }
    }
}
