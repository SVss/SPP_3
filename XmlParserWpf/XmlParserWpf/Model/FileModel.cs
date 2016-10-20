using System.Collections.Generic;
using System.IO;
using System.Xml;
using TracerLib;

namespace XmlParserWpf.Model
{
    public class FileModel
    {
        public string Path { get; set; }
        public List<ThreadModel> ThreadsList { get; }
        public bool IsSaved { get; set; }
        
        // Public

        public static FileModel LoadFromFile(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException();

            var result = new FileModel { Path = path };
            var doc = new XmlDocument();

            try
            {
                doc.Load(path);
            }
            catch (XmlException ex)
            {
                throw new BadXmlException(ExceptionMessages.ErrorLoadingMessage, ex);
            }

            result.LoadFromXmlDocument(doc);
            return result;
        }
        
        public void Save()
        {
            var doc = new XmlDocument();
            var root = doc.AppendChild(doc.CreateElement(XmlConstants.RootTag));

            foreach (var item in ThreadsList)
            {
                root.AppendChild(item.ToXmlElement(doc));
            }

            doc.Save(Path);
            IsSaved = true;
        }

        // Internal

        private void LoadFromXmlDocument(XmlNode doc)
        {
            var xe = doc.FirstChild;
            if (xe == null || xe.Name != XmlConstants.RootTag)
            {
                throw new BadXmlException();
            }

            foreach (XmlElement child in xe.ChildNodes)
            {
                var thread = ThreadModel.FromXmlElement(child);
                // thread.ChangeEvent += delegate { IsSaved = false; };
                ThreadsList.Add(thread);
            }

            IsSaved = true;
        }

        private FileModel()
        {
            ThreadsList = new List<ThreadModel>();
        }

    }

    // Constants

    internal class ExceptionMessages
    {
        public static string ErrorLoadingMessage => "Error loading XML-file";
    }

}
