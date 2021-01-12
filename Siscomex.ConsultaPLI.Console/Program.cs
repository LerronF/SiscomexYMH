using System;
using System.Threading.Tasks;

namespace Siscomex.ConsultaPLI.Console
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                var lib = new Siscomex.ConsultaPLI.Lib.Work();
                await lib.ExecutarAsync();
            }
            catch (Exception ex)
            {

            }
        }
    }
}
