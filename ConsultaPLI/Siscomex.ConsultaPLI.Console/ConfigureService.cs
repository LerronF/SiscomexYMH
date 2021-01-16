using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace Siscomex.ConsultaPLI.Console
{
    public class ConfigureService
    {
        internal static void Configure()
        {
            HostFactory.Run(configure =>
            {
                configure.Service<SiscomexWS>(service =>
                {
                    service.ConstructUsing(s => new SiscomexWS());
                    service.WhenStarted(s => s.Start());
                    service.WhenStopped(s => s.Stop());
                });
                //Configure a Conta que o serviço do Windows usa para rodar
                configure.RunAsLocalSystem();
                configure.SetServiceName("MeuServicoWindowsComTopshelf");
                configure.SetDisplayName("MeuServicoWindowsComTopshelf");
                configure.SetDescription("Meu serviço Windows com Topshelf");
            });
        }
    }
}
