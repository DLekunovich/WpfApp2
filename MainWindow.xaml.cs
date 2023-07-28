using System.Windows;
using System.Windows.Controls;

namespace WpfApp2
{
    public interface IMainWindowActions
    {
        void ScrollToLine(int lineNumber);
    }

    public partial class MainWindow : Window, IMainWindowActions
    {
        public MainWindow()
        {
            InitializeComponent();

            var viewModel = new MainWindowViewModel(this);

            DataContext = viewModel;
        }

        public void ScrollToLine(int lineNumber)
        {
            string line = lineNumber.ToString();


            for (int i = 0; i < textBoxXml.LineCount; i++) {
                if(textBoxXml.GetLineText(i).StartsWith(line + "."))
                {
                    textBoxXml.ScrollToLine(i);
                    break;
                }
            }
        }
    }

}
