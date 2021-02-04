using Siscomex.Core;
using System;
using System.IO;
using GSoft.CertificateTool;
using System.Security.Cryptography.X509Certificates;

namespace ConsultaPLI.Core.Console
{
    class Program
    {
        static void Main()
        {
            string path = Directory.GetCurrentDirectory() + @"\Certificado\Randerson A1 2020-2021.pfx";

            //string[] args = new string[1] { path };

            GSoft.CertificateTool.Program.InstallPfxCertificate(path, "yamaha2020", StoreName.My, StoreLocation.LocalMachine);

            Consumer.DownloadFile();

            var lib = new Lib.Work();
            _ = lib.ExecutarAsync();

            Publisher.UploadFile();
        }
    }
}
