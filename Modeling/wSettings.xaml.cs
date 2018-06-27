using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Modeling
{
    public partial class wSettings : Window
    {
        private bool isApplied;
        private bool myDialogResult;
        private bool isPathChanged;
        public static wSettings Instance { get; }
        public wSettings()
        {
            InitializeComponent();
        }
        static wSettings()
        {
            Instance = new wSettings();
        }

        //true - изменилось
        public new bool ShowDialog()
        {
            lb_Path.Content = Data.DataBasePath;
            tb_MinTime.Text = Data.MinimalCalculateTime.ToString();
            tb_MaxFrames.Text = Data.MaxFramesCount.ToString();
            tb_MinScale.Text = Data.MinScale.ToString();
            tb_TranslateFields.Text = Data.TranslateFields.ToString();
            tb_ScrollStep.Text = Data.ScrollStep.ToString();
            tb_AverageSpeedRatio.Text = Data.AverageSpeedRatio.ToString();
            myDialogResult = false;
            isApplied = true;
            base.ShowDialog();
            return (myDialogResult);
        }

        private void tb_MinTime_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            try
            {
                if (double.Parse(tb.Text) < Data.MinTimeDefault)
                    tb.Text = Data.MinTimeDefault.ToString();
                isApplied = false;
            }
            catch
            {
                tb.Text = Data.MinimalCalculateTime.ToString();
                MessageBox.Show("Не коректні дані!", "Помилка", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }
        private void tb_MaxFrames_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            try
            {
                if (int.Parse(tb.Text) < Data.MaxFramesCountDefault)
                    tb.Text = Data.MaxFramesCountDefault.ToString();
                isApplied = false;
            }
            catch
            {
                tb.Text = Data.MaxFramesCount.ToString();
                MessageBox.Show("Не коректні дані!", "Помилка", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }
        private void tb_ScrollStep_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            try
            {
                double value = double.Parse(tb.Text);
                if (value < 1)
                    tb.Text = "1";
                else
                    if (value > 10)
                    tb.Text = "10";
                isApplied = false;
            }
            catch
            {
                tb.Text = Data.ScrollStep.ToString();
                MessageBox.Show("Не коректні дані!", "Помилка", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }
        private void tb_MinScale_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            try
            {
                double value = double.Parse(tb.Text);
                if (value <= 0)
                    tb.Text = Data.MinScaleDefault.ToString();
                else
                    if (value > 1)
                        tb.Text = "1";
                isApplied = false;
            }
            catch
            {
                tb.Text = Data.MinScale.ToString();
                MessageBox.Show("Не коректні дані!", "Помилка", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }
        private void tb_TranslateFields_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            try
            {
                if (double.Parse(tb.Text) < 0)
                    tb.Text = "0";
                isApplied = false;
            }
            catch
            {
                tb.Text = Data.TranslateFields.ToString();
                MessageBox.Show("Не коректні дані!", "Помилка", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }
        private void tb_AverageSpeedRatio_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            try
            {
                double value = double.Parse(tb.Text);
                if (value < Data.minSpeedRatio)
                    tb.Text = Data.minSpeedRatio.ToString();
                else
                    if (value > Data.maxSpeedRatio)
                        tb.Text = Data.maxSpeedRatio.ToString();
                isApplied = false;
            }
            catch
            {
                tb.Text = Data.AverageSpeedRatio.ToString();
                MessageBox.Show("Не коректні дані!", "Помилка", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }
        private void btn_ChangePath_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog of = new OpenFileDialog();
            of.InitialDirectory = Environment.CurrentDirectory + "\\Sources";
            of.Filter = "*.xml|*.xml";
            if (of.ShowDialog() == true)
            {
                if (File.Exists(of.FileName))
                {
                    lb_Path.Content = of.FileName;
                    isApplied = false;
                    isPathChanged = true;
                }
                
            }
        }

        private void btn_Back_Click(object sender, RoutedEventArgs e)
        {
            close();
        }
        private void btn_Reset_Click(object sender, RoutedEventArgs e)
        {
            lb_Path.Content = Data.DataBasePathDefault;
            tb_MinTime.Text = Data.MinTimeDefault.ToString();
            tb_MinScale.Text = Data.MinScaleDefault.ToString();
            tb_MaxFrames.Text = Data.MaxFramesCountDefault.ToString();
            tb_ScrollStep.Text = Data.ScrollStepDefault.ToString();
            tb_TranslateFields.Text = Data.TranslateFieldsDefault.ToString();
            tb_AverageSpeedRatio.Text = Data.AverageSpeedRatioDefault.ToString();
            isApplied = false;
        }
        private void btn_Apply_Click(object sender, RoutedEventArgs e)
        {
            apply();
            close();
        }
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.CompareTo(Key.Escape) == 0 && e.IsDown)
                close();
        }

        private void apply()
        {
            try
            {
                String path = lb_Path.Content.ToString();
                double minTime = double.Parse(tb_MinTime.Text);
                int framesCount = int.Parse(tb_MaxFrames.Text);
                double minScale = double.Parse(tb_MinScale.Text);
                double translateFields = double.Parse(tb_TranslateFields.Text);
                double scrollStep = double.Parse(tb_ScrollStep.Text);
                double averageSpeedRatio = double.Parse(tb_AverageSpeedRatio.Text);

                //тільки після перевірки коректності усіх даних аби запобігти частковому збереженню інформації
                Data.MinimalCalculateTime = minTime;
                Data.MaxFramesCount = framesCount;
                Data.MinScale = minScale;
                Data.TranslateFields = translateFields;
                Data.ScrollStep = scrollStep;
                Data.AverageSpeedRatio = averageSpeedRatio;
                if (isPathChanged)
                    Data.DataBasePath = path;//тільки піля синхронізації інших змінних

                myDialogResult = true;
                isApplied = true;
            }
            catch {
                MessageBox.Show("Не коректні дані!\nЗбереження не відбулось.","Помилка", MessageBoxButton.OK,MessageBoxImage.Exclamation);
            }
        }
        private void close()
        {
            if (isApplied)
                this.Hide();
            
            else
            {
                switch (MessageBox.Show("Застосувати зміни?", "Застосування", MessageBoxButton.YesNoCancel, MessageBoxImage.Question))
                {
                    case MessageBoxResult.Yes: apply(); Hide(); break;
                    case MessageBoxResult.No: Hide(); break;
                }

            }
        }



        [System.Obsolete("Використовуйте ShowDialog() метод!", true)]//забороняє відкриття не як модального вікна
        public new void Show() { }
    }
}
