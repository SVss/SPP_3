﻿using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Xml;
using TracerLib;
using TracerLibXmlParser;

namespace XmlParserWpf
{
    public class FilesListItem: INotifyPropertyChanged
    {
        public string Name => System.IO.Path.GetFileName(Path);
        public List<ThreadsListItem> ThreadsList { get; }

        private string _path;
        private bool _isSaved;
        private MethodsListItem _selectedMethod;
        private bool _isExpanded;

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

        public bool IsExpanded { get; private set; } = true;

        public void ExpandAll()
        {
            IsExpanded = true;
            OnPropertyChanged("IsExpanded");
        }

        public void CollapseAll()
        {
            IsExpanded = false;
            OnPropertyChanged("IsExpanded");
        }

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
                throw new BadXmlException("Error loading XML-file", ex);
            }
            result.FromXmlDocument(doc);

            return result;
        }

        public void SaveAs(string path)
        {
            Path = path;
            Save();
        }

        public void Save()
        {
            XmlDocument result = new XmlDocument();
            XmlElement root = (XmlElement)result.AppendChild(result.CreateElement(XmlConstants.RootTag));

            foreach (ThreadsListItem item in ThreadsList)
            {
                root.AppendChild(item.ToXmlElement(result));
            }

            result.Save(Path);
            IsSaved = true;
        }
        
        // Internal

        private void FromXmlDocument(XmlDocument doc)
        {
            XmlElement xe = doc.FirstChild as XmlElement;
            if (xe == null || xe.Name != XmlConstants.RootTag)
            {
                throw new BadXmlException();
            }

            foreach (XmlElement child in xe.ChildNodes)
            {
                var thread = ThreadsListItem.FromXmlElement(child);
                thread.PropertyChanged += delegate { IsSaved = false; };
                ThreadsList.Add(thread);
            }

            IsSaved = true;
        }

        private FilesListItem()
        {
            ThreadsList = new List<ThreadsListItem>();
        }

        // INotifyPropertyChange

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
