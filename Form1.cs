using System;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Security.Authentication;
using System.IO;
using System.Text;
using System.Net.Security;

namespace ClientApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string messagetosend = textBox2.Text;
            connectandsendmessage(messagetosend);
        }

        private void connectandsendmessage(string messagetosend)
        {
            try
            {
                var client = new TcpClient("localhost", 12345);
                var sslStream = new SslStream(client.GetStream(), false, new RemoteCertificateValidationCallback(ValidateServerCertificate), null);
                sslStream.AuthenticateAsClient("localhost");

                using (var writer = new StreamWriter(sslStream, Encoding.UTF8, 1024, leaveOpen: true))
                {
                    writer.WriteLine(messagetosend);
                    writer.Flush();
                }
                                
                using (var reader = new StreamReader(sslStream, Encoding.UTF8))
                {
                    string serverResponse = reader.ReadLine();
                    textBox1.AppendText(serverResponse);
                }

                sslStream.Close();
                client.Close();
            }
            catch (AuthenticationException authEx)
            {
                textBox3.AppendText("Błąd autoryzacji: " + authEx.Message + "\n");
            }
            catch (Exception ex)
            {
                textBox3.AppendText("Błąd: " + ex.Message + "\n");
            }
        }

        public bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            /*if (sslPolicyErrors == SslPolicyErrors.None)
            {
                return true;
            }

            textBox3.AppendText("Błąd certyfikatu: " + sslPolicyErrors);*/
            return true;
        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
