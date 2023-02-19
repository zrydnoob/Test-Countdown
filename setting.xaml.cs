using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
using System.Windows.Shapes;
using HandyControl.Controls;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
namespace Test_countdown;

/// <summary>
/// setting.xaml 的交互逻辑
/// </summary>
public partial class setting : System.Windows.Window
{
    public delegate void ApplicationSettings();
    public static event ApplicationSettings SettingsChanged;

    public static JObject settingJsonsetting;
    public setting()
    {
        InitializeComponent();
        init();
    }

    

    private void Window_MouseDown(object sender, MouseButtonEventArgs e)
    {
        this.DragMove();
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        if (File.Exists("config.json") == false) //如果配置文件不存在就创建
        {
            FileStream f = File.Create("config.json");
            f.Close();
            Console.WriteLine("已创建config.json文件");
        }
        File.WriteAllText("config.json", GetSetting().ToString());
        Console.WriteLine("已写入config.json文件");
        this.Visibility = Visibility.Hidden;
        SettingsChanged();
        
    }

    public JObject GetSetting()
    {
        var job = new JObject()
        {
            {"title",titlesetting.Text},
            {"date",DateChoose.SelectedDate}
        };


        string[] lines = System.Text.RegularExpressions.Regex.Split(ChickenGULi.Text, "\r\n");
        int count = lines.Length - 1;

        var chicken = new JArray();

        for (int i = 0; i <= count; i++)
        {
            chicken.Add(ChickenGULi.GetLineText(i));
        }

        job.Add("chicken", chicken);

        return job;
    }
    async void init()
    {
        await Task.Delay(3000);
        titlesetting.Text = settingJsonsetting["title"].ToString();
        DateChoose.SelectedDate = ((DateTime)settingJsonsetting["date"]);
    }

}
