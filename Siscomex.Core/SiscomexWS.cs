using Siscomex.Core.Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Timers;

namespace Siscomex.Core
{
    public class SiscomexWS
    {
        public void Start()
        {
            //Coloque aqui o código que executa quando o Serviço do Windows Iniciar
            var timer1 = new Timer();
            timer1.Interval = 30000; //a cada 30 segundos
            timer1.Elapsed += new System.Timers.ElapsedEventHandler(timer1_Tick);
            timer1.Enabled = true;
            Console.WriteLine("Meu Serviço do Windows foi inicado");
        }

        private void timer1_Tick(object sender, ElapsedEventArgs e)
        {
            try
            {
                //string path = Directory.GetCurrentDirectory() + @"\Certificado\yamaha.pfx"; //string.Join(@"\",@"C:\Users\matheus.pinheiro\Downloads\certificados-digitais\certificados-digitais", "yamaha.pfx");
                string path = Environment.GetEnvironmentVariable("CERT");
                Console.WriteLine("CERTIFICADO: " + path);

                //VERIFICA CERTIFICADO
                var certificado = ControleCertificados.GetClientCertificate();

                if (certificado == null)
                {
                    GSoft.CertificateTool.Program.InstallPfxCertificate(path, "yamaha2020", StoreName.My, StoreLocation.CurrentUser);
                }

                //VERIFICA FILA RABBIT
                bool download = Consumer.DownloadFile();

                if (download)
                {
                    var lib = new ConsultaPLI.Core.Lib.Work();
                    _ = lib.ExecutarAsync();

                    Publisher.UploadFile();
                }

                Console.WriteLine("Meu Serviço do Windows sendo executado a cada 10 segundos");
            }
            catch (Exception ex)
            {
                Console.WriteLine("erro no serviço - " + ex.ToString());
            }
        }

        public void Stop()
        {
            //Coloque aqui o código que executa quando o Serviço do Windows Parar
            Console.WriteLine("Meu Serviço do Windows Encerrou");
        }
    }
}
