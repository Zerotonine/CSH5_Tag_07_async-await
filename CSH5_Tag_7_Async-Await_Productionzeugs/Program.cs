using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CSH5_Tag_7_Async_Await_Productionzeugs
{
    class Program
    {
        static int rohstoffe = 1000;
        const int S_WERT1 = 500;
        const int S_WERT2 = 100;
        static Random rnd = new Random();
        static int counter = 1;
        static void Main(string[] args)
        {
            bool auffuelenRunning = false;
            bool prodRunning = true;

            CancellationTokenSource ctsProd = new CancellationTokenSource();
            CancellationTokenSource ctsAuf = new CancellationTokenSource();
            Produktion(ctsProd.Token);

            while (true)
            {
                if (rohstoffe <= S_WERT1 && !auffuelenRunning)
                {
                    Auffuellen(ctsAuf.Token);
                    auffuelenRunning = true;
                }

                if (rohstoffe <= S_WERT2)
                {
                    ctsProd.Cancel();
                    prodRunning = false;
                }
                else if(rohstoffe >= 900 && !prodRunning)
                {
                    ctsProd = new CancellationTokenSource();
                    Produktion(ctsProd.Token);
                    prodRunning = true;

                    ctsAuf.Cancel();
                    auffuelenRunning = false;
                    ctsAuf = new CancellationTokenSource();
                }
            }
        }

        async static void Produktion(CancellationToken ct)
        {
            while(!ct.IsCancellationRequested)
            {
                rohstoffe -= rnd.Next(60, 81);
                Console.WriteLine("Produktion Runde " + counter++);
                Console.WriteLine("Rohstoffe: " + rohstoffe);
                
                await Task.Run(() => SchlafKindleinSchlaf(500));
            }
        }

        async static void Auffuellen(CancellationToken ct)
        {
            while (!ct.IsCancellationRequested)
            {
                rohstoffe += 100;
                Console.WriteLine("Rohstoffe hinzugefügt!");
                //await Task.Delay(1000);
                await Task.Run(() => SchlafKindleinSchlaf(1000));
            }
        }

        static void SchlafKindleinSchlaf(int dauer)
        {
            Thread.Sleep(dauer);
        }

    }
}
