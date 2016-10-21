using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using XmlParserWpf.Model;

namespace XmlParserWpf.ViewModel
{
    public class FileViewModel:
        IExpandable,
        INotifyPropertyChanged
    {
        private readonly FileModel _file;
        private object _selectedValue;
        public ObservableCollection<ThreadViewModel> ThreadsList { get; }

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
        }

        public void Save()
        {
            _file.Save();
        }

        // Public

        public FileViewModel()
        {
            ThreadsList = new ObservableCollection<ThreadViewModel>();
        }

        public FileViewModel(FileModel file)
        {
            _file = file;
            ThreadsList = new ObservableCollection<ThreadViewModel>();

            foreach (var threadModel in file.ThreadsList)
            {
                ThreadsList.Add(new ThreadViewModel(threadModel));
            }
        }

        // TreeView

        public void OnPreviewItem(object sender, MouseButtonEventArgs e)
        {
            var item = sender as MethodModel;

            if (item != null)
            {
                var propertiesWindow = new PropertiesWindow(item);
                propertiesWindow.ShowDialog();
            }
            e.Handled = true;
        }


        // IExpandable

        public bool Expanded { get; set; } = true;

        public void ExpandAll()
        {
            foreach (var thread in ThreadsList)
            {
                thread.ExpandAll();
            }
        }

        public void CollapseAll()
        {
            foreach (var thread in ThreadsList)
            {
                thread.CollapseAll();
            }
        }

        // INotifyPropertyChange

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }

}
