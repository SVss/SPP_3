using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using TracerLibXmlParser;

namespace XmlParserWpf
{
    /// <summary>
    /// Interaction logic for PropertiesWindow.xaml
    /// </summary>
    public partial class PropertiesWindow: INotifyPropertyChanged
    {
        private MethodsListItem _method;

        public MethodsListItem Method
        {
            get { return _method; }
            set
            {
                _method = value;
                OnPropertyChanged("Method");
            }
        }

        public PropertiesWindow()
        {
            InitializeComponent();
            // TODO: make copy of MethodsListItem properties to reset value => implement ICloneable in MethodsListItem
        }

        private void Ok_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Ok_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            // TODO: save results to real method
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
            // TODO: reset fields by cloning again
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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
