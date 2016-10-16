using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml;
using TracerLib;

namespace TracerLibXmlParser
{
    public class ThreadsListItem: INotifyPropertyChanged
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
                result.Methods.Add(MethodsListItem.FromXmlElement(child));
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

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class MethodsListItem: INotifyPropertyChanged
    {
        private string _name;
        private string _package;
        private long _paramsCount;
        private long _time;
        public List<MethodsListItem> Nested { get; }

        public string Name
        {
            get { return _name; }
            set
            {
                if (_name == value)
                    return;

                _name = value;
                OnPropertyChanged("Name");
            }
        }

        public string Package
        {
            get { return _package; }
            set
            {
                if (_package == value)
                    return;

                _package = value;
                OnPropertyChanged("Package");
            }
        }

        public long ParamsCount
        {
            get { return _paramsCount; }
            set
            {
                if (_paramsCount == value)
                    return;

                _paramsCount = value;
                OnPropertyChanged("ParamsCount");
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

        public static MethodsListItem FromXmlElement(XmlElement xe)
        {
            if (xe.Name != XmlConstants.MethodTag)
                throw new BadXmlException();

            string name, package;
            long paramsCount, time;
            try
            {
                name = xe.Attributes[XmlConstants.NameAttribute].Value;
                package = xe.Attributes[XmlConstants.PackageAttribute].Value;
                paramsCount = Convert.ToInt64(xe.Attributes[XmlConstants.ParamsAttribute].Value);
                time = Convert.ToInt64(xe.Attributes[XmlConstants.TimeAttribute].Value);
            }
            catch (Exception ex)
            {
                if (ex is XmlException || ex is FormatException || ex is OverflowException)
                    throw new BadXmlException();

                throw;
            }

            MethodsListItem result = new MethodsListItem()
            {
                Name = name,
                Package = package,
                ParamsCount = paramsCount,
                Time = time
            };

            foreach (XmlElement child in xe.ChildNodes)
            {
                result.Nested.Add(FromXmlElement(child));
            }

            return result;
        }

        public XmlElement ToXmlElement(XmlDocument document)
        {
            XmlElement result = document.CreateElement(XmlConstants.MethodTag);
            result.SetAttribute(XmlConstants.NameAttribute, Name);
            result.SetAttribute(XmlConstants.TimeAttribute, Time.ToString());

            result.SetAttribute(XmlConstants.PackageAttribute, Package);
            result.SetAttribute(XmlConstants.ParamsAttribute, ParamsCount.ToString());

            foreach (var child in Nested)
            {
                result.AppendChild(child.ToXmlElement(document));
            }
            return result;
        }

        // Internal 

        private MethodsListItem()
        {
            Nested = new List<MethodsListItem>();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
