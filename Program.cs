using System;

namespace AiAlgoritmos
{
    class Program
    {
        private static OitoDamas oitoDamas = new OitoDamas();
        private static NoThreeInLine noThreeInLine = new NoThreeInLine();

        private static Torres torres = new Torres();
        static void Main(string[] args)
        {

            torres.Teste();
            Console.ReadKey();
        }
    }
}