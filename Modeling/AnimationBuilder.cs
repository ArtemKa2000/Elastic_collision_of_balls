using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Modeling
{
    using static Math;

    static class AnimationBuilder
    {
        private static double time = 0;//поточний час. У секундах
        private static double collisionAngle = 0;
        private static Element element;
        private static Ball FirstBall;//дублювання посилань на поля element для спрощення синтаксису
        private static Ball SecondBall;
        private static MoveDirection firstBallDirection;
        private static MoveDirection secondBallDirection;
        private static bool guidesVisible;
        private static double firstCollisionTime;
        //FirstBall.SpeedAngle є[-PI;PI]
        public static double createAnimation(Element element,bool guidesVisible)
        {
            Element clone = wElementCreater.ElementCreater.getCopy(element);
            modelingFirstCollision(clone);
            AnimationBuilder.element = element;
            AnimationBuilder.guidesVisible = guidesVisible;
            FirstBall = element.FirstBall;
            SecondBall = element.SecondBall;

            if (FirstBall.SpeedAngle == 0)
                firstBallDirection = new MoveDirection { IsForce = true, LastPoint = FirstBall.Coord };
            else
                firstBallDirection = new MoveDirection { IsForce = false, LastPoint = FirstBall.Coord };
            if (SecondBall.SpeedAngle == 0)
                secondBallDirection = new MoveDirection { IsForce = true, LastPoint = SecondBall.Coord };
            else
                secondBallDirection = new MoveDirection { IsForce = false, LastPoint = SecondBall.Coord };

            if (FirstBall.Speed == 0)
            {
                if (clone.FirstBall.SpeedAngle <= PI / 2 && clone.FirstBall.SpeedAngle >= -PI / 2)
                    firstBallDirection.IsForce = true;
                else
                    firstBallDirection.IsForce = false;
            }
            if (SecondBall.Speed == 0)
            {
                if (clone.SecondBall.SpeedAngle <= PI / 2 && clone.SecondBall.SpeedAngle >= -PI / 2)
                    secondBallDirection.IsForce = true;
                else
                    secondBallDirection.IsForce = false;
            }
            firstCollisionTime = -1;


            FirstBall.animationX.KeyFrames.Clear();
            FirstBall.animationY.KeyFrames.Clear();
            SecondBall.animationX.KeyFrames.Clear();
            SecondBall.animationY.KeyFrames.Clear();

            FirstBall.moveForce(element.TimeInterval);
            SecondBall.moveForce(element.TimeInterval);
            while (checkCollision() && FirstBall.animationX.KeyFrames.Count < Data.MaxFramesCount)
            {
                time += element.TimeInterval;
                FirstBall.addShot(time);
                SecondBall.addShot(time);
                if (guidesVisible)
                {
                    addElipse(FirstBall,firstBallDirection);
                    addElipse(SecondBall,secondBallDirection);
                }

                FirstBall.moveForce(element.TimeInterval);
                SecondBall.moveForce(element.TimeInterval);
            }

            FirstBall.addShot(time);
            SecondBall.addShot(time);
            if (guidesVisible)
            {
                addElipse(FirstBall, firstBallDirection);
                addElipse(SecondBall, secondBallDirection);
            }

            reset();
            return firstCollisionTime;
        }
        public static bool modelingFirstCollision(Element element)
        {
            if (element.FirstBall.Speed * Cos(element.FirstBall.SpeedAngle) > element.SecondBall.Speed * Cos(element.SecondBall.SpeedAngle))
            {
                AnimationBuilder.element = element;
                guidesVisible = false;
                FirstBall = element.FirstBall;
                SecondBall = element.SecondBall;

                FirstBall.moveForce(element.TimeInterval);
                SecondBall.moveForce(element.TimeInterval);
                while (checkFirstCollision() && FirstBall.animationX.KeyFrames.Count < Data.MaxFramesCount)
                {
                    time += element.TimeInterval;
                    FirstBall.moveForce(element.TimeInterval);
                    SecondBall.moveForce(element.TimeInterval);
                }
                reset();
                return true;
            }

            reset();
            return false;
        }
        private static void reset()
        {
            element = null;
            FirstBall = SecondBall = null;
            time = 0;
            collisionAngle = 0;
        }


        //повинен виконуватися після кожного пересування обох куль!!!Повертає можливість/неможливість продовжувати анімацію
        private static bool checkFirstCollision()
        {
            if (FirstBall.Speed == 0 && SecondBall.Speed == 0)
                return false;
            //перевірка зіткнення куль
            if (Sqrt(Pow(FirstBall.Coord.X - SecondBall.Coord.X, 2) + Pow(FirstBall.Coord.Y - SecondBall.Coord.Y, 2)) <= FirstBall.Radius + SecondBall.Radius || element.checkBorder())
            {
                collisionBall();
                return false;
            }

            return true;
        }
        private static bool checkCollision()
        {
            //перевірка виходу куль за межі єкрану
            if (!element.checkBorder() || (FirstBall.Speed == 0 && SecondBall.Speed == 0))
                return false;

            //перевірка зіткненя з землею
            if (checkGround(FirstBall))
                collisionGround(FirstBall);
            if (checkGround(SecondBall))
                collisionGround(SecondBall);

            //перевірка зіткнення куль
            if (Sqrt(Pow(FirstBall.Coord.X - SecondBall.Coord.X, 2) + Pow(FirstBall.Coord.Y - SecondBall.Coord.Y, 2)) <= FirstBall.Radius + SecondBall.Radius)
            {
                if (firstCollisionTime == -1)
                    firstCollisionTime = time;
                collisionBall();
            }

            return true;
        }

        //чи торкається куля землі
        private static bool checkGround(Ball ball)
        {
            if (ball.Coord.Y + ball.Radius >= element.GroundY && ball.SpeedAngle < 0)//тільки якщо куля у польоті або швидкість направлена донизу
                return true;
            else
                return false;
        }


        public static void collisionBall()
        {
            //детальніше про рорахунки у файлі ggb.
            if (FirstBall.Coord.X != SecondBall.Coord.X)
                collisionAngle = Atan((SecondBall.Coord.Y - FirstBall.Coord.Y) / (FirstBall.Coord.X - SecondBall.Coord.X));//кут зіткнення куль
            else
                collisionAngle = (SecondBall.Coord.Y - FirstBall.Coord.Y > 0) ?-PI/2:PI/2;
            //імпульси куль, що передаються
            double firstSpeed = 0, secondSpeed = 0, resultSpeed=0;

            if (FirstBall.Coord.X < SecondBall.Coord.X)
            {
                firstSpeed = rightCollision(FirstBall);
                secondSpeed = leftCollision(SecondBall);
            }
            else
            {
                //розкладання імпульсів.
                firstSpeed = leftCollision(FirstBall);
                secondSpeed = rightCollision(SecondBall);
            }

            resultSpeed = firstSpeed - secondSpeed;
            firstSpeed = (FirstBall.Mass - SecondBall.Mass) / (FirstBall.Mass + SecondBall.Mass) * resultSpeed+secondSpeed;
            secondSpeed = 2 * FirstBall.Mass * resultSpeed / (FirstBall.Mass + SecondBall.Mass)+secondSpeed;

            addSpeed(FirstBall, firstSpeed);
            addSpeed(SecondBall, secondSpeed);
        }
        private static double rightCollision(Ball ball)//площина праворуч від кулі
        {
            //Повертає переданий імпульс
            double transmittedSpeed = ball.Speed * Cos(ball.SpeedAngle - collisionAngle);//cos(L)==cos(-L)
            ball.Speed *= Abs(Sin(ball.SpeedAngle - collisionAngle));//sin(L)=|sin(-L)|
            if (ball.SpeedAngle >= collisionAngle)
                ball.SpeedAngle = collisionAngle + PI / 2;
            else
                ball.SpeedAngle = collisionAngle - PI / 2;
           ball.SpeedAngle = checkFormat(ball.SpeedAngle);
           return transmittedSpeed;
        }
        private static double leftCollision(Ball ball)//площина ліворуч від кулі
        {
                //кут, що є протилежним до дотичної
                double transmittedSpeed = ball.Speed * Cos(ball.SpeedAngle - collisionAngle);//cos(L)==cos(-L). PI - ball.SpeedAngle + collisionAngle
                ball.Speed *= Abs(Sin(ball.SpeedAngle - collisionAngle));//sin(L)=|sin(-L)|

                //друга умова може виконуватись тільки у випадку, якщо collisionAngle > 0
                if ((ball.SpeedAngle >= collisionAngle && ball.SpeedAngle <= collisionAngle + PI) || ball.SpeedAngle <= collisionAngle - PI)
                    ball.SpeedAngle = collisionAngle + PI / 2;
                else
                    ball.SpeedAngle = collisionAngle - PI / 2;

            ball.SpeedAngle = checkFormat(ball.SpeedAngle);
            return transmittedSpeed;
        }
        private static void addSpeed(Ball b,double speed)
        {
            if(b.Speed == 0)
            {
                b.Speed = Abs(speed);
                b.SpeedAngle = (speed > 0) ?collisionAngle: checkFormat(collisionAngle-PI);
                return;
            }
            double newSpeed = Sqrt(Pow(b.Speed, 2) + Pow(speed, 2));
            double delta =Abs(Atan(speed/b.Speed));//Якщо speed<0, то і delta<0

            //if(collisionAngle<0)
                if (b.SpeedAngle * speed > 0)//якщо вони одного знаку
                    b.SpeedAngle -= delta;
                else
                    b.SpeedAngle += delta;
            //else
            //    if (b.SpeedAngle * speed > 0)//якщо вони одного знаку
            //    b.SpeedAngle += delta;
            //else
            //    b.SpeedAngle -= delta;
            b.SpeedAngle = checkFormat(b.SpeedAngle);
            b.Speed = newSpeed;
        }

        private static void collisionGround(Ball ball)
        {
            ball.SpeedAngle *=-1;//розраховуємо новий кут
        }


        private static double checkFormat(double angle)//перевіряє належність кута [-PI;PI]
        {
            while (angle > PI)
                angle -= 2 * PI;
            while (angle < -PI)
                angle += 2 * PI;
            return angle;
        }
        private static void addElipse(Ball b, MoveDirection direction)
        {
            Ellipse e = new Ellipse();
            e.Opacity = 0;
            e.Height = Sqrt(FirstBall.Radius*SecondBall.Radius)/10;
            e.Width = Sqrt(FirstBall.Radius * SecondBall.Radius) / 10;
            if (direction.IsForce)
                if (b.Coord.X >= direction.LastPoint.X)
                    e.Fill = Data.guidesForceBrush;
                else
                    e.Fill = Data.guidesBackBrush;
            else
                if (b.Coord.X <= direction.LastPoint.X)
                    e.Fill = Data.guidesForceBrush;
                else
                    e.Fill = Data.guidesBackBrush;
            direction.LastPoint = b.Coord;
            element.Canvas.Children.Add(e);
            Canvas.SetTop(e, b.Coord.Y-e.ActualHeight/2);
            Canvas.SetLeft(e, b.Coord.X - e.Width/2);

            DoubleAnimation da = new DoubleAnimation();
            da.To = 1;
            da.Duration = TimeSpan.FromMilliseconds(1);
            da.BeginTime = TimeSpan.FromSeconds(time);
            Storyboard.SetTarget(da, e);
            Storyboard.SetTargetProperty(da, new PropertyPath(UIElement.OpacityProperty));
            element.GuidesAppearance.Children.Add(da);
        }
        private class MoveDirection
        {
            public bool IsForce { get; set; }
            public Point LastPoint { get; set; }
        }
    }
}
