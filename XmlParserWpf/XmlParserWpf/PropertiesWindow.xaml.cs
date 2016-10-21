using System.Windows.Input;
using XmlParserWpf.Model;
using XmlParserWpf.ViewModel;

namespace XmlParserWpf
{
    /// <summary>
    /// Interaction logic for PropertiesWindow.xaml
    /// </summary>
    public partial class PropertiesWindow
    {
        private readonly MethodModel _sourceMethod;
        public MethodModel Method { get; private set; }

        // Public

        public PropertiesWindow(MethodModel method)
        {
            //Method = method.Clone() as MethodModel;
            //_sourceMethod = method;

            InitializeComponent();
        }

        // Private

        private void Ok_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Ok_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            _sourceMethod.Name = Method.Name;
            _sourceMethod.Package = Method.Package;
            _sourceMethod.ParamsCount = Method.ParamsCount;
            _sourceMethod.Time = Method.Time;

            Close();
        }

        private void Cancel_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Cancel_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Close();
        }

        private void Reset_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Reset_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Method.Name = _sourceMethod.Name;
            Method.Package = _sourceMethod.Package;
            Method.ParamsCount = _sourceMethod.ParamsCount;
            Method.Time = _sourceMethod.Time;
        }
    }
    
}
