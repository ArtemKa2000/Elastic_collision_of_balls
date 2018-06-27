using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Modeling
{
    /// <summary>
    /// Interaction logic for ApplicationMap.xaml
    /// </summary>
    public partial class WApplicationMap : Window
    {
        public static WApplicationMap Instance { get; }

        static WApplicationMap()
        {
            Instance = new WApplicationMap();
        }
        private WApplicationMap()
        {
            InitializeComponent();
            this.Show();
            grid_Main.Width = grid_Main.ActualWidth;
            grid_Main.Height = grid_Main.ActualHeight;
            grid_Main.VerticalAlignment = VerticalAlignment.Top;
            grid_Main.HorizontalAlignment = HorizontalAlignment.Left;
        }


        public double getRowHeight(int index)
        {
            return grid_Main.RowDefinitions[index].ActualHeight;
        }
        public double getColumWidth(int index)
        {
            return grid_Main.ColumnDefinitions[index].ActualWidth;
        }


        private void im_Back_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Hide();
            Data.getWindow(true).Show();
        }
        private void brd_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Border b = (Border)sender;
            switch (b.Name)
            {
                case "brd_LoadingBaner":
                    this.Hide();
                    Data.getWindow(true);
                    Data.getWindow(true).Show();
                    break;
                case "brd_MenuMain":
                    this.Hide();
                    Data.getWindow(true).Show();
                    break;
                case "brd_Test":
                    this.Hide();
                    Data.pushWindow(this);
                    WTests.Instance.Show();
                    break;
                case "brd_MenuModeling":
                    this.Hide();
                    Data.pushWindow(this);
                    WMenuModeling.Instance.Show();
                    break;
                case "brd_Theory":
                    this.Hide();
                    Data.pushWindow(this);
                    WTheory.Instance.Show();
                    break;
                case "brd_Settings":
                    wSettings.Instance.ShowDialog();
                    break;
                case "brd_ElementCreater":
                    wElementCreater.ElementCreater.ShowDialog();
                    break;
            }
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Data.applicationClosing();
        }
    }
}
