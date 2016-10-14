using System;
using System.Collections.Generic;
using System.Xml;
using TracerLib;

namespace XmlParserWpf
{
    internal class ThreadsListItem
    {
        public long Id { get; set; }
        public long Time { get; set; }
        public List<MethodsListItem> Methods { get; }

        // Public

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

            ThreadsListItem result = new ThreadsListItem()
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

        // Internal

        private ThreadsListItem()
        {
            Methods = new List<MethodsListItem>();
        }
    }

    internal class MethodsListItem
    {
        public string Name { get; set; }
        public string Package { get; set; }
        public long ParamsCount { get; set; }
        public long Time { get; set; }
        public List<MethodsListItem> Children { get; }

        // Public

        public override string ToString()
        {
            return $"{Name} (params=\"{ParamsCount}\" package=\"{Package})\" time=\"{Time}\"";
        }

        public static MethodsListItem FromXmlElement(XmlElement xe)
        {
            if (xe.Name != XmlConstants.ThreadTag)
                throw new BadXmlException();

            string name, package;
            long paramsCount, time;
            try
            {
                name = xe.Attributes[XmlConstants.NameAttribute].Value;
                package = xe.Attributes[XmlConstants.PackageAttribute].Value;
                paramsCount = Convert.ToInt64(xe.Attributes[XmlConstants.ParamsAttribute].Value);
                time = Convert.ToInt64(xe.Attributes[XmlConstants.TimeAttribute].Value);
            }
            catch (Exception ex)
            {
                if (ex is XmlException || ex is FormatException || ex is OverflowException)
                    throw new BadXmlException();

                throw;
            }

            MethodsListItem result = new MethodsListItem()
            {
                Name = name,
                Package = package,
                ParamsCount = paramsCount,
                Time = time
            };

            foreach (XmlElement child in xe.ChildNodes)
            {
                result.Children.Add(FromXmlElement(child));
            }

            return result;
        }

        // Internal 

        private MethodsListItem()
        {
            Children = new List<MethodsListItem>();
        }
    }
}
