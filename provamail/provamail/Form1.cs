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
using MailKit.Net.Smtp;
using MimeKit;
using Google.Apis.Auth;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Util.Store;
using MailKit.Security;
using System.Threading;
using MailKit.Net.Imap;
using System.Net.Mail;
using System.Net.Http;
using SocketIOClient;
using System.Net.Sockets;
using Newtonsoft.Json;



namespace provamail
{
    public partial class Form1 : Form
    {
        private SocketIOClient.SocketIO socket;
        public Form1()
        {
            InitializeComponent();
            InitializeSocket();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //MandaMail();
            //InitializeSocket();
        }

        private async void InitializeSocket()
        {
            socket = new SocketIOClient.SocketIO("http://localhost:5059/", new SocketIOOptions
            {
                Transport = SocketIOClient.Transport.TransportProtocol.WebSocket,
                Reconnection = true,
            });

            socket.OnConnected += async (sender, e) =>
            {
                Console.WriteLine("Connected to the server!");

                // Send a message to the server
                await socket.EmitAsync("message", "Hello, Python Server!");
            };

            socket.On("message", response =>
            {
                string message = response.GetValue<string>();
                Console.WriteLine($"Message from server: {message}");
                //MessageBox.Show($"Message from server: {message}");
            });

            socket.On("detected_plate", response =>
            {
                string message = response.ToString();
                Console.WriteLine($"Message from server: {message}");
                returnAsPlate(message);
                //MessageBox.Show($"Message from server: {message}");
            });

            /*socket.OnAny((eventName, response) =>
            {
                Console.WriteLine($"[ANY] Received event '{eventName}' with data: {response}");
            });*/

            socket.OnDisconnected += (sender, reason) =>
            {
                Console.WriteLine($"Disconnected: {reason}");
                MessageBox.Show($"Disconnected from server: {reason}");
            };

            socket.OnError += (sender, e) =>
            {
                Console.WriteLine("Error occurred: " + e);
                MessageBox.Show("Socket error: " + e);
            };


            await socket.ConnectAsync();
        }

        private void returnAsPlate(string data)
        {
            dynamic json = JsonConvert.DeserializeObject<dynamic>(data);
            Console.WriteLine(json[0]["plate_number"]);
        }

        async private void MandaMail()
        {
            string mailMittente = "ravasio.matilde.studente@itispaleocapa.it";

            ClientSecrets clientSecrets = new ClientSecrets
            {
                ClientId = "930357171326-qogghiulf0tv31a5ae6kmmukgjjrujlh.apps.googleusercontent.com",
                ClientSecret = "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",
            };

            var codeFlow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
            {
                // Cache tokens in ~/.local/share/google-filedatastore/CredentialCacheFolder on Linux/Mac
                DataStore = new FileDataStore("CredentialCacheFolder", false),
                Scopes = new[] { "https://mail.google.com/" },
                ClientSecrets = clientSecrets,
                LoginHint = mailMittente,
            });

            var codeReceiver = new LocalServerCodeReceiver();
            var authCode = new AuthorizationCodeInstalledApp(codeFlow, codeReceiver);

            var credential = await authCode.AuthorizeAsync(mailMittente, CancellationToken.None);

            if (credential.Token.IsStale)
                await credential.RefreshTokenAsync(CancellationToken.None);

            var oauth2 = new SaslMechanismOAuthBearer(credential.UserId, credential.Token.AccessToken);


            string mailDestinatario = "ballushi.erion.studente@itispaleocapa.it";

            MailboxAddress mittente = new MailboxAddress("Mittente", mailMittente); // maiolbox del mittente
            MailboxAddress destinatario = new MailboxAddress("Destinatario", mailDestinatario);//mailbox destinatario


            MimeMessage messaggio = new MimeMessage();
            messaggio.From.Add(mittente); //aggiungo la mail del mittente
            messaggio.To.Add(destinatario);//aggiungo la mail del destinatario
            messaggio.Subject = "Prova";

            TextPart html = new TextPart("html") 
            {
                Text= @"
                    <p>Ciao Erion<p>
                    <img src='cid:cattura' alt='foto cattura targa'>
                    "

            };

            string immagineBase64 = "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";
            byte[] imageBytes = Convert.FromBase64String(immagineBase64);

            MimePart immagine = new MimePart("image", "png")
            {
                Content= new MimeContent(new MemoryStream(imageBytes)),
                ContentDisposition= new ContentDisposition(ContentDisposition.Inline),
                ContentTransferEncoding= ContentEncoding.Base64,
                ContentId="cattura",
            };

            var immagineAllegato = new MimePart("image", "png")
            {
                Content = new MimeContent(new MemoryStream(imageBytes)),
                ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                ContentTransferEncoding = ContentEncoding.Base64,
                FileName = "cattura.png"
            };

            MultipartRelated body = new MultipartRelated();
            body.Add(html);
            body.Add(immagine);

            var multipartMixed = new Multipart("mixed");
            multipartMixed.Add(body);
            multipartMixed.Add(immagineAllegato);

            messaggio.Body = multipartMixed;

            MailKit.Net.Smtp.SmtpClient client = new MailKit.Net.Smtp.SmtpClient();
            client.Connect("smtp.gmail.com", 465, SecureSocketOptions.SslOnConnect);
            client.Authenticate(oauth2);
            client.Send(messaggio);
            client.Disconnect(true);
        }
    }
}
