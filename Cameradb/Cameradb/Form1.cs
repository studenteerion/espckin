using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Cameradb
{
    public partial class Form1 : Form
    {
        // URL di base dell'API
        private const string BASE_URL = "http://192.168.103.11:5000/api";

        public Form1()
        {
            InitializeComponent();
        }

        // Funzione per aggiungere una fotocamera (logica separata in un metodo asincrono)
        private async Task AggiungiCameraAsync()
        {
            // Controlla se tutti i campi sono riempiti
            if (string.IsNullOrEmpty(ip_txt.Text) || string.IsNullOrEmpty(nome_txt.Text) || string.IsNullOrEmpty(descrizione_txt.Text) || string.IsNullOrEmpty(coordinate_txt.Text))
            {
                MessageBox.Show("Tutti i campi devono essere compilati.");
                return;
            }

            // Creiamo un oggetto con i dati della fotocamera
            var cameraData = new
            {
                ip = ip_txt.Text,
                name = nome_txt.Text,
                description = descrizione_txt.Text,
                coordinates = coordinate_txt.Text
            };


            // Serializza l'oggetto in formato JSON
            string jsonString = JsonConvert.SerializeObject(cameraData);

            // Mostra il JSON in una MessageBox
            MessageBox.Show("JSON da inviare: " + jsonString, "Contenuto JSON", MessageBoxButtons.OK, MessageBoxIcon.Information);

            var jsonContent = new StringContent(
                JsonConvert.SerializeObject(cameraData),
                Encoding.UTF8,
                "application/json"
            );

            // Inizializza HttpClient per inviare la richiesta POST
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.PostAsync($"{BASE_URL}/add_camera", jsonContent);

                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Fotocamera aggiunta con successo.", "Successo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        // Leggi il corpo della risposta in caso di errore per ottenere più dettagli
                        string errorResponse = await response.Content.ReadAsStringAsync();
                        MessageBox.Show("Errore nell'aggiungere la fotocamera: " + response.StatusCode + "\n" + errorResponse, "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Errore nella richiesta: " + ex.ToString(), "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }

            }
        }

        // Funzione per ottenere tutte le fotocamere
        public async Task GetAllCameras()
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Esegui la richiesta GET per ottenere tutte le fotocamere
                    HttpResponseMessage response = await client.GetAsync($"{BASE_URL}/get_all_cameras");

                    if (response.IsSuccessStatusCode)
                    {
                        // Leggi la risposta JSON
                        string responseData = await response.Content.ReadAsStringAsync();

                        // Deserializza la risposta JSON in una lista di fotocamere
                        var cameras = JsonConvert.DeserializeObject<List<Camera>>(responseData);

                        // Pulisce la ListBox prima di aggiungere nuovi elementi
                        cameraListBox.Items.Clear();

                        // Aggiungi ogni fotocamera alla ListBox
                        foreach (var camera in cameras)
                        {
                            // Puoi decidere di visualizzare solo alcuni campi, come il nome o l'IP
                            cameraListBox.Items.Add($"Name: {camera.Name}, IP: {camera.Ip}, Coordinates: {camera.Coordinates}");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Errore nel recupero delle fotocamere: " + response.StatusCode, "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Errore nella richiesta: " + ex.Message, "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Funzione per aggiornare una fotocamera
        public async Task UpdateCamera(int cameraId)
        {
            var cameraData = new
            {
                id = cameraId,
                ip = "192.168.1.2",
                name = "Updated Camera",
                description = "Updated description for the camera",
                coordinates = "40.7306,-73.9352"
            };

            var jsonContent = new StringContent(
                JsonConvert.SerializeObject(cameraData),
                Encoding.UTF8,
                "application/json"
            );

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.PutAsync($"{BASE_URL}/update_camera", jsonContent);
                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Fotocamera aggiornata con successo.", "Successo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Errore nell'aggiornare la fotocamera: " + response.StatusCode, "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Errore nella richiesta: " + ex.Message, "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Funzione per eliminare una fotocamera
        public async Task DeleteCamera(int cameraId)
        {
            var cameraData = new
            {
                id = cameraId
            };

            var jsonContent = new StringContent(
                JsonConvert.SerializeObject(cameraData),
                Encoding.UTF8,
                "application/json"
            );

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Usa HttpRequestMessage per DELETE con body
                    var request = new HttpRequestMessage(HttpMethod.Delete, $"{BASE_URL}/delete_camera")
                    {
                        Content = jsonContent
                    };

                    HttpResponseMessage response = await client.SendAsync(request);
                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Fotocamera eliminata con successo.", "Successo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Errore nell'eliminare la fotocamera: " + response.StatusCode, "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Errore nella richiesta: " + ex.Message, "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Funzione per avviare il test
        public async Task TestRoutes()
        {
            // Aggiungi una fotocamera
            await AggiungiCameraAsync();  // Chiamato il metodo asincrono

            // Ottieni tutte le fotocamere
            await GetAllCameras();

            // Aggiorna una fotocamera
            int cameraId = 1; // Assicurati di sostituirlo con un ID valido
            await UpdateCamera(cameraId);

            // Ottieni tutte le fotocamere dopo l'aggiornamento
            await GetAllCameras();

            // Elimina una fotocamera
            await DeleteCamera(cameraId);

            // Ottieni tutte le fotocamere dopo l'eliminazione
            await GetAllCameras();
        }

        // Funzione per invocare il test
        private async void test_btn_Click(object sender, EventArgs e)
        {
            await TestRoutes(); // Avvia il test dei metodi asincroni
        }

        // Funzione per il pulsante "Aggiungi Fotocamera"
        private async void aggiungi_btn_Click(object sender, EventArgs e)
        {
            await AggiungiCameraAsync();
        }

        // Funzione per il pulsante "Mostra tutte le fotocamere"
        private async void showAll_btn_Click(object sender, EventArgs e)
        {
            await GetAllCameras();
        }

        private void cameraListBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private async void button1_Click(object sender, EventArgs e)
        {
            await GetAllCameras();
        }
    }

    public class Camera
    {
        public string Ip { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Coordinates { get; set; }
    }
}
