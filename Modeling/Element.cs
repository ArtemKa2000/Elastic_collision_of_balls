using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Media;
using System;

namespace Modeling
{
    public class Element 
    {
        protected Element(Element element)
        {
            //Спочатку ініціалізація об'єктів, потім змінних.
            Canvas = new Canvas();
            Canvas.Width = element.Canvas.Width;
            Canvas.Height = element.Canvas.Height;
            Canvas.Background = Data.skyColor;
            Canvas.ClipToBounds = true;
            Canvas.VerticalAlignment = VerticalAlignment.Center;
            Canvas.HorizontalAlignment = HorizontalAlignment.Center;
            Canvas.SizeChanged += SizeChanged;
            FirstBall = new Ball(this, element.FirstBall);
            SecondBall = new Ball(this, element.SecondBall);
            baseScaleTransform = element.getBaseScaleTransform().Clone();
            translateTransform = element.getTranslateTransform().Clone();
            setScaleTransform(element.getScaleTransform().Clone());//третій раз звертаємось до сеттера, аби застосувати перетворення.
            if (scaleTransform == null)
                setScaleTransform(new ScaleTransform(Data.MinScale, Data.MinScale));
            FPS = element.FPS;
            SpeedAnimation = element.SpeedAnimation;
            Fields = element.Fields;
            G = element.G;
            initializeUIElements();

            //тільки після initializeUIElements()!!!!!
            FontSize = element.FontSize;
            InfoVisibility = element.infoVisibility;
        }
        protected Element()
        {
            //дефолтне визначення елементу. Визначено тільки тут.
            Canvas = new Canvas();
            Canvas.Width = 20*Data.dataScale;
            Canvas.Height = 20 * Data.dataScale;
            Canvas.Background = Data.skyColor;
            Canvas.VerticalAlignment = VerticalAlignment.Center;
            Canvas.HorizontalAlignment = HorizontalAlignment.Center;
            Canvas.ClipToBounds = true;
            Canvas.SizeChanged += SizeChanged;
            FirstBall = new Ball(this, 2 * Data.dataScale, 5, new SolidColorBrush(Colors.Red),new SolidColorBrush(Colors.Black), 7 * Data.dataScale, 2 * Data.dataScale, 0);
            SecondBall = new Ball(this, 2 * Data.dataScale, 5, new SolidColorBrush(Colors.Green), new SolidColorBrush(Colors.Black), 13 * Data.dataScale, 0,0);
            baseScaleTransform = new ScaleTransform(1, 1);
            translateTransform = new TranslateTransform(0, 0);
            setScaleTransform(new ScaleTransform(1, 1));
            SpeedAnimation = 1;
            FPS = 25;
            Fields = 0;
            G = -9.80665*Data.dataScale;//px в секунду
            initializeUIElements();

            //тільки після initializeUIElements()!!!!!
            FontSize =Data.dataScale;
            InfoVisibility = false;
        }
        private void initializeUIElements()//обов'язково викликати в конструкторі!!!
        {
            ground = new StackPanel();
            ground.Orientation = Orientation.Vertical;
            ground.Background = Data.groundColor;
            groundLine = new Line();
            groundLine.X1 = 0;
            groundLine.X2 =ground.Width= Canvas.Width;
            groundLine.Y1 = groundLine.Y2 = Math.Sqrt(Canvas.Width * Canvas.Height) / 200;
            groundLine.Stroke = Data.grassColor;
            groundLine.StrokeThickness = Math.Sqrt(Canvas.Width*Canvas.Height)/100;
            ground.Children.Add(groundLine);

            Canvas.Children.Add(ground);
            Canvas.SetTop(ground, GroundY);
            Canvas.Children.Add(FirstBall.Ellipse);
            Canvas.Children.Add(SecondBall.Ellipse);
            positionBalls();

            infoPanel = new Grid();
            infoPanel.SizeChanged += SizeChanged;
            ground.Children.Add(infoPanel);

            GuidesAppearance = new Storyboard();

        }
        protected static Element getNewElement() { return new Element(); }
        protected static Element getNewElement(Element e) { return new Element(e); }
        private void SizeChanged(object sender, SizeChangedEventArgs e)
        {
            groundLine.X2 =ground.Width= e.NewSize.Width;
            groundLine.Y1 = groundLine.Y2 = Math.Sqrt(Canvas.Width * Canvas.Height) / 200;
            groundLine.StrokeThickness = Math.Sqrt(Canvas.Width * Canvas.Height) / 100;
            if(!InfoVisibility)
                infoPanel.Height = Canvas.Height / 10;
            GroundY = Canvas.Height - ground.ActualHeight;
        }

        public Canvas Canvas { get; private set; }
        private int fps;
        public int FPS
        {
            get { return fps; }
            set
            {
                fps = value;
                TimeInterval = (double)1 / fps;
            }
        }
        public double TimeInterval { get; set; }
        public double SpeedAnimation { get; set; }//регулює швидкість відображення анімації. 1- 1х; 2 - 2х...
        public double Fields { get; set; }
        private double fontSize;
        public double FontSize
        {
            get { return fontSize; }
            set
            {
                fontSize = value;
                updateInfo();
            }
        }
        private bool infoVisibility;
        public bool InfoVisibility
        {
            get { return infoVisibility; }
            set
            {
                infoVisibility = value;
                if (infoVisibility)
                {
                    infoPanel.IsEnabled = true;
                    infoPanel.Visibility = Visibility.Visible;
                    infoPanel.Height=double.NaN;
                    GroundY = Canvas.Height - ground.ActualHeight;
                }
                else
                {
                    infoPanel.Visibility = Visibility.Hidden;
                    infoPanel.IsEnabled = false;
                    infoPanel.Height = Canvas.Height / 10;
                    GroundY = Canvas.Height / 10 * 9;
                }
            }
        }
        private double groundY;
        public double GroundY
        {
            get { return groundY; }
            private set
            {
                groundY = value;
                Canvas.SetTop(ground, GroundY);
                FirstBall.Coord = new Point(FirstBall.Coord.X, GroundY - FirstBall.Radius);
                SecondBall.Coord = new Point(SecondBall.Coord.X, GroundY - SecondBall.Radius);
                positionBalls();
            }
        }//координата Y земли
        public double G { get; set; }
        public Ball FirstBall { get; private set; }
        public Ball SecondBall { get; private set; }
        public Storyboard GuidesAppearance { get; private set; }
        private Line groundLine { get; set; }
        private StackPanel ground { get; set; }
        private Grid infoPanel { get; set; }

        //трансформації
        private ScaleTransform baseScaleTransform;
        public ScaleTransform getBaseScaleTransform()
        {
            return baseScaleTransform;
        }
        public bool setBaseScaleTransform(ScaleTransform newScale)
        {
            baseScaleTransform = newScale;
            Canvas.LayoutTransform = baseScaleTransform;
            return true;
        }
        private ScaleTransform scaleTransform;
        public ScaleTransform getScaleTransform()
        {
            return scaleTransform;
        }
        public bool setScaleTransform(ScaleTransform newScale)
        {
            if (newScale.ScaleX >= Data.MinScale)
            {
                scaleTransform = newScale;
                Canvas.RenderTransform = getTransform();
                return true;
            }
            else
                return false;
        }
        private TranslateTransform translateTransform;
        public TranslateTransform getTranslateTransform()
        {
            return translateTransform;
        }
        public bool setTranslateTransform(TranslateTransform newTranslate)
        {
            Border parent =(Border)Canvas.Parent;
            if (newTranslate.X >= -Canvas.Width * scaleTransform.ScaleX*baseScaleTransform.ScaleX + Data.TranslateFields &&
                newTranslate.X <= parent.Width - Data.TranslateFields &&
                newTranslate.Y >= -Canvas.Height * scaleTransform.ScaleY * baseScaleTransform.ScaleY + Data.TranslateFields &&
                newTranslate.Y <= parent.Height - Data.TranslateFields)
            {
                translateTransform = newTranslate;
                Canvas.RenderTransform = getTransform();
                return true;
            }
            else
                return false;
        }



        public void positionBalls()
        {
            positionBalls(FirstBall);
            positionBalls(SecondBall);
        }
        public void positionBalls(Ball ball)
        {
            Canvas.SetLeft(ball.Ellipse, ball.Coord.X - ball.Radius);
            Canvas.SetTop(ball.Ellipse, ball.Coord.Y - ball.Radius);
        }
        public bool checkBorder() //вихід за межі єкрану.
        {
            //false - вийшла за межі хоча б одна куля
            //true - в допустимих межах обидві кулі
            bool result = true;
            if (FirstBall.Coord.X - FirstBall.Radius < -Fields ||
                FirstBall.Coord.X + FirstBall.Radius > Canvas.Width + Fields ||
                FirstBall.Coord.Y - FirstBall.Radius < -Fields ||
                FirstBall.Coord.Y + FirstBall.Radius > Canvas.Height + Fields)
                result = false;
            else
                if (SecondBall.Coord.X - SecondBall.Radius < -Fields ||
                    SecondBall.Coord.X + SecondBall.Radius > Canvas.Width + Fields ||
                    SecondBall.Coord.Y - SecondBall.Radius < -Fields ||
                    SecondBall.Coord.Y + SecondBall.Radius > Canvas.Height + Fields)
                    result = false;

            return result;
        }
        public void setTransparentStyle()
        {
            FirstBall.FillBrush.Opacity = 0;
            SecondBall.FillBrush.Opacity = 0;
            Canvas.Background = new SolidColorBrush(Colors.Transparent);
            groundLine.Stroke = new SolidColorBrush(Colors.Black);
            ground.Background= new SolidColorBrush(Colors.Transparent);
            double maxStroke = FirstBall.Ellipse.StrokeThickness;
            if (SecondBall.Ellipse.StrokeThickness > maxStroke)
                maxStroke = SecondBall.Ellipse.StrokeThickness;
            FirstBall.Ellipse.StrokeThickness = maxStroke;
            SecondBall.Ellipse.StrokeThickness = maxStroke;
        }
        public void updateInfo()
        {
            infoPanel.Children.Clear();

            StackPanel ballPanel = new StackPanel();
            ballPanel.Orientation = Orientation.Vertical;
            ballPanel.Margin = new Thickness(fontSize/2);
            ballPanel.HorizontalAlignment = HorizontalAlignment.Left;
            StackPanel temp = new StackPanel();
            temp.Orientation = Orientation.Horizontal;
            TextBlock tb = getTextBlock();
            tb.Text = "Куля: ";
            temp.Children.Add(tb);
            Ellipse el = new Ellipse();
            el.Width = fontSize;
            el.Height = fontSize;
            el.Stroke = FirstBall.StrokeBrush;
            el.StrokeThickness = fontSize / 20;
            el.Fill = FirstBall.FillBrush;
            temp.Children.Add(el);
            ballPanel.Children.Add(temp);

            tb = getTextBlock();
            tb.Text = "Радіус: " + FirstBall.Radius / Data.dataScale+" м";
            ballPanel.Children.Add(tb);
            tb = getTextBlock();
            tb.Text = "Маса: " + FirstBall.Mass+" кг";
            ballPanel.Children.Add(tb);
            tb = getTextBlock();
            if (FirstBall.SpeedAngle == 0)
                tb.Text = "Швидкість: " + FirstBall.Speed / Data.dataScale + " м/с";
            else
                tb.Text = "Швидкість: " + FirstBall.Speed / Data.dataScale * -1 + " м/с";
            ballPanel.Children.Add(tb);
            infoPanel.Children.Add(ballPanel);

            ballPanel = new StackPanel();
            ballPanel.Orientation = Orientation.Vertical;
            ballPanel.Margin = new Thickness(fontSize/2);
            ballPanel.HorizontalAlignment = HorizontalAlignment.Right;
            temp = new StackPanel();
            temp.Orientation = Orientation.Horizontal;
            tb = getTextBlock();
            tb.Text = "Куля: ";
            temp.Children.Add(tb);
            el = new Ellipse();
            el.Width = fontSize;
            el.Height = fontSize;
            el.Stroke = SecondBall.StrokeBrush;
            el.Fill = SecondBall.FillBrush;
            el.StrokeThickness = fontSize / 20;
            temp.Children.Add(el);
            ballPanel.Children.Add(temp);

            tb = getTextBlock();
            tb.Text = "Радіус: " + SecondBall.Radius / Data.dataScale + " м";
            ballPanel.Children.Add(tb);
            tb = getTextBlock();
            tb.Text = "Маса: " + SecondBall.Mass+" кг";
            ballPanel.Children.Add(tb);
            tb = getTextBlock();
            if (SecondBall.SpeedAngle == 0)
                tb.Text = "Швидкість: " + SecondBall.Speed / Data.dataScale + " м/с";
            else
                tb.Text = "Швидкість: " + SecondBall.Speed / Data.dataScale * -1 + " м/с";
            ballPanel.Children.Add(tb);
            infoPanel.Children.Add(ballPanel);
        }
        private TextBlock getTextBlock()
        {
            TextBlock tb = new TextBlock();
            tb.FontSize = fontSize;
            tb.Foreground = Data.promptsForeground;
            tb.FontFamily = new FontFamily("Arial");
            return tb;
        }

        private TransformGroup getTransform()
        {
            TransformGroup tg = new TransformGroup();
            tg.Children.Add(scaleTransform);
            tg.Children.Add(translateTransform);
            return tg;
        }
    }
}