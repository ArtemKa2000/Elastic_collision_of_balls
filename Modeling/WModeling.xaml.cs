using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Modeling
{
    public partial class WModeling : Window
    {
        static WModeling()
        {
            Instance = new WModeling();
        }
        private WModeling()
        {
            InitializeComponent();
            elements = new List<Element>();
            storyboard = new Storyboard();
            storyboard.CurrentTimeInvalidated += storyboard_CurrentTimeInvalidated;
            storyboard.Completed += storyboard_Completed;
            slider_SpeedRatio.ValueChanged += slider_SpeedRatio_ValueChanged;//задавати треба не у XAML через порядок ініціалізації
        }

        public static WModeling Instance { get; }
        private Storyboard storyboard;
        private readonly List<Element> elements;
        private List<int> indexes;
        private bool isMouseDown;
        private Point oldPoint;
        private double firstCollisionTime;
        private bool isCollised;
        private double oldSliderValue;

        public void Open(List<int> indexes)
        {
            elements.Clear();
            //розмежовуємо грід
            grid_Main.ColumnDefinitions.Add(new ColumnDefinition());
            grid_Main.RowDefinitions.Add(new RowDefinition());
            if (indexes.Count > 1)
            {
                grid_Main.ColumnDefinitions.Add(new ColumnDefinition());
                if (indexes.Count > 2)
                {
                    grid_Main.RowDefinitions.Add(new RowDefinition());
                    if (indexes.Count > 4)
                        grid_Main.ColumnDefinitions.Add(new ColumnDefinition());
                }
            }

            this.indexes = new List<int>(indexes);

            //додаємо елементи
            foreach (int i in indexes)
            {
                Element clone = wElementCreater.ElementCreater.getCopy(Data.Elements[i]);
                elements.Add(clone);

                Border brdr = new Border();
                brdr.Margin = new Thickness(10);
                brdr.BorderThickness = new Thickness(5);
                brdr.BorderBrush = new SolidColorBrush(Colors.Black);
                brdr.HorizontalAlignment = HorizontalAlignment.Center;
                brdr.VerticalAlignment = VerticalAlignment.Center;
                Border inner = new Border();
                inner.ClipToBounds = true;
                inner.MouseWheel += border_MouseWheel;
                inner.MouseMove += border_MouseMove;
                inner.MouseDown += border_MouseDown;
                inner.MouseUp += border_MouseUp;
                inner.MouseLeave += border_MouseLeave;
                inner.MouseLeftButtonDown += border_DoubleClick;
                brdr.Child = inner;
                inner.Child = clone.Canvas;
                grid_Main.Children.Add(brdr);
                Grid.SetColumn(brdr, (elements.Count-1) % grid_Main.ColumnDefinitions.Count);
                Grid.SetRow(brdr, (elements.Count - 1) / grid_Main.ColumnDefinitions.Count);
            }

            //відображуємо та вирівнюємо макет вікна
            base.Show();
            grid_Main.UpdateLayout();
            grid_Container.UpdateLayout();
            //масштабуємо елементи
            refresh();
        }

        private void im_Settings_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(wSettings.Instance.ShowDialog())
            {
                reloadWithoutScale();
                refresh();
            }
                
        }
        private void btn_Start_Pause_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (btn_Start_Pause.Content.ToString().CompareTo("Старт") == 0)//зміна функціоналу кнопки
                {
                    try
                    {
                        if (storyboard.GetCurrentState(this) == ClockState.Stopped)
                            throw new Exception();
                        else
                            resumeStoryboard();
                    }
                    catch
                    {
                        startStoryboard();
                    }
                }
                else
                {   //зміна функціоналу кнопки
                    pauseStoryboard();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void btn_ToStart_Click(object sender, RoutedEventArgs e)
        {
            seek(100);

            if (oldSliderValue != 0)
                slider_SpeedRatio.Value = oldSliderValue;
            isCollised = false;
            oldSliderValue = 0;
        }
        private void btn_Stop_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                stopStoryboard();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void btn_ToEnd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!(storyboard.GetCurrentState(this) == ClockState.Stopped))
                    storyboard.SkipToFill(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void grid_Container_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            refresh();
        }
        private void slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (isMouseDown)
            {
                if (storyboard.GetCurrentState(this) == ClockState.Stopped)
                {
                    startStoryboard();
                    pauseStoryboard();
                }
                else
                    seek(slider.Value);
            }
        }
        private void slider_SpeedRatio_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            storyboard.SetSpeedRatio(this, e.NewValue);
            lb_SpeedRatio.Content = Math.Round(e.NewValue,1).ToString()+"x";
        }
        private void storyboard_CurrentTimeInvalidated(object sender, EventArgs e)
        {
            Clock cl = (Clock)sender;
            if (cl.CurrentProgress == null)
            {
                slider.Value = 0;
            }
            else
            {
                slider.Value =cl.CurrentTime.Value.Minutes*60000 + cl.CurrentTime.Value.Seconds*1000 + cl.CurrentTime.Value.Milliseconds;
            }
            if (cl.CurrentTime!=null && cl.CurrentTime.Value.Seconds+(double)cl.CurrentTime.Value.Milliseconds/1000 >= firstCollisionTime && !isCollised)
                collision();

        }
        private void storyboard_Completed(object sender, EventArgs eh)
        {
            if (storyboard.GetIsPaused(this))
                resumeStoryboard();
            stopStoryboard();
            for(int i = 0; i < elements.Count; i++)
                for(int j = 0; j < elements[i].Canvas.Children.Count; j++)
                    elements[i].Canvas.Children[j].Opacity = 1;
        }
        private void border_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                Border b = (Border)((Border)sender).Parent;
                int index = Grid.GetRow(b) * grid_Main.ColumnDefinitions.Count + Grid.GetColumn(b);
                wElementCreater.ElementCreater.ShowDialog(indexes[index]);
                reloadElements();
                refresh();
            }
        }

        private void seek(double time)
        {
            try
            {
                storyboard.Seek(this, TimeSpan.FromMilliseconds(time), TimeSeekOrigin.BeginTime);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void startStoryboard()
        {
            reloadElements();
            btn_Start_Pause.Content = "Пауза";
            btn_Start_Pause.ToolTip = "Поставити на паузу анімацію";
            btn_Stop.IsEnabled = true;
            btn_ToStart.IsEnabled = true;
            btn_ToEnd.IsEnabled = true;
            slider.IsEnabled = true;
            Storyboard sb = new Storyboard();
            firstCollisionTime = double.MaxValue;
            double sliderMax = -1;
            double temp=0;
            foreach (Element el in elements)
            {
                sb = new Storyboard();
                temp = AnimationBuilder.createAnimation(el, chb_Guides.IsChecked.Value)/el.SpeedAnimation;
                if (temp < firstCollisionTime)
                    firstCollisionTime = temp;
                sb.Children.Add(el.FirstBall.animationX);
                sb.Children.Add(el.FirstBall.animationY);
                sb.Children.Add(el.SecondBall.animationX);
                sb.Children.Add(el.SecondBall.animationY);
                sb.Children.Add(el.GuidesAppearance);
                sb.SpeedRatio = el.SpeedAnimation;
                storyboard.Children.Add(sb);
                //Можна брати час будь-якої анімації кулі, оскільки вони однакові
                temp = el.FirstBall.animationX.KeyFrames.Count - 1;
                temp = (el.FirstBall.animationX.KeyFrames[(int)temp].KeyTime.TimeSpan.Milliseconds + el.FirstBall.animationX.KeyFrames[(int)temp].KeyTime.TimeSpan.Seconds * 1000 + el.FirstBall.animationX.KeyFrames[(int)temp].KeyTime.TimeSpan.Minutes * 60000) / el.SpeedAnimation;
                if (temp > sliderMax)
                    sliderMax = temp;
            }
            storyboard.Begin(this, true);
            storyboard.SetSpeedRatio(this, slider_SpeedRatio.Value);
            slider.Maximum = sliderMax;//після попередніх двох строк!
            isCollised = false;
        }
        private void stopStoryboard()
        {
            pauseStoryboard();//для зміни написів на кнопці
            btn_Stop.IsEnabled = false;
            btn_ToStart.IsEnabled = false;
            btn_ToEnd.IsEnabled = false;
            storyboard.Stop(this);
            storyboard.Children.Clear();
            isCollised = false;
            if (oldSliderValue != 0)
            {
                slider_SpeedRatio.Value = oldSliderValue;
                oldSliderValue = 0;
            }
        }
        private void pauseStoryboard()
        {
            btn_Start_Pause.Content = "Старт";
            btn_Start_Pause.ToolTip = "Запустити анімацію";
            storyboard.Pause(this);
        }
        private void resumeStoryboard()
        {
            btn_Start_Pause.Content = "Пауза";
            btn_Start_Pause.ToolTip = "Поставити на паузу анімацію";
            storyboard.Resume(this);
        }

        private void reloadElements()
        {
            for (int i = 0; i < indexes.Count; i++)
            {
                Element clone = wElementCreater.ElementCreater.getCopy(Data.Elements[indexes[i]]);
                Border parent = (Border)elements[i].Canvas.Parent;
                parent.Child=clone.Canvas;
                clone.Canvas.UpdateLayout();
                //Тільки після того, як канвас став дочірнім бордер! Інакше невідомо відносно чого перенос.
                scaleElement(clone, i);//перед застосування трансформацій, через систему їх валідації
                clone.setScaleTransform(elements[i].getScaleTransform());
                clone.setTranslateTransform(elements[i].getTranslateTransform());
                elements[i] = clone;
            }
            stopStoryboard();
        }
        private void reloadWithoutScale()
        {
            for (int i = 0; i < indexes.Count; i++)
            {
                Element clone = wElementCreater.ElementCreater.getCopy(Data.Elements[indexes[i]]);
                Border parent = (Border)elements[i].Canvas.Parent;
                parent.Child = clone.Canvas;
                clone.Canvas.UpdateLayout();
                //Тільки після того, як канвас став дочірнім бордер! Інакше невідомо відносно чого перенос.
                scaleElement(clone, i);//перед застосування трансформацій, через систему їх валідації
                elements[i] = clone;
            }
            stopStoryboard();
        }
        private void refresh()
        {
            stopStoryboard();
            for (int i = 0; i < elements.Count; i++)
                scaleElement(elements[i],i);
            
        }
        private void scaleElement(Element element,int index)
        {
            //знаходимо відношення висот і широт
            double wScale = (grid_Main.ColumnDefinitions[index % grid_Main.ColumnDefinitions.Count].ActualWidth - 30) / element.Canvas.Width;//-30: -20 на Margin і -10 на BorderThickness
            double hScale = (grid_Main.RowDefinitions[index / grid_Main.ColumnDefinitions.Count].ActualHeight - 30) / element.Canvas.Height;
            if (wScale < hScale)//масштабуємо згідно з мінімальним відношенням
            {
                element.setBaseScaleTransform(new ScaleTransform(wScale, wScale));
                ((Border)element.Canvas.Parent).Height = element.Canvas.Height*wScale;
                ((Border)element.Canvas.Parent).Width = element.Canvas.Width*wScale;
            }
            else
            {
                element.setBaseScaleTransform(new ScaleTransform(hScale, hScale));
                ((Border)element.Canvas.Parent).Height = element.Canvas.Height * hScale;
                ((Border)element.Canvas.Parent).Width = element.Canvas.Width * hScale;
            }
            
        }
        private void collision()
        {
            isCollised = true;
            if (slider_SpeedRatio.Value > Data.AverageSpeedRatio)
            {
                oldSliderValue = slider_SpeedRatio.Value;
                slider_SpeedRatio.Value = Data.AverageSpeedRatio;
            }
        }


        //Трансформації
        private void border_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            Border brdr = (Border)sender;
            Canvas cnvs = (Canvas)brdr.Child;
            ScaleTransform scale = new ScaleTransform();
            TranslateTransform translate = new TranslateTransform();
            int index = Grid.GetRow((Border)brdr.Parent) * grid_Main.ColumnDefinitions.Count + Grid.GetColumn((Border)brdr.Parent);
            double delta = 0;
            if (e.Delta > 0)
            {
                scale.ScaleX = scale.ScaleY = elements[index].getScaleTransform().ScaleX * e.Delta*Data.ScrollStep / 120;
                delta = (e.MouseDevice.GetPosition(brdr).X - elements[index].getTranslateTransform().X) * e.Delta / 120 * Data.ScrollStep * -1 + e.MouseDevice.GetPosition(brdr).X;
                translate.X = delta;
                delta = (e.MouseDevice.GetPosition(brdr).Y - elements[index].getTranslateTransform().Y) * e.Delta / 120 * Data.ScrollStep * -1 + e.MouseDevice.GetPosition(brdr).Y;
                translate.Y = delta;
            }
            else
            {
                scale.ScaleX = scale.ScaleY = elements[index].getScaleTransform().ScaleX / (Math.Abs(e.Delta)*Data.ScrollStep) * 120;
                delta = (e.MouseDevice.GetPosition(brdr).X - elements[index].getTranslateTransform().X) / (Math.Abs(e.Delta) * Data.ScrollStep) * 120 * -1 + e.MouseDevice.GetPosition(brdr).X;
                translate.X = delta;
                delta = (e.MouseDevice.GetPosition(brdr).Y - elements[index].getTranslateTransform().Y) / (Math.Abs(e.Delta) * Data.ScrollStep) * 120 * -1 + e.MouseDevice.GetPosition(brdr).Y;
                translate.Y = delta;
            }
            if(elements[index].setScaleTransform(scale))
                elements[index].setTranslateTransform(translate);
            
        }
        private void border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton.CompareTo(MouseButtonState.Pressed) == 0)
            {
                isMouseDown = true;
                oldPoint = e.GetPosition((Border)sender);
            }
        }
        private void border_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMouseDown)
            {
                Border parent = (Border)((Border)sender).Parent;
                int index = Grid.GetRow(parent) * grid_Main.ColumnDefinitions.Count+ Grid.GetColumn(parent);
                TranslateTransform translate = new TranslateTransform();
                translate.X = elements[index].getTranslateTransform().X + e.GetPosition((Border)sender).X - oldPoint.X;
                translate.Y = elements[index].getTranslateTransform().Y + e.GetPosition((Border)sender).Y - oldPoint.Y;
                elements[index].setTranslateTransform(translate);
                oldPoint = e.GetPosition((Border)sender);
            }
        }
        private void border_MouseLeave(object sender, MouseEventArgs e)
        {
            isMouseDown = false;
        }
        private void border_MouseUp(object sender, MouseButtonEventArgs e)
        {
            isMouseDown = false;
        }

        //Службові
        private void im_Back_MouseDown(object sender, MouseButtonEventArgs e)
        {
            grid_Main.Children.Clear();
            grid_Main.ColumnDefinitions.Clear();
            grid_Main.RowDefinitions.Clear();
            this.Hide();
            Data.getWindow(true).Show();
        }
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.CompareTo(Key.F1) == 0 && e.IsDown)
                wExplanation.Instance.ShowDialog(wExplanation.ExplanationType.Modeling);
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Data.applicationClosing();
        }
        private void slider_MouseDown(object sender, MouseButtonEventArgs e)
        {
            isMouseDown = true;
        }
        private void slider_MouseUp(object sender, MouseButtonEventArgs e)
        {
            isMouseDown = false;
        }



        [System.Obsolete("Використовуйте Open(Element[]) метод!", true)]
        public new void Show() { }

    }
}