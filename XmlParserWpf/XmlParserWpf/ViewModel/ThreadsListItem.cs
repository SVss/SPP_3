using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml;
using TracerLib;

namespace XmlParserWpf.ViewModel
{
    public class ThreadsListItem:
        ITimed,
        IExpandable,
        IChangeable,
        INotifyPropertyChanged
    {
        private long _id;
        private long _time;
        public List<MethodsListItem> Methods { get; }
        private bool _expanded;

        public long Id
        {
            get { return _id; }
            set
            {
                if (_id == value)
                    return;
                _id = value;

                OnPropertyChanged();
                OnChange();
            }
        }

        public long Time
        {
            get { return _time; }
            set
            {
                if (_time == value)
                    return;

                _time = value;
                OnPropertyChanged();
                OnChange();
            }
        }

        // Public

        public static ThreadsListItem FromXmlElement(XmlElement xe)
        {
            if (xe.Name != XmlConstants.ThreadTag)
                throw new BadXmlException();

            long id, time;
            try
            {
                id = Convert.ToInt64(xe.Attributes[XmlConstants.ThreadIdAttribute].Value);
                time = Convert.ToInt64(xe.Attributes[XmlConstants.TimeAttribute].Value);
            }
            catch (Exception ex)
            {
                if (ex is XmlException || ex is FormatException || ex is OverflowException)
                    throw new BadXmlException();

                throw;
            }

            ThreadsListItem result = new ThreadsListItem()
            {
                Id = id,
                Time = time
            };

            foreach (XmlElement child in xe.ChildNodes)
            {
                var method = MethodsListItem.FromXmlElement(child, result);
                method.ChangeEvent += delegate { result.OnChange(); };

                result.Methods.Add(method);
            }

            return result;
        }

        public XmlElement ToXmlElement(XmlDocument document)
        {
            XmlElement result = document.CreateElement(XmlConstants.ThreadTag);
            result.SetAttribute(XmlConstants.ThreadIdAttribute, Id.ToString());
            result.SetAttribute(XmlConstants.TimeAttribute, Time.ToString());

            foreach (MethodsListItem item in Methods)
            {
                result.AppendChild(item.ToXmlElement(document));
            }
            return result;
        }

        // IExpandable

        public bool Expanded
        {
            get { return _expanded; }
            set
            {
                if (_expanded == value)
                    return;

                _expanded = value;
                OnPropertyChanged();
            }
        }

        public void ExpandAll()
        {
            Expanded = true;
            foreach (var method in Methods)
            {
                method.ExpandAll();
            }
        }

        public void CollapseAll()
        {
            Expanded = false;
            foreach (var method in Methods)
            {
                method.CollapseAll();
            }
        }

        // IChangeable

        public event ChangeDelegate ChangeEvent;

        public void OnChange()
        {
            ChangeEvent?.Invoke();
        }

        // INotifyPropertyChange

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Internal

        private ThreadsListItem()
        {
            Methods = new List<MethodsListItem>();
        }
    }
}
