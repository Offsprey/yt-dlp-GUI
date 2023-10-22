using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace yt_dlp_GUI
{
    public partial class Form1 : Form
    {
        static List<DownloadTask> dlTaskList;

        public Form1()
        {
            InitializeComponent();
            dlTaskList = new List<DownloadTask>();
        }
        

        private void button1_Click(object sender, EventArgs e)
        {
            if(dlTaskList.Count > 0 && !backgroundWorker1.IsBusy)
            {
                backgroundWorker1.RunWorkerAsync(dlTaskList[0]);
                button1.Text = "Cancel";
            }
            else if (backgroundWorker1.IsBusy)
            {
                backgroundWorker1.CancelAsync();
                button1.Text = "Start";
                Thread.Sleep(300);
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            DownloadTask dTask = (DownloadTask)e.Argument;
            var proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "yt-dlp.exe",
                    Arguments = dTask.Args,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                }
            };
            try
            {
                proc.Start();
            }
            catch (Exception ex)
            {
                backgroundWorker1.ReportProgress(0, ex.Message);
            }
            while (!proc.StandardOutput.EndOfStream)
            {
                if (backgroundWorker1.CancellationPending)
                {
                    try
                    {
                        proc.Kill();
                    }
                    catch (Exception ex)
                    {
                        dTask.StatusMsg = "Error : " + ex.Message;
                    }
                }
                string line = proc.StandardOutput.ReadLine();
                dTask.StatusMsg = line;
                backgroundWorker1.ReportProgress(0, dTask);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            ListViewItem nItem = new ListViewItem();
            String ytArg = textBox1.Text;
            if (textBox2.Text != "")
                ytArg += " -P " + textBox2.Text;
            if (textBox3.Text != "")
                ytArg += " " + textBox3.Text;
            nItem.Text = ytArg;
            dlTaskList.Add(new DownloadTask(ytArg));
            nItem.SubItems.Add("Pending");
            listView1.Items.Add(nItem);
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            DownloadTask cTask = (DownloadTask)e.UserState;
            foreach (ListViewItem item in listView1.Items)
            {
                if (item.Text == cTask.Args)
                    item.SubItems[1].Text = cTask.StatusMsg;
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            dlTaskList.RemoveAt(0);
            if (dlTaskList.Count > 0 && !backgroundWorker1.IsBusy)
            {
                backgroundWorker1.RunWorkerAsync(dlTaskList[0]);
            }
            else
            {
                button1.Text = "Start";
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                button4.Enabled = true;
            }
            else
            {
                button4.Enabled = false;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView1.SelectedItems)
            {

                foreach (DownloadTask task in dlTaskList)
                {
                    if (task.Args == item.Text)
                        dlTaskList.Remove(task);
            }
                listView1.Items.Remove(item);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            dlTaskList.Add(new DownloadTask(textBox1.Text));
            ListViewItem nItem = new ListViewItem();
            nItem.Text = textBox1.Text;
            nItem.SubItems.Add("Pending");
            listView1.Items.Add(nItem);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox2.Text = Properties.Settings.Default.DownloadDir;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.DownloadDir = textBox2.Text;
            Properties.Settings.Default.Save();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.SelectedPath = textBox2.Text;
            folderBrowserDialog1.ShowDialog();
            textBox2.Text = folderBrowserDialog1.SelectedPath;
            Properties.Settings.Default.DownloadDir = textBox2.Text;
            Properties.Settings.Default.Save();
        }

        private void folderBrowserDialog1_HelpRequest(object sender, EventArgs e)
        {
            

        }
    }
}
