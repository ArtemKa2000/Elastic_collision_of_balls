using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Windows;
using System.Windows.Media;
using static Modeling.wElementCreater;

namespace Modeling
{   
    static class Data
    {
        public static event Action ElementsChanged;

        //налаштвання за замовчанням. Існують тільки тут.
        public static string DataBasePathDefault { get; } = Environment.CurrentDirectory + "\\Sources\\Elements.xml";
        public static double MinTimeDefault { get; } = 0.0001;
        public static int MaxFramesCountDefault { get; } = 1000;
        public static double MinScaleDefault { get; } = 0.1;
        public static double TranslateFieldsDefault { get; } = 15;
        public static double ScrollStepDefault { get; } = 1.2;
        public static double AverageSpeedRatioDefault { get; } = 0.5;



        //public
        public static readonly double dataScale;
        public static readonly char splitSymbol;
        public static readonly double maxSpeedRatio;
        public static readonly double minSpeedRatio;
        public static readonly Brush groundColor;
        public static readonly Brush grassColor;
        public static readonly Brush skyColor;
        public static readonly Brush normalBorderBrush;
        public static readonly Brush chosentBorderBrush;
        public static readonly Brush guidesForceBrush;
        public static readonly Brush guidesBackBrush;
        public static readonly Brush promptsForeground;
        public static readonly List<Element> Elements;
            //Інкапсулюються властивостями
        private static string dataBasePath;
        private static double minimalCalculateTime;
        private static int maxFramesCount;
        private static double scrollStep;
        private static double minScale;
        private static double translateFields;
        private static double averageSpeedRatio;
            //Властивості
        public static string DataBasePath
        {
            get
            {
                return dataBasePath;
            }
            set
            {
                trySaveData();
                dataBasePath = value;
                Elements.Clear();
                if (ElementsChanged != null)
                    ElementsChanged();
                loadData();
            }
        }
        public static double MinimalCalculateTime {
            get { return minimalCalculateTime; }
            set
            {
                if (value < MinTimeDefault)
                    minimalCalculateTime = MinTimeDefault;
                else
                    minimalCalculateTime = value;
                isSettingsSave = false;
            }
        }
        public static int MaxFramesCount {
            get { return maxFramesCount; }
            set
            {
                if (value < MaxFramesCountDefault)
                    maxFramesCount = MaxFramesCountDefault;
                else
                    maxFramesCount = value;
                isSettingsSave = false;
            }
        }
        public static double ScrollStep
        {
            get { return scrollStep; }
            set
            {
                if (value < 1)
                    scrollStep = 1;
                else
                    if (value > 10)
                        scrollStep = 10;
                else
                    scrollStep = value;
                isSettingsSave = false;
            }
        }
        public static double MinScale
        {
            get { return minScale; }
            set
            {
                if (value <= 0)
                    minScale = MinScaleDefault;
                else
                    if (value > 1)
                        minScale = 1;
                else
                    minScale = value;
                isSettingsSave = false;
            }
        }
        public static double TranslateFields
        {
            get { return translateFields; }
            set
            {
                if (value < 0)
                    translateFields = 0;
                else
                    translateFields = value;
                isSettingsSave = false;
            }
        }
        public static double AverageSpeedRatio
        {
            get { return averageSpeedRatio; }
            set
            {
                if (value < minSpeedRatio)
                    averageSpeedRatio = minSpeedRatio;
                else
                    if (value > maxSpeedRatio)
                        averageSpeedRatio = maxSpeedRatio;
                else
                    averageSpeedRatio = value;
                isSettingsSave = false;
            }
        }
        public static String TheoryPath { get; }
        //private
        private static readonly string[] explanationPaths;
        private static Stack<Window> windowHistory;
        private static string settingsPath;
        private static bool isLoaded;
        private static bool isDataSave { get; set; }
        private static bool isSettingsSave { get; set; }


        //TODO: абсолютний шлях до теорії.
        static Data()
        {
            settingsPath = "Sources\\Settings.dat";
            splitSymbol = ' ';
            dataScale = 100;
            maxSpeedRatio = 5;
            minSpeedRatio = 0.2;
            groundColor = new SolidColorBrush( Color.FromRgb(93, 31, 0));
            grassColor = new SolidColorBrush(Colors.Green);
            skyColor = new SolidColorBrush(Color.FromRgb(0, 190, 255));
            normalBorderBrush = new SolidColorBrush(Colors.Black);
            chosentBorderBrush = new SolidColorBrush(Color.FromRgb(0,0,150));
            guidesForceBrush = new SolidColorBrush(Colors.Black);
            guidesBackBrush = new SolidColorBrush(Colors.Blue);
            promptsForeground = new SolidColorBrush(Colors.BlanchedAlmond);
            Elements = new List<Element>();
            windowHistory = new Stack<Window>();
            explanationPaths =new string[]{ Environment.CurrentDirectory + "\\Sources\\Explanations\\"+wExplanation.ExplanationType.MenuMain.ToString()+".html",
                                            Environment.CurrentDirectory + "\\Sources\\Explanations\\"+wExplanation.ExplanationType.MenuModeling.ToString()+".html",
                                            Environment.CurrentDirectory + "\\Sources\\Explanations\\"+wExplanation.ExplanationType.ElementCreater.ToString()+".html",
                                            Environment.CurrentDirectory + "\\Sources\\Explanations\\"+wExplanation.ExplanationType.Modeling.ToString()+".html"};
            TheoryPath = "Sources\\Theory";
        }



        //Ініціалізація. Повинна бути виконана при старті програми.
        public static void initialize()
        {
            try
            {
                //читаємо налаштування
                using (StreamReader sr = new StreamReader(settingsPath))
                {
                    Dictionary<String, String> dat = new Dictionary<String, String>();
                    bool isCorrect = true;
                    string[] temp = sr.ReadLine().Split(' ',':');
                    dat.Add(temp[0], temp[temp.Length-1]);
                    temp = sr.ReadLine().Split(' ', ':');
                    dat.Add(temp[0], temp[temp.Length - 1]);
                    temp = sr.ReadLine().Split(' ', ':');
                    dat.Add(temp[0], temp[temp.Length - 1]);
                    temp = sr.ReadLine().Split(' ', ':');
                    dat.Add(temp[0], temp[temp.Length - 1]);
                    temp = sr.ReadLine().Split(' ', ':');
                    dat.Add(temp[0], temp[temp.Length - 1]);
                    temp = sr.ReadLine().Split(' ', ':');
                    dat.Add(temp[0], temp[temp.Length - 1]);
                    temp = sr.ReadLine().Split(' ', ':');
                    dat.Add(temp[0], temp[temp.Length - 1]);


                    MinimalCalculateTime = double.Parse(dat["MinimalCalculateTime"]);
                    if (double.Parse(dat["MinimalCalculateTime"]) != MinimalCalculateTime)
                        isCorrect = false;
                    MaxFramesCount = int.Parse(dat["MaxFramesCount"]);
                    if (double.Parse(dat["MaxFramesCount"]) != MaxFramesCount)
                        isCorrect = false;
                    MinScale = double.Parse(dat["MinScale"]);
                    if (double.Parse(dat["MinScale"]) != MinScale)
                        isCorrect = false;
                    TranslateFields = double.Parse(dat["TranslateFields"]);
                    if (double.Parse(dat["TranslateFields"]) != TranslateFields)
                        isCorrect = false;
                    ScrollStep = double.Parse(dat["ScrollStep"]);
                    if (double.Parse(dat["ScrollStep"]) != ScrollStep)
                        isCorrect = false;
                    AverageSpeedRatio = double.Parse(dat["AverageSpeedRatio"]);
                    if (double.Parse(dat["AverageSpeedRatio"]) != AverageSpeedRatio)
                        isCorrect = false;
                    
                    if (dat["DataBasePath"].StartsWith("\\"))
                        temp[0] = Environment.CurrentDirectory + dat["DataBasePath"];
                    DataBasePath = temp[0];
                    if (!isDataSave)
                    {
                        DataBasePath = DataBasePathDefault;
                        isCorrect = false;
                    }

                    //Якщо досі isCorrect - true, то налаштування у файлі коректні
                    if (isCorrect)
                        isSettingsSave = true;

                    isDataSave = true;//Навіть  якщо дані не завантажились, вважаємо поточний стан збереженим.
                }
                
            }
            catch (Exception e)
            {
                if (e is IndexOutOfRangeException|| e is FormatException || e is OverflowException || e is ArgumentNullException)
                    MessageBox.Show("Файл: "+settingsPath+ " має не вірний формат. Застосовуватимуться налаштування за замовчуванням", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                else
                    MessageBox.Show(e.Message, "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);

                //Заносимо дефолтні значення
                MinimalCalculateTime = MinTimeDefault;
                MaxFramesCount = MaxFramesCountDefault;
                MinScale = MinScaleDefault;
                TranslateFields = TranslateFieldsDefault;
                ScrollStep = TranslateFieldsDefault;
                AverageSpeedRatio = AverageSpeedRatioDefault;
                DataBasePath = DataBasePathDefault;
                isSettingsSave = false;//Поточні налаштування не занесені до файлу. Не є необхідним, адже isSettingsSave буде й так false
            }
            finally
            {
                //Програма завантажилась
                isLoaded = true;
            }
        }
        private static void loadData()
        {
            try
            {
                using (FileStream fs = new FileStream(DataBasePath, FileMode.Open))
                {
                    DataSet ds = new DataSet();
                    ds.ReadXml(fs, XmlReadMode.InferTypedSchema);
                    fs.Close();
                    ElementCreater.createElements(ds.Tables[0]);//завжди одна таблиця
                    isDataSave = true;
                }
            }
            catch
            {
                MessageBox.Show("Не вдалося завантажити дані.\nФайл: "+ DataBasePath, "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        //Збереження
        public static void trySaveData()//зберегти данні з підтвердженням
        {
            if (!isDataSave && isLoaded)
                if (MessageBox.Show("Данні не збережені. Зберегти?", "Збереження", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    saveData();
        }
        public static void trySaveSettings()//зберегти налаштування з підтвердженням
        {
            if (!isSettingsSave )
                if (MessageBox.Show("Налаштування не збережені. Зберегти?", "Збереження", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    saveSettings();
        }
        public static void saveChange()//зберегти усе без підтверджень
        {
            saveData();
            saveSettings();
        }
        private static void saveSettings()
        {
            using (StreamWriter sw = new StreamWriter(settingsPath))
            {
                sw.WriteLine("MinimalCalculateTime: " + minimalCalculateTime.ToString());
                sw.WriteLine("MaxFramesCount: " + maxFramesCount.ToString());
                sw.WriteLine("MinScale: " + MinScale.ToString());
                sw.WriteLine("TranslateFields: " + TranslateFields.ToString());
                sw.WriteLine("ScrollStep: " + ScrollStep.ToString());
                sw.WriteLine("AverageSpeedRatio: " + AverageSpeedRatio.ToString());
                if(dataBasePath.Contains(Environment.CurrentDirectory))
                    sw.WriteLine("DataBasePath: " + DataBasePath.Replace(Environment.CurrentDirectory,""));
                else
                    sw.WriteLine("DataBasePath: " + DataBasePath);
                isSettingsSave = true;
            }
        }
        private static void saveData()
        {
            DataSet ds = new DataSet("Elements_Set");
            DataTable dt = new DataTable("Element");
            ds.Tables.Add(dt);
            dt.Columns.Add("Width",typeof(double));
            dt.Columns.Add("Height", typeof(double));
            dt.Columns.Add("FPS", typeof(Int32));
            dt.Columns.Add("SpeedAnimation", typeof(Int32));
            dt.Columns.Add("Fields", typeof(double));
            dt.Columns.Add("FontSize", typeof(double));
            dt.Columns.Add("InfoVisibility", typeof(bool));
            dt.Columns.Add("G", typeof(double));
            dt.Columns.Add("Radius1", typeof(double));
            dt.Columns.Add("Massa1", typeof(double));
            dt.Columns.Add("ColorFill1", typeof(string));
            dt.Columns.Add("ColorStroke1", typeof(string));
            dt.Columns.Add("CoordX1", typeof(double));
            dt.Columns.Add("Speed1", typeof(double));
            dt.Columns.Add("Radius2", typeof(double));
            dt.Columns.Add("Massa2", typeof(double));
            dt.Columns.Add("ColorFill2", typeof(string));
            dt.Columns.Add("ColorStroke2", typeof(string));
            dt.Columns.Add("CoordX2", typeof(double));
            dt.Columns.Add("Speed2", typeof(double));

            foreach (Element el in Elements)
            {
                DataRow dr = dt.Rows.Add();
                dr[0]=el.Canvas.Width/dataScale;
                dr[1] = el.Canvas.Height / dataScale;
                dr[2] = el.FPS;
                dr[3] = el.SpeedAnimation;
                dr[4] = el.Fields / dataScale;
                dr[5] = el.FontSize/dataScale;
                dr[6] = el.InfoVisibility;
                dr[7] = el.G / dataScale;
                dr[8] = el.FirstBall.Radius / dataScale;
                dr[9] = el.FirstBall.Mass;
                dr[10] = el.FirstBall.getFillARGBColor();
                dr[11] = el.FirstBall.getStrokeARGBColor();
                dr[12] = el.FirstBall.Coord.X / dataScale;
                if(el.FirstBall.SpeedAngle==0)
                    dr[13] = el.FirstBall.Speed / dataScale;
                else
                    dr[13] = el.FirstBall.Speed / dataScale * -1;
                dr[14] = el.SecondBall.Radius / dataScale;
                dr[15] = el.SecondBall.Mass;
                dr[16] = el.SecondBall.getFillARGBColor();
                dr[17] = el.SecondBall.getStrokeARGBColor();
                dr[18] = el.SecondBall.Coord.X / dataScale;
                if(el.SecondBall.SpeedAngle==0)
                    dr[19] = el.SecondBall.Speed / dataScale;
                else
                    dr[19] = el.SecondBall.Speed / dataScale * -1;
            }

            using (FileStream fs = new FileStream(DataBasePath, FileMode.Create))
                   ds.WriteXml(fs);
            isDataSave = true;
        }
        

        //Редагування списків
        public static void addElement(Element el)//додаємо елемент і зберігаємо його кисть
        {
            Elements.Add(el);
            isDataSave = false;
            if (ElementsChanged!=null)
                ElementsChanged();
        }
        public static void deleteElement(int pos)
        {
            Elements.RemoveAt(pos);
            isDataSave = false;
            if (ElementsChanged != null)
                ElementsChanged();
        }
        public static void replaceElement(Element el, int pos)
        {
            Elements[pos] = el;
            isDataSave = false;
            if (ElementsChanged != null)
                ElementsChanged();
        }
        public static void pushWindow(Window window)
        {
            windowHistory.Push(window);
        }
        public static Window getWindow(bool delete)
        {
            try
            {
                if (delete)
                    return windowHistory.Pop();
                else
                    return windowHistory.Peek();
            }catch(Exception ex)
            {
                MessageBox.Show("Додаток буде закрито через помилку:\n"+ex.ToString(),"Помилка",MessageBoxButton.OK,MessageBoxImage.Error);
                applicationClosing();
                return null;
            }
        }
        public static VisualBrush getBrush(UIElement el)
        {
            VisualBrush vb = new VisualBrush(el);
            vb.Stretch = Stretch.Uniform;
            return vb;
        }



        public static void applicationClosing()
        {
            //При закритті додатку цей метод викликається багаторазово. 
            //Аби запобігти повторенню запитів про збереження використовується змінна isLoaded
            if (isLoaded)
            {
                trySaveData();
                trySaveSettings();
                isLoaded = false;
                Application.Current.Shutdown();
            }
        }
        public static string getExplonationPath(int index)
        {
            return explanationPaths[index];
        }
    }
}