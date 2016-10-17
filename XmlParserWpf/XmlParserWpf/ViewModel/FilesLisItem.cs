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
        private string _path;
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
                OnPropertyChanged();
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
                OnPropertyChanged();
            }
        }

        public MethodsListItem SelectedMethod
        {
            get { return _selectedMethod; }
            set
            {
                _selectedMethod = value;
                OnPropertyChanged();
            }
        }

        // Public

        public static FilesListItem LoadFromFile(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException();
            }

            FilesListItem result = new FilesListItem()
            {
                Path = path,
                IsSaved = false
            };

            var doc = new XmlDocument();
            try
            {
                doc.Load(path);
            }
            catch (XmlException ex)
            {
                throw new BadXmlException(ErrorLoadingMessage, ex);
            }
            result.LoadFromXmlDocument(doc);

            return result;
        }

        public void SaveAs(string path)
        {
            Path = path;
            Save();
        }

        public void Save()
        {
            XmlDocument doc = new XmlDocument();
            XmlElement root = (XmlElement)doc.AppendChild(
                doc.CreateElement(XmlConstants.RootTag));

            foreach (ThreadsListItem item in ThreadsList)
            {
                root.AppendChild(item.ToXmlElement(doc));
            }

            doc.Save(Path);
            IsSaved = true;
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

        // Internal

        private void LoadFromXmlDocument(XmlDocument doc)
        {
            XmlElement xe = doc.FirstChild as XmlElement;
            if (xe == null || xe.Name != XmlConstants.RootTag)
            {
                throw new BadXmlException();
            }

            foreach (XmlElement child in xe.ChildNodes)
            {
                var thread = ThreadsListItem.FromXmlElement(child);
                thread.ChangeEvent += delegate { IsSaved = false; };
                ThreadsList.Add(thread);
            }

            IsSaved = true;
        }

        private FilesListItem()
        {
            ThreadsList = new List<ThreadsListItem>();
        }

        // Constants

        private const string ErrorLoadingMessage = "Error loading XML-file";
    }
}
