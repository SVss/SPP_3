using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using XmlParserWpf.Commands;
using XmlParserWpf.Model;
using XmlParserWpf.Views;
using XmlParserWpf.Utils;

namespace XmlParserWpf.ViewModel
{
    public class FileViewModel:
        IExpandable,
        INotifyPropertyChanged
    {
        private readonly FileModel _file;
        private object _selectedValue;
        public ObservableCollection<ThreadViewModel> ThreadsList { get; }

        public RelayCommand ExpandAllCommand { get; }
        public RelayCommand CollapseAllCommand { get; }

        public string Path
        {
            get { return _file.Path; }
            private set
            {
                if (_file.Path == value)
                    return;

                _file.Path = value;
                OnPropertyChanged("Path");
                OnPropertyChanged("Name");
            }
        }

        public string Name => System.IO.Path.GetFileName(Path);

        public bool IsSaved
        {
            get { return _file.IsSaved; }
            set
            {
                if (_file.IsSaved == value)
                    return;

                _file.IsSaved = value;
                OnPropertyChanged("IsSaved");
            }
        }

        public object SelectedValue
        {
            get { return _selectedValue; }
            set
            {
                _selectedValue = value;
                OnPropertyChanged("SelectedValue");
            }
        }

        public void SaveAs(string path)
        {
            Path = path;
            _file.Save();
            OnPropertyChanged("IsSaved");
        }

        public void Save()
        {
            _file.Save();
            OnPropertyChanged("IsSaved");
        }

        // Public
        
        public FileViewModel(FileModel file)
        {
            _file = file;
            ThreadsList = new ObservableCollection<ThreadViewModel>();

            foreach (var threadModel in file.ThreadsList)
            {
                var t = new ThreadViewModel(threadModel);
                t.ChangeEvent += ChangeFile;
                ThreadsList.Add(t);
            }

            ExpandAllCommand = new RelayCommand(ExpandAll);
            CollapseAllCommand = new RelayCommand(CollapseAll);

            AppManager.FileTreeViewSelectedItemChangedHandler = TreeView_OnSelectedItemChanged;
            AppManager.FileTreeViewMouseDoubleClick = TreeViewItem_MouseDoubleClick;
        }

        public void ChangeFile()
        {
            IsSaved = false;
        }

        // IExpandable

        public bool Expanded { get; set; } = true;

        public void ExpandAll(object sender)
        {
            foreach (var thread in ThreadsList)
            {
                thread.ExpandAll(this);
            }
        }

        public void CollapseAll(object sender)
        {
            foreach (var thread in ThreadsList)
            {
                thread.CollapseAll(this);
            }
        }

        // INotifyPropertyChange

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Internals

        private void TreeView_OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (!(sender is TreeView))
                return;
            
            SelectedValue = e.NewValue;
        }

        private void TreeViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if(!(sender is TreeView))
                return;

            var method = SelectedValue as MethodViewModel;
            if (method == null)
                return;

            var propertiesWindow = new PropertiesWindow(
                method.GetNewMethodEditingViewModel());

            propertiesWindow.ShowDialog();

            e.Handled = true;
        }

    }

}
