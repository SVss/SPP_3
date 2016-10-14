using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace XmlParserWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        private void Open_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Open_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            // TODO: add new file to filesList

            MessageBox.Show("Open", "Opening file");
        }

        private void Exit_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Exit_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            // TODO: check all tabs to be saved
            Close();
        }
    }

    internal static class CustomCommands
    {
        public static RoutedUICommand Open = new RoutedUICommand(
            "Open",
            "Open",
            typeof(CustomCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.O, ModifierKeys.Control)
            }
        );

        public static RoutedUICommand Exit = new RoutedUICommand(
            "Exit",
            "Exit",
            typeof(CustomCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.X, ModifierKeys.Alt)
            }
        );

        // TODO: more commands
    }
}
