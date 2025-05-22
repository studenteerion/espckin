using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Collections.Generic;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Xml.Linq;
using System.Drawing;

namespace Cameradb
{
    public partial class Form1 : Form
    {
        // URL di base dell'API
        private const string BASE_URL = "http://localhost:5050/api";

        public Form1()
        {
            InitializeComponent();
            StyleDataGridView(); 
            InitializeDataGridView();
            dataGridView1.CellClick += dataGridView1_CellClick;
            GetAllCameras();
        }
            
            
        private void InitializeDataGridView()
        {
            dataGridView1.Columns.Add("Ip", "IP");
            dataGridView1.Columns.Add("Name", "Nome");
            dataGridView1.Columns.Add("Description", "Descrizione");
            dataGridView1.Columns.Add("Coordinates", "Coordinate"); 
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Verifica che la selezione sia su una riga valida
            {
                var row = dataGridView1.Rows[e.RowIndex];

                // Aggiungi un messaggio di debug
                //MessageBox.Show($"Riga selezionata: Ip={row.Cells["Ip"].Value}, Nome={row.Cells["Name"].Value}");

                ip_txt.Text = row.Cells["Ip"]?.Value?.ToString() ?? "";
                nome_txt.Text = row.Cells["Name"]?.Value?.ToString() ?? "";
                descrizione_txt.Text = row.Cells["Description"]?.Value?.ToString() ?? "";
                coordinate_txt.Text = row.Cells["Coordinates"]?.Value?.ToString() ?? "";
            }
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
                ip = ip_txt.Text,           // Notare che l'API si aspetta "ip" in minuscolo
                name = nome_txt.Text,       // "name" in minuscolo
                description = descrizione_txt.Text,  // "description" in minuscolo
                coordinates = coordinate_txt.Text    // "coordinates" in minuscolo
            };

            // Serializza l'oggetto in formato JSON
            string jsonString = JsonConvert.SerializeObject(cameraData);

            // Mostra il JSON in una MessageBox (opzionale, per il debug)
            MessageBox.Show("JSON da inviare: " + jsonString, "Contenuto JSON", MessageBoxButtons.OK, MessageBoxIcon.Information);

            var jsonContent = new StringContent(
                jsonString,             // Invia il JSON correttamente formattato
                Encoding.UTF8,
                "application/json"      // Assicurati che l'intestazione sia impostata su "application/json"
            );

            // Inizializza HttpClient per inviare la richiesta POST
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Modifica l'URL in base all'endpoint della tua API (assicurati che il URL sia corretto)
                    HttpResponseMessage response = await client.PostAsync($"{BASE_URL}/add_camera", jsonContent);

                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Fotocamera aggiunta con successo.", "Successo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Pulisce i campi di input dopo l'aggiunta

                        // Ora aggiorna il DataGridView per mostrare la fotocamera appena aggiunta
                        // Creiamo un oggetto con i dati della fotocamera appena aggiunta
                        var addedCamera = new
                        {
                            ip = ip_txt.Text,
                            name = nome_txt.Text,
                            description = descrizione_txt.Text,
                            coordinates = coordinate_txt.Text
                        };

                        // Aggiungi la fotocamera alla griglia (adesso con il nome corretto del DataGridView)
                        dataGridView1.Rows.Add(addedCamera.ip, addedCamera.name, addedCamera.description, addedCamera.coordinates);
                        ip_txt.Clear();
                        nome_txt.Clear();
                        descrizione_txt.Clear();
                        coordinate_txt.Clear();
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

            // Dopo aver aggiunto la fotocamera, aggiorna la lista delle fotocamere (assumiamo che tu abbia una funzione GetAllCameras per aggiornare il DataGridView)
            await GetAllCameras();
        }

        // Funzione per ottenere tutte le fotocamere
        public async Task GetAllCameras()
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync($"{BASE_URL}/get_all_cameras");

                    if (response.IsSuccessStatusCode)
                    {
                        string responseData = await response.Content.ReadAsStringAsync();
                        var cameras = JsonConvert.DeserializeObject<List<Camera>>(responseData);

                        // Pulisce il DataGridView
                        dataGridView1.Rows.Clear();
                        dataGridView1.Columns.Clear();

                        // Imposta le colonne del DataGridView
                        dataGridView1.Columns.Add("Id", "Id");
                        dataGridView1.Columns.Add("Ip", "IP");
                        dataGridView1.Columns.Add("Name", "Nome");
                        dataGridView1.Columns.Add("Description", "Descrizione");
                        dataGridView1.Columns.Add("Coordinates", "Coordinate");

                        // Aggiungi le fotocamere al DataGridView
                        foreach (var camera in cameras)
                        {
                            dataGridView1.Rows.Add(camera.Id, camera.Ip, camera.Name, camera.Description, camera.Coordinates);
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
        private async Task UpdateCameraAsync()
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleziona una fotocamera da aggiornare.", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
            int cameraId = Convert.ToInt32(selectedRow.Cells["Id"].Value);

            var cameraData = new
            {
                id = cameraId,  // L'ID della fotocamera è necessario per l'API
                ip = ip_txt.Text,
                name = nome_txt.Text,
                description = descrizione_txt.Text,
                coordinates = coordinate_txt.Text
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
                        //MessageBox.Show("Fotocamera aggiornata con successo.", "Successo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        await GetAllCameras();  // Ricarica tutte le fotocamere nel DataGridView
                    }
                    else
                    {
                        string errorResponse = await response.Content.ReadAsStringAsync();
                        MessageBox.Show("Errore nell'aggiornare la fotocamera: " + errorResponse, "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Errore nella richiesta: {ex.Message}", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

            // Funzione per aggiornare una fotocamera
            public async Task UpdateCamera(int cameraId, string ip, string name, string description, string coordinates)
            {
            var cameraData = new
            {
                id = cameraId,  // Usa "id" in minuscolo come richiesto dalla tua API
                ip = ip,
                name = name,
                description = description,
                coordinates = coordinates
            };

            string jsonString = JsonConvert.SerializeObject(cameraData);
            MessageBox.Show("JSON inviato: " + jsonString, "Debug", MessageBoxButtons.OK, MessageBoxIcon.Information);

            var jsonContent = new StringContent(
                jsonString,
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
                        string errorResponse = await response.Content.ReadAsStringAsync();
                        MessageBox.Show("Errore nell'aggiornare la fotocamera: " + response.StatusCode + "\n" + errorResponse, "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Errore nella richiesta: {ex.Message}", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
         }

        // Funzione per eliminare una fotocamera
        public async Task DeleteCamera(int cameraId)
        {
            var cameraData = new { id = cameraId };

            var jsonContent = new StringContent(
                JsonConvert.SerializeObject(cameraData),
                Encoding.UTF8,
                "application/json"
            );

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var request = new HttpRequestMessage(HttpMethod.Delete, $"{BASE_URL}/delete_camera")
                    {
                        Content = jsonContent
                    };

                    HttpResponseMessage response = await client.SendAsync(request);

                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Fotocamera eliminata con successo.", "Successo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        await GetAllCameras();
                    }
                    else
                    {
                        string errorResponse = await response.Content.ReadAsStringAsync();
                        MessageBox.Show("Errore nell'eliminare la fotocamera: " + errorResponse, "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            var updatedCamera = new Camera
            {

                Ip = "192.168.1.2",
                Name = "Updated Test Camera",
                Description = "Updated Description",
                Coordinates = "45.4642,9.1900"
            };


            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleziona una fotocamera per aggiornare.", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];

            int cameraa = Convert.ToInt32(selectedRow.Cells["Id"].Value);
            string ip = ip_txt.Text;
            string name = nome_txt.Text;
            string description = descrizione_txt.Text;
            string coordinates = coordinate_txt.Text;

            await UpdateCamera(cameraa, ip, name, description, coordinates);

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

        private void StyleDataGridView()
        {
            // Colori generali
            dataGridView1.BackgroundColor = System.Drawing.Color.White;
            dataGridView1.BorderStyle = BorderStyle.None;
            dataGridView1.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dataGridView1.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;

            // Header style
            dataGridView1.EnableHeadersVisualStyles = false;
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(100, 149, 237); // CornflowerBlue
            dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);

            // Row style
            dataGridView1.DefaultCellStyle.BackColor = System.Drawing.Color.White;
            dataGridView1.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            dataGridView1.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(30, 144, 255); // DodgerBlue
            dataGridView1.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;
            dataGridView1.DefaultCellStyle.Font = new Font("Segoe UI", 9);

            // Alternanza colori righe
            dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(240, 248, 255); // AliceBlue

            // Altre proprietà estetiche
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleziona una fotocamera dalla tabella per salvarne le modifiche.", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];

            // Estrai i valori attuali dalla riga selezionata
            int cameraId = Convert.ToInt32(selectedRow.Cells["Id"].Value);
            string ip = ip_txt.Text;
            string name = nome_txt.Text;
            string description = descrizione_txt.Text;
            string coordinates = coordinate_txt.Text;

            // Aggiorna la fotocamera nel database
            await UpdateCamera(cameraId, ip, name, description, coordinates);

            // Aggiorna la riga del DataGridView con i nuovi valori
            selectedRow.Cells["Ip"].Value = ip;
            selectedRow.Cells["Name"].Value = name;
            selectedRow.Cells["Description"].Value = description;
            selectedRow.Cells["Coordinates"].Value = coordinates;

            ip_txt.Text = "";
            nome_txt.Text = "";
            descrizione_txt.Text = "";
            coordinate_txt.Text = "";

            //MessageBox.Show("Fotocamera aggiornata con successo.", "Successo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ip_txt_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private async void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleziona una fotocamera dalla tabella per eliminarla.", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Ottieni la riga selezionata nel DataGridView
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
                int cameraId = Convert.ToInt32(selectedRow.Cells["Id"].Value);

                // Conferma l'eliminazione
                var result = MessageBox.Show($"Sei sicuro di voler eliminare la fotocamera con ID {cameraId}?", "Conferma Eliminazione", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.No)
                {
                    return;
                }

                // Elimina la fotocamera dal backend tramite l'API
                await DeleteCamera(cameraId);

                // Rimuovi l'elemento dal DataGridView (UI)
                dataGridView1.Rows.Remove(selectedRow);

                // Svuota i campi di testo se non ci sono più righe
                if (dataGridView1.Rows.Count == 0)
                {
                    ip_txt.Clear();
                    nome_txt.Clear();
                    descrizione_txt.Clear();
                    coordinate_txt.Clear();
                }

                MessageBox.Show("Fotocamera eliminata con successo.", "Successo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Errore nell'eliminare la fotocamera: " + ex.Message, "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private async void save_btn_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleziona una fotocamera dalla tabella per salvarne le modifiche.", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Ottieni la riga selezionata
            DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];

            // Estrai i valori correnti dalla riga selezionata
            int cameraId = Convert.ToInt32(selectedRow.Cells["Id"].Value);
            string ip = ip_txt.Text;
            string name = nome_txt.Text;
            string description = descrizione_txt.Text;
            string coordinates = coordinate_txt.Text;

            try
            {
                // Aggiorna la fotocamera nel backend
                await UpdateCamera(cameraId, ip, name, description, coordinates);

                // Aggiorna la riga nel DataGridView
                selectedRow.Cells["Ip"].Value = ip;
                selectedRow.Cells["Name"].Value = name;
                selectedRow.Cells["Description"].Value = description;
                selectedRow.Cells["Coordinates"].Value = coordinates;

                MessageBox.Show("Fotocamera aggiornata con successo.", "Successo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Errore nell'aggiornare la fotocamera: " + ex.Message, "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }

    public class Camera
    {
        public int Id { get; set; }
        public string Ip { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Coordinates { get; set; }


    }
}
