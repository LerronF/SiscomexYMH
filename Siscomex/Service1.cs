using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace Siscomex
{
    public partial class Service1 : ServiceBase
    {
        private Timer _timer;

        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            _timer = new Timer(CriarLog, null, 0, 120000);
        }

        protected override void OnStop()
        {
        }

        private void CriarLog(object state)
        {
            var lib = new Siscomex.ConsultaPLI.Lib.Work();
            lib.ExecutarAsync();

            StreamWriter vWriter = new StreamWriter(@"C:\Logs\log.txt", true);
            vWriter.WriteLine("serviço rodando:" + DateTime.Now);
            vWriter.Flush();
            vWriter.Close();
        }
    }
}
