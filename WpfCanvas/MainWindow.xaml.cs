using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfCanvas
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        TicTacToe game;
        public MainWindow()
        {
            InitializeComponent();
            game = new TicTacToe();
        }




        private void btn1_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            int cell = int.Parse(btn.Name.Substring(3));
            MoveResult moveResult = game.makeMove(cell);
            if (moveResult==MoveResult.INVALID_MOVE)
            {
                MessageBox.Show("What are you doing");
                return;
            }
            btn.Content = game.isXTurn ? "O" : "X";
            if (moveResult != MoveResult.INVALID_MOVE)
            {
                if (moveResult == MoveResult.VICTORY)
                {
                    MessageBox.Show((game.isXTurn?"O":"X")+" is the Winner","we have a Winner");
                    return;
                }
                else if (moveResult==MoveResult.DRAW)
                {
                    MessageBox.Show("DRAW!!");
                    return;
                }
            }
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            game.reset();
            foreach (UIElement element in gridBoard.Children)
            {
                ((Button)element).Content = "";
            }
        }
    }
}
