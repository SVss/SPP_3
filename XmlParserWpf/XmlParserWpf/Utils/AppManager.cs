using System.ComponentModel;
using System.Windows;
using XmlParserWpf.ViewModel;

namespace XmlParserWpf.Utils
{
    public static class AppManager
    {
        public static Window FileTabsWindow { get; private set; }

        private static CancelEventHandler _windowCancelEventHandler = null;
        public static CancelEventHandler WindowCancelEventHandler
        {
            set
            {
                if (FileTabsWindow == null)
                {
                    _windowCancelEventHandler = value;
                }
                else
                {
                    FileTabsWindow.Closing += value;
                }
            }
        }

        public static void SetFileTabsWindow(Window window)
        {
            FileTabsWindow = window;

            if (_windowCancelEventHandler != null)
            {
                window.Closing += _windowCancelEventHandler;
            }
        }

        public static void ShowPropertiesDialog(MethodEditingViewModel mevm)
        {
            var propertiesWindow = new PropertiesWindow(mevm);
            propertiesWindow.ShowDialog();
        }
        
    }
}
