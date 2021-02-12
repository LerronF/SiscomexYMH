using Siscomex.Core.Shared;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Siscomex.Core
{
    public class Consumer
    {
        public static bool DownloadFile()
        {
            try
            {
                var horaData = DateTime.Now.ToString().Replace("/", "").Replace(":", "").Replace(" ", "");
                string aux = null;
                try{
                    aux = CapturaArquivo();
                }catch(Exception ex){
                    LogController.RegistrarLog("Erro inesperado!" + ex.ToString());
                }
                
                if (aux != "")
                {
                    string caminhoDownload = Environment.GetEnvironmentVariable("DOWNLOAD");

                    if (!System.IO.Directory.Exists(caminhoDownload))
                    {
                        System.IO.Directory.CreateDirectory(caminhoDownload);
                    }

                    string arquivoPath = Path.Combine(caminhoDownload, horaData + "-PLI.xml");

                    using (StreamWriter sw = File.CreateText(arquivoPath))
                    {
                        sw.WriteLine(aux);
                    }

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro no download: " + ex.Message.Trim());
                return false;
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
            LogController.RegistrarLog("Conectando ao rabbit ");
            //Cria a conexão
            IConnection connection = factory.CreateConnection();
            
            if(connection == null || !connection.IsOpen){
                LogController.RegistrarLog("Não foi estabelecida a conexão!");
            }else{
                LogController.RegistrarLog("Conexão realizada com sucesso!");
            }
            BasicGetResult data;
            using (var channel = connection.CreateModel())
            {
                data = channel.BasicGet(queueName, true);
                if(data == null){
                    LogController.RegistrarLog("Não há dados na fila");
                }else{
                    LogController.RegistrarLog("Há dados na fila");
                }
            }

            return data != null ? System.Text.Encoding.UTF8.GetString(data.Body) : "";
        }
    }
}