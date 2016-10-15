using System.Collections.ObjectModel;
using System.ComponentModel;

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

        public void AddAndSelect(FilesListItem item)
        {
            Add(item);
            SelectedIndex = IndexOf(item);
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
