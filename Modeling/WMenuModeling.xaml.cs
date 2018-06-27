using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Modeling
{
    public partial class WMenuModeling : Window
    {
        static WMenuModeling()
        {
            Instance = new WMenuModeling();
            normalThickness = new Thickness(10);
            minimizeThickness = new Thickness(8);
        }
        private WMenuModeling()
        {
            InitializeComponent();

            Data.ElementsChanged += ElementsChanged;
            update();
            if (grid_Content.Children.Count > 0)//обираємо перший елемент, якщо він є
                Element_Click_Once(grid_Content.Children[0], new EventArgs());
        }
        public static WMenuModeling Instance { get; }
        private static readonly Thickness normalThickness;
        private static readonly Thickness minimizeThickness;

        private readonly List<int> chosenElements = new List<int>();


        public new void Show()
        {
            base.Show();
            update();
        }

        private void ElementsChanged()//підписаний на подію Data.ElementsChanged 
        {
            update();
        }
        private void grid_Content_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            update();
        }


        private void Element_Double_Click(object sender, MouseButtonEventArgs e)
        {
            Element_Click_Once(sender, e);

            Border b = sender as Border;
            int position = Grid.GetRow(b) * 3 + Grid.GetColumn(b);//порядковий номер елементу у списку

            if (e.ClickCount == 2)
                wElementCreater.ElementCreater.ShowDialog(position);//редагування елементу
        }
        private void Element_MouseEnter(object sender, MouseEventArgs e)
        {
            ((Border)sender).Margin = minimizeThickness;
        }
        private void Element_MouseLeave(object sender, MouseEventArgs e)
        {
            ((Border)sender).Margin = normalThickness;
        }
        private void Element_Click_Once(object sender, EventArgs e)
        {
            Border b = sender as Border;
            int position = Grid.GetRow(b) * 3 + Grid.GetColumn(b);//порядковий номер елементу у списку

            if (chosenElements.Contains(position))
            {
                //прибираємо елемент з обраних
                b.BorderBrush = Data.normalBorderBrush;
                chosenElements.Remove(position);
            }
            else
            {
                //додаємо елемент до обраних
                if (chosenElements.Count < 6)//обмеження на 6 елементів
                {
                    b.BorderBrush = Data.chosentBorderBrush;
                    chosenElements.Add(position);
                }
            }
            chosenElementsChanged();
        }
        private void btn_Dell_Click(object sender, RoutedEventArgs e)
        {
            if (chosenElements[chosenElements.Count - 1] != 0)//заборона видалити перший елемент
            {
                Data.deleteElement(chosenElements[chosenElements.Count - 1]);
                chosenElements.RemoveAt(chosenElements.Count - 1);
            }
            chosenElementsChanged();
        }
        private void chosenElementsChanged()
        {
            if (chosenElements.Count > 0)
            {
                btn_Modeling.IsEnabled = true;
                btn_Edit.IsEnabled = true;
                btn_Dell.IsEnabled = true;
            }
            else
            {
                btn_Modeling.IsEnabled = false;
                btn_Edit.IsEnabled = false;
                btn_Dell.IsEnabled = false;
            }
        }
        private void update()
        {
            grid_Content.RowDefinitions.Clear();
            grid_Content.Children.Clear();

            int rowCount = Data.Elements.Count / 3;
            if (rowCount * 3 < Data.Elements.Count)//якщо відбулось округлення
                rowCount++;

            for (int i = 0; i < rowCount; i++)
            {
                RowDefinition rw = new RowDefinition();
                rw.Height = new GridLength(grid_Content.ColumnDefinitions[0].ActualWidth);
                grid_Content.RowDefinitions.Add(rw);
            }

            for (int i = 0; i < Data.Elements.Count; i++)
            {
                Border cellBorder = new Border();

                cellBorder.MouseLeftButtonDown += Element_Double_Click;
                cellBorder.MouseEnter +=Element_MouseEnter;
                cellBorder.MouseLeave += Element_MouseLeave;
                cellBorder.Margin = normalThickness;
                cellBorder.BorderThickness = new Thickness(5);
                if (chosenElements.Contains(i))
                    cellBorder.BorderBrush = Data.chosentBorderBrush;
                else
                    cellBorder.BorderBrush = Data.normalBorderBrush;
                cellBorder.Background = Data.getBrush(Data.Elements[i].Canvas);
                grid_Content.Children.Add(cellBorder);
                Grid.SetColumn(cellBorder, i % 3);
                Grid.SetRow(cellBorder, i / 3);
            }
        }


        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.CompareTo(Key.F1) == 0 && e.IsDown)
                wExplanation.Instance.ShowDialog(wExplanation.ExplanationType.MenuModeling);
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Data.applicationClosing();
        }
        private void im_Back_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Hide();
            Data.getWindow(true).Show();
        }
        private void im_Settings_MouseDown(object sender, MouseButtonEventArgs e)
        {
            wSettings.Instance.ShowDialog();
        }
        private void btn_CreateElement_Click(object sender, RoutedEventArgs e)
        {
            wElementCreater.ElementCreater.ShowDialog();
        }
        private void btn_Modeling_Click(object sender, RoutedEventArgs e)
        {
            if (chosenElements.Count > 0)
            {
                this.Hide();
                Data.pushWindow(this);
                WModeling.Instance.Open(chosenElements);
            }
        }
        private void bnt_Edit_Click(object sender, RoutedEventArgs e)
        {
            if(chosenElements.Count>0)
            wElementCreater.ElementCreater.ShowDialog(chosenElements[chosenElements.Count-1]);
        }
        private void btn_Save_Click(object sender, RoutedEventArgs e)
        {
            Data.saveChange();
        }
    }
}
