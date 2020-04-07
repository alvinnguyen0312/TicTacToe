using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TicTacToeLibrary;
using System.ServiceModel;
using System.Threading;

namespace TicTacToeClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [CallbackBehavior(ConcurrencyMode=ConcurrencyMode.Reentrant, UseSynchronizationContext =false)]
    public partial class MainWindow : Window, ICallback
    {
        private IGame tictactoe = null;
        public MainWindow()
        {
            InitializeComponent();

            // Create a tictictoe object
            try
            {


                // "this" indicate the MainWindow object which is callback object
                DuplexChannelFactory<IGame> channelFactory = new DuplexChannelFactory<IGame>(this, "GameEndPoint");


                tictactoe = channelFactory.CreateChannel();

                // To get the service recognize these client exist
                // we have to register it to receive callbacks
                tictactoe.RegisterForCallbacks();

                // Initialize the UI
                ResetUIContents();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void ButtonStart_Click(object sender, RoutedEventArgs e)
        {
            // Reset the list of Marks
            tictactoe.Repopulate();
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

            // Get the score from the server
            TextBoxPlayerAScore.Text = tictactoe.Player1Score.ToString();
            TextBoxPlayerBScore.Text = tictactoe.Player2Score.ToString();

            // Reset Result TextBox
            TextBoxResult.Text = String.Empty;
        }

        

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try 
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

                // Play and get the mark for the selected cell
                Mark mark = tictactoe.Play(player1Turn, index);

                if (mark != null)
                {

                    // Assign the mark to the button content
                    button.Content = mark;

                    // Check Winner and get the winning cells
                    tictactoe.CheckWinner();

                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
                this.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {            
                // Quitting, so unregister from the client callbacks
                tictactoe?.UnregisterFromCallbacks();

                // One of the players leave the game, reset the scores to zero
                tictactoe.CreateNewGame();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }



        // Callback contract implementation
        private delegate void ClientUpdateDelegate(CallbackInfo info);

        public void UpdateGameUI(CallbackInfo info)
        {
            if (System.Threading.Thread.CurrentThread == this.Dispatcher.Thread)
            {
                // Update the mark into the Gameboard
                Mark mark = info.SelectedMark;

                if (mark != null)
                {

                    switch (mark.CellPosition)
                    {
                        case 0:
                            Button0x0.Content = mark;
                            if (info.Player1Turn)
                                Button0x0.Foreground = Brushes.Red;
                            else
                                Button0x0.Foreground = Brushes.Black;
                            break;
                        case 1:
                            Button0x1.Content = mark;
                            if (info.Player1Turn)
                                Button0x1.Foreground = Brushes.Red;
                            else
                                Button0x1.Foreground = Brushes.Black;
                            break;
                        case 2:
                            Button0x2.Content = mark;
                            if (info.Player1Turn)
                                Button0x2.Foreground = Brushes.Red;
                            else
                                Button0x2.Foreground = Brushes.Black;
                            break;
                        case 3:
                            Button1x0.Content = mark;
                            if (info.Player1Turn)
                                Button1x0.Foreground = Brushes.Red;
                            else
                                Button1x0.Foreground = Brushes.Black;
                            break;
                        case 4:
                            Button1x1.Content = mark;
                            if (info.Player1Turn)
                                Button1x1.Foreground = Brushes.Red;
                            else
                                Button1x1.Foreground = Brushes.Black;
                            break;
                        case 5:
                            Button1x2.Content = mark;
                            if (info.Player1Turn)
                                Button1x2.Foreground = Brushes.Red;
                            else
                                Button1x2.Foreground = Brushes.Black;
                            break;
                        case 6:
                            Button2x0.Content = mark;
                            if (info.Player1Turn)
                                Button2x0.Foreground = Brushes.Red;
                            else
                                Button2x0.Foreground = Brushes.Black;
                            break;
                        case 7:
                            Button2x1.Content = mark;
                            if (info.Player1Turn)
                                Button2x1.Foreground = Brushes.Red;
                            else
                                Button2x1.Foreground = Brushes.Black;
                            break;
                        case 8:
                            Button2x2.Content = mark;
                            if (info.Player1Turn)
                                Button2x2.Foreground = Brushes.Red;
                            else
                                Button2x2.Foreground = Brushes.Black;
                            break;
                        default:
                            break;
                    }

                    if (!info.GameEnd)
                    {

                        // Update the turn
                        if (info.Player1Turn)
                        {
                            // Change Background color to indicate the next player turn                  
                            TextBoxPlayerAScore.Background = Brushes.LightGoldenrodYellow;
                            TextBoxPlayerBScore.Background = Brushes.White;
                        }
                        else
                        {
                            // Change Background color to indicate the next player turn
                            TextBoxPlayerAScore.Background = Brushes.White;
                            TextBoxPlayerBScore.Background = Brushes.LightGoldenrodYellow;
                        }

                    }
                    // If game ends
                    else
                    {
                        // Update the result
                        TextBoxResult.Text = info.Result;

                        // Update the score
                        TextBoxPlayerAScore.Text = info.Player1Score.ToString();
                        TextBoxPlayerBScore.Text = info.Player2Score.ToString();


                        // Show winning cells
                        List<int> marks = info.WinningCells;

                        if (marks.Count != 0) // If the games ends
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

                                } // end switch

                            } // end foreach


                        } // end if                 

                    } // end if   

                }
                // If mark nulls
                else
                {
                    // If the game repopulates, reset the UI.
                    if(!info.GameEnd)
                    {
                        // Reset UI
                        ResetUIContents();
                    }
                }

                
                
            }
            else
            {
                // Not the dispatcher thread that's running this method!
                this.Dispatcher.BeginInvoke(new ClientUpdateDelegate(UpdateGameUI), info);
            }

        }
    }
}
