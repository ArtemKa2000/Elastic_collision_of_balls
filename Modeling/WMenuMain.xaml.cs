using System.Windows;
using System.Windows.Input;

namespace Modeling
{
    /// <summary>
    /// Interaction logic for Menu.xaml
    /// </summary>
    public partial class WMenuMain : Window
    {
        public static WMenuMain Instance { get; }
        private WMenuMain()
        {
            InitializeComponent();
        }
        static WMenuMain()
        {
            Instance = new WMenuMain();
        }

        private void im_ModelingMenu_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Hide();
            Data.pushWindow(this);
            WMenuModeling.Instance.Show();
        }
        private void im_Theory_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Hide();
            Data.pushWindow(this);
            WTheory.Instance.Show();
        }
        private void im_Map_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Hide();
            Data.pushWindow(this);
            WApplicationMap.Instance.Show();
        }
        private void im_Test_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Hide();
            Data.pushWindow(this);
            WTests.Instance.Show();
        }
        private void im_Back_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Hide();
            Data.getWindow(true).Show();
        }
        private void im_Exit_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Exit();
        }
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.CompareTo(Key.F1) == 0 && e.IsDown)
                wExplanation.Instance.ShowDialog(wExplanation.ExplanationType.MenuMain);
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Exit();
        }
        private void Exit()
        {
            Data.applicationClosing();
        }
    }
}
