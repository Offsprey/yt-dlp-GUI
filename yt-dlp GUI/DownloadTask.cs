using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace yt_dlp_GUI
{
    internal class DownloadTask
    {
        
        String args;
        String statusMsg;

        public DownloadTask(string args)
        {
            this.args = args;
        }

        public string Args { get => args; set => args = value; }
        public string StatusMsg { get => statusMsg; set => statusMsg = value; }


    }
}
