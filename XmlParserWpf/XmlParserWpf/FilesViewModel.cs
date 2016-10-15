using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace XmlParserWpf
{
    public class FilesViewModel: ObservableCollection<FilesListItem>
    {
        private int _selectedIndex = -1;

        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set
            {
                _selectedIndex = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndex"));
            }
        }

        public FilesListItem SelectedFile => (SelectedIndex > 0 ? this[SelectedIndex] : null);

        public void AddAndSelect(FilesListItem item)
        {
            Add(item);
            SelectedIndex = IndexOf(item);
        }

        public bool HasFile(string path)
        {
            return this.Any(x => x.Path.Equals(path));
        }

        public void SelectIfExists(string path)
        {
            if (HasFile(path))
                SelectedIndex = IndexOf(this.First(x => x.Path.Equals(path)));
        }

        public void RemoveSelected()
        {
            if (SelectedIndex < 0)
                return;

            RemoveAt(SelectedIndex);

            if (Count < 0)
                SelectedIndex = -1;
        }
    }
}
