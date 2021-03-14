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

namespace _3D_vaza
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        List<Point> selectedPoints = new List<Point>();

        List<List<Point>> allPoints = new List<List<Point>>();

        int alf = 30;
        int bet = 30;
        int stupnuOtoceni = 5;

        public Point Axon3Dto2D(int alpha, int beta, double x, double y, double z, double zoom, string osaOtoceni, int uhelOtoceni)
        {
            double X, Y, Z;
            double radUhelOtoc = uhelOtoceni * Math.PI / 180;

            switch (osaOtoceni)
            {
                case "x":
                    X = x;
                    Y = y * Math.Cos(radUhelOtoc) - (z * Math.Sin(radUhelOtoc));
                    Z = y * Math.Sin(radUhelOtoc) + (z * Math.Cos(radUhelOtoc));
                    break;
                case "y":
                    X = x * Math.Cos(radUhelOtoc) + (z * Math.Sin(radUhelOtoc));
                    Y = y;
                    Z = -x * Math.Sin(radUhelOtoc) + (z * Math.Cos(radUhelOtoc));
                    break;
                case "z":
                    X = x * Math.Cos(radUhelOtoc) - (y * Math.Sin(radUhelOtoc));
                    Y = x * Math.Sin(radUhelOtoc) + (y * Math.Cos(radUhelOtoc));
                    Z = z;
                    break;
                default:
                    X = x;
                    Y = y;
                    Z = z;
                    break;
            }

            Point bod2D = new Point();
            double alfaR = alpha * Math.PI / 180;
            double betaR = beta * Math.PI / 180;


            bod2D.X = -(Math.Cos(alfaR) * X * zoom) + (Math.Cos(betaR) * Y * zoom) + Gridik.ActualWidth / 2;
            bod2D.Y = Gridik.ActualHeight - (-Math.Sin(alfaR) * X * zoom - (Math.Sin(betaR) * Y * zoom) + (Z * zoom) + (Gridik.ActualHeight / 2));

            return bod2D;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            selectedPoints = selectedPoints.OrderBy(p => p.Y).ToList();
            allPoints.Clear();
            Gridik.Children.Clear();

            foreach (Point p in selectedPoints)
            {
                Point bod1 = new Point();
                Point bod2 = new Point();
                bod1 = Axon3Dto2D(alf, bet, p.X, p.Y, 0, 1, "y", 0);

                List<Point> gPoints = new List<Point>();

                for (int i = -(Math.Abs(stupnuOtoceni)); i < 360; i+= Math.Abs(stupnuOtoceni))
                {
                    bod2 = Axon3Dto2D(alf, bet, p.X, p.Y, 0, 1, "y", i);

                    Line cara = new Line();
                    cara.Stroke = Brushes.White;
                    cara.X1 = bod1.X;
                    cara.Y1 = bod1.Y;
                    cara.X2 = bod2.X;
                    cara.Y2 = bod2.Y;

                    Gridik.Children.Add(cara);
                    gPoints.Add(bod2);
                    bod1 = bod2;
                }

                allPoints.Add(gPoints);
            }

            for(int list = 0; list < allPoints[0].Count; list++)
            {
                Point bod1 = new Point();
                Point bod2 = new Point();
                bod1 = allPoints[0][list];

                for (int i = 1; i < allPoints.Count; i++)
                {
                    bod2 = allPoints[i][list];

                    Line cara = new Line();
                    cara.Stroke = Brushes.White;
                    cara.X1 = bod1.X;
                    cara.Y1 = bod1.Y;
                    cara.X2 = bod2.X;
                    cara.Y2 = bod2.Y;

                    Gridik.Children.Add(cara);

                    bod1 = bod2;
                }
            }
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Ellipse ellipse = new Ellipse();
            ellipse.Fill = Brushes.White;
            Point position = e.GetPosition(Grid2D);
            Console.WriteLine(position);
            ellipse.Height = 5;
            ellipse.Width = 5;
            ellipse.HorizontalAlignment = HorizontalAlignment.Left;
            ellipse.VerticalAlignment = VerticalAlignment.Top;
            ellipse.Margin = new Thickness(position.X, position.Y, 0, 0);
            Grid2D.Children.Add(ellipse);

            selectedPoints.Add(position);
        }

        private void smazat_btn_Click(object sender, RoutedEventArgs e)
        {
            Gridik.Children.Clear();
            Grid2D.Children.Clear();
            selectedPoints.Clear();
            allPoints.Clear();
        }
    }
}
