using System;
using System.Threading.Tasks;

namespace Siscomex.ConsultaPLI.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            Consumer.DownloadFile();

            Publisher.UploadFile();           

            //var lib = new Siscomex.ConsultaPLI.Lib.Work();
            //_ = lib.ExecutarAsync();
        }
    }
}
