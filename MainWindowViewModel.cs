using GalaSoft.MvvmLight.Command;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace WpfApp2
{
    public class MainWindowViewModel : ViewModelBase
    {
        //private const string WindowsApplicationDriverUrl = "http://127.0.0.1:4723";
        private string appText = "";

        private WindowsDriver<WindowsElement> session;

        private readonly IMainWindowActions mainWindowActions;

        public MainWindowViewModel(IMainWindowActions mainWindowActions)
        {
            this.mainWindowActions = mainWindowActions;
        }

        private Int32 selectedItem;
        public Int32 SelectedItem
        {
            get { return selectedItem; }
            set
            {
                selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
            }
        }

        private ObservableCollection<String> items;

        public ObservableCollection<String> Items
        {
            get { return items; }
            set
            {
                items = (ObservableCollection<string>)value;
                OnPropertyChanged(nameof(Items));
            }
        }

        private string urlText = "http://127.0.0.1:4723";
        public string UrlText
        {
            get { return urlText; }
            set
            {
                urlText = value;
                OnPropertyChanged(nameof(UrlText));
            }
        }

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

        private string searchResults;
        public string SearchResults
        {
            get { return searchResults; }
            set
            {
                searchResults = value;
                OnPropertyChanged(nameof(SearchResults));
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

        private ICommand showCommand;
        public ICommand ShowCommand
        {
            get
            {
                if (showCommand == null)
                {
                    showCommand = new RelayCommand(Show);
                }
                return showCommand;
            }
        }

        private void Show()
        {
            mainWindowActions.ScrollToLine(selectedItem);
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
            Items = new ObservableCollection<String>();
            int j = 0;
            var text = xmlText.Split('\n');
            for(int i = 0; i < text.Length; i++)
            {
                j = 0;
                if (text.ElementAt(i).Contains(searchText))
                {
                    while(text.ElementAt(i).ElementAt(j).ToString() != ".")
                    {
                        SearchResults += text.ElementAt(i).ElementAt(j);
                        j++;
                    }

                    Items.Add(SearchResults);
                }
                SearchResults = "";
            }
            
        }


        private void Start()
        {

            int counter = 2;

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
                if (CreateSessionForAlreadyRunningApp(this, appText) == null)
                {
                    try
                    {
                        Setup(this, processText);
                    }
                    catch (OpenQA.Selenium.WebDriverException)
                    {
                        MessageBox.Show("Can't find such app");
                        return;
                    }
            }
                else
                    session = CreateSessionForAlreadyRunningApp(this, appText);
                string str = session.PageSource.ToString();
                string newStr = "1.";
                for (int i = 0; i < str.Length; i++)
                {
                    newStr += str[i];
                    if (str[i] == '>' && i != str.Length - 1)
                    {
                        newStr += '\n';
                        newStr += counter + ".    ";
                        counter++;
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

        private static WindowsDriver<WindowsElement> CreateSessionForAlreadyRunningApp(MainWindowViewModel viewModel, string appText)
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
                var appSession = new WindowsDriver<WindowsElement>(new Uri(viewModel.UrlText), appOptions);
                return appSession;
            }
            return null;
        }

        private void Setup(MainWindowViewModel viewModel, String appText)
        {
            if (session == null)
            {
                var appOptions = new AppiumOptions();
                appOptions.AddAdditionalCapability("app", appText);
                appOptions.AddAdditionalCapability("deviceName", "WindowsPC");
                session = new WindowsDriver<WindowsElement>(new Uri(viewModel.UrlText), appOptions);
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
