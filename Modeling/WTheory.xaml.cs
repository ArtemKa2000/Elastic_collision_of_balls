using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.IO;

namespace Modeling
{
    /// <summary>
    /// Interaction logic for Theory.xaml
    /// </summary>
    public partial class WTheory : Window
    {
        public static WTheory Instance { get; }
        private List<String> paths;
        static WTheory()
        {
            Instance = new WTheory();
        }
        private WTheory()
        {
            InitializeComponent();
            paths = new List<string>();
        }

        public new void Show()
        {
            base.Show();
            loadData();
        }
        private void loadData()
        {
            string[] files = Directory.GetFiles(Data.TheoryPath, "*.htm*");
            foreach (String s in files)
            {
                    paths.Add(Environment.CurrentDirectory + "\\" + s);
                    String temp = s.Substring(s.LastIndexOf('\\') + 1);
                    lb_Menu.Items.Add(temp.Substring(0, temp.LastIndexOf('.')));
            }
            if (lb_Menu.Items.Count > 0 && lb_Menu.SelectedIndex<0)
                lb_Menu.SelectedIndex = 0;
        }
        private void lb_Menu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (lb_Menu.SelectedIndex >= 0)
                    webBrowser.Source = new Uri(paths[lb_Menu.SelectedIndex]);
            }
            catch
            {
                MessageBox.Show("Не вдалося відкрити файл!", "Помилка", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }
        private void im_Back_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Hide();
            lb_Menu.Items.Clear();
            paths.Clear();
            Data.getWindow(true).Show();
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Data.applicationClosing();
        }
    }
}
