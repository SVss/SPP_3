using System.Windows;
using XmlParserWpf.Utils;

namespace XmlParserWpf.ViewModel
{
    public class MethodEditingViewModel: BaseViewModel, ITimed
    {
        private readonly MethodViewModel _method;
        private readonly MethodViewModel _realMethod;

        public string Name
        {
            get { return _method.Name; }
            set
            {
                if (_method.Name == value)
                    return;

                _method.Name = value;
                OnPropertyChanged("Name");
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
            }
        }

        public uint Time
        {
            get { return _method.Time; }
            set
            {
                if (_method.Time == value)
                    return;
                
                _method.Time = value;
                OnPropertyChanged("Time");
            }
        }

        public Window AssociatedWindow { get; set; }

        public RelayCommand OkCommand { get; }
        public RelayCommand CancelCommand { get; }
        public RelayCommand ResetCommand { get; }

        public MethodEditingViewModel(MethodViewModel method)
        {
            _realMethod = method;   // link
            _method = (MethodViewModel) method.Clone();

            OkCommand = new RelayCommand(OkCommand_OnExecute);
            CancelCommand = new RelayCommand(CancelCommand_OnExecute);
            ResetCommand = new RelayCommand(ResetCommand_OnExecute);
        }

        // Internals

        private void OkCommand_OnExecute(object sender)
        {
            SaveChanges();
            AssociatedWindow?.Close();
        }

        private void ResetCommand_OnExecute(object sender)
        {
            ResetChanges();
        }

        private void CancelCommand_OnExecute(object sender)
        {
            AssociatedWindow?.Close();
        }

        private void ResetChanges()
        {
            Name = _realMethod.Name;
            Package = _realMethod.Package;
            ParamsCount = _realMethod.ParamsCount;
            Time = _realMethod.Time;

            RefreshAll();
        }

        private void SaveChanges()
        {
            _realMethod.Name = Name;
            _realMethod.Package = Package;
            _realMethod.ParamsCount = ParamsCount;
            _realMethod.Time = Time;

            RefreshAll();
        }

        private void RefreshAll()
        {
            OnPropertyChanged("Name");
            OnPropertyChanged("Package");
            OnPropertyChanged("ParamsCount");
            OnPropertyChanged("Time");
        }
    }
}
