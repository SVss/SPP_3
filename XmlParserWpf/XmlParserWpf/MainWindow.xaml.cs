using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using MessageBox = System.Windows.MessageBox;
using OpenFileDialog = System.Windows.Forms.OpenFileDialog;
using TracerLibXmlParser;

namespace XmlParserWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public FilesViewModel FilesList { get; set; } = new FilesViewModel();

        private static readonly OpenFileDialog OpenFileDialog = new OpenFileDialog()
        {
            Title = @"Choose file to open",
            Filter = @"XML-file|*.xml"
        };

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Open_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Open_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            DialogResult openResult = OpenFileDialog.ShowDialog();
            if (openResult != System.Windows.Forms.DialogResult.OK)
                return;

            string path = OpenFileDialog.FileNames[0];
            if (FilesList.Any(x => x.Path.Equals(path)))
            {
                FilesList.SelectedIndex = FilesList.IndexOf(FilesList.First(x => x.Path.Equals(path)));
                return;
            }

            try
            {
                FilesList.AddAndSelect(FilesListItem.LoadFromFile(path));
                MessageBox.Show("Open", "Opening file");
            }
            catch (BadXmlException)
            {
                MessageBox.Show("Error", $"Can't open file {path}", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Exit_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Exit_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            // TODO: check all tabs to be saved
            Close();
        }

        private void Close_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = FilesList.Count > 0;
        }

        private void Close_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            FilesList.RemoveSelected();
        }
    }

    internal static class CustomCommands
    {
        public static RoutedUICommand Open = new RoutedUICommand(
            "Open",
            "Open",
            typeof(CustomCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.O, ModifierKeys.Control)
            }
        );

        public static RoutedUICommand Close = new RoutedUICommand(
            "Close",
            "Close",
            typeof(CustomCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.W, ModifierKeys.Control)
            }
        );

        public static RoutedUICommand Exit = new RoutedUICommand(
            "Exit",
            "Exit",
            typeof(CustomCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.X, ModifierKeys.Alt)
            }
        );

        // TODO: more commands
    }
}
