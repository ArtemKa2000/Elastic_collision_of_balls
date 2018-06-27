using System;
using System.Windows;
using System.Windows.Input;

namespace Modeling
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class wExplanation : Window
    {
        public static wExplanation Instance { get; }
        static wExplanation()
        {
            Instance = new wExplanation();
        }
        private wExplanation()
        {
            InitializeComponent();
        }
        
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.CompareTo(Key.Escape) == 0 && e.IsDown)
                this.Hide();
        }
        public void ShowDialog(ExplanationType type)
        {
            loadFile(Data.getExplonationPath((int)type));
            base.ShowDialog();
           
        }
        private void loadFile(String path)
        {
            webBrowser.Source = new Uri(path);
        }

        [System.Obsolete("Використовуйте ShowDialog(ExpalnationType)", true)]//забороняє відкриття не вірними методами
        public new void Show() { }
        [System.Obsolete("Використовуйте ShowDialog(ExpalnationType)", true)]
        public new void ShowDialog() { }

        public enum ExplanationType
        {
            MenuMain, MenuModeling, ElementCreater, Modeling
        }
    }

}
