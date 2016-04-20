using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLogic.Games.GameComponents
{
    public class RouletteWheel
    {
        private int MAX_NUMBER = 36;

        public RouletteWheel()
        {

        }

        public int Spin()
        {
            int spunNum;

            Random spin = new Random();
            spunNum = spin.Next(MAX_NUMBER, 0);

            return spunNum;
        }

        public String getColor (int number)
        {
            if (number == 0)
                return "green";

            else if ((number == 2) || (number == 4) || (number == 6) ||
                (number == 8) || (number == 10) || (number == 11) || (number == 13) ||
                (number == 15) || (number == 17) || (number == 20) || (number == 22) ||
                (number == 24) || (number == 26) || (number == 28) || (number == 29) ||
                (number == 31) || (number == 33) || (number == 35))
                return "black";
            else
                return "red";
        }
    }
}
