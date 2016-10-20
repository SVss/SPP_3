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

                OnPropertyChanged("Id");
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
                OnPropertyChanged("Time");
                OnChange();
            }
        }

        // Public

       
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
