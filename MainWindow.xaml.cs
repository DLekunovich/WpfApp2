using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace WpfApp2
{
    public partial class MainWindow : Window
    {

        private const string WindowsApplicationDriverUrl = "http://127.0.0.1:4723";
        private const string CalculatorAppId = "Microsoft.WindowsCalculator_8wekyb3d8bbwe!App";

        protected static WindowsDriver<WindowsElement> session;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (textBoxProcess.Text == "calculator.exe")
            {
                Setup();
                textBoxXml.Text = session.PageSource;
                TearDown();
            }
        }

        public static void Setup()
        {
            
            if (session == null)
            {
                var appOptions = new AppiumOptions();
                appOptions.AddAdditionalCapability("app", CalculatorAppId);
                appOptions.AddAdditionalCapability("deviceName", "WindowsPC");
                session = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), appOptions);
                
            }
        }

        public static void TearDown()
        {
            
            if (session != null)
            {
                session.Quit();
                session = null;
            }
        }
    }
}
