using System;
using System.Collections.Generic;
using System.Text;
using Topshelf;

namespace Siscomex.Core
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
                configure.SetServiceName("SiscomexWS");
                configure.SetDisplayName("SiscomexWS");
                configure.SetDescription("Meu serviço SiscomexWS com Topshelf");
            });
        }
    }
}
