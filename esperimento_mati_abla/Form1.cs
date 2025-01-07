using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Newtonsoft.Json;


namespace esperimento_mati_abla
{

    public partial class Form1 : Form

    {
        

        public class Info
        {
            public string targa { get; set; }
            public string mail { get; set; }
            public string zona_accesso { get; set; }
        }

        public List<Info> info=new List<Info>();


        public string targa = "aa11aa";
        public Form1()
        {
            InitializeComponent();

        }

        private async void button1_Click(object sender, EventArgs e)
        {
            // Esegui una richiesta GET o POST
            string url = "http://127.0.0.1:9000/" + targa;  // Modifica con l'URL desiderato
            string result = await MakeHttpRequest(url);
            label1.Text = result;
        }

        
        private async Task<string> MakeHttpRequest(string url)
        {
            // Crea un'istanza di HttpClient
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    
                    // Inviamo una richiesta GET (modifica a POST se necessario)
                    HttpResponseMessage response = await client.GetAsync(url);  // Usa PostAsync per le richieste POST
                    
                    var resultString= await response.Content.ReadAsStringAsync();
                    
                    response.EnsureSuccessStatusCode();  // Lancia un'eccezione se il codice di stato non è OK
                    var professore = JsonConvert.DeserializeObject<Info>(resultString);
                    // Ottieni la risposta come stringa
                    string responseBody = await response.Content.ReadAsStringAsync();

                    return responseBody;
                }
                catch (Exception ex)
                {
                    // Gestione degli errori, ad esempio se il server non è raggiungibile
                    return $"Errore: {ex.Message}";
                }
            }
        }

        


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
