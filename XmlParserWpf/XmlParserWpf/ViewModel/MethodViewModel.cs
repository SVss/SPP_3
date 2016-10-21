using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using XmlParserWpf.Model;

namespace XmlParserWpf.ViewModel
{
    public class MethodViewModel :
        ITimed,
        IExpandable,
        IChangeable,
        INotifyPropertyChanged
    {
        protected MethodModel _method;
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

        public MethodViewModel(MethodModel method, MethodViewModel parent)
        {
            _method = method;
            Parent = parent;
            NestedMethods = new ObservableCollection<MethodViewModel>();

            foreach (var nestedMethod in method.NestedMethods)
            {
                NestedMethods.Add(new MethodViewModel(nestedMethod, this));
            }
        }

        public MethodEditingViewModel GetNewMethodEditingViewModel()
        {
            return new MethodEditingViewModel((MethodModel)_method.Clone(), this);
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

        // INotifyPropertyChange

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Internals

        private MethodViewModel()
        {
            NestedMethods = new ObservableCollection<MethodViewModel>();
        }

    }
}
