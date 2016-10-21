using System.Windows;
using XmlParserWpf.Commands;
using XmlParserWpf.Model;

namespace XmlParserWpf.ViewModel
{
    public class MethodEditingViewModel: MethodViewModel
    {
        public Window AssociatedWindow { get; set; }

        public RelayCommand OkCommand { get; }
        public RelayCommand CancelCommand { get; }
        public RelayCommand ResetCommand { get; }

        public MethodEditingViewModel(MethodModel method) : base(method)
        {
        }

        public MethodEditingViewModel(MethodModel method, MethodViewModel parent) : base(method, parent)
        {
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
            var p = Parent as MethodModel;
            if (p == null)
                return;

            Name = p.Name;
            Package = p.Package;
            ParamsCount = p.ParamsCount;
            Time = p.Time;

            RefreshAll();
        }

        private void SaveChanges()
        {
            var p = Parent as MethodModel;
            if (p == null)
                return;

            p.Name = Name;
            p.Package = Package;
            p.ParamsCount = ParamsCount;
            p.Time = Time;

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
