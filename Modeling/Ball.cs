using System.Windows;
using System.Windows.Shapes;
using System.Windows.Media;
using System;
using System.Windows.Media.Animation;
using System.Windows.Controls;

namespace Modeling
{
    using static Data;
    using static Math;
    public class Ball
    {
        private Point coord;
        public Point Coord
        {
            get { return coord; }
            set {
                coord = value;
                Element.positionBalls(this);
            }
        }//координата центру.
        private double radius;
        public double Radius {
            get { return radius; }
            set
            {
                radius = value;
                Ellipse.Width = radius * 2;
                Ellipse.Height = radius * 2;
                Ellipse.StrokeThickness = Ellipse.Width / 60;
            }
        }
        public double Speed { get;set;}//px/секунду
        public double SpeedAngle {get;set;}
        public double Mass{ get; set; }
        private SolidColorBrush fillBrush;
        public SolidColorBrush FillBrush {
            get { return fillBrush; }
            set
            {
                fillBrush = value;
                Ellipse.Fill = fillBrush;
            }
        }
        private SolidColorBrush strokeBrush;
        public SolidColorBrush StrokeBrush
        {
            get { return strokeBrush; }
            set
            {
                strokeBrush = value;
                Ellipse.Stroke = strokeBrush;
            }
        }
        public Ellipse Ellipse{ get; private set;}
        private Element Element { get; set; }
        public DoubleAnimationUsingKeyFrames animationX { get; private set; }
        public DoubleAnimationUsingKeyFrames animationY { get; private set; }

        public Ball(Element Element,double radius,double massa,SolidColorBrush fillBrush,SolidColorBrush strokeBrush, double x, double speed,double speedAngle)
        {
            this.Element = Element;
            Speed = speed;
            SpeedAngle = 0;
            Mass = massa;
            Ellipse = new Ellipse();
            Ellipse.Width = radius * 2;
            Ellipse.Height = radius * 2;
            FillBrush = fillBrush;//Піля ініціалізації Ellipse!!!
            StrokeBrush = strokeBrush;
            Radius = radius;
            Coord = new Point(x, 0);
            SpeedAngle = speedAngle;
            animationX = new DoubleAnimationUsingKeyFrames();
            animationY = new DoubleAnimationUsingKeyFrames();
            Storyboard.SetTarget(animationX, Ellipse);
            Storyboard.SetTarget(animationY, Ellipse);
            Storyboard.SetTargetProperty(animationX, new PropertyPath(Canvas.LeftProperty));
            Storyboard.SetTargetProperty(animationY, new PropertyPath(Canvas.TopProperty));
        }
        public Ball(Element element,Ball ball):this(element,ball.Radius,ball.Mass, new SolidColorBrush(ball.fillBrush.Color),new SolidColorBrush(ball.strokeBrush.Color), ball.Coord.X,ball.Speed,ball.SpeedAngle){   }
        public void moveForce(double deltaTime)//переміщення на наступну позицію
        {
            double x = Coord.X;
            double y = Coord.Y;

            //проекції швидкостей
            double speedY = Speed * Sin(SpeedAngle);
            double speedX = Speed * Cos(SpeedAngle);

            x += speedX * deltaTime;
            if (y + Radius < Element.GroundY || speedY > 0)//тільки якщо куля у польоті або швидкість направлена вгору
            {
                double D = speedY * speedY - 2 * Element.G * (Element.GroundY - y - Round(Radius,2));//завжди більше або дорівнює speedY
                double time = (-speedY - Sqrt(D)) / Element.G;//(-b-sqrt(D))/2a - час польоту до першого зіткнення з землею

                if (time >= deltaTime)//прості розрахунки польоту
                {
                    y -= speedY * deltaTime + Element.G * deltaTime * deltaTime / 2;//h=v0t+at^2/2
                    speedY += Element.G * deltaTime;
                }
                else
                {
                    speedY += Element.G * time;//максимальна можлива швидкість - уся єнергія перейшла у кінетичну.
                    speedY = Abs(speedY);//відбиття швидкості від землі. Швидкість гарантовано <0
                    deltaTime -= time;
                    time = 2 * -speedY / Element.G;//час, за який кулька описує дугу
                    if (time > MinimalCalculateTime)//якщо інтервал стрибку дуже малий, то наступні дії не мають сенсу і не можливі до виконання.
                    {
                        while (deltaTime > time)//знаходимо залишок часу після виконання усіх "стрибків"
                            deltaTime -= time;

                        y = (Element.GroundY - Radius) - speedY * deltaTime - Element.G * deltaTime * deltaTime / 2;
                        speedY += Element.G * deltaTime;
                    }
                }
            }
            if (y + Radius > Element.GroundY)
                y = Element.GroundY - Radius;

            //новий стан об'єкту
            Coord = new Point(x, y);
            Speed = Sqrt(Pow(speedX, 2) + Pow(speedY, 2));
            SpeedAngle = Atan2(speedY, speedX);
        }
        public void addShot(double time)//додавання поточної позициїї до анімації з заданим часом
        {
            animationX.KeyFrames.Add(new LinearDoubleKeyFrame(Coord.X - Radius, TimeSpan.FromSeconds(time)));
            animationY.KeyFrames.Add(new LinearDoubleKeyFrame(Coord.Y - Radius, TimeSpan.FromSeconds(time)));
        }
        public String getFillARGBColor()
        {
            return fillBrush.Color.A + splitSymbol.ToString() + fillBrush.Color.R + splitSymbol.ToString() + 
                    fillBrush.Color.G + splitSymbol.ToString() + fillBrush.Color.B;
        }
        public String getStrokeARGBColor()
        {
            return strokeBrush.Color.A + splitSymbol.ToString() + strokeBrush.Color.R + splitSymbol.ToString() +
                    strokeBrush.Color.G + splitSymbol.ToString() + strokeBrush.Color.B;
        }
    }
}