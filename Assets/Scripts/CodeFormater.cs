using System.Collections;
using System.Collections.Generic;
using System.Xml; 
using UnityEngine;

/// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/// /// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/// /// ///////////////////////////////////////////////THIS IS DEPRECATED CODE///////////////////////////////////////////////////////////////
/// /// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/// /// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/// /// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/// 

public class CodeFormater 
{
    private stringLib stringLibrary;
    TextColoration textColoration; 
   public CodeFormater()
    {
        stringLibrary = new stringLib();
        textColoration = new TextColoration(); 

    }

    //.................................>8.......................................
    //************************************************************************//
    // Method: public void PrepareOuterXMLToGameScreen();
    // Description: Colorize the Outer XML for use on the game screen.
    // This method will convert Outer XML to Inner XML if required
    //************************************************************************//
    //@TODO: This needs to convert outerXML to InnerXML
    public string PrepareOuterXMLToGameScreen(string s)
    {
        string sReturn = s;
        IList<XmlNode> nodelist = XMLReader.GetNodesInString(s);
        if (nodelist.Count == 0)
        {
            sReturn = textColoration.ColorizeText(s, GlobalState.level.Language);
        }
        else
        {
            sReturn = textColoration.ColorizeText(s, GlobalState.level.Language);
        }
        return sReturn;
    }


    //.................................>8.......................................
    //************************************************************************//
    // Method: public string NodeToColorString(XmlNode    );
    // Description: Read an XML node and return a colorized string
    //************************************************************************//
    public string NodeToColorString(XmlNode node)
    {
        string sReturn = "";
        switch (node.Name)
        {
            case stringLib.NODE_NAME_PRINT:
                sReturn = stringLibrary.node_color_print + node.InnerText + stringLib.CLOSE_COLOR_TAG;
                break;
            case stringLib.NODE_NAME_WARP:
                sReturn = stringLibrary.node_color_warp + node.InnerText + stringLib.CLOSE_COLOR_TAG;
                break;
            case stringLib.NODE_NAME_RENAME:
                sReturn = stringLibrary.node_color_rename + node.InnerText + stringLib.CLOSE_COLOR_TAG;
                break;
            case stringLib.NODE_NAME_QUESTION:
                sReturn = stringLibrary.node_color_question + node.InnerText + stringLib.CLOSE_COLOR_TAG;
                break;
            case stringLib.NODE_NAME_VARIABLE_COLOR:
                sReturn = stringLibrary.node_color_rename + node.InnerText + stringLib.CLOSE_COLOR_TAG;
                break;
            case stringLib.NODE_NAME_COMMENT:
                {
                    string commentStyle;
                    string commentLanguage;
                    string multilineCommentOpenSymbolPython = @"'''";
                    string multilineCommentCloseSymbolPython = @"'''";
                    string multilineCommentOpenSymbolCpp = @"/* ";
                    string multilineCommentCloseSymbolCpp = @" */";
                    string singlelineCommentOpenSymbolPython = @"# ";
                    string singlelineCommentOpenSymbolCpp = @"// ";
                    string commentOpenSymbol = multilineCommentOpenSymbolPython;
                    string commentCloseSymbol = multilineCommentCloseSymbolPython;
                    try
                    {
                        commentStyle = node.Attributes[stringLib.XML_ATTRIBUTE_COMMENT_STYLE].Value;
                        commentLanguage = GlobalState.level.Language;
                    }
                    catch
                    {
                        commentStyle = "single";
                        commentLanguage = "default";
                    }
                    switch (commentLanguage)
                    {
                        case "python":
                            {
                                commentOpenSymbol = (commentStyle == "multi") ? multilineCommentOpenSymbolPython : singlelineCommentOpenSymbolPython;
                                commentCloseSymbol = (commentStyle == "multi") ? multilineCommentCloseSymbolPython : "";
                                break;
                            }
                        case "c":
                        case "c++":
                        case "c#":
                            {
                                commentOpenSymbol = (commentStyle == "multi") ? multilineCommentOpenSymbolCpp : singlelineCommentOpenSymbolCpp;
                                commentCloseSymbol = (commentStyle == "multi") ? multilineCommentCloseSymbolCpp : "";
                                break;
                            }
                        default:
                            {
                                commentOpenSymbol = (commentStyle == "multi") ? multilineCommentOpenSymbolPython : singlelineCommentOpenSymbolPython;
                                commentCloseSymbol = (commentStyle == "multi") ? multilineCommentCloseSymbolPython : "";
                                break;
                            }
                    }
                    switch (node.Attributes[stringLib.XML_ATTRIBUTE_TYPE].Value)
                    {
                        case "description":
                            switch (node.Attributes[stringLib.XML_ATTRIBUTE_CORRECT].Value)
                            {
                                case "true":
                                    sReturn = node.InnerText;
                                    break;
                                case "false":
                                    sReturn = node.InnerText;
                                    break;
                                default:
                                    break;
                            }
                            break;
                        case "code":
                            string[] sNewParts = node.InnerText.Split('\n');
                            switch (node.Attributes[stringLib.XML_ATTRIBUTE_CORRECT].Value)
                            {
                                case "true":
                                    if (sNewParts.Length != 1 && commentStyle == "single")
                                    {
                                        // multiple lines using single-line commenting style
                                        sReturn = "";
                                        for (int i = 0; i < sNewParts.Length; i++)
                                        {
                                            sReturn += stringLibrary.node_color_uncomment + commentOpenSymbol + sNewParts[i] + commentCloseSymbol + stringLib.CLOSE_COLOR_TAG;
                                            sReturn += "\n";
                                        }
                                    }
                                    else
                                    {
                                        sReturn = stringLibrary.node_color_uncomment + commentOpenSymbol + node.InnerText + commentCloseSymbol + stringLib.CLOSE_COLOR_TAG;
                                    }
                                    break;
                                case "false":
                                    if (sNewParts.Length != 1 && commentStyle == "single")
                                    {
                                        // multiple lines using single-line commenting style.
                                        sReturn = "";
                                        for (int i = 0; i < sNewParts.Length; i++)
                                        {
                                            sReturn += stringLibrary.node_color_incorrect_uncomment + commentOpenSymbol + sNewParts[i] + commentCloseSymbol + stringLib.CLOSE_COLOR_TAG;
                                            sReturn += "\n";
                                        }
                                    }
                                    else
                                    {
                                        sReturn = stringLibrary.node_color_incorrect_uncomment + commentOpenSymbol + node.InnerText + commentCloseSymbol + stringLib.CLOSE_COLOR_TAG;
                                    }
                                    break;
                                default:
                                    break;
                            }
                            break;
                        case "robobug":
                            sReturn = node.InnerText + stringLibrary.node_color_comment + commentOpenSymbol + commentCloseSymbol + stringLib.CLOSE_COLOR_TAG;
                            break;
                        default:
                            break;
                    }
                    break;
                }
            default:
                string thislanguage;
                try
                {
                    thislanguage = GlobalState.level.Language;
                }
                catch
                {
                    thislanguage = "python";
                }
                sReturn = textColoration.ColorizeText(node.InnerText, thislanguage);
                break;
        }
        return sReturn;
    }
}

/// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/// /// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/// /// ///////////////////////////////////////////////THIS IS DEPRECATED CODE///////////////////////////////////////////////////////////////
/// /// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/// /// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/// /// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/// 