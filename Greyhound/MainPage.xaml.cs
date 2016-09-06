using System;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Greyhound
{
    // Comments Start on the "Punter_Factory.cs" Page...

    public sealed partial class MainPage : Page
    {
        // create an array of default punters
        private Punter[] Punters = new Punter[3];

        public MainPage()
        {
            this.InitializeComponent();
            Punters[0] = Punter_Factory.ReturnPunter(1);
            Punters[1] = Punter_Factory.ReturnPunter(2);
            Punters[2] = Punter_Factory.ReturnPunter(3);
            // then fill that array with real punters via our factory class
            SetLabels();
        }

        public void SetLabels()
        {
            // basically handling the user interface of the game so that you can see the punter's information, i'll go through one only because the others are the same
            // this used to be a foreach but i thought it was too clunky and requires the punters to have sliders and combo boxes in their class.
            if (Punters[0].Busted)
            {
                // if we have busted turn everything off
                Name1.Text = Punters[0].Name + " has Busted!";
                slider1.Maximum = 0;
                slider1.Value = 0;
                slider1.IsEnabled = false;
                comboBox1.SelectedIndex = -1;
                comboBox1.IsEnabled = false;
            }
            else
            {
                // if we havent busted update the things
                slider1.IsEnabled = true;
                comboBox1.IsEnabled = true;
                Name1.Text = Punters[0].Name + " has " + Punters[0].Money.ToString() + " tokens.";
                slider1.Maximum = Punters[0].Money;
            }
            if (Punters[1].Busted)
            {
                Name2.Text = Punters[1].Name + " has Busted!";
                slider2.Maximum = 0;
                slider2.Value = 0;
                slider2.IsEnabled = false;
                comboBox2.SelectedIndex = -1;
                comboBox2.IsEnabled = false;
            }
            else
            {
                slider2.IsEnabled = true;
                comboBox2.IsEnabled = true;
                Name2.Text = Punters[1].Name + " has " + Punters[1].Money.ToString() + " tokens.";
                slider2.Maximum = Punters[1].Money;
            }
            if (Punters[2].Busted)
            {
                Name3.Text = Punters[2].Name + " has Busted!";
                slider3.Maximum = 0;
                slider3.Value = 0;
                slider3.IsEnabled = false;
                comboBox3.SelectedIndex = -1;
                comboBox3.IsEnabled = false;
            }
            else
            {
                slider3.IsEnabled = true;
                comboBox3.IsEnabled = true;
                Name3.Text = Punters[2].Name + " has " + Punters[2].Money.ToString() + " tokens.";
                slider3.Maximum = Punters[2].Money;
            }
        }

        private void Go_Click(object sender, RoutedEventArgs e)
        {
            if (Go.Content.ToString() == "Try Again?") // if everyone is busted and the button is clicked, the user wants to play again so reset everything
            {
                Punters[0].Busted = false;
                Punters[0].Money = 50;
                Punters[1].Busted = false;
                Punters[1].Money = 50;
                Punters[2].Busted = false;
                Punters[2].Money = 50;
                SetLabels();
                Reset();
            }
            else if (Go.Content.ToString() == "Reset") // resets after races
            {
                Reset();
            }
            else // race
            {
                Betting();
                MoveRacers();
            }
        }

        public void Betting()
        {
            // if our punter isnt busted, and he has selected a value for the slider, and he has selected a racer
            // then place a bet for him and also add a mention of it to the listbox
            string console;
            if (Punters[0].Busted == false && slider1.Value != 0 && comboBox1.SelectedIndex != -1)
            {
                Punters[0].Bet(Convert.ToInt32(slider1.Value), comboBox1.SelectedIndex + 1);
                console = (Punters[0].Name + " has bet " + slider1.Value + " on Racer " + (comboBox1.SelectedIndex + 1));
                listBox.Items.Add(console);
            }
            if (Punters[1].Busted == false && slider2.Value != 0 && comboBox2.SelectedIndex != -1)
            {
                Punters[1].Bet(Convert.ToInt32(slider2.Value), comboBox2.SelectedIndex + 1);
                console = (Punters[1].Name + " has bet " + slider2.Value + " on Racer " + (comboBox2.SelectedIndex + 1));
                listBox.Items.Add(console);
            }
            if (Punters[2].Busted == false && slider3.Value != 0 && comboBox3.SelectedIndex != -1)
            {
                Punters[2].Bet(Convert.ToInt32(slider3.Value), comboBox3.SelectedIndex + 1);
                console = (Punters[2].Name + " has bet " + slider3.Value + " on Racer " + (comboBox3.SelectedIndex + 1));
                listBox.Items.Add(console);
            }
        }

        public async void MoveRacers()
        {
            // Handles the movement of the racers, uses bytes to create an alpha red green blue value and then slowly climbs up to go from black to white
            // As soon as a racer has a fully white color (255) they win!
            Go.IsEnabled = false;
            Random rng = new Random();
            byte x = 0;
            byte y = 0;
            byte z = 0;
            byte b = 0;
            do
            {
                x = LimitByte(x += Convert.ToByte(rng.Next(4)));
                y = LimitByte(y += Convert.ToByte(rng.Next(4)));
                z = LimitByte(z += Convert.ToByte(rng.Next(4)));
                b = LimitByte(b += Convert.ToByte(rng.Next(4)));
                Racer_1.Background = new SolidColorBrush(Color.FromArgb(255, x, x, x));
                Racer_2.Background = new SolidColorBrush(Color.FromArgb(255, y, y, y));
                Racer_3.Background = new SolidColorBrush(Color.FromArgb(255, z, z, z));
                Racer_4.Background = new SolidColorBrush(Color.FromArgb(255, b, b, b));
                await Task.Delay(50);
            } while (x != 255 && y != 255 && z != 255 && b != 255);
            // If two of the racers reach 255 in the same number of rolls (rare) it's a tie and we refund everyone who bet
            if (x == 255 && y == 255 || x == 255 && z == 255 || x == 255 && b == 255 || y == 255 && z == 255 || y == 255 && b == 255
                || z == 255 && b == 255)
            {
                Winner(5);
            }
            else // if not someone had to win, find out
            if (x == 255)
            {
                Winner(1);
            }
            else if (y == 255)
            {
                Winner(2);
            }
            else if (z == 255)
            {
                Winner(3);
            }
            else if (b == 255)
            {
                Winner(4);
            }
        }

        public Byte LimitByte(Byte Byte1) // this method is in response to a (bug? not really since i knew it was going to happen) bad thing where
                                          // if a byte was on say 254, 1 before winning and then rolled a 4 then the byte would overflow and reset to 0
                                          // So now if a byte rolls a 252, 253 or 254 it wins also, this has reduced ties dramatically
        {
            if (Byte1 > 251)
            {
                Byte1 = 255;
            }
            return Byte1;
        }

        public void Winner(int Winner)
        {
            switch (Winner)
            {
                // Simple check and then update the text so that the user knows who won
                case 1:
                    Racer_1_Text.Text = "Winner!";
                    break;

                case 2:
                    Racer_2_Text.Text = "Winner!";
                    break;

                case 3:
                    Racer_3_Text.Text = "Winner!";
                    break;

                case 4:
                    Racer_4_Text.Text = "Winner!";
                    break;

                case 5:
                default:
                    // for a tie or an error
                    Racer_1_Text.Text = "Tie!";
                    Racer_2_Text.Text = "Tie!";
                    Racer_3_Text.Text = "Tie!";
                    Racer_4_Text.Text = "Tie!";
                    break;
            }
            Go.Content = "Reset";
            // let the user reset everything if they want
            Go.IsEnabled = true;
            Punters[0].Resolve(Winner);
            Punters[1].Resolve(Winner);
            Punters[2].Resolve(Winner);
            SetLabels();
            CheckGameOver();
            // resolve all the bets and then set the labels, check if maybe everyone lost
        }

        public void CheckGameOver()
        {
            if (Punters[0].Busted && Punters[1].Busted && Punters[2].Busted)
            {
                Go.Content = "Try Again?";
                // everyone lost, so ask if the user wants to reset everything
            }
        }

        public void Reset()
        {
            Racer_1_Text.Text = "Racer 1";
            Racer_2_Text.Text = "Racer 2";
            Racer_3_Text.Text = "Racer 3";
            Racer_4_Text.Text = "Racer 4";
            Racer_1.Background = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
            Racer_2.Background = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
            Racer_3.Background = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
            Racer_4.Background = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
            Go.Content = "Race!";
            // Reset stuff and get ready for the next race.
        }
    }
}