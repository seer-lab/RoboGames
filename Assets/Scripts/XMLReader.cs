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
    //Debug.Log("Line count for this level is: " + GetOuterXML(doc).Length);
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
    return substrings;
  }

  public static string GetNextLevel(XmlDocument doc) {
    foreach(XmlNode xmlnode in doc.DocumentElement.ChildNodes) {
      if (xmlnode.Name == stringLib.NODE_NAME_NEXT_LEVEL) {
        return xmlnode.InnerText;
      }
    }
    return "NotFound";
  }

  public static string GetLevelDescription(XmlDocument doc) {
    foreach(XmlNode xmlnode in doc.DocumentElement.ChildNodes) {
      if (xmlnode.Name == stringLib.NODE_NAME_DESCRIPTION) {
        //Debug.Log(xmlnode.InnerText);
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

    //Parses Through the file for hint tag
    // TODO: Handle cases within child node of code
    public static string GetHints(XmlDocument doc) {
        foreach(XmlNode xmlNode in doc.DocumentElement.ChildNodes) {
            if(xmlNode.Name == stringLib.NODE_NAME_HINT) {
                return xmlNode.InnerText;
            }
        }
        return "hint sucks";
    }

  public static string GetFailureLevel(XmlDocument doc){
    foreach(XmlNode xmlNode in doc.DocumentElement.ChildNodes){
      if(xmlNode.Name == "failure_level"){
        return xmlNode.InnerText;
      }
    }
      return "Null";
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

  // StoreInnerXML() and StoreOuterXML() functionality was lost so I need a new way to handle Outer and Inner XML parsing.
  public static string convertOuterToInnerXML(string outerXml, string language) {
	    TextColoration textColoration = new TextColoration();
      string sReturn = "";
		XmlDocument doc = new XmlDocument();
		Regex rgxTail = new Regex(@"[<][^>]*[>]");
		Match m = rgxTail.Match(outerXml);
		while (m.Success){
      string tmpT = rgxTail.Replace(m.Value, "");
      string tmp = outerXml.Replace(m.Value, rgxTail.Replace(m.Value, ""));
			outerXml = tmp;
			m = rgxTail.Match(outerXml);
		}
    //Debug.Log(outerXml);
		
		//multiline check
		Regex rgxMulti = new Regex("(<comment.*>)((.|\n)*)(</comment>)");
		m = rgxMulti.Match(outerXml);
		while (m.Success){
			outerXml = outerXml.Replace(m.Value, rgxMulti.Replace(m.Value, "$2"));
			m = rgxMulti.Match(outerXml);
		}
		
		//clean stray tags
		Regex rgxTag = new Regex("(<.*>)");
		m = rgxTag.Match(outerXml);
		while (m.Success){
			outerXml = outerXml.Replace(m.Value, rgxTag.Replace(m.Value, ""));
			m = rgxTag.Match(outerXml);
		}
    
		sReturn = textColoration.ColorizeText(outerXml, language);
		
		//NONFUNCTIONAL CODE
		/*sHead = m.Groups[1].Value;
		sTail = m.Groups[2].Value;
		sPretagText = outerXml.Substring(0, outerXml.Length - sHead.Length - sTail.Length);

		// Not a tag, just colorize it
		if (sHead == "" && sTail == "") {
			sReturn = textColoration.ColorizeText(outerXml, language);
		}
		// Code node
		else if (sHead.IndexOf("<" + stringLib.NODE_NAME_CODE + " ") == -1 && sHead.IndexOf("</" + stringLib.NODE_NAME_CODE + ">") == -1) {
			doc.LoadXml("<" + stringLib.NODE_NAME_CODE + ">" + sHead + "</" + stringLib.NODE_NAME_CODE + ">");
			XmlNode node = doc.FirstChild;
			/*foreach(XmlNode codenode in node.ChildNodes) {
				sReturn += (codenode.Name == stringLib.NODE_NAME_NEWLINE) ? sReturn += "" : textColoration.NodeToColorString(codenode);
				Debug.Log("sReturn1 = " + sReturn);
			}
			if (sPretagText.Length > 0) {
				sReturn = textColoration.ColorizeText(sPretagText, language) + sReturn;
				Debug.Log("sReturn2 = " + sReturn);
			}
			if (sHead.IndexOf("<" + stringLib.NODE_NAME_NEWLINE + " />") == -1 && sHead.IndexOf("<" + stringLib.NODE_NAME_NEWLINE + "/>") == -1 && sTail != "") {
				sReturn += textColoration.ColorizeText(sTail, language);
				Debug.Log("sReturn3 = " + sReturn);
			}
		}
		else if (sHead.IndexOf("</" + stringLib.NODE_NAME_CODE + ">") != -1) {
			sReturn = "";
			Debug.Log("sReturn4 = " + sReturn);
		}
		else {
			sReturn += textColoration.ColorizeText(sTail, language);
			Debug.Log("sReturn5 = " + sReturn);
		}*/
		
		
		
		// Remove &gt; and &lt;
		sReturn = sReturn.Replace("&gt;", ">").Replace("&lt;", "<");
		
		// Remove &amp;
		sReturn = sReturn.Replace("&amp;", "&");
		
		//Debug.Log("sReturn = " + sReturn);
		return sReturn;
	}


}
