using System.Windows;
using XmlParserWpf.Utils;

namespace XmlParserWpf.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow: Window
    {
        public MainWindow()
        {
            InitializeComponent();
            EventsManager.ProvideFileTabsWindowToSubscribeEvents(this);
        }
    }
}
