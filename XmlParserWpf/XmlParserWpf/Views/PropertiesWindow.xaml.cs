using XmlParserWpf.ViewModel;

namespace XmlParserWpf.Views
{
    /// <summary>
    /// Interaction logic for PropertiesWindow.xaml
    /// </summary>
    public partial class PropertiesWindow
    {

        public PropertiesWindow(MethodEditingViewModel method)
        {
            DataContext = method;
            method.AssociatedWindow = this;

            InitializeComponent();
        }   
    }
}
