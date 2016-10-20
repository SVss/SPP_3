using System;
using System.Collections.Generic;
using System.Xml;
using TracerLib;

namespace XmlParserWpf.Model
{
    public class ThreadModel
    {
        public uint Id { get; set; }
        public uint Time { get; set; }
        public List<MethodModel> Methods { get; }

        // Public

        public static ThreadModel FromXmlElement(XmlElement xe)
        {
            if (xe.Name != XmlConstants.ThreadTag)
                throw new BadXmlException();

            uint id, time;
            try
            {
                id = Convert.ToUInt32(xe.Attributes[XmlConstants.ThreadIdAttribute].Value);
                time = Convert.ToUInt32(xe.Attributes[XmlConstants.TimeAttribute].Value);
            }
            catch (Exception ex)
            {
                if (ex is XmlException || ex is FormatException || ex is OverflowException)
                    throw new BadXmlException();

                throw;
            }

            var result = new ThreadModel()
            {
                Id = id,
                Time = time
            };

            foreach (XmlElement child in xe.ChildNodes)
            {
                var method = MethodModel.FromXmlElement(child, result);
                // method.ChangeEvent += delegate { result.OnChange(); };

                result.Methods.Add(method);
            }

            return result;
        }

        public XmlElement ToXmlElement(XmlDocument document)
        {
            XmlElement result = document.CreateElement(XmlConstants.ThreadTag);
            result.SetAttribute(XmlConstants.ThreadIdAttribute, Id.ToString());
            result.SetAttribute(XmlConstants.TimeAttribute, Time.ToString());

            foreach (var item in Methods)
            {
                result.AppendChild(item.ToXmlElement(document));
            }
            return result;
        }

        // Internal

        private ThreadModel()
        {
            Methods = new List<MethodModel>();
        }

    }

}
