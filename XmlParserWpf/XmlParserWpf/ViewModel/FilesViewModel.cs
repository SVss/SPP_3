﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using XmlParserWpf.Commands;
using XmlParserWpf.Model;
using MessageBox = System.Windows.MessageBox;
using OpenFileDialog = System.Windows.Forms.OpenFileDialog;

namespace XmlParserWpf.ViewModel
{
    public class FilesViewModel: ObservableCollection<FilesListItem>
    {
        public RelayCommand OpenCommand { get; }
        public RelayCommand CloseCommand { get; }
        public RelayCommand SaveCommand { get; }
        public RelayCommand SaveAsCommand { get; }
        public RelayCommand ExpandAllCommand { get; }
        public RelayCommand CollapseAllCommand { get; }

        private static readonly OpenFileDialog OpenFileDialog = new OpenFileDialog()
        {
            Filter = StringConstants.FileDialogsFilters
        };

        private static readonly SaveFileDialog SaveFileDialog = new SaveFileDialog()
        {
            Filter = StringConstants.FileDialogsFilters
        };


        private int _selectedIndex = NoneSelection;

        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set
            {
                if (_selectedIndex == value)
                    return;

                _selectedIndex = value;
                OnPropertyChanged(
                    new PropertyChangedEventArgs("SelectedIndex"));
                OnPropertyChanged(
                    new PropertyChangedEventArgs("SelectedFile"));
            }
        }

        public FilesListItem SelectedFile => 
            (SelectedIndex != NoneSelection) ? this[SelectedIndex] : null;

        // Public

        public FilesViewModel()
        {
            OpenCommand = new RelayCommand(Open_OnExecuted);
            CloseCommand = new RelayCommand(Close_OnExecuted, Close_OnCanExecute);
            SaveCommand = new RelayCommand(Save_OnExecuted, SaveAs_OnCanExecute);
            SaveAsCommand = new RelayCommand(SaveAs_OnExecuted, SaveAs_OnCanExecute);
            ExpandAllCommand = new RelayCommand(ExpandAll_OnExecuted);
            CollapseAllCommand = new RelayCommand(CollapseAll_OnExecuted);
        }

        public void AddAndSelect(FilesListItem item)
        {
            Add(item);
            SelectedIndex = IndexOf(item);
        }

        public void SelectIfExists(string path)
        {
            if (HasFile(path))
                SelectedIndex = IndexOf(this.First(x => x.Path.Equals(path)));
        }

        public bool HasFile(string path) => this.Any(x => x.Path.Equals(path));

        public void RemoveSelected()
        {
            if (SelectedIndex == NoneSelection)
                return;

            RemoveAt(SelectedIndex);
            if (Count == 0)
                SelectedIndex = NoneSelection;
        }


        // TreeView

        public void OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (SelectedFile == null)
                return;

            SelectedFile.SelectedMethod =
                e.NewValue as MethodsListItem;
        }

        public void OnPreviewItem(object sender, MouseButtonEventArgs e)
        {
            TreeViewItem item = sender as TreeViewItem;
            if ((item == null)
                || (!item.IsSelected)
                || (SelectedFile.SelectedMethod == null))
                return;

            var propertiesWindow = new PropertiesWindow(
                SelectedFile.SelectedMethod);

            propertiesWindow.ShowDialog();
            e.Handled = true;
        }


        // Internals

        
        // Open command

        private void Open_OnExecuted(object sender)
        {
            DialogResult openResult = OpenFileDialog.ShowDialog();
            if (openResult != System.Windows.Forms.DialogResult.OK)
                return;

            string path = OpenFileDialog.FileNames[0];
            if (HasFile(path))
            {
                SelectIfExists(path);
                return;
            }

            try
            {
                AddAndSelect(FilesListItem.LoadFromFile(path));
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


        // Close command

        private bool Close_OnCanExecute(object sender)
        {
            return (Count > 0);
        }

        private void Close_OnExecuted(object sender)
        {
            if (CanCloseFile(SelectedFile))
                RemoveSelected();
        }

        public static bool CanCloseFile(FilesListItem file)
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


        // SaveAs command

        private bool SaveAs_OnCanExecute(object sender)
        {
            return (Count > 0);
        }

        private void SaveAs_OnExecuted(object sender)
        {
            DialogResult saveResult = SaveFileDialog.ShowDialog();
            if (saveResult != System.Windows.Forms.DialogResult.OK)
                return;

            string path = SaveFileDialog.FileNames[0];
            try
            {
                SelectedFile.SaveAs(path);
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


        // Save command

        private bool Save_OnCanExecute(object sender)
        {
            return (Count > 0)
                && (!SelectedFile.IsSaved);  // TODO: check if files exists
        }

        private void Save_OnExecuted(object sender)
        {
            SelectedFile.Save();
        }

        
        // ExpandAll command

        private void ExpandAll_OnExecuted(object sender)
        {
            SelectedFile.ExpandAll();
        }

        // CollapseAll command

        private void CollapseAll_OnExecuted(object sender)
        {
            SelectedFile.CollapseAll();
        }

        // Exit command

        private void Exit_OnExecuted(object sender)
        {
            // close main window
        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            e.Cancel = !CloseAllSucceeded();
        }


        // Constants

        private const int NoneSelection = -1;

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

        public bool CloseAllSucceeded()
        {
            SelectedIndex = 0; // start closing from 1st file
            while (Count > 0)
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

    }
}
