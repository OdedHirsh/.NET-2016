using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace WpsApp2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool go = true;
        double velocityX = 1.1, velocityY = 1.1;
        double X = 20.03, Y = 12.8;
        double imgBobHeight, imgBobWidth, w, h;
        public MainWindow()
        {
            InitializeComponent();
            imgBobHeight = imageBob.Height;
            imgBobWidth = imageBob.Width;
            h = this.Height;
            w = this.Width;
            Thread thread = new Thread(() =>
            {
                while (go)
                {
                    X += velocityX;
                    Y += velocityY;
                    if (X<0)
                    {
                        velocityX *= -1;
                        X = 0;
                    }
                    if (X+ imgBobWidth > w)
                    {
                        velocityX *= -1;
                        X = w - imgBobWidth;
                    }
                    if (Y < 0)
                    {
                        velocityY *= -1;
                         Y = 0;
                    }
                    if (Y + imgBobHeight > h)
                    {
                        velocityY *= -1;
                        Y = h - imgBobHeight;
                    }
                    this.Dispatcher.Invoke(() =>
                    {
                        Canvas.SetLeft(imageBob, X);
                        Canvas.SetTop(imageBob, Y);
                    });
                    Thread.Sleep(100);
                }
                
                
            });
            thread.Start();



        }
    }
}
