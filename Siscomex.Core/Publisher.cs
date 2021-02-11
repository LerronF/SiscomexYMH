using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace Siscomex.Core
{
    public class Publisher
    {
        public static void UploadFile()
        {
            try
            {
                string caminhoUpload = Environment.GetEnvironmentVariable("UPLOAD");
                string[] Arquivos = Directory.GetFiles(caminhoUpload, "*.xml");
                XmlDocument doc = new XmlDocument();

                foreach (var file in Arquivos)
                {
                    doc.Load(file);

                    var texto = doc.InnerXml;

                    bool upload = EnviaArquivo(texto);

                    if (!upload)
                    {
                        Console.WriteLine("Arquivo " + file + " não enviado !");
                    }
                    else
                    {
                        File.Delete(file);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro no upload: " + ex.Message.Trim());
            }
        }

        public static bool EnviaArquivo(string texto)
        {
            try
            {
                string hostName = "172.31.16.240";
                int port = 5672;
                string userName = "rabbitmq";
                string password = "rabbitmq";
                string queueName = "pli-siscomex-retorno";

                //Cria a conexão com o RabbitMq
                var factory = new ConnectionFactory()
                {
                    HostName = hostName,
                    UserName = userName,
                    Password = password,
                    Port = port,
                };
                //Cria a conexão
                IConnection connection = factory.CreateConnection();

                BasicGetResult data;
                using (var channel = connection.CreateModel())
                {
                    channel.BasicPublish(string.Empty, queueName, null, Encoding.ASCII.GetBytes(texto));
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}