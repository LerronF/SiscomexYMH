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

        int aux = 0;

        ChromeOptions options = null;

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
            //options = new ChromeOptions();

            //var downloadDirectory = @"C:\iTriad\yamaha\prints";

            //options.AddArguments("ignore-certificate-errors", "ignore-urlfetcher-cert-requests");
            //options.AddUserProfilePreference("download.default_directory", downloadDirectory);
            //options.AddUserProfilePreference("download.prompt_for_download", false);
            //options.AddUserProfilePreference("disable-popup-blocking", true);
            //options.AddUserProfilePreference("safebrowsing.enabled", "false");
            ////options.AddArguments("headless");
            ////options.AddArgument("--start-maximized");
            //options.AddArgument("test-type");
            //options.AddArgument("no-sandbox");
            //options.AddArgument(Directory.GetCurrentDirectory() + @"\chromedriver.exe");

            using (var service = PhantomJSDriverService.CreateDefaultService(Directory.GetCurrentDirectory()))
            {
                LogController.RegistrarLog("Carregando certificado...");
                ControleCertificados.CarregarCertificado(service);
               
                service.AddArgument("test-type");
                service.AddArgument("no-sandbox");
                service.HideCommandPromptWindow = true;

                //var _driverCH = new ChromeDriver(options);

                using (var _driver = new PhantomJSDriver(service))
                //using (var _driver = new ChromeDriver(options))
            {
                    try
                    {
                        
                        Acessar(_driver);

                        LogController.RegistrarLog($"Execução concluída.");
                    }
                    catch (Exception)
                    {
                        _driver.Close();
                    }
                }
            }
        }

        private async Task Acessar(PhantomJSDriver _driver)
        {
            try
            {
                //LogController.RegistrarLog("Acessando URL...");

                //_driver.Navigate().GoToUrl(_urlSite);
                //Thread.Sleep(500);
                //var retorno = capturaImagem(_driver, aux.ToString());
                //aux++;

                //LogController.RegistrarLog("Acessando ÁREA DE CONSULTA DO PLI...");
                //_driver.Navigate().GoToUrl(_urlConsultaPLI);
                //Thread.Sleep(500);
                //retorno = capturaImagem(_driver, aux.ToString());
                //aux++;

                ////#arquivo

                //LogController.RegistrarLog($"SELECIONANDO ARQUIVO .XML PARA ENVIO...");
                //OpenQA.Selenium.IWebElement element = _driver.FindElementByCssSelector("#arquivo");
                //element.SendKeys(_arquivo);
                //Thread.Sleep(900);
                //retorno = capturaImagem(_driver, aux.ToString());
                //aux++;

                //LogController.RegistrarLog($"CLICK NO BOTÃO ENVIAR....");
                //element = _driver.FindElementByCssSelector("#enviarArquivo");
                ////element.Submit();



                DownloadXML(@"https://www1c.siscomex.receita.fazenda.gov.br/li_web-7/liweb_menu_li_download_xsd_consulta_lote.do?");

               

                //LogController.RegistrarLog($"CLICK NO BOTÃO ENVIAR....");
                //element = _driver.FindElementByCssSelector("#enviarArquivo");
                //element.Submit();
                //Thread.Sleep(900);
                //var horaData = DateTime.Now.ToString().Replace("/", "").Replace(":", "").Replace(" ", "");
                //string arquivoPath = Path.Combine(@"C:\iTriad\yamaha\prints" + "\\", horaData + "-Extrato.xml");

                //File.WriteAllBytes(arquivoPath, ConvertToByteArray(_driver.PageSource));

                //retorno = capturaImagem(_driver, aux.ToString());
                //aux++;

            }
            catch
            {
                LogController.RegistrarLog($"Fim da Consulta");
                _driver.Close();
            }
        }

        public void Screenshot(IWebDriver driver, string screenshotsPasta)
        {
            ITakesScreenshot camera = driver as ITakesScreenshot;
            Screenshot foto = camera.GetScreenshot();
            foto.SaveAsFile(screenshotsPasta, ScreenshotImageFormat.Png);
        }

        public bool capturaImagem(PhantomJSDriver _driver, string numero)
        {
            try
            {
                //FUTURAMENTE ESSE CAMINHO SERÁ CONFIGURADO EM UMA TABELA
                if (!System.IO.Directory.Exists(@"C:\iTriad\yamaha\prints\"))
                {
                    System.IO.Directory.CreateDirectory(@"C:\iTriad\yamaha\prints\");
                }

                string arquivoPath = Path.Combine(@"C:\iTriad\yamaha\prints\", numero + "-PLI.jpg");

                Screenshot(_driver, arquivoPath);
                Thread.Sleep(1000);

                FileInfo fileInfo = new FileInfo(arquivoPath);
                var tam = fileInfo.Length;
                if (fileInfo.Length <= 0)
                {
                    File.Delete(arquivoPath);

                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        protected bool DownloadXML(string _url)
        {
            try
            {
                var tentarNovamente = true;
                var tentativas = 1;
                string arquivoPath = "";

                while (tentarNovamente)
                {
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
                            driver._my.Navigate(_urlConsultaPLI);
                            driver.FindElement(By.CssSelector("#arquivo")).SendKeys(_arquivo);
                            driver.FindElement(By.CssSelector("#enviarArquivo")).Submit();

                            Thread.Sleep(5000);

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
                            LogController.RegistrarLog(tentativas + "º Tentativa de Baixar o Extrato - XML.");
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

                    return false;

                }
                else
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                LogController.RegistrarLog("Erro ao Baixar Extrato XML " + e.Message.Trim());

                return false;
            }
        }

        public static byte[] ConvertToByteArray(string str)
        {
            byte[] arr = System.Text.Encoding.ASCII.GetBytes(str);
            return arr;
        }
    }
}
