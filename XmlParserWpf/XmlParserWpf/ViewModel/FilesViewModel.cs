using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace XmlParserWpf.ViewModel
{
    public class FilesViewModel: ObservableCollection<FilesListItem>
    {
        private int _selectedIndex = NoneSelection;

        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set
            {
                if (_selectedIndex == value)
                    return;

                _selectedIndex = value;
                OnPropertyChanged(
                    new PropertyChangedEventArgs("SelectedIndex"));
                OnPropertyChanged(
                    new PropertyChangedEventArgs("SelectedFile"));
            }
        }

        public FilesListItem SelectedFile => 
            (SelectedIndex != NoneSelection) ? this[SelectedIndex] : null;

        // Public

        public void AddAndSelect(FilesListItem item)
        {
            Add(item);
            SelectedIndex = IndexOf(item);
        }

        public void SelectIfExists(string path)
        {
            if (HasFile(path))
                SelectedIndex = IndexOf(this.First(x => x.Path.Equals(path)));
        }

        public bool HasFile(string path) => this.Any(x => x.Path.Equals(path));

        public void RemoveSelected()
        {
            if (SelectedIndex == NoneSelection)
                return;

            RemoveAt(SelectedIndex);
            if (Count == 0)
                SelectedIndex = NoneSelection;
        }

        // Constants

        public const int NoneSelection = -1;
    }
}
