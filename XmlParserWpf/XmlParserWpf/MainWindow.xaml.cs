using System.ComponentModel;
using XmlParserWpf.ViewModel;

namespace XmlParserWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            var dc = this.DataContext as TabsViewModel;
            if (dc == null) return;

            e.Cancel = !dc.CloseAllSucceeded();
        }

    }
}
