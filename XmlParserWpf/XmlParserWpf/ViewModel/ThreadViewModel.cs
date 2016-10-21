using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using XmlParserWpf.Model;

namespace XmlParserWpf.ViewModel
{
    public class ThreadViewModel:
        ITimed,
        IExpandable,
        IChangeable,
        INotifyPropertyChanged
    {
        private readonly ThreadModel _thread;
        private bool _expanded;
        public ObservableCollection<MethodViewModel> Methods { get; }

        public uint Id
        {
            get { return _thread.Id; }
            set
            {
                if (_thread.Id == value)
                    return;
                _thread.Id = value;

                OnPropertyChanged("Id");
                OnChange();
            }
        }

        public uint Time
        {
            get { return _thread.Time; }
            set
            {
                if (_thread.Time == value)
                    return;

                _thread.Time = value;
                OnPropertyChanged("Time");
                OnChange();
            }
        }

        // Public

        public ThreadViewModel(ThreadModel threadModel)
        {
            _thread = threadModel;
            Methods = new ObservableCollection<MethodViewModel>();

            foreach (var method in threadModel.Methods)
            {
                var m = new MethodViewModel(method);
                m.ChangeEvent += OnChange;
                Methods.Add(m);
            }
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
            foreach (var method in Methods)
            {
                method.ExpandAll(this);
            }
        }

        public void CollapseAll(object sender)
        {
            Expanded = false;
            foreach (var method in Methods)
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

    }
}
