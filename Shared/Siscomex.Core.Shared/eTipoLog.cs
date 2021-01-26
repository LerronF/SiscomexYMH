using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Siscomex.Core.Shared
{
    public enum eTipoLog
    {
        [Description("Info")]
        INFO,
        [Description("Erro")]
        ERRO,
        [Description("Alerta")]
        ALERTA
    }
}
