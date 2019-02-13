using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Text;
using System.Xml;
using System.Text.RegularExpressions;
using System;

public static class XMLReader {

  public static XmlDocument ReadFile(string filename) {
    XmlDocument doc = new XmlDocument();
    doc.PreserveWhitespace = true;
    doc.Load(filename);
    return doc;
  }

  public static int GetLineCount(XmlDocument doc) {
    Debug.Log("Line count for this level is: " + GetOuterXML(doc).Length);
    return GetOuterXML(doc).Length;
  }

  public static string[] GetOuterXML(XmlDocument doc) {
    string outerxml = "";
    foreach(XmlNode xmlnode in doc.DocumentElement.ChildNodes) {
      if (xmlnode.Name == stringLib.NODE_NAME_CODE) {
        outerxml = xmlnode.OuterXml;
      }
    }
    // Find and convert all </newline>'s to '\n'
    outerxml = outerxml.Replace("<newline />", "\n");
    outerxml = outerxml.Replace("<newline/>", "\n");
    // Break the OuterXML into an array, newline seperated.
    Regex rgxNewlineSplit = new Regex("\n");
    string[] substrings = rgxNewlineSplit.Split(outerxml);
    // We want to remove <code language="lang"> from the start of the text.
    Regex rgxCodeTag = new Regex("(<code)(.*)(>)(.*)");
    Match m = rgxCodeTag.Match(substrings[0]);
    substrings[0] = m.Groups[4].Value;
    // The last array element will contain </code> so let's remove that too.
    rgxCodeTag = new Regex("(.*)(</code>)");
    m = rgxCodeTag.Match(substrings[substrings.Length-1]);
    substrings[substrings.Length-1] = m.Groups[1].Value;
    if (substrings[substrings.Length-1] == "") {
      Array.Resize(ref substrings, substrings.Length-1);
    }
    foreach(string s in substrings) {
      Debug.Log("XMLReader.GetOuterXML: " + s);
    }
    return substrings;
  }

  public static string GetNextLevel(XmlDocument doc) {
    foreach(XmlNode xmlnode in doc.DocumentElement.ChildNodes) {
      if (xmlnode.Name == stringLib.NODE_NAME_NEXT_LEVEL) {
        Debug.Log(xmlnode.InnerText);
        return xmlnode.InnerText;
      }
    }
    return "NotFound";
  }

  public static string GetLevelDescription(XmlDocument doc) {
    foreach(XmlNode xmlnode in doc.DocumentElement.ChildNodes) {
      if (xmlnode.Name == stringLib.NODE_NAME_DESCRIPTION) {
        Debug.Log(xmlnode.InnerText);
        return xmlnode.InnerText;
      }
    }
    return "NotFound";
  }

  public static string GetIntroText(XmlDocument doc) {
    foreach(XmlNode xmlnode in doc.DocumentElement.ChildNodes) {
      if (xmlnode.Name == stringLib.NODE_NAME_INTRO_TEXT) {
        return xmlnode.InnerText;
      }
    }
    return "NotFound";
  }

  public static string GetEndText(XmlDocument doc) {
    foreach(XmlNode xmlnode in doc.DocumentElement.ChildNodes) {
      if (xmlnode.Name == stringLib.NODE_NAME_END_TEXT) {
        return xmlnode.InnerText;
      }
    }
    return "NotFound";
  }

  public static string GetTimeLimit(XmlDocument doc) {
    foreach(XmlNode xmlnode in doc.DocumentElement.ChildNodes) {
      if (xmlnode.Name == stringLib.NODE_NAME_TIME) {
        return xmlnode.InnerText;
      }
    }
    return "NotFound";
  }

  public static string GetLanguage(XmlDocument doc) {
    foreach(XmlNode xmlnode in doc.DocumentElement.ChildNodes) {
      if (xmlnode.Name == stringLib.NODE_NAME_CODE) {
        try {
          return xmlnode.Attributes[stringLib.XML_ATTRIBUTE_LANGUAGE].Value;
        }
        catch {
          return "python";
        }
      }
    }
    return "python";
  }

  public static IList<XmlNode> GetNodesInString(string s) {
    IList<XmlNode> nodelist = new List<XmlNode>();
    XmlDocument doc = new XmlDocument();
    doc.PreserveWhitespace = true;

    // Find all Xmlnodes in the substring, then try LoadXml() on each.
    Regex rgxXML = new Regex(@"(<.*?<\/.*?>)");
    MatchCollection matches = rgxXML.Matches(s);
    foreach(Match m in matches) {
      GroupCollection groups = m.Groups;
      doc.LoadXml(groups[0].Value);
      XmlNode node = doc.FirstChild;
      nodelist.Add(node);
    }
    return nodelist;
  }

  public static IList<string> FindXMLSubstrings(string s) {
    IList<string> strings = new List<string>();
    // Parse everything
    Regex rgxAll = new Regex(@"[\s\w]*(<\w*>[\s\w]*<\/\w*>)[\s\w]*");
    MatchCollection matches = rgxAll.Matches(s);
    foreach(Match m in matches) {
      GroupCollection groups = m.Groups;
      strings.Add(groups[0].Value);
    }
    return strings;
  }

  public static IList<XmlNode> GetToolNodes(XmlDocument doc) {
    IList<XmlNode> nodelist = new List<XmlNode>();
    foreach(XmlNode xmlnode in doc.DocumentElement.ChildNodes) {
      if (xmlnode.Name == stringLib.NODE_NAME_TOOLS) {
        foreach(XmlNode tool in xmlnode)
        {
          if (tool.Name == stringLib.NODE_NAME_TOOL) {
            nodelist.Add(tool);
          }
        }
      }
    }
      return nodelist;
  }

}
