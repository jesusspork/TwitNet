using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace TwitNetStub.Util.Misc
{
    public static class Twitter
    {
        public static string GetTweet(string xmlString)
        {
            try
            {
                using (XmlReader myReader = XmlReader.Create(new StringReader(xmlString)))
                {
                    while (myReader.Read())
                    {
                        if ((myReader.NodeType == XmlNodeType.Element) & (myReader.Name == "text"))
                        {
                            return myReader.ReadElementContentAsString();
                        }
                    }
                }
            }
            catch (Exception ex) { return ex.Message; }
            return "err";
        }
    }
}
