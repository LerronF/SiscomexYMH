using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System;
using System.Text;
using System.Threading;
using System.Xml;

namespace Siscomex
{
    public class Publisher
    {
        public static void UploadFile()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(@"C:\iTriad\yamaha\prints\18012021204810-Extrato.xml");

            var texto = doc.InnerXml;

            EnviaArquivo(texto);
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
