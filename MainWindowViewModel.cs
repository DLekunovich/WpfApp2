using GalaSoft.MvvmLight.Command;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

namespace WpfApp2
{
    public class MainWindowViewModel : ViewModelBase
    {
        private const string WindowsApplicationDriverUrl = "http://127.0.0.1:4723";
        //private const string CalculatorAppId = "Microsoft.WindowsCalculator_8wekyb3d8bbwe!App";
        private string appText = "";

        private WindowsDriver<WindowsElement> session;


        private string processText;
        public string ProcessText
        {
            get { return processText; }
            set
            {
                processText = value;
                OnPropertyChanged(nameof(ProcessText));
            }
        }

        private string searchText;
        public string SearchText
        {
            get { return searchText; }
            set
            {
                searchText = value;
                OnPropertyChanged(nameof(SearchText));
            }
        }

        private string xmlText;
        public string XmlText
        {
            get { return xmlText; }
            set
            {
                xmlText = value;
                OnPropertyChanged(nameof(XmlText));
            }
        }

        private ICommand startCommand;
        public ICommand StartCommand
        {
            get
            {
                if (startCommand == null)
                {
                    startCommand = new RelayCommand(Start);
                }
                return startCommand;
            }
        }

        private ICommand searchCommand;
        public ICommand SearchCommand
        {
            get
            {
                if (searchCommand == null)
                {
                    searchCommand = new RelayCommand(Search);
                }
                return searchCommand;
            }
        }

        private void Search()
        {
            String str = xmlText;

            
        }

        public void Start()
        {
            if (processText == "" || processText == null)
            {
                MessageBox.Show("Enter path to .exe file!");
                return;
            }
            else
            {
                appText = (processText
                    .Substring(processText.LastIndexOf('\\'))).Substring(1, (processText
                    .Substring(processText.LastIndexOf('\\'))).Length - 6);
            }
            
            try
            {
                if (CreateSessionForAlreadyRunningApp(appText) == null)
                {
                    try
                    {
                        Setup(processText);
                    }
                    catch (OpenQA.Selenium.WebDriverException)
                    {
                        MessageBox.Show("Can't find such app");
                        return;
                    }
            }
                else
                    session = CreateSessionForAlreadyRunningApp(appText);
                string str = session.PageSource.ToString();
                string newStr = "";
                for (int i = 0; i < str.Length; i++)
                {
                    newStr += str[i];
                    if (str[i] == '>')
                    {
                        newStr += '\n';
                        newStr += "    ";
                    }
                }
                XmlText = newStr;
            }
            catch (OpenQA.Selenium.WebDriverException)
            {
                MessageBox.Show("You should run WinAppDriver.exe");
                return;
            }

            TearDown();
        }

        private static WindowsDriver<WindowsElement> CreateSessionForAlreadyRunningApp(string appText)
        {
            bool ses = false;
            IntPtr appTopLevelWindowHandle = new IntPtr();
            foreach (Process clsProcess in Process.GetProcesses())
            {
                if (clsProcess.ProcessName.IndexOf(appText, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    clsProcess.MainWindowTitle.IndexOf(appText, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    appTopLevelWindowHandle = clsProcess.MainWindowHandle;
                    ses = true;
                    break;
                }
            }
            if (ses)
            {
                var appTopLevelWindowHandleHex = appTopLevelWindowHandle.ToString("x"); //convert number to hex string
                var appOptions = new AppiumOptions();
                appOptions.AddAdditionalCapability("appTopLevelWindow", appTopLevelWindowHandleHex);
                var appSession = new WindowsDriver<WindowsElement>(new Uri("http://127.0.0.1:4723"), appOptions);
                return appSession;
            }
            return null;
        }

        private void Setup(String appText)
        {
            if (session == null)
            {
                var appOptions = new AppiumOptions();
                appOptions.AddAdditionalCapability("app", appText);
                appOptions.AddAdditionalCapability("deviceName", "WindowsPC");
                session = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), appOptions);
            }
        }

        private void TearDown()
        {
            if (session != null)
            {
                session.Quit();
                session = null;
            }
        }
    }
}
