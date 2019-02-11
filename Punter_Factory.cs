namespace Greyhound
{
    // Our Abstract class, this is like the template for all our punters.
    public abstract class Punter
    {
        // we make the string here abstract becuase it changes
        public abstract string Name { get; }
        public int Money = 50;
        public int Current_Bet;
        public int Square_Bet_On;
        public bool Busted = false;
        // if the punter is not busted, get their bet and take it away from their total money, also get the square they bet on
        public void Bet(int Bet, int Square)
        {
            if (Busted == false)
            {
                Current_Bet = Bet;
                Money -= Current_Bet;
                Square_Bet_On = Square;
            }
        }


        // if there was a tie, give my money back. else if we won we want our bet back plus our bet again. if we are out of money after this it means we have busted!!!
        public void Resolve(int Winner_Case)
        {
            if (Winner_Case == 5)
            {
                Money += Current_Bet;
            }
            else if (Winner_Case == Square_Bet_On)
            {
                Money += (Current_Bet * 2);
            }
            if (Money == 0)
            {
                Busted = true;
            }
            Current_Bet = 0;
            Square_Bet_On = 0;
        }


    }

    // Inherited classes here, litterally the only difference is their names, but other things could be changed here (like their starting balance, different methods etc...)
    internal class Joe : Punter
    {
        public override string Name
        {
            get
            {
                return "Joe";
            }
        }
    }

    internal class Bob : Punter
    {
        public override string Name
        {
            get
            {
                return "Bob";
            }
        }
    }

    internal class Al : Punter
    {
        public override string Name
        {
            get
            {
                return "Al";
            }
        }
    }

    // Factory class! when you give it a number it will return a punter corresponding with that number
    public static class Punter_Factory
    {
        static public Punter ReturnPunter(int Which)
        {
            Punter ThePunter = null;

            switch (Which)
            {
                case 1:
                    ThePunter = new Joe();
                    break;

                case 2:
                    ThePunter = new Bob();
                    break;

                default:
                    ThePunter = new Al();
                    break;
            }
            return ThePunter;
        }
    }
}