using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.PhantomJS;
using SimpleBrowser;
using Siscomex.Shared;
using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Siscomex.ConsultaPLI.Lib
{
    public class Work
    {
        #region Variaveis Estáticas

        public string _urlSite = @"https://www1c.siscomex.receita.fazenda.gov.br/siscomexImpweb-7/private_siscomeximpweb_inicio.do";
        public string _urlConsultaPLI = @"https://www1c.siscomex.receita.fazenda.gov.br/li_web-7/liweb_menu_li_consultar_lote_li.do";
        public string _arquivo = @"C:\iTriad\yamaha\testes\CONS_LI_SISCOMEX.xml";

        #endregion

        public Work()
        {
            LogController.RegistrarLog("#################  INICIALIZANDO - CE MERCANTE  ################# ");
        }

        public async Task ExecutarAsync()
        {
            await CarregarListaPLIAsync();
        }

        private async Task CarregarListaPLIAsync()
        {
            LogController.RegistrarLog($"Iniciando Automação....");

            _ = Acessar();

        }

        private async Task Acessar()
        {
            try
            {
                var tentarNovamente = true;
                var tentativas = 1;
                string arquivoPath = "";

                while (tentarNovamente)
                {
                    LogController.RegistrarLog($"Carregando Certificado...");
                    var certificado = ControleCertificados.GetClientCertificate();
                    using (var driver = new SimpleBrowser.WebDriver.SimpleBrowserDriver(certificado))
                    {
                        var horaData = DateTime.Now.ToString().Replace("/", "").Replace(":", "").Replace(" ", "");

                        //FUTURAMENTE ESSE CAMINHO SERÁ CONFIGURADO EM UMA TABELA
                        if (!System.IO.Directory.Exists(@"C:\iTriad\yamaha\prints\"))
                        {
                            System.IO.Directory.CreateDirectory(@"C:\iTriad\yamaha\prints\");
                        }

                        arquivoPath = Path.Combine(@"C:\iTriad\yamaha\prints\", horaData + "-Extrato.xml");


                        if (!File.Exists(arquivoPath))
                        {
                            driver._my.Navigate(_urlSite);
                            LogController.RegistrarLog($"Autenticando...");

                            driver._my.Navigate(_urlConsultaPLI);
                            LogController.RegistrarLog($"Acessando SISCOMEX...");

                            driver.FindElement(By.CssSelector("#arquivo")).SendKeys(_arquivo);
                            LogController.RegistrarLog($"Consultado PLI...");

                            driver.FindElement(By.CssSelector("#enviarArquivo")).Submit();
                            LogController.RegistrarLog($"Enviando Consulta...");

                            Thread.Sleep(2000);

                            LogController.RegistrarLog($"Download do arquivo de consulta do PLI...");
                            File.WriteAllBytes(arquivoPath, ConvertToByteArray(driver.PageSource));
                        }
                    }

                    FileInfo fileInfox = new FileInfo(arquivoPath);
                    var tamx = fileInfox.Length;
                    if (fileInfox.Length > 0)
                    {
                        tentarNovamente = false;
                    }
                    else
                    {
                        if (tentativas <= 5)
                        {
                            LogController.RegistrarLog(tentativas + "º Tentativa de Baixar o - XML.");
                            tentativas++;
                        }
                        else
                        {
                            tentarNovamente = false;
                        }
                    }
                }

                FileInfo fileInfo = new FileInfo(arquivoPath);
                var tam = fileInfo.Length;
                if (fileInfo.Length <= 0)
                {
                    File.Delete(arquivoPath);
                }
            }
            catch (Exception e)
            {
                LogController.RegistrarLog("Erro ao Baixar XML " + e.Message.Trim());
            }
        }

        public static byte[] ConvertToByteArray(string str)
        {
            byte[] arr = System.Text.Encoding.ASCII.GetBytes(str);
            return arr;
        }
    }
}
