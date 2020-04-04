using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TicTacToeLibrary;
using System.ServiceModel;

namespace TicTacToeClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [CallbackBehavior(ConcurrencyMode=ConcurrencyMode.Reentrant,UseSynchronizationContext =false)]
    public partial class MainWindow : Window, ICallback
    {
        private IGame tictactoe = null;
        public MainWindow()
        {
            InitializeComponent();

            // Start a new game
            //tictactoe = new GamePlay();
            try
            {
                DuplexChannelFactory<IGame> channel = new DuplexChannelFactory<IGame>(this, "GameEndPoint");
                tictactoe = channel.CreateChannel();

                // Register for the callbacks
                tictactoe.RegisterForCallbacks();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void ButtonStart_Click(object sender, RoutedEventArgs e)
        {
            // Reset the list of Marks
            tictactoe.CreateNewGame();

            // Reset the UI contents
            ResetUIContents();

           
        }

        private void ResetUIContents()
        {
            // Iterate all button on the game board to reset the button contents
            GameBoardContainer.Children.Cast<Button>().ToList().ForEach(button =>
            {
                button.Content = "";
                button.Background = Brushes.Transparent;
                button.IsEnabled = true;
                button.Opacity = 1;
            });

            // Reset the scores on the UI
            TextBoxPlayerAScore.Background = Brushes.LightGoldenrodYellow;
            TextBoxPlayerBScore.Background = Brushes.White;
        }

        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            if (tictactoe != null)
                // Quitting, so unregister from the client callbacks
                tictactoe.UnregisterFromCallbacks();
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (tictactoe.GameEnd)
            {
                return;
            }

            var button = (Button)sender;

            // Get the row and coloum of this button
            var row = Grid.GetRow(button);
            var column = Grid.GetColumn(button);

            // Find the index
            var index = column + (row * 3);

            // Get which player's turn
            var player1Turn = tictactoe.Player1Turn;

            // Change Player B's mark red
            if (!player1Turn)
                button.Foreground = Brushes.Red;
            else
                button.Foreground = Brushes.Black;

            // Play
            tictactoe.Play(player1Turn, index);

            // Get the mark based on the player and available index
            var mark = tictactoe.GetMark(index);

            // Assign the mark to the button content
            button.Content = mark;

            // Check Winner
            List<int> marks = tictactoe.CheckWinner();

            if (marks != null)
            {
                foreach (var i in marks)
                {
                    switch (i)
                    {
                        case 0:
                            Button0x0.Background = Brushes.Goldenrod;
                            Button0x0.Opacity = 0.5;
                            break;
                        case 1:
                            Button0x1.Background = Brushes.Goldenrod;
                            Button0x1.Opacity = 0.5;
                            break;
                        case 2:
                            Button0x2.Background = Brushes.Goldenrod;
                            Button0x2.Opacity = 0.5;
                            break;
                        case 3:
                            Button1x0.Background = Brushes.Goldenrod;
                            Button1x0.Opacity = 0.5;
                            break;
                        case 4:
                            Button1x1.Background = Brushes.Goldenrod;
                            Button1x1.Opacity = 0.5;
                            break;
                        case 5:
                            Button1x2.Background = Brushes.Goldenrod;
                            Button1x2.Opacity = 0.5;
                            break;
                        case 6:
                            Button2x0.Background = Brushes.Goldenrod;
                            Button2x0.Opacity = 0.5;
                            break;
                        case 7:
                            Button2x1.Background = Brushes.Goldenrod;
                            Button2x1.Opacity = 0.5;
                            break;
                        case 8:
                            Button2x2.Background = Brushes.Goldenrod;
                            Button2x2.Opacity = 0.5;
                            break;
                        default:
                            break;

                    }
                } // end foreach

                if(tictactoe.GameEnd)
                // Count the score in the server
                tictactoe.CountScores();

                // Get the score from the server
                if (player1Turn)
                    TextBoxPlayerAScore.Text = tictactoe.Player1Score.ToString();
                else
                    TextBoxPlayerBScore.Text = tictactoe.Player2Score.ToString();


            }

            if (player1Turn && !tictactoe.GameEnd)
            {
                // Change Background color to indicate which player's trun
                TextBoxPlayerAScore.Background = Brushes.White;
                TextBoxPlayerBScore.Background = Brushes.LightGoldenrodYellow;        
            }
            else
            {
                // Change Background color to indicate which player's trun
                TextBoxPlayerAScore.Background = Brushes.LightGoldenrodYellow;
                TextBoxPlayerBScore.Background = Brushes.White;
            }

            

            
        }

        private void HoldButton()
        {
            // Iterate all button on the game board to reset the button contents
            GameBoardContainer.Children.Cast<Button>().ToList().ForEach(button =>
            {
                button.IsEnabled = false;
                button.Background = Brushes.Transparent;
            });
        }

        // Callback contract implementation

        private delegate void ClientUpdateDelegate(CallbackInfo info);

        public void UpdateGameUI(CallbackInfo info)
        {
            if (System.Threading.Thread.CurrentThread == this.Dispatcher.Thread)
            {
                // Update the GUI
                TextBoxPlayerAScore.Text = info.Player1Score.ToString();
                TextBoxPlayerBScore.Text =info.Player2Score.ToString();
            }
            else
            {
                // Not the dispatcher thread that's running this method!
                this.Dispatcher.BeginInvoke(new ClientUpdateDelegate(UpdateGameUI), info);
            }
        }
    }
}
