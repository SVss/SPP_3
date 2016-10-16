using System.Windows.Input;
using XmlParserWpf.ViewModel;

namespace XmlParserWpf
{
    /// <summary>
    /// Interaction logic for PropertiesWindow.xaml
    /// </summary>
    public partial class PropertiesWindow
    {
        private readonly MethodsListItem _sourceMethod;
        public MethodsListItem Method { get; private set; }

        // Public

        public PropertiesWindow(MethodsListItem method)
        {
            Method = method.Clone() as MethodsListItem;
            _sourceMethod = method;

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

    internal static partial class CustomCommands
    {
        public static RoutedUICommand Ok = new RoutedUICommand(
            "Ok",
            "Ok",
            typeof(CustomCommands),
            new InputGestureCollection()
        );

        public static RoutedUICommand Cancel = new RoutedUICommand(
            "Cancel",
            "Cancel",
            typeof(CustomCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.Escape)
            }
        );

        public static RoutedUICommand Reset = new RoutedUICommand(
            "Reset",
            "Reset",
            typeof(CustomCommands),
            new InputGestureCollection()
        );

    }
}
