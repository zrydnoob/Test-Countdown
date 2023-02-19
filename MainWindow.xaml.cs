using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Animation;
using System.IO;
using System.Windows.Threading;
using System.Text.Json.Nodes;
using Newtonsoft.Json.Linq;
using System.Threading;
using System.Diagnostics;

namespace Test_countdown
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// 
    /// </summary>
    public partial class MainWindow : Window
    {
        public static JObject settingJson;
        public Window Setting = new setting();

        public MainWindow()
        {
            InitializeComponent();
            this.ShowInTaskbar = false;
            Setting.Visibility = Visibility.Hidden;
            setting.SettingsChanged += ApplicationSettings;
            ApplicationSettings();
            Update();


            System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            // Tick 超过计时器间隔时发生。
            dispatcherTimer.Tick += new EventHandler(timer_Tick);

            // Interval 获取或设置计时器刻度之间的时间段
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);

            dispatcherTimer.Start();
            ChickenSoup();

        }
        //窗口移动
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
        //最小化按钮
        async private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            BeginStoryboard(FindResource("CloseStoryboard") as Storyboard);
            await Task.Delay(1000);
            this.Visibility= Visibility.Hidden;
        }
        //显示窗口
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Visible;
            BeginStoryboard(FindResource("OpenStoryboard") as Storyboard);
        }

        //关闭窗口
        async private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            await Task.Delay(1000);
            Application.Current.Shutdown();
            Process.GetCurrentProcess().Kill();
        }

        //打开设置页面
        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            Setting.Visibility = Visibility.Visible;
        }

        public void ApplicationSettings()
        {
            if (File.Exists("config.json") == false) //如果配置文件不存在就创建
            {
                FileStream f = File.Create("config.json");
                f.Close();
                File.WriteAllText("config.json", "{\r\n  \"title\": \"title\",\r\n  \"date\": \"2023-06-20T00:00:00\",\r\n  \"chicken\": [\r\n    \r\n  ]\r\n}");
                Console.WriteLine("已创建config.json文件");
            }
            settingJson = JObject.Parse(File.ReadAllText("config.json"));
            Console.WriteLine(settingJson);
            setting.settingJsonsetting = settingJson;
        }

        public string dt;
        public string week;

        private void timer_Tick(object sender, EventArgs e)
        {
            Update();
            
        }

        void Update()
        {
            dt = DateTime.Now.DayOfWeek.ToString();
            switch (dt)
            {
                case ("Monday"):
                    week = "星期一";
                    break;
                case ("Tuesday"):
                    week = "星期二";
                    break;
                case ("Wednesday"):
                    week = "星期三";
                    break;
                case ("Thursday"):
                    week = "星期四";
                    break;
                case ("Friday"):
                    week = "星期五";
                    break;
                case ("Saturday"):
                    week = "星期六";
                    break;
                case ("Sunday"):
                    week = "星期日";
                    break;
            }
            TodayDateAndTime.Text = String.Format("今天是:{0} {1}", DateTime.Now.ToLongDateString().ToString(), week);
            MainTitle.Text = settingJson["title"].ToString();
            Day.Text = (((DateTime)settingJson["date"]) - DateTime.Now.Date).TotalDays.ToString() + "天";
            Time.Text = string.Format("{0}小时{1}分钟{2}秒",24 - DateTime.Now.Hour, 60 - DateTime.Now.Minute, 60 - DateTime.Now.Second);
        }
       
        private void ChickenSoup()
        {
            Thread thread = new Thread(() =>
            {
                while (true)
                {
                    Random ran = new Random();
                    int n = ran.Next(0, settingJson["chicken"].ToList().Count);
                    Thread.Sleep(5000);
                    this.Dispatcher.Invoke(new Action(delegate
                    {
                        ChickenSoupText.Text = settingJson["chicken"].ToList()[n].ToString();
                    }));
                }
            });
            thread.Start();
        }
    }
}
