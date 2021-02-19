using Siscomex.Core.Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
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
                const string VAR_CERT = "CERT";
                LogController.RegistrarLog("Iniciar Processo!");

                string path = Environment.GetEnvironmentVariable(VAR_CERT);
                if (path != null)
                {
                    LogController.RegistrarLog("Certificado Carregado: " + path);
                }
                else
                {
                    LogController.RegistrarLog("Certificado Não foi carregado! Verifique a variavel de ambiente '" + VAR_CERT + "'");
                }

                //VERIFICA CERTIFICADO
                X509Certificate certificado = null;
                try
                {
                    certificado = ControleCertificados.GetClientCertificate();
                }
                catch (System.Security.Cryptography.CryptographicException ex)
                {
                    LogController.RegistrarLog("Certificado ainda não existe!");
                }
                catch (Exception ex)
                {
                    LogController.RegistrarLog("Erro inesperado!" + ex.ToString());
                }
                if (certificado == null)
                {
                    Console.WriteLine("Certificado instalando... " +  path);
                    GSoft.CertificateTool.Program.InstallPfxCertificate(path, "yamaha2020", StoreName.My, StoreLocation.CurrentUser);
                }
                else
                {
                    LogController.RegistrarLog("Certificado já está instalado!");
                }

                certificado = ControleCertificados.GetClientCertificate();
                Console.WriteLine("Certificado instalado! " + certificado);
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

        public static void ImprimirTodosOsCertificados()
        {
            Console.WriteLine("\r\nExists Certs Name and Location");
            Console.WriteLine("------ ----- -------------------------");

            foreach (StoreLocation storeLocation in (StoreLocation[])
                Enum.GetValues(typeof(StoreLocation)))
            {
                foreach (StoreName storeName in (StoreName[])
                    Enum.GetValues(typeof(StoreName)))
                {
                    X509Store store = new X509Store(storeName, storeLocation);

                    try
                    {
                        store.Open(OpenFlags.OpenExistingOnly);

                        Console.WriteLine("Yes    {0,4}  {1}, {2}",
                            store.Certificates.Count, store.Name, store.Location);
                    }
                    catch (CryptographicException ex)
                    {
                        Console.WriteLine("No           {0}, {1}",
                            store.Name, store.Location);
                    }
                }
                Console.WriteLine();
            }
        }

        public void Stop()
        {
            //Coloque aqui o código que executa quando o Serviço do Windows Parar
            Console.WriteLine("Meu Serviço do Windows Encerrou");
        }
    }
}