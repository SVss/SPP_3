using System.Collections.Generic;
using System.Xml;
using TracerLib;

namespace XmlParserWpf
{
    internal class ThreadsListItem
    {
        public ThreadsListItem()
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
    }

    internal class MethodsListItem
    {
        public MethodsListItem()
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
    }
}
