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

            else if (number % 2 == 0)
                return "black";
            else
                return "red";
        }
    }
}
