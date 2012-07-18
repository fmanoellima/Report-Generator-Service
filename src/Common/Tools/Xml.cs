using System;
using System.Text;
using System.Xml;
using System.IO;
using System.Xml.Xsl;
using System.Collections;
using System.Xml.Schema;
using System.Data;
using System.Xml.Serialization;

namespace Common.Tools
{
    public static class Xml
    {

        // Validation Error Count
        static int ErrorsCount;
        // Validation Error Message
        static string ErrorMessage = "";

        // Methods
        public static void AppendInnerXml(XmlNode node, string strXml, AppendPosition Position)
        {
            if (Position == AppendPosition.AppendEnd)
            {
                node.InnerXml = node.InnerXml + strXml;
            }
            else
            {
                node.InnerXml = strXml + node.InnerXml;
            }
        }

        public static void AppendInnerXml(XmlDocument doc, string xpath, string strXml, AppendPosition Position)
        {
            XmlNode node;
            node = doc.SelectSingleNode(xpath);
            if (node != null)
            {
                if (Position == AppendPosition.AppendEnd)
                {
                    node.InnerXml = node.InnerXml + strXml;
                }
                else
                {
                    node.InnerXml = strXml + node.InnerXml;
                }
            }
        }

        public static void CreateAttribute(XmlNode node, string name, object value)
        {
            if (node != null)
            {
                if (node.Attributes != null && node.Attributes[name] == null)
                {
                    if (node.OwnerDocument != null) node.Attributes.Append(node.OwnerDocument.CreateAttribute(name));
                    node.Attributes[name].Value = value.ToString();
                }
                
            }
        }

        public static void CreateAttribute(XmlDocument xmlDoc, string xpath, string name, object value)
        {
            CreateAttribute(xmlDoc.SelectSingleNode(xpath), name, value);
        }

        public static XmlNode CreateAttributeNode(XmlNode node, string tags, SortedList attributes, CreateNodeAttributeAction mode)
        {
            XmlDocument ownerDocument;
            XmlElement newChild;
            if (node == null)
            {
                return null;
            }
            ownerDocument = node.OwnerDocument;
            if (mode == CreateNodeAttributeAction.OverwriteIfNodeExists)
            {
                XmlNode node2 = node.SelectSingleNode(tags);
                if (node2 != null)
                {
                    newChild = (XmlElement)node2;
                    foreach (DictionaryEntry entry in attributes)
                    {
                        if (newChild.Attributes[entry.Key.ToString()] == null)
                        {
                            if (ownerDocument != null)
                                newChild.Attributes.Append(ownerDocument.CreateAttribute(entry.Key.ToString()));
                        }
                        newChild.Attributes[entry.Key.ToString()].Value = Cxml(entry.Value.ToString());
                    }
                    return node2;
                }
            }
            else if (mode == CreateNodeAttributeAction.IgnoreIfNodeExists)
            {
                XmlNode node3 = node.SelectSingleNode(tags);
                if (node3 != null)
                {
                    return node3;
                }
            }
            else if (mode == CreateNodeAttributeAction.IgnoreIfNodeAndAttributesExist)
            {
                string xpath = "";
                foreach (DictionaryEntry entry2 in attributes)
                {
                    if (xpath.Length > 0)
                    {
                        xpath = xpath + " and ";
                    }
                    xpath = xpath + string.Format("@{0}=\"{1}\"", entry2.Key, Cxml(entry2.Value.ToString()));
                }
                if (xpath.Length == 0)
                {
                    xpath = tags;
                }
                else
                {
                    xpath = tags + "[" + xpath + "]";
                }
                XmlNode node4 = node.SelectSingleNode(xpath);
                if (node4 != null)
                {
                    return node4;
                }
            }
            int index = 0;
        Label_020D:
            if (index < tags.Split(new[] { '/' }).Length)
            {
                string name = tags.Split(new[] { '/' })[index];
                if (node != null && ((node[name] == null) || (index == (tags.Split(new[] { '/' }).Length - 1))))
                {
                    newChild = ownerDocument.CreateElement(name);
                    node = node.AppendChild(newChild);
                }
                else
                {
                    node = node[name];
                }
                index++;
                goto Label_020D;
            }
            foreach (DictionaryEntry entry3 in attributes)
            {
                node.Attributes.Append(ownerDocument.CreateAttribute(entry3.Key.ToString()));
                node.Attributes[entry3.Key.ToString()].Value = entry3.Value.ToString();
            }
            return node;
        }

        public static XmlNode CreateAttributeNode(XmlDocument xmlDoc, string xpath, string tag, SortedList attributes, CreateNodeAttributeAction mode)
        {
            XmlNode node = xmlDoc.SelectSingleNode(xpath);
            if (node == null)
            {
                node = xmlDoc.SelectSingleNode("/");
                foreach (string str in xpath.Split(new[] { '/' }))
                {
                    if (str.Trim().Length != 0)
                    {
                        node = CreateNode(node, str, null, CreateNodeAction.IgnoreIfNodeExists);
                    }
                }
            }
            return CreateAttributeNode(node, tag, attributes, mode);
        }

        public static XmlNode CreateNode(XmlNode node, string tag, string innerText, CreateNodeAction action)
        {
            XmlElement newChild;
            if (node == null)
            {
                return null;
            }
            innerText = (innerText == null) ? "" : innerText;
            if ((action != CreateNodeAction.AlwaysCreate) && (node[tag] != null))
            {
                newChild = node[tag];
                if (action == CreateNodeAction.OverwriteIfNodeExists)
                {
                    newChild.InnerText = innerText;
                    node.AppendChild(newChild);
                    return newChild;
                }
                return newChild;
            }
            newChild = node.OwnerDocument.CreateElement(tag);
            newChild.InnerText = innerText;
            node.AppendChild(newChild);
            return newChild;
        }

        public static XmlNode CreateNode(XmlDocument xmlDoc, string xpath, string tag, string innexText, CreateNodeAction action)
        {
            XmlNode node = xmlDoc.SelectSingleNode(xpath);
            if (node == null)
            {
                node = xmlDoc.SelectSingleNode("/");
                foreach (string str in xpath.Split(new[] { '/' }))
                {
                    if (str.Trim().Length != 0)
                    {
                        node = CreateNode(node, str, null, CreateNodeAction.IgnoreIfNodeExists);
                    }
                }
            }
            return CreateNode(node, tag, innexText, action);
        }

        public static string Cxml(string strSource)
        {
            StringWriter w = new StringWriter();
            XmlTextWriter writer2 = new XmlTextWriter(w);
            StringBuilder stringBuilder;
            writer2.WriteAttributeString("a", strSource);
            stringBuilder = w.GetStringBuilder();
            stringBuilder.Remove(0, 3);
            stringBuilder.Remove(stringBuilder.Length - 1, 1);
            return stringBuilder.ToString();
        }

        public static bool GetAttributeBoolean(XmlNode node, string attribute, bool defaultValue)
        {
            bool flag = defaultValue;
            if (node.Attributes[attribute] != null)
            {
                flag = Convert.ToBoolean(node.Attributes[attribute].Value);
            }
            return flag;
        }

        public static bool GetAttributeBoolean(XmlDocument xmlDoc, string xpath, string attribute, bool defaultValue)
        {
            return GetAttributeBoolean(xmlDoc.SelectSingleNode(xpath), attribute, defaultValue);
        }

        public static int GetAttributeInt(XmlNode node, string attribute, int defaultValue)
        {
            int num = defaultValue;
            if ((node.Attributes[attribute] != null) && Misc.IsNumeric(node.Attributes[attribute].Value))
            {
                num = Convert.ToInt32(node.Attributes[attribute].Value);
            }
            return num;
        }

        public static int GetAttributeInt(XmlDocument xmlDoc, string xpath, string attribute, int defaultValue)
        {
            return GetAttributeInt(xmlDoc.SelectSingleNode(xpath), attribute, defaultValue);
        }

        public static long GetAttributeLong(XmlNode node, string attribute, long defaultValue)
        {
            long num = defaultValue;
            if (node.Attributes[attribute] != null)
            {
                num = Convert.ToInt64(node.Attributes[attribute].Value);
            }
            return num;
        }

        public static long GetAttributeLong(XmlDocument xmlDoc, string xpath, string attribute, long defaultValue)
        {
            return GetAttributeLong(xmlDoc.SelectSingleNode(xpath), attribute, defaultValue);
        }

        public static decimal GetAttributeDecimal(XmlNode node, string attribute, decimal defaultValue)
        {
            decimal num = defaultValue;
            if ((node.Attributes[attribute] != null) && Misc.IsDecimal(node.Attributes[attribute].Value))
            {
                num = Convert.ToDecimal(node.Attributes[attribute].Value);
            }
            return num;
        }

        public static decimal GetAttributeDecimal(XmlDocument xmlDoc, string xpath, string attribute, decimal defaultValue)
        {
            return GetAttributeDecimal(xmlDoc.SelectSingleNode(xpath), attribute, defaultValue);
        }



        public static string GetAttributeString(XmlNode node, string attribute, string defaultValue)
        {
            string str = (defaultValue == null) ? "" : defaultValue;
            if (node.Attributes[attribute] != null)
            {
                str = node.Attributes[attribute].Value;
            }
            return str;
        }

        public static string GetAttributeString(XmlDocument xmlDoc, string xpath, string attribute, string defaultValue)
        {
            return GetAttributeString(xmlDoc.SelectSingleNode(xpath), attribute, defaultValue);
        }

        public static object GetAttributeObject(XmlDocument xmlDoc, string xpath, string attribute, object defaultValue)
        {
            return GetAttributeObject(xmlDoc.SelectSingleNode(xpath), attribute, defaultValue);
        }

        public static object GetAttributeObject(XmlNode node, string attribute, object defaultValue)
        {
            return (node.Attributes[attribute].Value == null) ? defaultValue : node.Attributes[attribute].Value;
        }


        public static XmlNode GetInnerNode(XmlDocument doc, string xpath, ref bool blnFound)
        {
            XmlNode node;
            blnFound = false;
            node = doc.SelectSingleNode(xpath);
            if (node != null)
            {
                blnFound = true;
                return node;
            }
            return null;
        }

        public static string GetInnerText(XmlDocument doc, string xpath, ref bool blnFound)
        {
            XmlNode node;
            blnFound = false;
            node = doc.SelectSingleNode(xpath);
            if (node != null)
            {
                blnFound = true;
                return node.InnerText;
            }
            return "";
        }

        public static string GetInnerXML(XmlDocument doc, string xpath, ref bool blnFound)
        {
            XmlNode node;
            blnFound = false;
            node = doc.SelectSingleNode(xpath);
            if (node != null)
            {
                blnFound = true;
                return node.InnerXml;
            }
            return "";
        }

        public static string GetXml(DataSet ds)
        {
            DataSet set = ds.Clone();
            foreach (DataTable table in set.Tables)
            {
                foreach (DataColumn column in table.Columns)
                {
                    if (column.DataType == typeof(DateTime))
                    {
                        column.DataType = typeof(string);
                    }
                }
            }
            foreach (DataTable table2 in ds.Tables)
            {
                foreach (DataRow row in table2.Rows)
                {
                    DataRow row2 = set.Tables[table2.TableName].NewRow();
                    foreach (DataColumn column2 in table2.Columns)
                    {
                        DateTime time;
                        if (((column2.DataType == typeof(DateTime)) && (row[column2.ColumnName] != DBNull.Value)) && DateTime.TryParse(row[column2.ColumnName].ToString(), out time))
                        {
                            row2[column2.ColumnName] = time.ToString("yyyy-MM-dd HH:mm:ss.fff");
                        }
                        else
                        {
                            row2[column2.ColumnName] = row[column2.ColumnName];
                        }
                    }
                    set.Tables[table2.TableName].Rows.Add(row2);
                }
            }
            string xml = set.GetXml().Replace("xmlns", "xmlns1");
            XmlDocument document = new XmlDocument();
            document.LoadXml(xml);
            XmlAttribute node = document.DocumentElement.Attributes["xmlns1"];
            if (node != null)
            {
                document.DocumentElement.Attributes.Remove(node);
            }
            return document.OuterXml;
        }

        public static string GetXSLFileXMLFile(string xslFileName, string xmlFileName)
        {
            StringBuilder sb = new StringBuilder();
            StringWriter writer = new StringWriter(sb);
            XmlDocument document = new XmlDocument();
            document.Load(xmlFileName);
            XslCompiledTransform transform = new XslCompiledTransform();
            transform.Load(xslFileName);
            transform.Transform(document, null, writer);
            string str = sb.ToString();
            return str;
        }

        public static string GetXSLFileXMLString(string xslFileName, string xmlString)
        {
            StringBuilder sb = new StringBuilder();
            StringWriter writer = new StringWriter(sb);
            XmlDocument document = new XmlDocument();
            document.LoadXml(xmlString);
            XslCompiledTransform transform = new XslCompiledTransform();
            transform.Load(xslFileName);
            transform.Transform(document, null, writer);
            string str = sb.ToString();
            return str;
        }

        public static string GetXSLStringXMLFile(string xslString, string xmlFileName)
        {
            StringBuilder sb = new StringBuilder();
            StringWriter writer = new StringWriter(sb);
            XmlDocument document = new XmlDocument();
            document.Load(xmlFileName);
            XslCompiledTransform transform = new XslCompiledTransform();
            XmlDocument stylesheet = new XmlDocument();
            stylesheet.LoadXml(xslString);
            transform.Load(stylesheet);
            transform.Transform(document, null, writer);
            string str = sb.ToString();
            return str;
        }

        public static string GetXSLStringXMLString(string xslString, string xmlString)
        {
            StringBuilder sb = new StringBuilder();
            StringWriter writer = new StringWriter(sb);
            XmlDocument document = new XmlDocument();
            document.LoadXml(xmlString);
            XslCompiledTransform transform = new XslCompiledTransform();
            XmlDocument stylesheet = new XmlDocument();
            stylesheet.LoadXml(xslString);
            transform.Load(stylesheet);
            transform.Transform(document, null, writer);
            string str = sb.ToString();
            return str;
        }

        public static void SetInnerText(XmlDocument doc, string xpath, string value, ref bool blnFound)
        {
            XmlNode node;
            blnFound = false;
            node = doc.SelectSingleNode(xpath);
            if (node != null)
            {
                blnFound = true;
                node.InnerText = value;
            }
            else
            {
                blnFound = false;
            }
        }

        // Nested Types
        #region Nested Types
        [Serializable]
        public enum AppendPosition
        {
            AppendEnd = 2,
            AppendFront = 1
        }

        public enum CreateNodeAction
        {
            AlwaysCreate = 1,
            IgnoreIfNodeExists = 3,
            OverwriteIfNodeExists = 2
        }

        public enum CreateNodeAttributeAction
        {
            AlwaysCreate = 1,
            IgnoreIfNodeAndAttributesExist = 4,
            IgnoreIfNodeExists = 3,
            OverwriteIfNodeExists = 2
        }

        public enum ValidationReturnType
        {
            Boolean = 1,
            StringErrors = 2,
            IntErrorCount = 3
        }
        #endregion


        private static void ValidationHandler(object sender, ValidationEventArgs args)
        {
            ErrorMessage = ErrorMessage + args.Message + "\r\n";
            ErrorsCount++;
        }

        public static object ValidateXmlByXsd(string strXMLDoc, string strXSDDoc, string targetNamespace, ValidationReturnType returnType)
        {
            StringReader srXmlDoc = null;
            StringReader srXmlSchema = null;
            XmlSchemaSet schemaSet;
            XmlReader rdXsdDoc = null;
            XmlReaderSettings settings;
            XmlReader rdXmlDoc = null;
            XmlDocument xmlDoc;

            ErrorMessage = String.Empty;
            ErrorsCount = 0;
            try
            {
                xmlDoc = new XmlDocument();
                //converts strXMLDoc string to a XmlDocument
                xmlDoc.LoadXml(strXMLDoc);
                //gets the first Node from strXMLDoc
                XmlNode mainNode = xmlDoc.FirstChild;

                //createsor substitute the xmlns attribute with the targetNamespace value
                CreateAttribute(mainNode, "xmlns", targetNamespace);

                //substitutes the strXMLDoc value with the changed value
                strXMLDoc = xmlDoc.OuterXml;

                srXmlDoc = new StringReader(strXMLDoc);
                srXmlSchema = new StringReader(strXSDDoc);

                // Create the XmlSchemaSet
                schemaSet = new XmlSchemaSet();
                rdXsdDoc = XmlReader.Create(srXmlSchema);

                // Add the schema to the collection.
                schemaSet.Add(targetNamespace, rdXsdDoc);


                // Set the validation settings.
                settings = new XmlReaderSettings();
                settings.ValidationType = ValidationType.Schema;
                settings.Schemas = schemaSet;
                settings.ValidationFlags |= XmlSchemaValidationFlags.ProcessInlineSchema;
                settings.ValidationFlags |= XmlSchemaValidationFlags.ProcessSchemaLocation;
                settings.ValidationFlags |= XmlSchemaValidationFlags.ReportValidationWarnings;
                settings.ValidationEventHandler += ValidationHandler;

                // Create the XmlReader object.
                rdXmlDoc = XmlReader.Create(srXmlDoc, settings);

                // Parse the file. 
                while (rdXmlDoc.Read())
                {
                }

                // XML Validation succeeded
                switch (returnType)
                {
                    case ValidationReturnType.Boolean:
                        return (ErrorsCount == 0);
                    case ValidationReturnType.StringErrors:
                        return ErrorMessage;
                    case ValidationReturnType.IntErrorCount:
                        return ErrorsCount;
                    default:
                        return (ErrorsCount == 0);
                }

            }
            catch (Exception error)
            {
                // XML Validation failed
                //return "XML validation failed." + "\r\n" + "Error Message: " + error.Message;
                ErrorMessage = error.Message;
                ErrorsCount = 1;

                // XML Validation succeeded
                switch (returnType)
                {
                    case ValidationReturnType.Boolean:
                        return false;
                    case ValidationReturnType.StringErrors:
                        return ErrorMessage;
                    case ValidationReturnType.IntErrorCount:
                        return ErrorsCount;
                    default:
                        return (ErrorsCount == 0);
                }
            }
            finally
            {
                if (srXmlDoc != null)
                    srXmlDoc.Close();
                if (srXmlSchema != null)
                    srXmlSchema.Close();
                if (rdXsdDoc != null)
                    rdXsdDoc.Close();
                if (rdXmlDoc != null)
                    rdXmlDoc.Close();
            }
        }

        public static string Serialize<T>(T obj)
        {
            string xmlOut;

            using (MemoryStream memoryStream = new MemoryStream())
            {
                memoryStream.Position = 0;

                XmlSerializer xs = new XmlSerializer(typeof(T));
                XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);

                xs.Serialize(xmlTextWriter, obj);
                xmlOut = Encoding.UTF8.GetString(memoryStream.ToArray());
            }
            return xmlOut;
        }

        public static string Serialize<T>(T obj, Type[] ExtraTypes)
        {
            string xmlOut;

            using (MemoryStream memoryStream = new MemoryStream())
            {
                memoryStream.Position = 0;

                XmlSerializer xs = new XmlSerializer(typeof(T), ExtraTypes);
                XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);

                xs.Serialize(xmlTextWriter, obj);
                xmlOut = Encoding.UTF8.GetString(memoryStream.ToArray());
            }
            return xmlOut;
        }

        public static T Deserialize<T>(string _xml)
        {
            T _object;

            XmlSerializer xs = new XmlSerializer(typeof(T));
            MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(_xml));

            memoryStream.Position = 0;

            _object = (T)xs.Deserialize(memoryStream);

            return _object;
        }

        public static T Deserialize<T>(string _xml, Type[] ExtraTypes)
        {
            T _object;

            XmlSerializer xs = new XmlSerializer(typeof(T), ExtraTypes);
            MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(_xml));

            memoryStream.Position = 0;

            _object = (T)xs.Deserialize(memoryStream);

            return _object;
        }
    }
}
