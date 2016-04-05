using ServerLogic.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServerLogic.Games.GameComponents;
using System.Threading;

namespace ServerLogic.Games
{
    public enum RouletteStates
    {
        Betting,
        Spinning,
        GainsOrLoses
    }
    public class Roulette : Game
    {
        public RouletteStates rouletteState;

        public Roulette(List<User> users)
        {
            rouletteState = RouletteStates.Betting;

            foreach (User u in users)
            {
                Console.WriteLine("----------------------------");
                Console.Out.Write(u.FullName + "'s Balance: $" + u.Balance + "\n");
                Console.Out.Write("Enter your buy in: $");

                string buyInStr = Console.ReadLine();
                decimal buyIn;

                while (!decimal.TryParse(buyInStr, out buyIn) || buyIn > u.Balance)
                {
                    Console.Out.Write("You can't afford that buy in. Try Again: \n");
                    Console.Out.Write("Enter your buy in: $");

                    buyInStr = Console.ReadLine();
                }

                Players.Add(new RoulettePlayer(u, buyIn));
            }
        }

        public void Start()
        {
            while (true)
            {
                bool noBet = false;

                RouletteWheel wheel = new RouletteWheel();
                rouletteState = RouletteStates.Betting;
                GameState = GameStates.Playing;

                foreach (RoulettePlayer player in Players)
                {
                    player.IndicateBet();
                }

                decimal bet;
                int betNum;
                string betColor;

                foreach (RoulettePlayer player in Players)
                {
                    Console.WriteLine("-----" + player.GetFullName() + "'s Turn -----");
                    string output = " ";
                    string output1 = " ";
                    do
                    {
                        try
                        {
                            Console.WriteLine("Place your bet: \n");
                            Reader();
                            output = ReadLine(30000);
                        }
                        catch (TimeoutException)
                        {
                            Console.WriteLine("You waited too long...Sit this one out.\n");
                            player.ForceNoBet();
                        }
                    }
                    while (!decimal.TryParse(output, out bet));

                    if (bet > player.getGameBalance())
                    {
                        bet = player.getGameBalance();
                        Console.Out.Write("over your balance, bet set to: " + bet + "\n");
                    }

                    do
                    {
                        try
                        {
                            Console.WriteLine("What are you betting on?\n\t Number[0-36]?\n\t Color[red,black]?\n");
                            Reader();
                            output1 = ReadLine(30000);
                        }
                        catch (TimeoutException)
                        {
                            Console.WriteLine("You waited too long...Sit this one out.\n");
                            player.ForceNoBet();
                        }
                    }
                    while ((!int.TryParse(output, out betNum)) || ((output != "red") && (output != "black")));

                    bool isNumber = int.TryParse(output, out betNum);

                    if (!isNumber)
                    {
                        betColor = output;
                        player.SetUserBet(bet, betColor);
                    }

                    else
                    {
                        player.SetUserBet(bet, betNum);
                    }
                }
                Console.Clear();

                //SPINNING
                Console.Out.Write("------------------Spinning------------------\n");
                rouletteState = RouletteStates.Spinning;

                int chosenNum = wheel.Spin();
                string chosenColor = wheel.getColor(chosenNum);

                Console.Out.Write("The wheel landed on: " + chosenNum + " " + chosenColor + "\n");

                //GAME CONCLUSION
                rouletteState = RouletteStates.GainsOrLoses;
                foreach (RoulettePlayer player in Players)
                {
                    Console.Out.Write("\n-------" + player.GetFullName() + "Conclusion------- \n");

                    if (player.getBetColor() == chosenColor && player.hasBetColor)
                    {
                        Console.WriteLine("The color you chose was correct, you win!\n");
                        player.UpdateGameBalance(true);
                    }

                    else if (player.getBetNum() == chosenNum && !player.hasBetColor)
                    {
                        Console.WriteLine("The number you chose was correct, you win!\n");
                        player.UpdateGameBalance(true);
                    }

                    else
                    {
                        Console.WriteLine("You did not choose correctly...you lose.\n");
                        player.UpdateGameBalance(false);
                    }

                    Console.WriteLine(player.GetFullName() + "'s game balance: $" + player.getGameBalance());

                    player.ClearBets();
                    player.UpdateUserBalance();

                    if (player.GetBalance() <= 0)
                    {
                        Console.WriteLine(player.GetFullName() + "'s total balance is now 0. Adding $10..");
                        player.GiftMoney(10);
                    }

                    if (player.getGameBalance() <= 0)
                    {
                        Console.WriteLine("\nYour game balance is 0! You lose the game!");
                        player.inGame = false;

                        if (Players.Count == 0)
                        {
                            Console.WriteLine("Press any key to exit the console..");
                            Console.ReadKey();
                            Environment.Exit(0);
                        }
                    }
                }

                foreach (RoulettePlayer p in Players)
                {
                    if (!p.inGame) Players.Remove(p);
                }

                if (Players.Count == 0)
                {
                    Console.ReadKey();
                    Environment.Exit(0);
                }

                //step8: loop back through game, check connections, if no connections break out of loop
            }

        }

        private static Thread inputThread;
        private static AutoResetEvent getInput, gotInput;
        private static string input;

        static void Reader()
        {
            getInput = new AutoResetEvent(false);
            gotInput = new AutoResetEvent(false);

            inputThread = new Thread(reader);
            inputThread.IsBackground = true;
            inputThread.Start();
        }

        private static void reader()
        {
            while (true)
            {
                getInput.WaitOne();
                input = Console.ReadLine();
                gotInput.Set();
            }
        }

        public static string ReadLine(int timeOutMillisecs)
        {
            getInput.Set();

            bool success = gotInput.WaitOne(timeOutMillisecs);

            if (success) return input;
            else throw new TimeoutException("User did not provide input within the time limit.");
        }
    }
}

