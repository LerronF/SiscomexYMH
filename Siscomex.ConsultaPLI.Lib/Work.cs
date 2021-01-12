using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.PhantomJS;
using Siscomex.Shared;
using System;
using System.IO;
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
            using (var service = PhantomJSDriverService.CreateDefaultService(Directory.GetCurrentDirectory()))
            {
                LogController.RegistrarLog("Carregando certificado...");
                ControleCertificados.CarregarCertificado(service);

                service.AddArgument("test-type");
                service.AddArgument("no-sandbox");
                //service.HideCommandPromptWindow = true;

                using (var _driver = new PhantomJSDriver(service))
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
                LogController.RegistrarLog("Acessando URL...");

                _driver.Navigate().GoToUrl(_urlSite);
                Thread.Sleep(500);
                var retorno = capturaImagem(_driver, aux.ToString());
                aux++;

                LogController.RegistrarLog("Acessando ÁREA DE CONSULTA DO PLI...");
                _driver.Navigate().GoToUrl(_urlConsultaPLI);
                Thread.Sleep(500);
                retorno = capturaImagem(_driver, aux.ToString());
                aux++;

                //#arquivo

                LogController.RegistrarLog($"SELECIONANDO ARQUIVO .XML PARA ENVIO...");
                OpenQA.Selenium.IWebElement element = _driver.FindElementByCssSelector("#arquivo");
                element.SendKeys(_arquivo);
                Thread.Sleep(900);
                retorno = capturaImagem(_driver, aux.ToString());
                aux++;

                LogController.RegistrarLog($"CLICK NO BOTÃO ENVIAR....");
                element = _driver.FindElementByCssSelector("#enviarArquivo");
                element.Click();
                Thread.Sleep(900);
                retorno = capturaImagem(_driver, aux.ToString());
                aux++;

            }
            catch
            {
                LogController.RegistrarLog($"Fim da Consulta");
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

        protected bool DownloadXML(string numero)
        {
            try
            {
                var tentarNovamente = true;
                var tentativas = 1;
                string arquivoPath = "";

                while (tentarNovamente)
                {
                    numero = numero.Replace("/", "");
                    numero = numero.Replace("-", "");
                    numero = numero.Replace("%2F", "");

                    var certificado = ControleCertificados.GetClientCertificate();
                    using (var driver = new SimpleBrowser.WebDriver.SimpleBrowserDriver(certificado))
                    {
                        var horaData = DateTime.Now.ToString().Replace("/", "").Replace(":", "").Replace(" ", "");

                        //FUTURAMENTE ESSE CAMINHO SERÁ CONFIGURADO EM UMA TABELA
                        if (!System.IO.Directory.Exists(@"C:\Versatilly\" + _nome_cliente + "\\"))
                        {
                            System.IO.Directory.CreateDirectory(@"C:\Versatilly\" + _nome_cliente + "\\");
                        }

                        arquivoPath = Path.Combine(@"C:\Versatilly\" + _nome_cliente + "\\", horaData + "-Extrato.xml");


                        if (!File.Exists(arquivoPath))
                        {
                            driver._my.Navigate(_urlSite);
                            driver._my.Navigate(_urlConsultaDI);
                            driver._my.Navigate(new Uri(_urlDownloadXML),
                                "perfil=IMPORTADOR&rdpesq=pesquisar&nrDeclaracao=" + numero + "&numeroRetificacao=&enviar=Consultar",
                                "application/x-www-form-urlencoded");

                            driver.FindElement(By.Id("nrDeclaracaoXml")).SendKeys(numero);
                            driver.FindElement(By.Name("ConsultarDiXmlForm")).Submit();

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
                            LogController.RegistrarLog(_nome_cliente + " - " + tentativas + "º Tentativa de Baixar o Extrato - XML.", eTipoLog.INFO, _cd_bot_exec, "bot");
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
                LogController.RegistrarLog(_nome_cliente + " - " + "Erro ao Baixar Extrato XML " + e.Message.Trim(), eTipoLog.INFO, _cd_bot_exec, "bot");

                return false;
            }
        }
    }
}
