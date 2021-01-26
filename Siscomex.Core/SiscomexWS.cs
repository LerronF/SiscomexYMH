using System;
using System.Collections.Generic;
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
            timer1.Interval = 100000; //a cada 10 segundos
            timer1.Elapsed += new System.Timers.ElapsedEventHandler(timer1_Tick);
            timer1.Enabled = true;
            Console.WriteLine("Meu Serviço do Windows foi inicado");
        }

        private void timer1_Tick(object sender, ElapsedEventArgs e)
        {
            Consumer.DownloadFile();

            var lib = new ConsultaPLI.Core.Lib.Work();
            _ = lib.ExecutarAsync();

            Publisher.UploadFile();

            Console.WriteLine("Meu Serviço do Windows sendo executado a cada 10 segundos");
        }

        public void Stop()
        {
            //Coloque aqui o código que executa quando o Serviço do Windows Parar
            Console.WriteLine("Meu Serviço do Windows Encerrou");
        }
    }
}
