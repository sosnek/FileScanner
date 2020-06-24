using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileScan
{
    
    public partial class Form1 : Form
    {

        public static string _APIkey;

        public Form1()
        {
            InitializeComponent();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    var filePath = openFileDialog.FileName;
                    filePath_textbox.Text = filePath.ToString();

                    FileInfo.FileInfoInstance.File_Path = filePath.ToString();
                    FileInfo.FileInfoInstance.File_Name = openFileDialog.SafeFileName;

                    fileName_textbox.Text = FileInfo.FileInfoInstance.File_Name;
                    MD5_textbox.Text = Utility.CalculateMD5(filePath);
                    SHA1_textbox.Text = Utility.CalculateSHA1(filePath);
                    SHA256_textbox.Text = Utility.CalculateSHA256(filePath);
                    fileSize_textBox.Text = Utility.CalculateFileSize(filePath).ToString() + " bytes";
                }
            }
        }

        private async void scan_button_Click(object sender, EventArgs e)
        {
            File_Path_Label_Warning.Visible = false;
            API_Key_Warning_Label.Visible = false;
            if (filePath_textbox.Text.Equals("File path") || filePath_textbox.Text.Equals(""))
            {
                File_Path_Label_Warning.Visible = true;
                return;
            }

            if (APIKey_textbox.Text.Length < 64)
            {
                API_Key_Warning_Label.Visible = true;
                return;
            }

            if(_APIkey == null)
            {
                _APIkey = APIKey_textbox.Text;
                APIHelper.ApiClient.DefaultRequestHeaders.Add("x-apikey", _APIkey);
            }
            pictureBox7.Visible = true;

            //TODO: check if file is larger than 33,554,432 bytes
            await GetScanResultsAsync();
            pictureBox7.Visible = false;
        }


        private async Task GetScanResultsAsync()
        {
            ScanResults scanResults = null;
            do
            {
                scanResults = await UploadFile.CreateScanReqAsync();
                
                if (scanResults.Data.Attributes.LastAnalysisResults.Count > 72)
                {
                    break;
                }
                else
                {
                    // if results are still empty, wait 10 seconds and request them again
                    await Task.Delay(10000);
                }
            } while (true);
            
            int malicious = 0;
            int undetected = 0;
            int unknown = 0;
            if(scanResults != null)
            {
                foreach (KeyValuePair<string, LastAnalysisResult> entry in scanResults.Data.Attributes.LastAnalysisResults)
                {
                    // do something with entry.Value or entry.Key
                    if ((int)entry.Value.Category == 0)
                    {
                        malicious++;
                    }
                    else if ((int)entry.Value.Category == 1)
                    {
                        unknown++;
                    }
                    else if ((int)entry.Value.Category == 2)
                    {
                        undetected++;
                    }
                    else if ((int)entry.Value.Category == 3)
                    {
                        unknown++;
                    }
                    else if ((int)entry.Value.Category == 4)
                    {
                        unknown++;
                    }
                }

                Func<ChartPoint, string> labelPoint = chartPoint => string.Format("{0} ({1:P})", chartPoint.Y, chartPoint.Participation);
                SeriesCollection piechartData = new SeriesCollection
                {
                    new PieSeries
                    {
                        Title = "Malicious",
                        Values = new ChartValues<double> {malicious},
                        DataLabels = true,
                        LabelPoint = labelPoint,
                        Fill = System.Windows.Media.Brushes.Maroon
                    },
                    new PieSeries
                    {
                        Title = "Undetected",
                        Values = new ChartValues<double> {undetected},
                        DataLabels = true,
                        LabelPoint = labelPoint,
                        Fill = System.Windows.Media.Brushes.MediumBlue
                    },
                    new PieSeries
                    {
                        Title = "Unknown",
                        Values = new ChartValues<double> {unknown},
                        DataLabels = true,
                        LabelPoint = labelPoint,
                        Fill = System.Windows.Media.Brushes.Gray
                    }
                };

                pieChart1.Series = piechartData;
                CommunityVotesChart(scanResults.Data.Attributes.TotalVotes.Harmless, scanResults.Data.Attributes.TotalVotes.Malicious);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }


        public void CommunityVotesChart(long safeCount, long maliciousCount)
        {
            chart1.Series.Clear();
            chart1.Series.Add("Safe");
            chart1.Series["Safe"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Bar;
            chart1.Series.Add("Malicious");
            chart1.Series["Malicious"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Bar;

            chart1.Series["Safe"].Color = Color.MediumBlue;
            chart1.Series["Malicious"].Color = Color.Maroon;

            if(safeCount < 1 && maliciousCount < 1)
            {
                chart1.Series.Clear();
                chart1.Series.Add("No Votes Found");
                chart1.Series["No Votes Found"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Bar;
                chart1.Series["No Votes Found"].Color = System.Drawing.Color.Gray;
                chart1.Series["No Votes Found"].Points.AddXY("No Votes Found", 1);
            }
            else
            {
                chart1.Series["Safe"].Points.AddXY("2", safeCount);
                chart1.Series["Malicious"].Points.Add(0);
                chart1.Series["Malicious"].Points.AddXY("1", maliciousCount);
            }
            
        }

        private void label13_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void label14_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }


        private void mouse_Hover(object sender, EventArgs e)
        {
            Label hoveredLabel = (Label)sender;

            hoveredLabel.ForeColor = Color.Purple;
        }

        private void mouse_Leave(object sender, EventArgs e)
        {
            Label hoveredLabel = (Label)sender;

            hoveredLabel.ForeColor = Color.Silver;
        }


        // https://stackoverflow.com/questions/1592876/make-a-borderless-form-movable

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        private void Form1_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
}
