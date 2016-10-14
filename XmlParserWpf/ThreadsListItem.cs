using System;
using System.Collections.Generic;
using System.Xml;
using TracerLib;

namespace XmlParserWpf
{
    internal class ThreadsListItem
    {
        private ThreadsListItem()
        {
            Methods = new List<MethodsListItem>();
        }

        public long Id { get; set; }
        public long Time { get; set; }
        public List<MethodsListItem> Methods { get; }

        public override string ToString()
        {
            return $"thread (id=\"{Id}\" time=\"{Time}\")";
        }

        public static ThreadsListItem FromXmlElement(XmlElement xe)
        {
            if (xe.Name != XmlConstants.ThreadTag)
                throw new BadXmlException();

            long id, time;
            try
            {
                id = Convert.ToInt64(xe.Attributes[XmlConstants.ThreadIdAttribute].Value);
                time = Convert.ToInt64(xe.Attributes[XmlConstants.TimeAttribute].Value);
            }
            catch (Exception ex)
            {
                if (ex is XmlException || ex is FormatException || ex is OverflowException)
                    throw new BadXmlException();

                throw;
            }

            var result = new ThreadsListItem()
            {
                Id = id,
                Time = time
            };

            foreach (XmlElement child in xe.ChildNodes)
            {
                result.Methods.Add(MethodsListItem.FromXmlElement(child));
            }

            return result;
        }
    }

    internal class MethodsListItem
    {
        private MethodsListItem()
        {
            Children = new List<MethodsListItem>();
        }

        public string Name { get; set; }
        public string Package { get; set; }
        public long ParamsCount { get; set; }
        public long Time { get; set; }

        public List<MethodsListItem> Children { get; }

        public override string ToString()
        {
            return $"{Name} (params=\"{ParamsCount}\" package=\"{Package})\" time=\"{Time}\"";
        }

        public static MethodsListItem FromXmlElement(XmlElement xe)
        {
            MethodsListItem result = null;

            // TODO: load nested methods here

            return result;
        }
    }
}
