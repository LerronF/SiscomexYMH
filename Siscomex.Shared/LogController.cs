using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Siscomex.Shared
{
    public static class LogController
    {
        public static void RegistrarLog(string msg, eTipoLog tipo = eTipoLog.INFO)
        {
            EscreverLog(msg, tipo.GetDescription());
        }

        private static void EscreverLog(string msg, string tipo)
        {
            Console.WriteLine($"{tipo} [ {DateTime.Now} ]: {msg}");
        }
    }
}
