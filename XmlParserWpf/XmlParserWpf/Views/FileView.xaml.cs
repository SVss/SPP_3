using System.Windows.Controls;
using XmlParserWpf.Utils;

namespace XmlParserWpf.Views
{
    /// <summary>
    /// Interaction logic for FileView.xaml
    /// </summary>
    public partial class FileView : UserControl
    {
        public FileView()
        {
            InitializeComponent();
            EventsManager.ProvideFileTreeViewToSubscribeEvents(FileTreeView);
        }        
    }
}
