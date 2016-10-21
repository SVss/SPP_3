using System;
using System.Collections.Generic;
using System.Xml;
using TracerLib;

namespace XmlParserWpf.Model
{
    public class MethodModel: ICloneable    // shallow copy
    {
        public string Name { get; set; }
        public string Package { get; set; }
        public uint ParamsCount { get; set; }
        public uint Time { get; set; }
        public List<MethodModel> NestedMethods { get; }

        public object Parent { get; private set; }

        // Public

        public static MethodModel FromXmlElement(XmlElement xe, object parent = null)
        {
            if (xe.Name != XmlConstants.MethodTag)
                throw new BadXmlException();

            string name, package;
            uint paramsCount, time;
            try
            {
                name = xe.Attributes[XmlConstants.NameAttribute].Value;
                package = xe.Attributes[XmlConstants.PackageAttribute].Value;
                paramsCount = Convert.ToUInt32(xe.Attributes[XmlConstants.ParamsAttribute].Value);
                time = Convert.ToUInt32(xe.Attributes[XmlConstants.TimeAttribute].Value);
            }
            catch (Exception ex)
            {
                if (ex is XmlException || ex is FormatException || ex is OverflowException)
                    throw new BadXmlException();

                throw;
            }

            var result = new MethodModel()
            {
                Name = name,
                Package = package,
                ParamsCount = paramsCount,
                Time = time
            };

            foreach (XmlElement child in xe.ChildNodes)
            {
                var nested = FromXmlElement(child, result);
                result.NestedMethods.Add(nested);
            }

            result.Parent = parent;
            return result;
        }

        public XmlElement ToXmlElement(XmlDocument document)
        {
            XmlElement result = document.CreateElement(XmlConstants.MethodTag);
            result.SetAttribute(XmlConstants.NameAttribute, Name);
            result.SetAttribute(XmlConstants.TimeAttribute, Time.ToString());

            result.SetAttribute(XmlConstants.PackageAttribute, Package);
            result.SetAttribute(XmlConstants.ParamsAttribute, ParamsCount.ToString());

            foreach (var child in NestedMethods)
            {
                result.AppendChild(child.ToXmlElement(document));
            }
            return result;
        }

        // ICloneable

        public object Clone()
        {
            MethodModel result = new MethodModel()
            {
                Name = Name,
                Package = Package,
                ParamsCount = ParamsCount,
                Time = Time
            };
            return result;
        }

        // Internal

        private MethodModel()
        {
            NestedMethods = new List<MethodModel>();
            Parent = null;
        }

    }

}
