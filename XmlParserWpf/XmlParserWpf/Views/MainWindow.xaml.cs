using System.ComponentModel;
using XmlParserWpf.Utils;
using XmlParserWpf.ViewModel;

namespace XmlParserWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            AppManager.FileTabsWindow = this;
        }
    }
}
