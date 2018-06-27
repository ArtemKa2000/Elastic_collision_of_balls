using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace Modeling
{
    using static Math;
    public partial class WTests : Window
    {
        public static WTests Instance { get; }
        public WTests()
        {
            InitializeComponent();
            isDataFormed = false;
            grid_First.Visibility = Visibility.Visible;
            grid_Second.Visibility = Visibility.Hidden;
            grid_Third.Visibility = Visibility.Hidden;
            r = new Random();
            firstTask =new RadioButton[4]{ rb1,rb2,rb3,rb4};
            secondTask = new RadioButton[4] { rb5, rb6, rb7, rb8 };
            cGroup = new RadioButton[4] { rb_c1, rb_c2, rb_c3, rb_c4 };
            dGroup = new RadioButton[4] { rb_d1, rb_d2, rb_d3, rb_d4 };
            eGroup = new RadioButton[4] { rb_e1, rb_e2, rb_e3, rb_e4 };
            fGroup = new RadioButton[4] { rb_f1, rb_f2, rb_f3, rb_f4 };
            gGroup = new RadioButton[4] { rb_g1, rb_g2, rb_g3, rb_g4 };
            hGroup = new RadioButton[4] { rb_h1, rb_h2, rb_h3, rb_h4 };

            isRight = new DropShadowEffect();
            isRight.BlurRadius = 20;
            isRight.Direction = 0;
            isRight.Color = Colors.SpringGreen;
            isRight.ShadowDepth = 0;
            isWrong = new DropShadowEffect();
            isWrong.BlurRadius = 30;
            isWrong.Direction = 0;
            isWrong.Color = Colors.Red;
            isRight.ShadowDepth = 0;
            loadElement();
            createData();
            createData2();
            createData3();
            isDataFormed = true;
        }
        static WTests()
        {
            Instance = new WTests();
        }

        private Random r;
        private bool isDataFormed;
        private Element element;
        private RadioButton[] firstTask;
        private RadioButton[] secondTask;
        private DropShadowEffect isRight;
        private DropShadowEffect isWrong;

        private RadioButton[] cGroup;
        private RadioButton[] dGroup;
        private RadioButton[] eGroup;
        private RadioButton[] fGroup;
        private RadioButton[] gGroup;
        private RadioButton[] hGroup;

        public new void Show()
        {
            base.Show();
            grid_Main.UpdateLayout();
            sortFirstPageButtons();
        }


        //THIRD PAGE
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Data.applicationClosing();
        }
        private void im_Back_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Hide();
            clearGroupEffect(firstTask);
            clearGroupEffect(secondTask);
            clearGroupEffect(cGroup);
            clearGroupEffect(dGroup);
            clearGroupEffect(eGroup);
            clearGroupEffect(fGroup);
            clearGroupEffect(gGroup);
            clearGroupEffect(hGroup);

            clearSelection(firstTask);
            clearSelection(secondTask);
            clearSelection(cGroup);
            clearSelection(dGroup);
            clearSelection(eGroup);
            clearSelection(fGroup);
            clearSelection(gGroup);
            clearSelection(hGroup);

            Data.getWindow(true).Show();
        }
        private void btn_Check_Click(object sender, RoutedEventArgs e)
        {
            clearGroupEffect(firstTask);
            clearGroupEffect(secondTask);
            int balls = 0;
            if(checkGroup(firstTask))
                balls++;
            if(checkGroup(secondTask))
                balls++;
            tb_Ball3.Text = balls.ToString();
        }
        private void btn_Show_Click(object sender, RoutedEventArgs e)
        {
            firstTask[0].Effect = isRight;
            secondTask[0].Effect = isRight;
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                TextBox tb = (TextBox)sender;
                switch (tb.Name)
                {
                    case "tb_R1":
                        element.FirstBall.Radius = getPositiveDouble(tb)*Data.dataScale;
                        break;
                    case "tb_S1":
                        element.FirstBall.Speed = getDouble(tb) * Data.dataScale;
                        if (element.FirstBall.Speed <= element.SecondBall.Speed*Cos(element.SecondBall.SpeedAngle) && isDataFormed)
                        {
                            MessageBox.Show("Некоректні дані. Зіткнення не відбудеться.", "Некоректні дані", MessageBoxButton.OK, MessageBoxImage.Information);
                            createData();
                            break;
                        }
                        if (element.FirstBall.Speed < 0)
                        {
                            element.FirstBall.Speed *= -1;
                            element.FirstBall.SpeedAngle = PI;
                        }
                        else
                            element.FirstBall.SpeedAngle = 0;
                        break;
                    case "tb_M1": element.FirstBall.Mass = getPositiveInt(tb); break;

                    case "tb_R2":
                        element.SecondBall.Radius = getPositiveDouble(tb) * Data.dataScale;
                        break;
                    case "tb_S2":
                        element.SecondBall.Speed = getDouble(tb) * Data.dataScale;
                        if (element.SecondBall.Speed >= element.FirstBall.Speed * Cos(element.FirstBall.SpeedAngle) && isDataFormed)
                        {
                            MessageBox.Show("Некоректні дані. Зіткнення не відбудеться.", "Некоректні дані", MessageBoxButton.OK, MessageBoxImage.Information);
                            createData();
                            break;
                        }
                        if (element.SecondBall.Speed < 0)
                        {
                            element.SecondBall.Speed *= -1;
                            element.SecondBall.SpeedAngle = PI;
                        }
                        else
                            element.SecondBall.SpeedAngle = 0;

                        break;
                    case "tb_M2": element.SecondBall.Mass = getPositiveInt(tb); break;
                }
                createAnswers();
            }
            catch { }
           
        }
        private void btn_Generate_Click(object sender, RoutedEventArgs e)
        {
            createData();
        }
        private void grid_Main_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            updateView();
        }
        private void btn_Back2_Click(object sender, RoutedEventArgs e)
        {
            grid_Third.Visibility = Visibility.Hidden;
            grid_Second.Visibility = Visibility.Visible;
        }

        private void createData()
        {
            isDataFormed = false;
            double temp = 0;
            tb_R1.Text = Round(r.Next((int)Data.dataScale, 5*(int)Data.dataScale) / Data.dataScale,2).ToString();
            tb_M1.Text = r.Next(1, 51).ToString();
            tb_S1.Text = (r.NextDouble() >= 0.5) ? Round(r.Next(1,5 * (int)Data.dataScale) / Data.dataScale,2).ToString() : Round(r.Next(1, 5 * (int)Data.dataScale)  / Data.dataScale,2).ToString();

            tb_R2.Text = Round(r.Next((int)Data.dataScale, 5 * (int)Data.dataScale) / Data.dataScale,2).ToString();
            tb_M2.Text = r.Next(1, 51).ToString();
            temp = double.MaxValue-2;
            while(temp+2 >= element.FirstBall.Speed/Data.dataScale*Cos(element.FirstBall.SpeedAngle))//гарантія зіткнення
                temp = (r.NextDouble() >= 0.5) ? Round(r.Next(5 * (int)Data.dataScale) / Data.dataScale, 2): Round(r.Next(5 * (int)Data.dataScale) / Data.dataScale,2) * -1;
            tb_S2.Text = temp.ToString();
            isDataFormed = true;
            createAnswers();
        }
        private void createAnswers()
        {
            tb_Ball3.Text = "0";
            clearGroupEffect(firstTask);
            clearGroupEffect(secondTask);
            clearSelection(firstTask);
            clearSelection(secondTask);

            alignCenter();
            double[] dat = new double[2];
            if (!AnimationBuilder.modelingFirstCollision(element) && isDataFormed)
            {
                MessageBox.Show("Некоректні дані. Зіткнення не відбудеться.", "Некоректні дані", MessageBoxButton.OK, MessageBoxImage.Information);
                createData();
                return;
            }
            dat[0] = Round(element.FirstBall.Speed/Data.dataScale * element.FirstBall.Mass,2);
            dat[1] = Round(element.SecondBall.Speed / Data.dataScale * element.SecondBall.Mass,2);


            double t1=0;
            List<double> used = new List<double>();
            firstTask[0].Content = (dat[0]).ToString();
            used.Add(dat[0]);
            for(int i = 1; i <firstTask.Length; i++)
            {
                t1 = used[0];
                while(used.Contains(t1))
                    t1 = (r.NextDouble() >= 0.5) ?  Round(dat[0]+ r.Next((int)Data.dataScale, 5*(int)Data.dataScale)/Data.dataScale,2) :  Round(Abs(dat[0]- r.Next((int)Data.dataScale, 5 * (int)Data.dataScale) / Data.dataScale),2);

                firstTask[i].Content = t1.ToString();
                used.Add(t1);
            }
            sortGroup(firstTask);
            used.Clear();

            secondTask[0].Content = (dat[1]).ToString();
            used.Add(dat[1]);
            for (int i = 1; i < firstTask.Length; i++)
            {
                t1 = used[0];
                while (used.Contains(t1))
                    t1 = (r.NextDouble() >= 0.5) ? Round(dat[1] + r.Next((int)Data.dataScale, 5 * (int)Data.dataScale) / Data.dataScale, 2) : Round(Abs(dat[1] - r.Next((int)Data.dataScale, 5 * (int)Data.dataScale) / Data.dataScale), 2);

                secondTask[i].Content = t1.ToString();
                used.Add(t1);
            }
            sortGroup(secondTask);

            loadElement();
            alignCollision();
        }
        private void loadElement()
        {
            try
            {
                element = wElementCreater.ElementCreater.getDefault();
                element.FPS = 35;

                element.FirstBall.Radius = getPositiveDouble(tb_R1)*Data.dataScale;
                element.FirstBall.Mass = getPositiveInt(tb_M1);
                element.FirstBall.Speed = getDouble(tb_S1)* Data.dataScale;
                if (element.FirstBall.Speed < 0)
                {
                    element.FirstBall.Speed *= -1;
                    element.FirstBall.SpeedAngle = PI;
                }
                else
                    element.FirstBall.SpeedAngle = 0;
                element.SecondBall.Radius = getPositiveDouble(tb_R2)* Data.dataScale;
                element.SecondBall.Mass = getPositiveInt(tb_M2);
                element.SecondBall.Speed = getDouble(tb_S2) * Data.dataScale;
                if (element.SecondBall.Speed < 0)
                {
                    element.SecondBall.Speed *= -1;
                    element.SecondBall.SpeedAngle = PI;
                }
                else
                    element.SecondBall.SpeedAngle = 0;

                element.setTransparentStyle();
                brd_View.Child = element.Canvas;
                alignCenter();
                updateView();
            }
            catch { }
        }
        private void updateView()
        {
            try
            {
                double wScale = (grid_Main.ColumnDefinitions[1].ActualWidth - 30) / element.Canvas.Width;
                double hScale = (grid_Main.RowDefinitions[1].ActualHeight - 30) / element.Canvas.Height;
                if (wScale < hScale)//масштабуємо згідно з мінімальним відношенням
                {
                    element.setBaseScaleTransform(new ScaleTransform(wScale, wScale));
                    brd_View.Height = element.Canvas.Height * wScale;
                    brd_View.Width = element.Canvas.Width * wScale;
                }
                else
                {
                    element.setBaseScaleTransform(new ScaleTransform(hScale, hScale));
                    brd_View.Height = element.Canvas.Height * hScale;
                    brd_View.Width = element.Canvas.Width * hScale;
                }
            }
            catch { }
        }
        private void alignCollision()
        {
            alignCenter();
            while(Sqrt(Pow(element.FirstBall.Coord.X - element.SecondBall.Coord.X, 2) + Pow(element.FirstBall.Coord.Y - element.SecondBall.Coord.Y, 2)) > element.FirstBall.Radius + element.SecondBall.Radius)
                element.FirstBall.Coord = new Point(element.FirstBall.Coord.X + 2, element.FirstBall.Coord.Y);
        }
        private void alignCenter()
        {
            element.FirstBall.Coord = new Point(element.Canvas.Width / 2 - element.FirstBall.Radius, element.GroundY - element.FirstBall.Radius);
            element.SecondBall.Coord = new Point(element.Canvas.Width / 2 + element.SecondBall.Radius, element.GroundY - element.SecondBall.Radius);
        }


        //FIRST PAGE
        private void btn_Check1_Click(object sender, RoutedEventArgs e)
        {
            clearGroupEffect(cGroup);
            clearGroupEffect(dGroup);
            clearGroupEffect(eGroup);
            clearGroupEffect(fGroup);
            int balls = 0;
            if(checkGroup(cGroup))
                balls++;
            if (checkGroup(dGroup))
                balls++;
            if (checkGroup(eGroup))
                balls++;
            if (checkGroup(fGroup))
                balls++;
            tb_Ball1.Text = balls.ToString();
        }
        private void btn_Show1_Click(object sender, RoutedEventArgs e)
        {
            cGroup[0].Effect = isRight;
            dGroup[0].Effect = isRight;
            eGroup[0].Effect = isRight;
            fGroup[0].Effect = isRight;
        }
        private void btn_Next1_Click(object sender, RoutedEventArgs e)
        {
            grid_First.Visibility = Visibility.Hidden;
            grid_Second.Visibility = Visibility.Visible;
        }
        private void btn_Update1_Click(object sender, RoutedEventArgs e)
        {
            clearGroupEffect(cGroup);
            clearGroupEffect(dGroup);
            clearGroupEffect(eGroup);
            clearGroupEffect(fGroup);
            clearSelection(cGroup);
            clearSelection(dGroup);
            clearSelection(eGroup);
            clearSelection(fGroup);
            tb_Ball1.Text = "0";
        }

        //SECOND PAGE
        private void tb_Task1_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                TextBox tb = (TextBox)sender;
                int temp;
                switch (tb.Name)
                {
                    case "tb_Mass1":
                        getPositiveInt(tb);
                        break;
                    case "tb_Speed1":
                        temp = getInt(tb);
                        if (temp <= getInt(tb_Speed2) && isDataFormed)
                        {
                            MessageBox.Show("Некоректні дані. Зіткнення не відбудеться.", "Некоректні дані", MessageBoxButton.OK, MessageBoxImage.Information);
                            createData2();
                        }
                        break;
                    case "tb_Mass2": getPositiveInt(tb); break;

                    case "tb_Speed2":
                        temp = getInt(tb);
                        if (temp >= getInt(tb_Speed1) && isDataFormed)
                        {
                            MessageBox.Show("Некоректні дані. Зіткнення не відбудеться.", "Некоректні дані", MessageBoxButton.OK, MessageBoxImage.Information);
                            createData2();
                        }
                        break;
                }
                createAnswers2();
            }
            catch { }

        }
        private void tb_Task2_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                TextBox tb = (TextBox)sender;
                int temp;
                switch (tb.Name)
                {
                    case "tb_P1":
                    case "tb_P2": getPositiveInt(tb); break;
                    case "tb_L1":
                    case "tb_L2":
                        temp = getPositiveInt(tb);
                        if (temp > 360 || temp < -360)
                            tb.Text = (temp % 360).ToString();
                        break;
                }
                createAnswers3();
            }
            catch { }
        }
        private void btn_Back1_Click(object sender, RoutedEventArgs e)
        {
            grid_Second.Visibility = Visibility.Hidden;
            grid_First.Visibility = Visibility.Visible;
        }
        private void btn_Check2_Click(object sender, RoutedEventArgs e)
        {
            clearGroupEffect(gGroup);
            clearGroupEffect(hGroup);
            int balls = 0;

            if (checkGroup(gGroup))
            {
                balls++;
                isFirstCounted = true;
            }
            if (checkGroup(hGroup))
            {
                balls++;
                isSecondCounted = true;
            }
            tb_Ball2.Text = balls.ToString();
        }
        private void btn_Show2_Click(object sender, RoutedEventArgs e)
        {
            rb_g1.Effect = isRight;
            rb_h1.Effect = isRight;
        }
        private void btn_Next2_Click(object sender, RoutedEventArgs e)
        {
            grid_Second.Visibility = Visibility.Hidden;
            grid_Third.Visibility = Visibility.Visible;
        }
        private void btn_Generate2_Click(object sender, RoutedEventArgs e)
        {
            createData2();
            createData3();
        }

        private bool isFirstCounted;
        private bool isSecondCounted;
        private void createData2()
        {
            isDataFormed = false;
            int temp = 0;
            tb_Mass1.Text = r.Next(61).ToString();
            tb_Speed1.Text= (r.NextDouble() >= 0.5) ? r.Next(41).ToString() : (r.Next(41) * -1).ToString();

            tb_Mass2.Text = r.Next(61).ToString();
            temp= int.MaxValue;
            while (temp >= getInt(tb_Speed1))//гарантія зіткнення
                temp = (r.NextDouble() >= 0.5) ? r.Next(61) : (r.Next(61) * -1);
            tb_Speed2.Text = temp.ToString();
            isDataFormed = true;
            createAnswers2();
        }
        private void createAnswers2()
        {
            if(isFirstCounted)
                tb_Ball2.Text = (int.Parse(tb_Ball2.Text)-1).ToString();
            isFirstCounted = false;
            clearGroupEffect(gGroup);
            clearSelection(gGroup);

            double m1 = getInt(tb_Mass1);
            double m2 = getInt(tb_Mass2);
            int speed = getInt(tb_Speed1) - getInt(tb_Speed2);//швидкість першої вагонетки відносно другої.

            rb_g1.Content = Abs((int)(((m1-m2)/(m1+m2)*speed + getInt(tb_Speed2)) * m1)) + " і " + Abs((int)((2*m1/(m1+m2)*speed + getInt(tb_Speed2)) * m2));
            
            rb_g2.Content = Abs(m1*getInt(tb_Speed1)) + " i " + Abs(m2*getInt(tb_Speed2));
            int rand = 0;
            while (rand ==0)
                rand = r.Next(5);
            rb_g3.Content= Abs((int)(((m1 - m2) / (m1 + m2) * speed + getInt(tb_Speed2)) * m1)) + rand + " і " + Abs((int)((2 * m1 / (m1 + m2) * speed + getInt(tb_Speed2)) * m2));
            rb_g4.Content= Abs((int)(((m1 - m2) / (m1 + m2) * speed + getInt(tb_Speed2)) * m1)) + " і " + Abs((int)((2 * m1 / (m1 + m2) * speed + getInt(tb_Speed2)) * m2)) + rand;
            sortGroup(gGroup);

        }
        private void createData3()
        {
            isDataFormed = false;
            tb_P1.Text = r.Next(61).ToString();
            tb_P2.Text = r.Next(61).ToString();
            tb_L1.Text = r.Next(361).ToString();
            tb_L2.Text = (Abs(r.Next(721)-360)).ToString();
            isDataFormed = true;
            createAnswers3();
        }
        private void createAnswers3()
        {
            if (isSecondCounted)
                tb_Ball2.Text = (int.Parse(tb_Ball2.Text) - 1).ToString();
            isSecondCounted = false;
            clearGroupEffect(hGroup);
            clearSelection(hGroup);

            double x, y;
            x = getInt(tb_P1) * Cos(getInt(tb_L1) * PI / 180);
            x+= getInt(tb_P2) * Cos(getInt(tb_L2) * PI / 180);
            y= getInt(tb_P1) * Sin(getInt(tb_L1) * PI / 180);
            y+= getInt(tb_P2) * Sin(getInt(tb_L2) * PI / 180);
            int result = (int)Sqrt(Pow(x, 2) + Pow(y, 2));
            hGroup[0].Content = result;
            List<int> used = new List<int>();
            used.Add(result);
            for(int i = 1; i < hGroup.Length; i++)
            {
                x = used[i - 1];
                while (used.Contains((int)x))
                    x = result + r.Next(21);
                hGroup[i].Content = (int)x;
                used.Add((int)x);
            }
            sortGroup(hGroup);
        }



        private void sortFirstPageButtons()
        {
            sortGroup(cGroup);
            sortGroup(dGroup);
            sortGroup(eGroup);
            sortGroup(fGroup);
        }
        private void sortGroup(UIElement[] elements)
        {
            List<UIElement> els = new List<UIElement>(elements);
            int temp = 0;
            for (int i = 0; i < elements.Length; i++)
            {
                temp = r.Next(els.Count);
                Grid.SetColumn(els[temp], i);
                els.RemoveAt(temp);
            }
        }
        private bool checkGroup(RadioButton[] buttons)
        {
            for (int i = 0; i < buttons.Length; i++)
                if (buttons[i].IsChecked.Value)
                {
                    if (i == 0)
                    {
                        buttons[i].Effect = isRight;
                        return true;
                    }
                    else
                        buttons[i].Effect = isWrong;
                    break;
                }
            return false;
        }
        private void clearGroupEffect(UIElement[] elements)
        {
            for (int i = 0; i < elements.Length; i++)
                elements[i].Effect = null;
        }
        private void clearSelection(RadioButton[] buttons)
        {
            for (int i = 0; i < buttons.Length; i++)
                buttons[i].IsChecked = false;
        }
        private double getPositiveDouble(TextBox tb)
        {
            double value = getDouble(tb);
            if(value<0)
            {
                value = Round(1 / Data.dataScale,2);
                tb.Text = value.ToString();
            }
            return value;
        }
        private double getDouble(TextBox tb)
        {
            return Round(double.Parse(tb.Text),2);
        }
        private int getPositiveInt(TextBox tb)
        {
            int value = getInt(tb);
            if (value < 0)
            {
                value = 1;
                tb.Text = value.ToString();
            }
            return value;
        }
        private int getInt(TextBox tb)
        {
            return int.Parse(tb.Text);
        }
    }
}
