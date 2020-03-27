using System;


namespace projekt1
{
    class Program
    {
        static void Main(string[] args)
        {
            string rownanie;
            double x, xMin, xMax;
            int n;

            rownanie = args[0];
            x = double.Parse(args[1]);
            xMin = double.Parse(args[2]);
            xMax = double.Parse(args[3]);
            n = int.Parse(args[4]);

            RPN sprawdz = new RPN(rownanie, x, xMin, xMax, n);
            sprawdz.Tokeny();
            sprawdz.walidacja();
            sprawdz.Postfix();
            Console.WriteLine(sprawdz.oblicz());
            sprawdz.Przedzial();
        }
    }
}
