using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using Microsoft.Win32;
using XmlParserWpf.Model;
using XmlParserWpf.Utils;
using MessageBox = System.Windows.MessageBox;

namespace XmlParserWpf.ViewModel
{
    public class TabsViewModel: BaseViewModel
    {
        public ObservableCollection<FileViewModel> FilesList { get; } = new ObservableCollection<FileViewModel>();
        private int _selectedIndex = NoneSelection;

        public RelayCommand OpenCommand { get; }
        public RelayCommand CloseCommand { get; }
        public RelayCommand SaveCommand { get; }
        public RelayCommand SaveAsCommand { get; }
        public RelayCommand ExitCommand { get; }
        public RelayCommand ExpandAllCommand { get; }
        public RelayCommand CollapseAllCommand { get; }
        //public RelayCommand OpenProperties { get; }

        private static readonly OpenFileDialog OpenFileDialog = new OpenFileDialog()
        {
            Filter = StringConstants.FileDialogsFilters
        };

        private static readonly SaveFileDialog SaveFileDialog = new SaveFileDialog()
        {
            Filter = StringConstants.FileDialogsFilters
        };

        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set
            {
                if (_selectedIndex == value)
                    return;

                _selectedIndex = value;
                OnPropertyChanged("SelectedIndex");
                OnPropertyChanged("SelectedFile");
            }
        }

        public FileViewModel SelectedFile => 
            (SelectedIndex != NoneSelection) ? FilesList[SelectedIndex] : null;

        // Public

        public TabsViewModel()
        {
            EventsManager.SubscribeFileTabsWindowCancelEvent(FileTabsWindow_OnClosing);

            OpenCommand = new RelayCommand(Open_OnExecuted);
            CloseCommand = new RelayCommand(Close_OnExecuted, Close_OnCanExecute);
            SaveCommand = new RelayCommand(Save_OnExecuted, Save_OnCanExecute);
            SaveAsCommand = new RelayCommand(SaveAs_OnExecuted, SaveAs_OnCanExecute);
            ExitCommand = new RelayCommand(Exit_OnExecuted);
            ExpandAllCommand = new RelayCommand(ExpandAll_OnExecuted);
            CollapseAllCommand = new RelayCommand(CollapseAll_OnExecuted);
        }

        public void AddAndSelect(FileViewModel item)
        {
            FilesList.Add(item);
            SelectedIndex = FilesList.IndexOf(item);
        }

        public void SelectIfExists(string path)
        {
            if (HasFile(path))
                SelectedIndex = FilesList.IndexOf(FilesList.First(x => x.Path.Equals(path)));
        }

        public bool HasFile(string path) => FilesList.Any(x => x.Path.Equals(path));

        public void RemoveSelected()
        {
            if (SelectedIndex == NoneSelection)
                return;

            FilesList.RemoveAt(SelectedIndex);
            if (FilesList.Count == 0)
                SelectedIndex = NoneSelection;
        }

        public void OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (SelectedFile == null)
                return;

            SelectedFile.SelectedValue =
                e.NewValue as MethodViewModel;
        }

        public bool CloseAllSucceeded()
        {
            SelectedIndex = 0; // start closing from 1st file
            while (FilesList.Count > 0)
            {
                if (CanCloseFile(SelectedFile)) // ask user here
                    RemoveSelected();
                else
                {
                    return false;
                }
            }
            return true;
        }

        // Internals

        // Open command

        private void Open_OnExecuted(object sender)
        {
            bool? openResult = OpenFileDialog.ShowDialog();
            if (openResult == null || !openResult.Value)
                return;

            string path = OpenFileDialog.FileNames[0];
            if (HasFile(path))
            {
                SelectIfExists(path);
                return;
            }

            try
            {
                AddAndSelect(new FileViewModel(
                    FileModel.LoadFromFile(path)));
            }
            catch (BadXmlException)
            {
                MessageBox.Show(
                    string.Format(MessagesConstants.FileCantLoadMessage, path),
                    MessagesConstants.ErrorMessageCaption,
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        // Close command

        private bool Close_OnCanExecute(object sender)
        {
            return (FilesList.Count > 0);
        }

        private void Close_OnExecuted(object sender)
        {
            if (CanCloseFile(SelectedFile))
                RemoveSelected();
        }

        public static bool CanCloseFile(FileViewModel file)
        {
            if (file.IsSaved)
                return true;

            MessageBoxResult answ = MessageBox.Show(
                string.Format(MessagesConstants.FileNotSavedMessage, file.Path),
                MessagesConstants.WarningMessageCaption,
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

        // SaveAs command

        private bool SaveAs_OnCanExecute(object sender)
        {
            return (FilesList.Count > 0);
        }

        private void SaveAs_OnExecuted(object sender)
        {
            bool? saveResult = SaveFileDialog.ShowDialog();
            if (saveResult == null || !saveResult.Value)
                return;

            string path = SaveFileDialog.FileNames[0];
            try
            {
                SelectedFile.SaveAs(path);
            }
            catch (Exception)
            {
                MessageBox.Show(
                    string.Format(MessagesConstants.FileCantSaveMessage, path),
                    MessagesConstants.ErrorMessageCaption,
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        // Save command

        private bool Save_OnCanExecute(object sender)
        {
            return (FilesList.Count > 0)
                && (!SelectedFile.IsSaved);  // TODO: check if files exists
        }

        private void Save_OnExecuted(object sender)
        {
            SelectedFile.Save();
        }

        // ExpandAll command

        private void ExpandAll_OnExecuted(object sender)
        {
            SelectedFile.ExpandAll(this);
        }

        // CollapseAll command

        private void CollapseAll_OnExecuted(object sender)
        {
            SelectedFile.CollapseAll(this);
        }

        // Exit command

        private void Exit_OnExecuted(object sender)
        {
            Application.Current.MainWindow.Close();
        }

        // Window Closing Event

        private void FileTabsWindow_OnClosing(object sender, CancelEventArgs e)
        {
            e.Cancel = !CloseAllSucceeded();
        }

        // Constants

        private const int NoneSelection = -1;
    }

    // Constants

    internal static class MessagesConstants
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
