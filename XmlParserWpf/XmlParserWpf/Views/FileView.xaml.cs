using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using XmlParserWpf.ViewModel;

namespace XmlParserWpf.Views
{
    /// <summary>
    /// Interaction logic for FileView.xaml
    /// </summary>
    public partial class FileView : UserControl
    {
        public FileView()
        {
            InitializeComponent();
        }

        private void TreeViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;

            TreeViewItem item = sender as TreeViewItem;
            if ((item == null) 
                || (!item.IsSelected))
                return;

            var dc = DataContext as FileViewModel;
            if (dc == null)
                return;

            var method = dc.SelectedValue as MethodViewModel;
            if (method == null)
                return;

            var propertiesWindow = new PropertiesWindow(
                method.GetNewMethodEditingViewModel());

            propertiesWindow.ShowDialog();
        }

        private void TreeView_OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var dc = DataContext as FileViewModel;
            if (dc == null)
                return;

            dc.SelectedValue = e.NewValue;
        }
    }
}
