using System;
using System.Collections.Generic;
using System.Xml;
using TracerLib;

namespace TracerLibXmlParser
{
    public class ThreadsListItem
    {
        public long Id { get; set; }
        public long Time { get; set; }
        public List<MethodsListItem> Methods { get; }

        public string AsString => $"thread (id=\"{Id}\" time=\"{Time}\")";

        // Public

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

    public class MethodsListItem
    {
        public string Name { get; set; }
        public string Package { get; set; }
        public long ParamsCount { get; set; }
        public long Time { get; set; }
        public List<MethodsListItem> Nested { get; }

        public string AsString => $"{Name} (params=\"{ParamsCount}\" package=\"{Package})\" time=\"{Time}\"";

        // Public

        public static MethodsListItem FromXmlElement(XmlElement xe)
        {
            if (xe.Name != XmlConstants.MethodTag)
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
                result.Nested.Add(FromXmlElement(child));
            }

            return result;
        }

        // Internal 

        private MethodsListItem()
        {
            Nested = new List<MethodsListItem>();
        }
    }
}
