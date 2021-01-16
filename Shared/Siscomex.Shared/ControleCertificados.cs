using OpenQA.Selenium.PhantomJS;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Siscomex.Shared
{
    public static class ControleCertificados
    {
        private static string subName1 = @"CN=RANDERSON MENDES VIEIRA:52734471272, OU=AR TREVOCHECK, OU=01146957000103, OU=AC SERASA RFB v5, OU=RFB e-CPF A1, OU=Secretaria da Receita Federal do Brasil - RFB ,OU=000001009572261, O=ICP-Brasil ,C=BR";
        //"CN=ENDERSON RUIZ DE CASTRO:67635466291, OU=Certificado PF A1, OU=16994652000129, OU=AC SOLUTI Multipla, OU=AC SOLUTI, OU=Autoridade Certificadora Raiz Brasileira v2, O=ICP-Brasil, C=BR";
        private static string subName2 = "CN=RANDERSON MENDES VIEIRA:52734471272, OU=AR TREVOCHECK, OU=01146957000103, OU=AC SERASA RFB v5, OU=RFB e-CPF A1, OU=Secretaria da Receita Federal do Brasil - RFB ,OU=000001009572261, O=ICP-Brasil ,C=BR";

        private static string Thumbprint = "a8d3d9d459e0f18169b6eb75f30df21cb4da3391";

        public static void CarregarCertificado( PhantomJSDriverService service)
        {
            service.IgnoreSslErrors = true;
            string cert = $"--ssl-client-certificate-file={Directory.GetCurrentDirectory()}\\Certificado\\randerson.cer";
            service.AddArgument(cert);
            service.AddArgument($"--ssl-client-key-file={Directory.GetCurrentDirectory()}\\Certificado\\randerson.key");
            service.AddArgument($"--ssl-client-key-passphrase=yamaha2020");
        }

        //public static X509Certificate FindClientCertificate(string serialNumber)
        //{
        //    return
        //        FindCertificate(StoreLocation.CurrentUser) ??
        //        FindCertificate(StoreLocation.LocalMachine);
        //    X509Certificate FindCertificate(StoreLocation location)
        //    {
        //        X509Store store = new X509Store(location);
        //        store.Open(OpenFlags.OpenExistingOnly);
        //        var certs = store.Certificates.Find(X509FindType.FindBySerialNumber, serialNumber, true);
        //        return certs.OfType<X509Certificate>().FirstOrDefault();
        //    };
        //}



        public static X509Certificate GetClientCertificate()
        {
            return
                FindCertificate(StoreLocation.CurrentUser) ??
                FindCertificate(StoreLocation.LocalMachine);
            X509Certificate FindCertificate(StoreLocation location)
            {
                X509Store store = new X509Store(location);
                store.Open(OpenFlags.OpenExistingOnly);
                X509Certificate2Collection certs = store.Certificates.Find(X509FindType.FindByThumbprint, Thumbprint, true);

                if (certs == null || certs.Count == 0)
                    certs = store.Certificates.Find(X509FindType.FindBySubjectDistinguishedName, subName2, true);

                return certs.OfType<X509Certificate>().FirstOrDefault();
            };
        }

       
    }
}
