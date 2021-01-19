using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Xceed.Document.NET;

namespace Siscomex
{
    public class Consumer
    {
        public static void DownloadFile()
        {
            var horaData = DateTime.Now.ToString().Replace("/", "").Replace(":", "").Replace(" ", "");
            var aux = CapturaArquivo();

            if (!System.IO.Directory.Exists(@"C:\iTriad\yamaha\Download\"))
            {
                System.IO.Directory.CreateDirectory(@"C:\iTriad\yamaha\Download\");
            }

            string arquivoPath = Path.Combine(@"C:\iTriad\yamaha\Download\", horaData + "-PLI.xml");

            using (StreamWriter sw = File.CreateText(arquivoPath))
            {
                sw.WriteLine(aux);
            }
        }

        public static string CapturaArquivo()
        {
            string hostName = "172.31.16.240";
            int port = 5672;
            string userName = "rabbitmq";
            string password = "rabbitmq";
            string queueName = "pli-siscomex-consulta";

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
                data = channel.BasicGet(queueName, true);
            }

            return data != null ? System.Text.Encoding.UTF8.GetString(data.Body) : null;
        }
    }
}
