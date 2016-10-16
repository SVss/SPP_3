using System;
using System.ComponentModel;
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
        public FilesViewModel FilesList { get; } = new FilesViewModel();

        private static readonly OpenFileDialog OpenFileDialog = new OpenFileDialog()
        {
            Title = @"Choose file to open",
            Filter = @"XML-file|*.xml"
        };

        private static readonly SaveFileDialog SaveFileDialog = new SaveFileDialog()
        {
            Title = @"Choose location to save to",
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
            if (FilesList.HasFile(path))
            {
                FilesList.SelectIfExists(path);
                return;
            }

            try
            {
                FilesList.AddAndSelect(FilesListItem.LoadFromFile(path));
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
            Close();
        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            FilesList.SelectedIndex = 0;    // start closing from 1st file
            while (FilesList.Count > 0)
            {
                if (CanCloseFile(FilesList.SelectedFile))   // ask user here
                    FilesList.RemoveSelected();
                else
                {
                    e.Cancel = true;
                    return;             // <- user aborted closing
                }
            }
        }

        private void Close_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (FilesList.Count > 0);
        }

        private void Close_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (CanCloseFile(FilesList.SelectedFile) )
                FilesList.RemoveSelected();
        }

        private static bool CanCloseFile(FilesListItem file)
        {
            if (file.IsSaved)
                return true;

            var closeAnswer = MessageBox.Show("File is unsaved.\nDo you really want to close it ?", "Warning?", MessageBoxButton.YesNo,
                MessageBoxImage.Exclamation);

            return (closeAnswer == MessageBoxResult.Yes);
        }

        private void SaveAs_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (FilesList.Count > 0);
        }

        private void SaveAs_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            DialogResult saveResult = SaveFileDialog.ShowDialog();
            if (saveResult != System.Windows.Forms.DialogResult.OK)
                return;

            string path = SaveFileDialog.FileNames[0];
            try
            {
                FilesList.SelectedFile.SaveAs(path);
            }
            catch (Exception)
            {
                MessageBox.Show("Can't save file", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Save_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (FilesList.Count > 0) && (!FilesList.SelectedFile.IsSaved);  // + check if files exists
        }

        private void Save_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            FilesList.SelectedFile.Save();
        }
        
        private void FileTreeView_OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (FilesList.SelectedFile != null)
                FilesList.SelectedFile.SelectedMethod = e.NewValue as MethodsListItem;
        }
    }

    // Custom commands

    internal static partial class CustomCommands
    {
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
    }
}
