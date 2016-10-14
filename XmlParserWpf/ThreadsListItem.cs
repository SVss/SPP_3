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
            ThreadsListItem result = null;
            if (xe.Name != XmlConstants.ThreadTag)
            {
                throw new BadXmlException();
            }

            // TODO: load methods here

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
