using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using MessageBox = System.Windows.MessageBox;
using OpenFileDialog = System.Windows.Forms.OpenFileDialog;
using XmlParserWpf.ViewModel;

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
            Filter = StringConstants.FileDialogsFilters
        };

        private static readonly SaveFileDialog SaveFileDialog = new SaveFileDialog()
        {
            Filter = StringConstants.FileDialogsFilters
        };

        // Public

        public MainWindow()
        {
            InitializeComponent();
        }

        // Internal

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
                MessageBox.Show(
                    string.Format(MessagesConsts.FileCantLoadMessage, path),
                    MessagesConsts.ErrorMessageCaption,
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
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

            MessageBoxResult answ = MessageBox.Show(
                string.Format(MessagesConsts.FileNotSavedMessage, file.Path),
                MessagesConsts.WarningMessageCaption,
                MessageBoxButton.YesNoCancel,
                MessageBoxImage.Exclamation
            );

            bool result = false;
            switch (answ)
            {
                case MessageBoxResult.No:
                    result = true;
                    break;

                case MessageBoxResult.Yes:
                    file.Save();
                    result = true;
                    break;
            }
            return result;
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
                MessageBox.Show(
                    string.Format(MessagesConsts.FileCantSaveMessage, path),
                    MessagesConsts.ErrorMessageCaption,
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void Save_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (FilesList.Count > 0)
                && (!FilesList.SelectedFile.IsSaved);  // TODO: check if files exists
        }

        private void Save_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            FilesList.SelectedFile.Save();
        }
        
        private void FileTreeView_OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (FilesList.SelectedFile == null)
                return;

            FilesList.SelectedFile.SelectedMethod = 
                e.NewValue as MethodsListItem;
        }

        private void FileTreeItem_OnPreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TreeViewItem item = sender as TreeViewItem;
            if ((item == null)
                || (!item.IsSelected)
                || (FilesList.SelectedFile.SelectedMethod == null))
                return;

            var propertiesWindow = new PropertiesWindow(
                FilesList.SelectedFile.SelectedMethod);

            propertiesWindow.ShowDialog();
            e.Handled = true;
        }

        private void ExpandAll_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void ExpandAll_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            FilesList.SelectedFile.ExpandAll();
        }

        private void CollapseAll_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void CollapseAll_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            FilesList.SelectedFile.CollapseAll();
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

        public static RoutedUICommand ExpandAll = new RoutedUICommand(
            "Expand all",
            "ExpandAll",
            typeof(CustomCommands),
            new InputGestureCollection()
        );

        public static RoutedUICommand CollapseAll = new RoutedUICommand(
            "Collapse all",
            "CollapseAll",
            typeof(CustomCommands),
            new InputGestureCollection()
        );
    }

    // Constants

    internal static class MessagesConsts
    {
        public static string WarningMessageCaption => "Warning";
        public static string ErrorMessageCaption => "Error";
        public static string FileNotSavedMessage => "File \"{0}\" is not saved.\nDo you want to save it before closing?";
        public static string FileCantSaveMessage => "Can't save file \"{0}\".";
        public static string FileCantLoadMessage => "Can't load file \"{0}\".";
    }

    internal static class StringConstants
    {
        public static string FileDialogsFilters => "XML-file|*.xml";
    }
}
