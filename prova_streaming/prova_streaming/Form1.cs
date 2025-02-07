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
        System.Drawing.Image imgGlobal;
        private MjpegDecoder mjpeg;
        private string plateOCR_url;
        
        public Form1()
        {
            InitializeComponent();
            plateOCR_url = "http://127.0.0.1:5000/process_image";
            getframe();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            getframe();
            PlateOCR detection = await plateOCR();

            if (detection != null)
            {
                Bitmap bitmap = new Bitmap(imgGlobal);
                Graphics g = Graphics.FromImage(bitmap);

                //MessageBox.Show(detection.bbox_points.Count.ToString());
                //[[(103, 581), (386, 581), (103, 674), (386, 674)]]

                foreach (List<List<int>> bbox in detection.bbox_points)
                {
                    //[(103, 581), (386, 581), (103, 674), (386, 674)]

                    //MessageBox.Show(bbox[0][0].ToString());

                    List<Point> punti = new List<Point>();

                    foreach (List<int> point in bbox)
                    {
                        //[103, 581]
                        Point punto = new Point(point[0], point[1]);
                        punti.Add(punto);
                        //MessageBox.Show(punto.ToString());
                    }

                    Pen penna = new Pen(Color.Green, 16);

                    g.DrawPolygon(penna, punti.ToArray());

                }

                pictureBox1.Image = bitmap; //disegna nella box
                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                
                if (detection.Text.Count == 0)
                {
                    textBox1.Text = "Impossibile leggere la targa";
                    return;
                }

                // Set the first text element
                textBox1.Text = detection.Text[0];

                // Append the rest with a comma and space
                foreach (string text in detection.Text.Skip(1))
                {
                    textBox1.Text += ", " + text;
                }

            }

        }

        private void getframe()
        {
            mjpeg = new MjpegDecoder();
            mjpeg.FrameReady += mjpeg_FrameReady;
            mjpeg.Error += mjpeg_Error;

            mjpeg.ParseStream(new Uri("http://192.168.103.50:81/stream"));
            //MessageBox.Show("Frame Catturato");
            
            
            //mjpeg.FrameReady -= mjpeg_FrameReady;
            mjpeg.StopStream();

        }

        private async Task<PlateOCR> plateOCR()
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Convert the frame (imgGlobal) to a byte array
                    if (imgGlobal == null)
                    {
                        MessageBox.Show("No image captured.");
                        return null;
                    }

                    using (var ms = new MemoryStream())
                    {
                        imgGlobal.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                        byte[] imageBytes = ms.ToArray();

                        // Prepare the image as FormData
                        var form = new MultipartFormDataContent();
                        var imageContent = new ByteArrayContent(imageBytes);
                        imageContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
                        form.Add(imageContent, "image", "image.jpg");

                        // Send POST request to Flask API with the image
                        HttpResponseMessage response = await client.PostAsync(plateOCR_url, form);

                        // Handle the response
                        if (response.IsSuccessStatusCode)
                        {
                            string resultString = await response.Content.ReadAsStringAsync();

                            Console.WriteLine(resultString);

                            // Parse the JSON result into plateOCR class
                            var results = JsonConvert.DeserializeObject<PlateOCR>(resultString);

                            return results;
                        }
                        else
                        {
                            MessageBox.Show($"Error from Flask API: {response.StatusCode}, {await response.Content.ReadAsStringAsync()}");
                            return null;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
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
            System.Drawing.Font drawFont = new System.Drawing.Font("Arial", 36);
            System.Drawing.SolidBrush drawBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Red);

            float x = 585.0f;
            float y = 0.0f;
            gr.FillRectangle(new System.Drawing.SolidBrush(System.Drawing.Color.Green), new System.Drawing.Rectangle(545, 18, 25, 25));

            gr.DrawString(drawString, drawFont, drawBrush, x, y);

            gr.DrawString(DateTime.Now.ToLocalTime().ToString(), drawFont, drawBrush, 0, 0);

            imgGlobal = newImg;
            

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
