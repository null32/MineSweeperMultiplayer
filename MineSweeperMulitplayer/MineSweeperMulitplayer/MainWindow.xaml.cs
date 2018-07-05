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

namespace MineSweeperMulitplayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int ButtonSize = 20;
        private Tile[,] Tiles;
        private Button[,] Buttons;
        private uint Rows;
        private uint Cols;
        private uint Mines;
        private bool IsAlive;

        public MainWindow()
        {
            InitializeComponent();
            Rows = 15;
            Cols = 10;
            Mines = 30;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Init();
            Draw();
        }

        private void Grid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Close();
            }
        }

        private void Init()
        {
            IsAlive = true;
            //Generate field
            Tiles = new Tile[Cols, Rows];
            Buttons = new Button[Cols, Rows];
            for (uint i = 0; i < Cols; i++)
            {
                for (uint j = 0; j < Rows; j++)
                {
                    var t = new Tile(i, j);
                    Tiles[i, j] = t;
                    var b = new Button();
                    Buttons[i, j] = b;
                    b.Content = " ";
                    b.Click += (object sender1, RoutedEventArgs e1) => { Tile_Cliked(t.Row, t.Col); };

                    b.Margin = new Thickness(i * ButtonSize, j * ButtonSize, 0, 0);
                    b.HorizontalAlignment = HorizontalAlignment.Left;
                    b.VerticalAlignment = VerticalAlignment.Top;

                    b.BorderThickness = new Thickness(0.5);
                    b.BorderBrush = Brushes.DarkSlateBlue;
                    b.Background = Brushes.CornflowerBlue;
                    b.FontWeight = FontWeights.Bold;
                    b.FontFamily = new FontFamily("Calibri");
                    b.FontSize = 18;
                    b.Padding = new Thickness(-3);

                    b.Width = ButtonSize;
                    b.Height = ButtonSize;

                    GameField.Children.Add(b);
                }
            }
            //Generate mines
            var rng = new Random((int)(DateTime.Now.Subtract(new DateTime(1970, 1, 1)).TotalSeconds));
            for (uint i = 0; i < Mines; i++)
            {
                var pos = rng.Next((int)(Cols * Rows));
                while (Tiles[pos / Rows, pos % Rows].IsMine)
                {
                    pos = rng.Next((int)(Cols * Rows));
                }
                Tiles[pos / Rows, pos % Rows].IsMine = true;
            }
            //Count mines
            uint getMines(uint row, uint col)
            {
                if (Tiles[row, col].IsMine)
                {
                    return 0;
                }
                var shift = new Tuple<int, int>[]
                {
                    Tuple.Create(-1, -1), Tuple.Create(-1, 0), Tuple.Create(-1, 1),
                    Tuple.Create(0, -1),                       Tuple.Create(0, 1),
                    Tuple.Create(1, -1),  Tuple.Create(1, 0),  Tuple.Create(1, 1)
                };
                uint res = 0;

                foreach (var item in shift)
                {
                    if (row + item.Item1 < 0 || row + item.Item1 >= Cols ||
                        col + item.Item2 < 0 || col + item.Item2 >= Rows)
                    {
                        continue;
                    }
                    if (Tiles[row + item.Item1, col + item.Item2].IsMine)
                    {
                        res++;
                    }
                }

                return res;
            }
            for (uint i = 0; i < Cols; i++)
            {
                for (uint j = 0; j < Rows; j++)
                {
                    Tiles[i, j].Mines = getMines(i, j);
                }
            }
        }

        private void Draw()
        {
            for (uint i = 0; i < Cols; i++)
            {
                for (uint j = 0; j < Rows; j++)
                {
                    if (Tiles[i, j].Visible)
                    {
                        Buttons[i, j].Background = Brushes.LightCyan;
                        if (Tiles[i, j].Mines > 0)
                        {
                            Buttons[i, j].Content = Tiles[i, j].Mines;
                            Buttons[i, j].Foreground = getBrush(Tiles[i, j].Mines);
                        }
                        if (Tiles[i, j].IsMine)
                        {
                            Buttons[i, j].Content = 'X';
                        }
                    }
                }
            }
        }

        private void Tile_Cliked(uint row, uint column)
        {
            DebugInfo.Content = String.Format("Clicked {0} {1}", row, column);

            if (Tiles[row, column].IsMine)
            {
                IsAlive = false;
                return;
            }

        }

        private Brush getBrush(uint count)
        {
            switch (count)
            {
                case 1:
                    return Brushes.DarkBlue;
                case 2:
                    return Brushes.DarkGreen;
                case 3:
                    return Brushes.DarkRed;
                default:
                    break;
            }
            return Brushes.Black;
        }
    }
}
