using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MjpegProcessor;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Policy;
using Newtonsoft.Json;

namespace prova_streaming
{
    public partial class Form1 : Form
    {
        private MjpegDecoder mjpeg;
        private string plateOCR_url;

        public Form1()
        {
            InitializeComponent();
            plateOCR_url = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            getframe();
            plateOCR();
        }

        private void getframe()
        {
            mjpeg = new MjpegDecoder();
            mjpeg.FrameReady += mjpeg_FrameReady;
            mjpeg.Error += mjpeg_Error;

            mjpeg.ParseStream(new Uri("http://192.168.103.50:81/stream"));
            MessageBox.Show("Frame Catturato");

            mjpeg.StopStream();
        }

        private async Task<plateOCR> plateOCR()
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(plateOCR_url);  // Usa PostAsync per le richieste POST

                    var resultString = await response.Content.ReadAsStringAsync();

                    response.EnsureSuccessStatusCode();  // Lancia un'eccezione se il codice di stato non è OK

                    var results = JsonConvert.DeserializeObject<plateOCR>(resultString);

                    return results;
                }
                catch (Exception ex)
                {
                    // Gestione degli errori, ad esempio se il server non è raggiungibile
                    return null;
                }
            }
        }

        private void mjpeg_FrameReady(object sender, FrameReadyEventArgs e)
        {
            Bitmap bmp;
            using (var ms = new MemoryStream(e.FrameBuffer))
            {
                bmp = new Bitmap(ms);
            }

            System.Drawing.Image newImg = (System.Drawing.Image)bmp.Clone();
            bmp.Dispose();

            newImg.RotateFlip(RotateFlipType.Rotate270FlipNone);

            System.Drawing.Graphics gr = System.Drawing.Graphics.FromImage(newImg);
            string drawString = "camera!";
            System.Drawing.Font drawFont = new System.Drawing.Font("Arial", 12);
            System.Drawing.SolidBrush drawBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Red);

            float x = 560.0f;
            float y = 460.0f;
            gr.FillRectangle(new System.Drawing.SolidBrush(System.Drawing.Color.Green), new System.Drawing.Rectangle(545, 465, 10, 10));

            gr.DrawString(drawString, drawFont, drawBrush, x, y);

            gr.DrawString(DateTime.Now.ToLocalTime().ToString(), drawFont, drawBrush, 12, y);

            pictureBox1.Image = newImg; //disegna nella box

            drawFont.Dispose();
            drawBrush.Dispose();
            gr.Dispose();
        }

        private void mjpeg_Error(object sender, MjpegProcessor.ErrorEventArgs e)
        {
            MessageBox.Show(e.Message);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
