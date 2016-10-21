using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using XmlParserWpf.Commands;
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
        }

        public void Save()
        {
            _file.Save();
        }

        // Public

        public FileViewModel(RelayCommand expandAllCommand, RelayCommand collapseAllCommand)
        {
            ExpandAllCommand = expandAllCommand;
            CollapseAllCommand = collapseAllCommand;
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

            ExpandAllCommand = new RelayCommand(ExpandAll);
            CollapseAllCommand = new RelayCommand(CollapseAll);
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

    }

}
