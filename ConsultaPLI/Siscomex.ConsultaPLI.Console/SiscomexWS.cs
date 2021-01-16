using System;
using System.Timers;

namespace Siscomex.ConsultaPLI.Console
{
    public class SiscomexWS
    {
        public void Start()
        {
            var lib = new Siscomex.ConsultaPLI.Lib.Work();
            lib.ExecutarAsync();

            //Coloque aqui o código que executa quando o Serviço do Windows Iniciar
            var timer1 = new Timer();
            timer1.Interval = 10000; //a cada 10 segundos
            timer1.Elapsed += new System.Timers.ElapsedEventHandler(timer1_Tick);
            timer1.Enabled = true;
            //LogController.RegistrarLog("Meu Serviço do Windows foi inicado");
        }

        private void timer1_Tick(object sender, ElapsedEventArgs e)
        {
            //Console.WriteLine("Meu Serviço do Windows sendo executado a cada 10 segundos");
        }

        public void Stop()
        {
            //Coloque aqui o código que executa quando o Serviço do Windows Parar
           // LogController.RegistrarLog("Meu Serviço do Windows Encerrou");
        }
    }
}
