using System;
using System.Threading.Tasks;

namespace Siscomex.ConsultaPLI.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            Consumer.DownloadFile();

            var lib = new Siscomex.ConsultaPLI.Lib.Work();
            _ = lib.ExecutarAsync();

            Publisher.UploadFile();
        }
    }
}
