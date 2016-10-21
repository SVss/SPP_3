using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using XmlParserWpf.ViewModel;
using XmlParserWpf.Views;

namespace XmlParserWpf.Utils
{
    public static class AppManager
    {
        // Window Events handling

        private static Window _fileTabsWindow = null;
        public static Window FileTabsWindow
        {
            set
            {
                if (Equals(_fileTabsWindow, value))
                    return;

                _fileTabsWindow = value;

                if (_windowCancelEventHandler != null)
                {
                    _fileTabsWindow.Closing += _windowCancelEventHandler;
                    _windowCancelEventHandler = null;
                }
            }
        }

        private static CancelEventHandler _windowCancelEventHandler = null;
        public static CancelEventHandler WindowCancelEventHandler
        {
            set
            {
                if (_fileTabsWindow == null)
                {
                    _windowCancelEventHandler = value;
                }
                else
                {
                    _fileTabsWindow.Closing += value;
                    _windowCancelEventHandler = null;
                }
            }
        }

        // TreeView Events handling

        private static TreeView _fileTreeView = null;
        public static TreeView FileTreeView
        {
            set
            {
                if (Equals(_fileTreeView, value))
                    return;

                _fileTreeView = value;

                if (_fileTreeViewSelectedItemChangedHandler != null)
                {
                    _fileTreeView.SelectedItemChanged += _fileTreeViewSelectedItemChangedHandler;
                    _fileTreeViewSelectedItemChangedHandler = null;
                }

                if (_fileTreeViewMouseDoubleClick != null)
                {
                    _fileTreeView.MouseDoubleClick += _fileTreeViewMouseDoubleClick;
                    _fileTreeViewMouseDoubleClick = null;
                }
            }
        }

        private static RoutedPropertyChangedEventHandler<object> _fileTreeViewSelectedItemChangedHandler = null;
        public static RoutedPropertyChangedEventHandler<object> FileTreeViewSelectedItemChangedHandler
        {
            set
            {
                if (_fileTreeView == null)
                {
                    _fileTreeViewSelectedItemChangedHandler = value;
                }
                else
                {
                    _fileTreeView.SelectedItemChanged += value;
                    _fileTreeViewSelectedItemChangedHandler = null;
                }
            }
        }

        private static MouseButtonEventHandler _fileTreeViewMouseDoubleClick = null;
        public static MouseButtonEventHandler FileTreeViewMouseDoubleClick
        {
            set
            {
                if (_fileTreeView == null)
                {
                    _fileTreeViewMouseDoubleClick = value;
                }
                else
                {
                    _fileTreeView.MouseDoubleClick += value;
                    _fileTreeViewMouseDoubleClick = null;
                }
            }
        }

        // Dialog

        public static void ShowPropertiesDialog(MethodEditingViewModel mevm)
        {
            var propertiesWindow = new PropertiesWindow(mevm);
            propertiesWindow.ShowDialog();
        }
        
    }
}
