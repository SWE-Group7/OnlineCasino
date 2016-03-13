using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServerLogic.Games.GameComponents;

namespace ServerTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Deck d = new Deck();
            Console.Out.Write(d.Count());
            Console.Read();
        }
    }
}
