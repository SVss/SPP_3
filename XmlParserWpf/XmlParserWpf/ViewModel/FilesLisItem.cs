using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml;
using TracerLib;

namespace XmlParserWpf.ViewModel
{
    public class FilesListItem:
        IExpandable,
        INotifyPropertyChanged
    {
        private bool _isSaved;
        private MethodsListItem _selectedMethod;
        public List<ThreadsListItem> ThreadsList { get; }

        public string Path
        {
            get { return _path; }
            private set
            {
                if (_path == value)
                    return;

                _path = value;
                OnPropertyChanged("Path");
                OnPropertyChanged("Name");
            }
        }

        public string Name => System.IO.Path.GetFileName(Path);

        public bool IsSaved
        {
            get { return _isSaved; }
            set
            {
                if (_isSaved == value)
                    return;

                _isSaved = value;
                OnPropertyChanged("IsSaved");
            }
        }

        public MethodsListItem SelectedMethod
        {
            get { return _selectedMethod; }
            set
            {
                _selectedMethod = value;
                OnPropertyChanged("SelectedMethod");
            }
        }

        public void SaveAs(string path)
        {
            Path = path;
            Save();
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
