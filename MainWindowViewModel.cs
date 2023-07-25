﻿using GalaSoft.MvvmLight.Command;
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
        private const string CalculatorAppId = "Microsoft.WindowsCalculator_8wekyb3d8bbwe!App";

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
            if (ProcessText == "calculator.exe")
            {
                try
                {
                    Setup();
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
                }
                TearDown();
            }
        }

        private void Setup()
        {
            if (session == null)
            {
                var appOptions = new AppiumOptions();
                appOptions.AddAdditionalCapability("app", CalculatorAppId);
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