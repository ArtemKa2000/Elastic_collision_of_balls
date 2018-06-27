using System.Windows;
using System.Windows.Input;
using System.Threading;
using System.Globalization;

namespace Modeling
{
    public partial class wLoadingBaner : Window
    {
        private bool isMouseDown;
        private Point oldPoint;

        public wLoadingBaner()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("uk-UA");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("uk-UA");
            InitializeComponent();

            //Викликати тільки після інших налаштувань і саме у такій послідовності!
            Data.initialize();
            Show();
            lb_toProgram.IsEnabled = true;
        }

        private void Label_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Hide();
            Data.pushWindow(this);
            WMenuMain.Instance.Show();
        }
        private void im_Exit_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Data.applicationClosing();
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Data.applicationClosing();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            isMouseDown = true;
            oldPoint = e.GetPosition(this);
        }
        private void Window_MouseUp(object sender, MouseButtonEventArgs e)
        {
            isMouseDown = false;
        }
        private void Window_MouseLeave(object sender, MouseEventArgs e)
        {
            isMouseDown = false;
        }
        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMouseDown)
            {
                Point newPoint = e.GetPosition(this);
                this.Top += newPoint.Y - oldPoint.Y;
                this.Left += newPoint.X - oldPoint.X;
                oldPoint = newPoint;
            }
        }
    }
}
