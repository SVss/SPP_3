using System.Collections.Generic;
using System.IO;
using System.Xml;
using TracerLib;

namespace XmlParserWpf
{
    internal class FilesListItem
    {
        public string Path { get; set; }
        public string Name => System.IO.Path.GetFileName(Path);
        public bool IsSaved { get; set; }

        public List<ThreadsListItem> ThreadsList { get; }

        private FilesListItem()
        {
            ThreadsList = new List<ThreadsListItem>();
        }

        private static FilesListItem LoadFromFile(string path)
        {
            var result = new FilesListItem();

            // TODO: load and parse XML-file here

            return result;
        }
        
        public void OnChanged()
        {
            IsSaved = false;
        }
    }
}
