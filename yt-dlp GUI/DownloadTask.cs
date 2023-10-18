using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace yt_dlp_GUI
{
    internal class DownloadTask
    {
        String url;
        String args;
        String statusMsg;

        public DownloadTask(string url)
        {
            this.url = url;
        }

        public string Url { get => url; set => url = value; }
        public string StatusMsg { get => statusMsg; set => statusMsg = value; }
    }
}
