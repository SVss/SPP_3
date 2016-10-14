using System.Collections.Generic;
using System.IO;
using System.Xml;
using TracerLib;
using TracerLibXmlParser;

namespace XmlParserWpf
{
    internal class FilesListItem
    {
        public string Path { get; set; }
        public string Name => System.IO.Path.GetFileName(Path);
        public bool IsSaved { get; set; }

        public List<ThreadsListItem> ThreadsList { get; }

        public static FilesListItem LoadFromFile(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException();
            }

            FilesListItem result = new FilesListItem()
            {
                Path = path,
                IsSaved = false
            };

            var doc = new XmlDocument();
            try
            {
                doc.Load(path);
            }
            catch (XmlException ex)
            {
                throw new BadXmlException("Error loading XML-file", ex);
            }
            result.FromXmlDocument(doc);

            return result;
        }

        public void OnChanged()
        {
            IsSaved = false;
        }

        // Internal

        private void FromXmlDocument(XmlDocument doc)
        {
            XmlElement xe = doc.FirstChild as XmlElement;
            if (xe == null || xe.Name != XmlConstants.RootTag)
            {
                throw new BadXmlException();
            }

            foreach (XmlElement child in xe.ChildNodes)
            {
                ThreadsList.Add(ThreadsListItem.FromXmlElement(child));
            }
        }

        private FilesListItem()
        {
            ThreadsList = new List<ThreadsListItem>();
        }
    }
}
