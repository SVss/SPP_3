using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using XmlParserWpf.ViewModel;
using XmlParserWpf.Views;

namespace XmlParserWpf.Utils
{
    public static class EventsManager
    {
        // Window Events

        private static Window _fileTabsWindow = null;
        public static void ProvideFileTabsWindowToSubscribeEvents(Window fileTabsWindow)
        {
            if (Equals(_fileTabsWindow, fileTabsWindow))
                return;

            _fileTabsWindow = fileTabsWindow;

            if (_windowCancelEventHandler != null)
            {
                _fileTabsWindow.Closing += _windowCancelEventHandler;
                _windowCancelEventHandler = null;
            }
        }

        private static CancelEventHandler _windowCancelEventHandler = null;
        public static void SubscribeFileTabsWindowCancelEvent(CancelEventHandler windowCancelEventHandler)
        {
            if (_fileTabsWindow == null)
            {
                _windowCancelEventHandler = windowCancelEventHandler;
            }
            else
            {
                _fileTabsWindow.Closing += windowCancelEventHandler;
                _windowCancelEventHandler = null;
            }
        }

        // TreeView Events

        private static TreeView _fileTreeView = null;
        public static void ProvideFileTreeViewToSubscribeEvents(TreeView fileTreeView)
        {
            if (Equals(_fileTreeView, fileTreeView))
                return;

            _fileTreeView = fileTreeView;

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
        
        private static RoutedPropertyChangedEventHandler<object> _fileTreeViewSelectedItemChangedHandler = null;
        public static void SubscribeFileTreeViewSelectedItemChangedEvent(RoutedPropertyChangedEventHandler<object> selectedItemChangedEventHandler)
        {
            if (_fileTreeView == null)
            {
                _fileTreeViewSelectedItemChangedHandler = selectedItemChangedEventHandler;
            }
            else
            {
                _fileTreeView.SelectedItemChanged += selectedItemChangedEventHandler;
                _fileTreeViewSelectedItemChangedHandler = null;
            }
        }

        private static MouseButtonEventHandler _fileTreeViewMouseDoubleClick = null;
        public static void SubscribeFileTreeViewMouseDoubleClick(MouseButtonEventHandler mouseButtonEventHandler)
        {
            if (_fileTreeView == null)
            {
                _fileTreeViewMouseDoubleClick = mouseButtonEventHandler;
            }
            else
            {
                _fileTreeView.MouseDoubleClick += mouseButtonEventHandler;
                _fileTreeViewMouseDoubleClick = null;
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
