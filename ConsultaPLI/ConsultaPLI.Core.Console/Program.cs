using Siscomex.Core;
using System;

namespace ConsultaPLI.Core.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            Consumer.DownloadFile();

            var lib = new Lib.Work();
            _ = lib.ExecutarAsync();

            Publisher.UploadFile();
        }
    }
}
