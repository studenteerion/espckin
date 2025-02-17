using System;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;
//using EnvReaderApp;
using System.IO;
using System.Web;

namespace DatabaseCrud
{
    public partial class Form1 : Form
    {
        private static readonly HttpClient client = new HttpClient();
        class EnvReader
        {
            public static void Load(string filePath)
            {
                if (!File.Exists(filePath))
                    throw new FileNotFoundException($"The file '{filePath}' does not exist.");

                foreach (var line in File.ReadAllLines(filePath))
                {
                    if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#"))
                        continue; // Skip empty lines and comments

                    var parts = line.Split('=');
                    if (parts.Length != 2)
                        continue; // Skip lines that are not key-value pairs

                    var key = parts[0].Trim();
                    var value = parts[1].Trim();
                    Environment.SetEnvironmentVariable(key, value);
                }
            }
        }

        public Form1()
        {
            EnvReader.Load(".env");
            InitializeComponent();
            CustomizeDataGridView();
            dataGridView1.AutoGenerateColumns = true;

        }


        private async Task LoadDataFromApi(string url)
        {

            try
            {
                // Invia una richiesta HTTP GET all'API
                HttpResponseMessage response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    // Leggi la risposta come stringa
                    string content = await response.Content.ReadAsStringAsync();

                    // Deserializza il JSON ricevuto in una lista di oggetti ApiData
                    var apiResponse = JsonConvert.DeserializeObject<List<ApiData>>(content);
                    // Popola il DataGridView con i dati ricevuti
                    dataGridView1.DataSource = apiResponse;
                }
                else
                {
                    // Gestisci il caso in cui la risposta non sia positiva
                    MessageBox.Show($"Errore nella richiesta: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                // Gestisci eventuali eccezioni
                MessageBox.Show($"Errore: {ex.Message}");
            }
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var row = dataGridView1.Rows[e.RowIndex];
                // Popola i TextBox con i dati della riga selezionata
                textBoxnome.Text = row.Cells["Nome"].Value.ToString();
                textBoxcognome.Text = row.Cells["Cognome"].Value.ToString();
                textBoxmail.Text = row.Cells["Mail"].Value.ToString();
                textBoxtarga.Text = row.Cells["Targa"].Value.ToString();
                textBoxaccesso.Text = row.Cells["zona_accesso"].Value.ToString();
                textBoxid.Text = row.Cells["id_professore"].Value.ToString();
            }
        }


        private async void button1_Click_1(object sender, EventArgs e)
        {
            string apiIp = Environment.GetEnvironmentVariable("API_IP");
            string apiPort = Environment.GetEnvironmentVariable("API_PORT");
            string apiKey = Environment.GetEnvironmentVariable("API_KEY");

            //string apiIp = $"{apiIp1}.{apiIp2}.{apiIp3}.{apiIp4}";

            string apiSocket = apiIp + ':' + apiPort;

            string apiUrl = "http://" + apiSocket + "/all?key=" + apiKey;  // Indirizzo dell'API

            // Carica i dati dall'API e li popola nel DataGridView
            await LoadDataFromApi(apiUrl);
        }
        private async Task UpdateDataOnApi(ApiData updatedData)
        {
            string apiIp = Environment.GetEnvironmentVariable("API_IP");
            string apiPort = Environment.GetEnvironmentVariable("API_PORT");
            string apiKey = Environment.GetEnvironmentVariable("API_KEY");
            string apiSocket = apiIp + ':' + apiPort;

            string apiUrl = "http://" + apiSocket + "/update"; // Endpoint di aggiornamento

            var content = new StringContent(JsonConvert.SerializeObject(updatedData), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PutAsync(apiUrl, content);

            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show("Dati aggiornati con successo!");
                // Ricarica i dati nel DataGridView
                await LoadDataFromApi(apiUrl);
            }
            else
            {
                MessageBox.Show($"Errore nell'aggiornamento: {response.StatusCode}");
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        private void CustomizeDataGridView()
        {
            // Impostazioni generali del DataGridView
            dataGridView1.BackgroundColor = Color.White; // Colore di sfondo del DataGridView
            dataGridView1.GridColor = Color.LightGray;   // Colore delle linee della griglia

            // Impostazioni per la selezione delle righe
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.DefaultCellStyle.SelectionBackColor = Color.CornflowerBlue;  // Colore di sfondo della riga selezionata
            dataGridView1.DefaultCellStyle.SelectionForeColor = Color.White;  // Colore del testo della riga selezionata

            // Impostazioni delle celle
            dataGridView1.DefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240); // Colore di sfondo delle celle
            dataGridView1.DefaultCellStyle.ForeColor = Color.Black;  // Colore del testo nelle celle
            dataGridView1.DefaultCellStyle.Font = new Font("Segoe UI", 10);  // Impostazione del font

            // Impostazioni per le intestazioni
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.DodgerBlue;  // Colore di sfondo delle intestazioni
            dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;  // Colore del testo delle intestazioni
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 12, FontStyle.Bold);  // Impostazione del font delle intestazioni

            // Impostazioni per le righe alterne
            dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;  // Colore di sfondo per le righe alternate
        }

        private void button2_Click(object sender, EventArgs e)
        {
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            // Ottieni i dati modificati dai TextBox
            ApiData updatedData = new ApiData
            {
                Nome = textBoxnome.Text,
                Cognome = textBoxcognome.Text,
                Mail = textBoxmail.Text,
                Targa = textBoxtarga.Text,
                zona_accesso = textBoxaccesso.Text,
                id_professore = textBoxid.Text, 
            };

            string apiIp = Environment.GetEnvironmentVariable("API_IP");
            string apiPort = Environment.GetEnvironmentVariable("API_PORT");
            string apiKey = Environment.GetEnvironmentVariable("API_KEY");
            string apiSocket = apiIp + ':' + apiPort;
            // Aggiorna i dati sul server
            await LoadDataFromApi("http://" + apiSocket + "/all?key=" + apiKey);  // Usa l'endpoint per recuperare tutti i dati

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }

    public class ApiData
    {
        public string Cognome { get; set; }
        public string id_professore { get; set; }
        public string Mail { get; set; }
        public string Nome { get; set; }
        public string Targa { get; set; }
        public string zona_accesso { get; set; }
    }
}







