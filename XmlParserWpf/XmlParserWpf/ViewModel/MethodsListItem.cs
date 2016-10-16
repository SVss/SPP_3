using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml;
using TracerLib;

namespace XmlParserWpf.ViewModel
{
    public class MethodsListItem : ITimed, IExpandable, IChangeable, INotifyPropertyChanged, ICloneable   // without Nested reference
    {
        private string _name;
        private string _package;
        private long _paramsCount;
        private long _time;
        private bool _expanded;
        public List<MethodsListItem> Nested { get; }

        public object Parent { get; private set; }

        public string Name
        {
            get { return _name; }
            set
            {
                if (_name == value)
                    return;

                _name = value;
                OnPropertyChanged("Name");
                OnChange();
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
                OnChange();
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

                long delta = value - _time;
                _time = value;

                OnPropertyChanged("Time");
                OnChange();

                ITimed timed = Parent as ITimed;
                if (timed != null)
                    timed.Time += delta;
            }
        }

        public bool Expanded
        {
            get { return _expanded; }
            set
            {
                if (_expanded == value)
                    return;

                _expanded = value;
                OnPropertyChanged("Expanded");
            }
        }

        public void ExpandAll()
        {
            Expanded = true;
            foreach (var method in Nested)
            {
                method.ExpandAll();
            }
        }

        public void CollapseAll()
        {
            Expanded = false;
            foreach (var method in Nested)
            {
                method.CollapseAll();
            }
        }

        // Public

        public static MethodsListItem FromXmlElement(XmlElement xe, object parent = null)
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
                var nested = FromXmlElement(child, result);
                nested.ChangeEvent += delegate { result.OnChange(); };

                result.Nested.Add(nested);
            }

            result.Parent = parent;
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

        // ICloneable

        public object Clone()
        {
            MethodsListItem result = new MethodsListItem
            {
                Name = Name,
                Package = Package,
                ParamsCount = ParamsCount,
                Time = Time
            };
            return result;
        }

        // Internal 

        private MethodsListItem()
        {
            Nested = new List<MethodsListItem>();
        }
    }
}
