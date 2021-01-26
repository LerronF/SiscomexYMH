using System;
using System.Collections.Generic;
using System.Text;

namespace Siscomex.Core.Shared
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
