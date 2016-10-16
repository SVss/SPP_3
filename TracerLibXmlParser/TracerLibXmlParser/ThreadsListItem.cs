using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml;
using TracerLib;

namespace TracerLibXmlParser
{
    internal interface ITimed
    {
        long Time { get; set; }
    }

    public class ThreadsListItem: ITimed, INotifyPropertyChanged
    {
        private long _id;
        private long _time;
        public List<MethodsListItem> Methods { get; }

        public long Id
        {
            get { return _id; }
            set
            {
                if (_id == value)
                    return;
                _id = value;

                OnPropertyChanged("Id");
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
                OnPropertyChanged("Time");
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
                result.Methods.Add(MethodsListItem.FromXmlElement(child, result));
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

        // Internal

        private ThreadsListItem()
        {
            Methods = new List<MethodsListItem>();
        }

        // INotifyPropertyChange

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
