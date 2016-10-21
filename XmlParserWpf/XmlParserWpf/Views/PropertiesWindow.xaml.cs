using XmlParserWpf.ViewModel;

namespace XmlParserWpf
{
    /// <summary>
    /// Interaction logic for PropertiesWindow.xaml
    /// </summary>
    public partial class PropertiesWindow
    {
        public MethodEditingViewModel Method { get; private set; }

        // Public

        public PropertiesWindow(MethodEditingViewModel method)
        {
            Method = method;
            DataContext = Method;

            Method.AssociatedWindow = this;

            InitializeComponent();
        }
        
    }
    
}
