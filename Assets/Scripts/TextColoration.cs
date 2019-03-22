using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Text;
using System.Xml;
using System.Text.RegularExpressions;
using System;

public class TextColoration {

  public stringLib stringLibrary = new stringLib();

  public string ColorizeText(string sText, string language = "c++") {
    Debug.Log("ColorizeText: test string: " + sText);
    // Turn all comments and their following text green. Remove all color tags from following text.
    Regex rgxStringLiteral = new Regex("(\"|\')([^\"\']*)(\"|\')");
    //string patternCommentPython = @"(\/\/|\s#|\n#|#)(.*)";
    string patternCommentPython = @"(\/\/|\n#|\s#)(.*)";
    string patternCommentCpp = @"(\/\/|\*\/)(.*)";
    string patternKeywordPython = @"(^| |\n|\t)(else if|class|print|not|or|and|def|bool|auto|double|int|struct|break|else|long|switch|case|enum|register|typedef|char|extern|return|union|continue|for|signed|void|do|if|static|while|default|goto|sizeof|volatile|const|float|short|unsigned|string)(\W|$)";
    string patternKeywordCpp = @"(^| )(else if|class|cout|not|or|and|def|bool|auto|double|int|struct|break|else|long|switch|case|enum|register|typedef|char|extern|return|union|continue|for|signed|void|do|if|static|while|default|goto|sizeof|volatile|const|float|short|unsigned|string)(\W|$)";
    string patternIncludeGeneric = @"(#include\s)(.*)";
    string patternComment = patternCommentPython;
    string patternKeyword = patternKeywordPython;
    string patternInclude = patternIncludeGeneric;

    switch(language) {
      case "python": {
        patternComment = patternCommentPython;
        patternKeyword = patternKeywordPython;
        break;
      }
      case "c++":
      case "c":
      case "c#": {
        patternComment = patternCommentCpp;
        patternKeyword = patternKeywordCpp;
        break;
      }
      default: {
        patternComment = patternCommentPython;
        patternKeyword = patternKeywordPython;
        break;
      }
    }
    /*
    // Test case
    Debug.Log("Beginning Test case for include foobar");
    sText = "#include foobar";
    Regex rgxTest = new Regex(@"(#include\s)(.*)");
    Match mTest = rgxTest.Match(sText);
    if (mTest.Success) {
      Debug.Log("Test case was successful in finding pattern #include and .*");
    }
    else {
      Debug.Log("Test case was unsuccessful in finding pattern #include and .*");
    }
    Debug.Log("Ending Test case for include foobar");
    // end of test case
    */
	
    Regex rgxComment = new Regex(patternComment);
    Regex rgxKeyword = new Regex(patternKeyword);
    Regex rgxInclude = new Regex(patternInclude);

    Match mStringLiteral, mComment, mKeyword, mInclude;
    mInclude = rgxInclude.Match(sText);
	
    while (mInclude.Success) {

      sText = sText.Replace(mInclude.Value, stringLibrary.syntax_color_include + mInclude.Value + stringLib.CLOSE_COLOR_TAG);
	  mInclude = mInclude.NextMatch();
    }
    mKeyword = rgxKeyword.Match(sText);
	while (mKeyword.Success){
		Debug.Log("key found: " + mKeyword.Value + " in " + sText);

		//int keyplace = sText.IndexOf(mKeyword.Value, mKeyword.Index);
		//fix to handle finding keywords inside other keywords and similar issues
		//sText = sText.Remove(keyplace, mKeyword.Value.Length).Insert(keyplace,stringLibrary.syntax_color_keyword + mKeyword.Value + stringLib.CLOSE_COLOR_TAG);
		sText = sText.Replace(mKeyword.Value, stringLibrary.syntax_color_keyword + mKeyword.Value + stringLib.CLOSE_COLOR_TAG);
		
		Debug.Log("key result " + sText);

		mKeyword = mKeyword.NextMatch();
		//mKeyword = rgxKeyword.Match(sText);
		
	}
    mStringLiteral = rgxStringLiteral.Match(sText);
	while (mStringLiteral.Success)
	{
		string cleanedstring = DecolorizeText(mStringLiteral.Value);
      sText = sText.Replace(mStringLiteral.Value, stringLibrary.syntax_color_string + cleanedstring + stringLib.CLOSE_COLOR_TAG);
	  mStringLiteral = mStringLiteral.NextMatch();
	}
    mComment = rgxComment.Match(sText);
    while (mComment.Success) {
		string cleanedstring = DecolorizeText(mComment.Value);
        sText = sText.Replace(mComment.Value, stringLibrary.syntax_color_comment + cleanedstring + stringLib.CLOSE_COLOR_TAG);
		mComment = mComment.NextMatch();
	  //}
    }
	
	//Decolorize tags stuck inside words
    Regex inrgx = new Regex(@"(?s)(\w)(<color=#.{8}>)([^<]*)(</color>)");
	sText = inrgx.Replace(sText, "$1$3");
	
	//fix out of order color tags
	Regex ordrgx = new Regex(@"(?s)(<color=#.{8}>)(</color>)");
	sText = ordrgx.Replace(sText, "$2$1");
	
	//clean duplicated tags 
	Regex duprgx = new Regex(@"(?s)(<color=#.{8}>)\1+");
	sText = duprgx.Replace(sText, "$1");
	duprgx = new Regex(@"(?s)(</color>)\1+");
	sText = duprgx.Replace(sText, "$1");
	
	//fix parentheses stuck in color tags
	Regex parrgx = new Regex(@"(?s)(\)|\()(<\/color>)");
	sText = parrgx.Replace(sText, "$2$1");
	
	
    // Keyword found in this substring
    Debug.Log("ColorizeText processedString: " + sText);
    return sText;
  }

  public string DecolorizeText(string sText) {
    Debug.Log("DecolorizeText: Decolorizing " + sText);
    Regex rgx = new Regex("(?s)(.*)(<color=#.{8}>)(.*)(</color>)(.*)");
	string returnstring;
	do{
	returnstring = sText;
    sText = rgx.Replace(sText, "$1$3$5");
	} while (returnstring != sText);
	Debug.Log("decolored text = " + sText);
    return sText;
  }

  public string ColorTaskLine(string sLine, int nLine, LevelGenerator lg)
	{
		for (int toolCheck = 0; toolCheck < stateLib.NUMBER_OF_TOOLS; toolCheck++) {
			if (lg.taskOnLines[nLine,toolCheck] != 1) {
				continue;
			}
			switch(toolCheck) {
				case stateLib.TOOL_CATCHER_OR_ACTIVATOR:
				break;

				case stateLib.TOOL_PRINTER_OR_QUESTION:
				if (sLine.IndexOf(stringLibrary.node_color_print) != -1) return stringLibrary.node_color_print;
				else if (sLine.IndexOf(stringLibrary.node_color_question) != -1) return stringLibrary.node_color_question;
				break;

				case stateLib.TOOL_WARPER_OR_RENAMER:
				if (sLine.IndexOf(stringLibrary.node_color_warp) != -1) return stringLibrary.node_color_warp;
				else if (sLine.IndexOf(stringLibrary.node_color_rename) != -1) return stringLibrary.node_color_rename;
				break;

				case stateLib.TOOL_COMMENTER:
				if (sLine.IndexOf(stringLibrary.node_color_correct_comment) == -1 && sLine != "") return stringLibrary.node_color_correct_comment;
				else if (sLine.IndexOf(stringLibrary.node_color_incorrect_comment) == -1 && sLine != "") return stringLibrary.node_color_incorrect_comment;
				else if (sLine.IndexOf(stringLibrary.node_color_comment) == -1 && sLine != "") return stringLibrary.node_color_comment;
				break;

				case stateLib.TOOL_CONTROL_FLOW:
				if (sLine.IndexOf(stringLibrary.node_color_uncomment) != -1) return stringLibrary.node_color_uncomment;
				else if (sLine.IndexOf(stringLibrary.node_color_incorrect_uncomment) != -1) return stringLibrary.node_color_incorrect_uncomment;
				break;
				case stateLib.TOOL_HELPER:
				break;
				default:
				break;
			}
		}
		return "";
	}
	
	public string NodeToColorString(XmlNode node) {
		string language = "python";
		string sReturn = "";
		switch (node.Name) {
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
			case stringLib.NODE_NAME_COMMENT: {
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
				try {
					commentStyle = node.Attributes[stringLib.XML_ATTRIBUTE_COMMENT_STYLE].Value;
					commentLanguage = language;
				}
				catch {
					commentStyle = "single";
					commentLanguage = "default";
				}
				switch(commentLanguage) {
					case "python": {
						commentOpenSymbol = (commentStyle == "multi") ? multilineCommentOpenSymbolPython : singlelineCommentOpenSymbolPython;
						commentCloseSymbol = (commentStyle == "multi") ? multilineCommentCloseSymbolPython : "";
						break;
					}
					case "c":
					case "c++":
					case "c#": {
						commentOpenSymbol = (commentStyle == "multi") ? multilineCommentOpenSymbolCpp : singlelineCommentOpenSymbolCpp;
						commentCloseSymbol = (commentStyle == "multi") ? multilineCommentCloseSymbolCpp : "";
						break;
					}
					default: {
						commentOpenSymbol = (commentStyle == "multi") ? multilineCommentOpenSymbolPython : singlelineCommentOpenSymbolPython;
						commentCloseSymbol = (commentStyle == "multi") ? multilineCommentCloseSymbolPython : "";
						break;
					}
				}
				switch(node.Attributes[stringLib.XML_ATTRIBUTE_TYPE].Value) {
					case "description":
					switch(node.Attributes[stringLib.XML_ATTRIBUTE_CORRECT].Value) {
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
					switch(node.Attributes[stringLib.XML_ATTRIBUTE_CORRECT].Value) {
						case "true":
						if (sNewParts.Length != 1 && commentStyle == "single") {
							// multiple lines using single-line commenting style
							sReturn = "";
							for (int i = 0 ; i < sNewParts.Length ; i++) {
								sReturn += stringLibrary.node_color_uncomment + commentOpenSymbol + sNewParts[i] + commentCloseSymbol + stringLib.CLOSE_COLOR_TAG;
								sReturn += "\n";
							}
						}
						else {
							sReturn = stringLibrary.node_color_uncomment + commentOpenSymbol + node.InnerText + commentCloseSymbol + stringLib.CLOSE_COLOR_TAG;
						}
						break;
						case "false":
						if (sNewParts.Length != 1 && commentStyle == "single") {
							// multiple lines using single-line commenting style.
							sReturn = "";
							for (int i = 0 ; i < sNewParts.Length ; i++) {
								sReturn += stringLibrary.node_color_incorrect_uncomment + commentOpenSymbol + sNewParts[i] + commentCloseSymbol + stringLib.CLOSE_COLOR_TAG;
								sReturn += "\n";
							}
						}
						else {
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
			try {
				thislanguage = language;
			}
			catch {
				thislanguage = "python";
			}
			sReturn = ColorizeText(node.InnerText, thislanguage);
			break;
		}
		return sReturn;
	}
}
