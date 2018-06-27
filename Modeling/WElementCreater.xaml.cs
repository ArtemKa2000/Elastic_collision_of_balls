using System;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Modeling
{
    /// <summary>
    /// Interaction logic for WElementCreater.xaml
    /// </summary>
    public partial class wElementCreater : Window
    {
        private Element element;
        private Border fieldsBorder;
        private int elementIndex;
        private System.Windows.Forms.ColorDialog cDialog;
        private wElementCreater()
        {
            InitializeComponent();
            cDialog = new System.Windows.Forms.ColorDialog();
            lb_Scale.Content = "1м = " + Data.dataScale + "px";
        }

        private void ShowDialog(Element element)
        {
            ShowDialog(element, -1);
        }
        private void ShowDialog(Element element, int elementIndex)
        {
            this.elementIndex = elementIndex;
            this.element = element;
            this.element.Canvas.ClipToBounds = false;

            Border elementBorder = new Border();
            elementBorder.BorderBrush = new SolidColorBrush(Colors.Black);
            fieldsBorder = new Border();
            fieldsBorder.BorderBrush = new SolidColorBrush(Colors.Gray);
            fieldsBorder.HorizontalAlignment = HorizontalAlignment.Stretch;
            fieldsBorder.VerticalAlignment = VerticalAlignment.Stretch;
            fieldsBorder.Margin = new Thickness(-1*element.Fields);
            elementBorder.Child = element.Canvas;
            element.Canvas.Children.Add(fieldsBorder);
            rebuildMargins();

            b_Frame.Background = Data.getBrush(elementBorder);
            if (elementIndex == -1)
                btn_Add.Content = "Додати";
            else
                btn_Add.Content = "Застосувати";
            initializeFields();
            base.ShowDialog();
        }

        private void initializeFields()
        {
            tb_Width.Text = (element.Canvas.Width/Data.dataScale).ToString();
            tb_Height.Text = (element.Canvas.Height / Data.dataScale).ToString();
            tb_FPS.Text = element.FPS.ToString();
            tb_G.Text = (element.G / Data.dataScale).ToString();
            tb_SpeedAnimation.Text = element.SpeedAnimation.ToString();
            tb_Fields.Text = (element.Fields / Data.dataScale).ToString();
            tb_FontSize.Text = (element.FontSize/Data.dataScale).ToString();
            chb_isDataShow.IsChecked = element.InfoVisibility;
            tb_FontSize.IsEnabled = element.InfoVisibility;


            tb_Radius1.Text = (element.FirstBall.Radius / Data.dataScale).ToString();
            tb_Weight1.Text = element.FirstBall.Mass.ToString();
            if(element.FirstBall.SpeedAngle==0)
                tb_Speed1.Text = (element.FirstBall.Speed / Data.dataScale).ToString();
            else
                tb_Speed1.Text = (element.FirstBall.Speed / Data.dataScale* -1).ToString();
            tb_X1.Text = (element.FirstBall.Coord.X / Data.dataScale).ToString();
            rect_ColorStroke1.Fill = element.FirstBall.StrokeBrush;
            rect_Color1.Fill =element.FirstBall.FillBrush;

            tb_Radius2.Text = (element.SecondBall.Radius / Data.dataScale).ToString();
            tb_Weight2.Text = element.SecondBall.Mass.ToString();
            if (element.SecondBall.SpeedAngle == 0)
                tb_Speed2.Text = (element.SecondBall.Speed / Data.dataScale).ToString();
            else
                tb_Speed2.Text = (element.SecondBall.Speed / Data.dataScale * -1).ToString();
            tb_X2.Text = (element.SecondBall.Coord.X / Data.dataScale).ToString();
            rect_ColorStroke2.Fill = element.SecondBall.StrokeBrush;
            rect_Color2.Fill = element.SecondBall.FillBrush;
        }
        private void tb_TextChanged(object sender, TextChangedEventArgs e)
        {
            /* ОБМЕЖЕННЯ НА ЗМНИ:
                 * Можуть бути від'ємними:g, поля, швидкість, координати куль.
                 * Цілі величини: кількість опорних кадрів за секунду
                 * Дробові: усі інші.
             */
            TextBox tb = (TextBox)sender;
            try
            {
                switch (tb.Name)
                {
                    //Загальні
                    case "tb_Width":
                        element.Canvas.Width = getPositiveDouble(tb)*Data.dataScale;
                        rebuildMargins();
                        break;
                    case "tb_Height":
                        element.Canvas.Height = getPositiveDouble(tb) * Data.dataScale;
                        rebuildMargins();
                        break;
                    case "tb_FPS":
                        element.FPS = (int)getPositiveDouble(tb);
                        tb.Text = element.FPS.ToString();
                        break;
                    case "tb_G":
                        element.G = getDouble(tb)*Data.dataScale;
                        break;
                    case "tb_SpeedAnimation":
                        element.SpeedAnimation = getPositiveDouble(tb);
                        break;
                    case "tb_Fields":
                        element.Fields = getDouble(tb) * Data.dataScale;
                        fieldsBorder.Margin = new Thickness(-1*element.Fields);
                        rebuildMargins();
                        break;
                    case "tb_FontSize":
                        element.FontSize = getPositiveDouble(tb)*Data.dataScale;
                        break;

                    //Перша кулька
                    case "tb_Radius1":
                        element.FirstBall.Radius = getPositiveDouble(tb) * Data.dataScale;
                        element.FirstBall.Coord = new Point(element.FirstBall.Coord.X, element.GroundY - element.FirstBall.Radius);
                        element.updateInfo();
                        break;
                    case "tb_Weight1":
                        element.FirstBall.Mass = getPositiveDouble(tb);
                        element.updateInfo();
                        break;
                    case "tb_Speed1":
                        element.FirstBall.Speed = getDouble(tb) * Data.dataScale;
                        if (element.FirstBall.Speed < 0)
                        {
                            element.FirstBall.Speed *= -1;
                            element.FirstBall.SpeedAngle = Math.PI;
                        }
                        else
                            element.FirstBall.SpeedAngle = 0;
                        element.updateInfo();
                        break;
                    case "tb_X1":
                        element.FirstBall.Coord = new Point(getDouble(tb) * Data.dataScale, element.FirstBall.Coord.Y); break;

                    //Друга кулька
                    case "tb_Radius2":
                        element.SecondBall.Radius = getPositiveDouble(tb) * Data.dataScale;
                        element.SecondBall.Coord = new Point(element.SecondBall.Coord.X, element.GroundY - element.SecondBall.Radius);
                        element.updateInfo();
                        break;
                    case "tb_Weight2":
                        element.SecondBall.Mass = getPositiveDouble(tb);
                        element.updateInfo();
                        break;
                    case "tb_Speed2":
                        element.SecondBall.Speed = getDouble(tb) * Data.dataScale;
                        if (element.SecondBall.Speed < 0)
                        {
                            element.SecondBall.Speed *= -1;
                            element.SecondBall.SpeedAngle = Math.PI;
                        }
                        else
                            element.SecondBall.SpeedAngle = 0;
                        element.updateInfo();
                        break;
                    case "tb_X2": element.SecondBall.Coord = new Point(getDouble(tb) * Data.dataScale, element.SecondBall.Coord.Y); break;
                }
            }
            catch (Exception ex) {
               //відбувається як при введенні не коректних даних, так и при иніціалізації
            }
        }
        private void rect_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(cDialog.ShowDialog()==System.Windows.Forms.DialogResult.OK)
            {
                Rectangle r = (Rectangle)sender;
                r.Fill = new SolidColorBrush(Color.FromArgb(cDialog.Color.A, cDialog.Color.R, cDialog.Color.G, cDialog.Color.B));
                switch (r.Name)
                {
                    case "rect_Color1":
                        element.FirstBall.FillBrush = (SolidColorBrush)r.Fill;
                        break;
                    case "rect_Color2":
                        element.SecondBall.FillBrush = (SolidColorBrush)r.Fill;
                        break;
                    case "rect_ColorStroke1":
                        element.FirstBall.StrokeBrush = (SolidColorBrush)r.Fill;
                        break;
                    case "rect_ColorStroke2":
                        element.SecondBall.StrokeBrush = (SolidColorBrush)r.Fill;
                        break;
                }
            }
        }
        private void chb_isDataShow_Checked(object sender, RoutedEventArgs e)
        {
            element.InfoVisibility=true;
            tb_FontSize.IsEnabled = true;
        }
        private void chb_isDataShow_Unchecked(object sender, RoutedEventArgs e)
        {
            element.InfoVisibility=false;
            tb_FontSize.IsEnabled = false;
        }
        private void rebuildMargins()
        {
            fieldsBorder.Width = element.Canvas.Width + 2 * element.Fields;
            fieldsBorder.Height = element.Canvas.Height + 2 * element.Fields;
            fieldsBorder.BorderThickness = new Thickness(Math.Sqrt(element.Canvas.Width * element.Canvas.Height) / 100);
            ((Border)element.Canvas.Parent).BorderThickness = fieldsBorder.BorderThickness;
        }
        private double getPositiveDouble(TextBox tb)
        {
              double value = getDouble(tb);
                if (value < 0)
                {
                    value = 1;
                    tb.Text = (1/Data.dataScale).ToString();
                }
                return value;
        }
        private double getDouble(TextBox tb)
        {
            if (tb.Text.Contains('.'))
                tb.Text = tb.Text.Replace('.', ',');
            return double.Parse(tb.Text);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.CompareTo(Key.Escape) == 0 && e.IsDown && !e.IsRepeat)
                this.Hide();
            else
                if (e.Key.CompareTo(Key.Enter) == 0 && e.IsDown && !e.IsRepeat)
                    btn_Save_Click(sender, e);
            else
                if (e.Key.CompareTo(Key.F1) == 0 && e.IsDown)
                wExplanation.Instance.ShowDialog(wExplanation.ExplanationType.ElementCreater);
        }
        private void btn_Back_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }
        private void btn_Save_Click(object sender, RoutedEventArgs e)
        {
            if (element.G >= 0)
                MessageBox.Show("Прискорення вільного падіння має бути від'ємним.","Некоректні дані",MessageBoxButton.OK,MessageBoxImage.Information);
            else
            {
                double distance = Math.Sqrt(Math.Pow(element.FirstBall.Coord.X - element.SecondBall.Coord.X, 2) + Math.Pow(element.FirstBall.Coord.Y - element.SecondBall.Coord.Y, 2));
                if (distance <= element.FirstBall.Radius + element.SecondBall.Radius)
                    MessageBox.Show("Кулі не можуть перетинатись.", "Некоректні дані", MessageBoxButton.OK, MessageBoxImage.Information);
                else
                    if (!element.checkBorder())
                    MessageBox.Show("Кулі виходять за допустимі межі.", "Некоректні дані", MessageBoxButton.OK, MessageBoxImage.Information);
                else
                {
                    element.Canvas.Children.Remove(fieldsBorder);
                    element.Canvas.ClipToBounds = true;
                    if (elementIndex == -1)
                        Data.addElement(element);
                    else
                        Data.replaceElement(element, elementIndex);
                    this.Hide();
                }
            }
        }


        [System.Obsolete("Використовуйте ShowDialog(Element) метод!", true)]
        public new void Show() { }//заблоковано
        [System.Obsolete("Використовуйте ShowDialog(Element) метод!", true)]
        public new void ShowDialog() { }//заблоковано



        public class ElementCreater : Element
        {
            static ElementCreater()
            {
                Instance = new wElementCreater();
            }
            private static wElementCreater Instance { get; }

            public static void createElements(DataTable table)
            {
                int i=0;
                try
                {
                    for (; i < table.Rows.Count; i++)
                    {
                        //Загальні
                        Element element = getNewElement();
                        element.Canvas.Width = double.Parse(table.Rows[i].ItemArray[0].ToString())*Data.dataScale;
                        element.Canvas.Height = double.Parse(table.Rows[i].ItemArray[1].ToString()) * Data.dataScale;
                        element.FPS = int.Parse(table.Rows[i].ItemArray[2].ToString());
                        element.SpeedAnimation = double.Parse(table.Rows[i].ItemArray[3].ToString());
                        element.Fields = double.Parse(table.Rows[i].ItemArray[4].ToString()) * Data.dataScale;
                        element.FontSize = double.Parse(table.Rows[i].ItemArray[5].ToString())*Data.dataScale;
                        element.InfoVisibility = bool.Parse(table.Rows[i].ItemArray[6].ToString());
                        element.G = double.Parse(table.Rows[i].ItemArray[7].ToString()) * Data.dataScale;

                        //Перша куля
                        element.FirstBall.Radius = double.Parse(table.Rows[i].ItemArray[8].ToString()) * Data.dataScale;
                        element.FirstBall.Mass = double.Parse(table.Rows[i].ItemArray[9].ToString());
                        string[] temp = table.Rows[i].ItemArray[10].ToString().Split(Data.splitSymbol);
                        element.FirstBall.FillBrush = new SolidColorBrush(Color.FromArgb(byte.Parse(temp[0]), byte.Parse(temp[1]), byte.Parse(temp[2]), byte.Parse(temp[3])));
                        temp = table.Rows[i].ItemArray[11].ToString().Split(Data.splitSymbol);
                        element.FirstBall.StrokeBrush = new SolidColorBrush(Color.FromArgb(byte.Parse(temp[0]), byte.Parse(temp[1]), byte.Parse(temp[2]), byte.Parse(temp[3])));
                        element.FirstBall.Coord = new Point(double.Parse(table.Rows[i].ItemArray[12].ToString()) * Data.dataScale, 0);
                        element.FirstBall.Speed = double.Parse(table.Rows[i].ItemArray[13].ToString()) * Data.dataScale;
                        if (element.FirstBall.Speed < 0)
                        {
                            element.FirstBall.Speed *= -1;
                            element.FirstBall.SpeedAngle = Math.PI;
                        }

                        //Друга куля
                        element.SecondBall.Radius = double.Parse(table.Rows[i].ItemArray[14].ToString()) * Data.dataScale;
                        element.SecondBall.Mass = double.Parse(table.Rows[i].ItemArray[15].ToString());
                        temp = table.Rows[i].ItemArray[16].ToString().Split(Data.splitSymbol);
                        element.SecondBall.FillBrush = new SolidColorBrush(Color.FromArgb(byte.Parse(temp[0]), byte.Parse(temp[1]), byte.Parse(temp[2]), byte.Parse(temp[3])));
                        temp = table.Rows[i].ItemArray[17].ToString().Split(Data.splitSymbol);
                        element.SecondBall.StrokeBrush = new SolidColorBrush(Color.FromArgb(byte.Parse(temp[0]), byte.Parse(temp[1]), byte.Parse(temp[2]), byte.Parse(temp[3])));
                        element.SecondBall.Coord = new Point(double.Parse(table.Rows[i].ItemArray[18].ToString()) * Data.dataScale, 0);
                        element.SecondBall.Speed = double.Parse(table.Rows[i].ItemArray[19].ToString()) * Data.dataScale;
                        if (element.SecondBall.Speed < 0)
                        {
                            element.SecondBall.Speed *= -1;
                            element.SecondBall.SpeedAngle = Math.PI;
                        }

                        element.positionBalls();
                        element.updateInfo();

                        Data.addElement(element);
                    }
                }catch
                {
                    MessageBox.Show("Не вірний формат елементу: "+i+". Дані не були коректно завантажені", "Помилка", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
            public static void ShowDialog()
            {
                if (Data.Elements.Count > 0)
                    Instance.ShowDialog(getNewElement(Data.Elements[0]));
                else
                    Instance.ShowDialog(getNewElement());
            }
            public static void ShowDialog(int elementIndex)
            {
                //передаємо копію для подальшої можливості відмінити зміни
                Instance.ShowDialog(getNewElement( Data.Elements[elementIndex]),elementIndex);
            }
            public static Element getDefault()
            {
               return getNewElement();
            }
            public static Element getCopy(Element e)
            {
                return getNewElement(e);
            }
        }
    }
}
