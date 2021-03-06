﻿using System;
using System.Collections.ObjectModel;
using XmlParserWpf.Model;

namespace XmlParserWpf.ViewModel
{
    public class MethodViewModel : BaseViewModel, ICloneable, ITimed, IExpandable, IChangeable
    {
        private readonly MethodModel _method;
        private bool _expanded;
        public ObservableCollection<MethodViewModel> NestedMethods { get; }

        public object Parent { get; private set; }

        public string Name
        {
            get { return _method.Name; }
            set
            {
                if (_method.Name == value)
                    return;

                _method.Name = value;
                OnPropertyChanged("Name");
                OnChange();
            }
        }

        public string Package
        {
            get { return _method.Package; }
            set
            {
                if (_method.Package == value)
                    return;

                _method.Package = value;
                OnPropertyChanged("Package");
                OnChange();
            }
        }

        public uint ParamsCount
        {
            get { return _method.ParamsCount; }
            set
            {
                if (_method.ParamsCount == value)
                    return;

                _method.ParamsCount = value;
                OnPropertyChanged("ParamsCount");
                OnChange();
            }
        }

        public uint Time
        {
            get { return _method.Time; }
            set
            {
                if (_method.Time == value)
                    return;

                long delta = value - _method.Time;

                ITimed timed = Parent as ITimed;
                if (timed != null)
                {
                    long newTime = timed.Time + delta;
                    if (newTime < 0)
                    {
                        return;
                    }
                    timed.Time = (uint)newTime;
                }

                _method.Time = value;
                OnPropertyChanged("Time");
                OnChange();
            }
        }

        // Public

        public MethodViewModel(MethodModel method):
            this(method, null)
        {
        }

        public MethodViewModel(MethodModel method, ITimed parent, bool createNestedTree = true)
        {
            _method = method;
            Parent = parent;
            NestedMethods = new ObservableCollection<MethodViewModel>();

            if (!createNestedTree)
                return;

            foreach (var nestedMethod in method.NestedMethods)
            {
                var m = new MethodViewModel(nestedMethod, this);
                m.ChangeEvent += OnChange;
                NestedMethods.Add(m);
            }
        }

        // Get MethodEditingViewModel for PropertiesWindow

        public MethodEditingViewModel GetNewMethodEditingViewModel()
        {
            return new MethodEditingViewModel(this);
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
                OnPropertyChanged("Expanded");
            }
        }

        public void ExpandAll(object sender)
        {
            Expanded = true;
            foreach (var method in NestedMethods)
            {
                method.ExpandAll(this);
            }
        }

        public void CollapseAll(object sender)
        {
            Expanded = false;
            foreach (var method in NestedMethods)
            {
                method.CollapseAll(this);
            }
        }

        // IChangeable

        public event ChangeDelegate ChangeEvent;

        public void OnChange()
        {
            ChangeEvent?.Invoke();
        }
        
        // ICloneable

        public object Clone()
        {
            var methodClone = (MethodModel) _method.Clone();
            var result = new MethodViewModel(methodClone, null, false);
            return result;
        }
    }
}
