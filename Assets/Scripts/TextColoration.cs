using System.Data;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Text;
using System.Xml;
using System.Text.RegularExpressions;
using System;

public class TextColoration {


  public string ColorizeText(string sText, string language) {

		sText = DecolorizeText(sText);
    //Debug.Log("ColorizeText: test string: " + sText);
    // Turn all comments and their following text green. Remove all color tags from following text.
    Regex rgxStringLiteral = new Regex("(\")([^\"]*)(\")");
    //string patternCommentPython = @"(\/\/|\s#|\n#|#)(.*)";
    string patternCommentPython = @"(\/\/|\n#|\s#|\r#|\t#)(.*)";
    string patternCommentCpp = @"(\/\/|\*\/)(.*)";
    string patternKeywordPython = @"(^| |\t|\()(class|in|as|range|print|not|or|and|def|bool|auto|double|int|struct|break|else|elif|using|namespace|long|switch|case|enum|register|typedef|char|extern|return|union|continue|for|signed|void|do|if|static|while|default|goto|sizeof|volatile|const|float|short|unsigned|string)(\W|$|\))";
    string patternKeywordCpp = @"(^| |\n|\t|\()(class|cout|cin|not|or|and|def|bool|auto|double|struct|break|if|else|using|namespace|long|switch|case|enum|register|typedef|char|extern|return|union|continue|for|signed|void|do|static|while|default|goto|sizeof|volatile|const|short|float|unsigned|string)(\W|$|\))";
    string patternIncludeGeneric = @"(#include\s)(.*)";
		string patternIncludePythonJava = @"(#include\w|import)";
    string patternKeywordJava = @"(^| |\n|\t|\(|\.)(class|not|or|and|bool|double|struct|break|if|else|long|switch|case|enum|char|extern|return|int|continue|public|for|this|abstract|void|do|static|private|boolean|while|default|goto|sizeof|volatile|const|short|float|String|System|out|print|println)(|\W|$|\))";
		string patternCommentJava = @"(\/\/|\*\/)(.*)";
		string patternComment = patternCommentPython;
    string patternKeyword = patternKeywordPython;
    string patternInclude = patternIncludeGeneric;

    switch(language) {
      case "python": {
        patternComment = patternCommentPython;
        patternKeyword = patternKeywordPython;
				patternInclude = patternIncludePythonJava;
        break;
      }
      case "c++":
      case "c":
      case "c#": {
        patternComment = patternCommentCpp;
        patternKeyword = patternKeywordCpp;
        break;

      }
			case "java": {
        patternComment = patternCommentJava;
        patternKeyword = patternKeywordJava;
				patternInclude = patternIncludePythonJava;
        break;
      }
      default: {
        patternComment = patternCommentPython;
        patternKeyword = patternKeywordPython;
        break;
      }
    }
	
    Regex rgxComment = new Regex(patternComment);
    Regex rgxKeyword = new Regex(patternKeyword);
    Regex rgxInclude = new Regex(patternInclude);

    Match mStringLiteral, mComment, mKeyword, mInclude, mBlockComment;

    mInclude = rgxInclude.Match(sText);
	
    while (mInclude.Success) {

      sText = sText.Replace(mInclude.Value, GlobalState.StringLib.syntax_color_include + mInclude.Value + stringLib.CLOSE_COLOR_TAG);
	  mInclude = mInclude.NextMatch();
    }
    mKeyword = rgxKeyword.Match(sText);
	while (mKeyword.Success){

		sText = sText.Replace(mKeyword.Value, GlobalState.StringLib.syntax_color_keyword + mKeyword.Value + stringLib.CLOSE_COLOR_TAG);
		
		//Debug.Log("key result " + sText);

		mKeyword = mKeyword.NextMatch();
		
	}

		//find ints 
	Regex intrgx = new Regex(@"()(int)(?=[\s\[])"); 
	sText = intrgx.Replace(sText, GlobalState.StringLib.syntax_color_keyword + "int" + stringLib.CLOSE_COLOR_TAG);
			Regex ifs = new Regex(@"(if(?!<\/color>)(?<!<color=#.{8}))"); 
		sText = ifs.Replace(sText, GlobalState.StringLib.syntax_color_keyword + "if" + stringLib.CLOSE_COLOR_TAG); 
		Regex elsergx = new Regex(@"(else(?!<\/color>)(?<!<color=#.{8}))"); 
		sText = elsergx.Replace(sText, GlobalState.StringLib.syntax_color_keyword + "else" + stringLib.CLOSE_COLOR_TAG);
    mStringLiteral = rgxStringLiteral.Match(sText);
	while (mStringLiteral.Success)
	{
		string cleanedstring = DecolorizeText(mStringLiteral.Value);
      sText = sText.Replace(mStringLiteral.Value, GlobalState.StringLib.syntax_color_string + cleanedstring + stringLib.CLOSE_COLOR_TAG);
	  mStringLiteral = mStringLiteral.NextMatch();
	}

	//block comments (todo: Add to previous comment loop)
	rgxComment = new Regex(patternComment); 
	//TODO: I discovered the lazy "?" after doing a lot of modification; 
	//this probably could be used elsewhere
	
	mComment = rgxComment.Match(sText);
	while (mComment.Success) {
		//Debug.Log("Value: " + mComment.Value + " t/f:" + mComment.Value.IndexOf("#"));
		if(mComment.Value.Contains("'''") && mComment.Value.Contains("#") && false){
			mComment = mComment.NextMatch();
			continue;

		}else{
			string cleanedstring = DecolorizeText(mComment.Value);
			sText = sText.Replace(mComment.Value, GlobalState.StringLib.syntax_color_comment + cleanedstring + stringLib.CLOSE_COLOR_TAG);
			mComment = mComment.NextMatch();
		}
	}

	//Block Comments/First Comment
	if(language.Equals("python")){

		string keywordPassTwo = @"(^| |\t|\b|\()(range)(\W|$|\))";
		rgxKeyword = new Regex(keywordPassTwo);

    mKeyword = rgxKeyword.Match(sText);
		while (mKeyword.Success){
			sText = sText.Replace(mKeyword.Value, GlobalState.StringLib.syntax_color_keyword + mKeyword.Value + stringLib.CLOSE_COLOR_TAG);
			//Debug.Log("key result " + sText);
			mKeyword = mKeyword.NextMatch();
		}


		Regex rgxBlock = new Regex(@"(\/\/|\#)(.*)");
		mBlockComment = rgxBlock.Match(sText);

		//First Comments
		while(mBlockComment.Success){
			string cleanedstring = DecolorizeText(mBlockComment.Value);
			sText = sText.Replace(mBlockComment.Value, GlobalState.StringLib.syntax_color_comment + cleanedstring + stringLib.CLOSE_COLOR_TAG);
			break;
		}

		string block = @"(['" + "])\\1\\1(.*?)\\1{3}";
		RegexOptions options = RegexOptions.Multiline| RegexOptions.Singleline;
		rgxBlock = new Regex(block,options);
		mBlockComment = rgxBlock.Match(sText);
		//Block Comments
		while(mBlockComment.Success){
			string cleanedstring = DecolorizeText(mBlockComment.Value);
			sText = sText.Replace(mBlockComment.Value, GlobalState.StringLib.syntax_color_comment + cleanedstring + stringLib.CLOSE_COLOR_TAG);
			mBlockComment=mBlockComment.NextMatch();
		}
}else if(language.Equals("c++") || language.Equals("c") ||language.Equals("c#")|| language.Equals("java")){
		string block = @"\/\*+((([^\*])+)|([\*]+(?!\/)))[*]+\/";
		RegexOptions options = RegexOptions.Multiline| RegexOptions.Singleline;
		Regex rgxBlock = new Regex(block,options);
		mBlockComment = rgxBlock.Match(sText);

		while(mBlockComment.Success){
			string cleanedstring = DecolorizeText(mBlockComment.Value);
			sText = sText.Replace(mBlockComment.Value, GlobalState.StringLib.syntax_color_comment + cleanedstring + stringLib.CLOSE_COLOR_TAG);
			mBlockComment=mBlockComment.NextMatch();
		}

}
/* 
		
	//Debug.Log("Pre-cleaning text = " + sText);
	*/
	//Decolorize tags stuck inside words
  Regex inrgx = new Regex(@"(?s)(\w)(<color=#.{8}>)([^<]*)(<\/color>)");
	sText = inrgx.Replace(sText, "$1$3");
	
	//fix out of order color tags
	Regex ordrgx = new Regex(@"(?s)(<color=#.{8}>)(<\/color>)");
	sText = ordrgx.Replace(sText, "$2$1");
	
	//clean duplicated tags 
	Regex duprgx = new Regex(@"(?s)(<color=#.{8}>)\1+");
	sText = duprgx.Replace(sText, "$1");
	duprgx = new Regex(@"(?s)(</color>)\1+");
	sText = duprgx.Replace(sText, "$1");
	
	//fix parentheses stuck in color tags
	Regex parrgx = new Regex(@"(?s)(\)|\()(<\/color>)");
	sText = parrgx.Replace(sText, "$2$1");
	parrgx = new Regex(@"(?s)(<color=#.{8}>)(\)|\()");
	sText = parrgx.Replace(sText, "$2$1");
	
	//fix braces stuck in color tags
	Regex brrgx = new Regex(@"(?s)(}|{)(<\/color>)");
	sText = brrgx.Replace(sText, "$2$1");
	brrgx = new Regex(@"(?s)(<color=#.{8}>)(}|{)");
	sText = brrgx.Replace(sText, "$2$1");

	//fix ampersands stuck in color tags
  Regex amprgx = new Regex(@"(?s)(&)(<\/color>)");
	sText = amprgx.Replace(sText, "$2$1");
    //Debug.Log("ColorizeText processedString: " + sText);

		// Regex colorLine = new Regex(@"()(<color=.{10}\n)"); 
		// sText = colorLine.Replace(sText, '\n' + GlobalState.StringLib.syntax_color_keyword);
    return sText;
  }

  public string DecolorizeText(string sText) {
    //Debug.Log("DecolorizeText: Decolorizing " + sText);
    Regex rgx = new Regex("(?s)(.*)(<color=#.{8}>)(.*)(</color>)(.*)");
	string returnstring;
	do{
	returnstring = sText;
    sText = rgx.Replace(sText, "$1$3$5");
	} while (returnstring != sText);
	//Debug.Log("decolored text = " + sText);
    return sText;
  }
	

  public string ColorTaskLine(string sLine, int nLine, LevelGenerator lg)
	{
		for (int toolCheck = 0; toolCheck < stateLib.NUMBER_OF_TOOLS; toolCheck++) {
			if (GlobalState.level.TaskOnLine[nLine,toolCheck] != 1) {
				continue;
			}
			switch(toolCheck) {
				case stateLib.TOOL_CATCHER_OR_CONTROL_FLOW:
				break;

				case stateLib.TOOL_PRINTER_OR_QUESTION:
				if (sLine.IndexOf(GlobalState.StringLib.node_color_print) != -1) return GlobalState.StringLib.node_color_print;
				else if (sLine.IndexOf(GlobalState.StringLib.node_color_question) != -1) return GlobalState.StringLib.node_color_question;
				break;

				case stateLib.TOOL_WARPER_OR_RENAMER:
				if (sLine.IndexOf(GlobalState.StringLib.node_color_warp) != -1) return GlobalState.StringLib.node_color_warp;
				else if (sLine.IndexOf(GlobalState.StringLib.node_color_rename) != -1) return GlobalState.StringLib.node_color_rename;
				break;

				case stateLib.TOOL_COMMENTER:
				if (sLine.IndexOf(GlobalState.StringLib.node_color_correct_comment) == -1 && sLine != "") return GlobalState.StringLib.node_color_correct_comment;
				else if (sLine.IndexOf(GlobalState.StringLib.node_color_incorrect_comment) == -1 && sLine != "") return GlobalState.StringLib.node_color_incorrect_comment;
				else if (sLine.IndexOf(GlobalState.StringLib.node_color_comment) == -1 && sLine != "") return GlobalState.StringLib.node_color_comment;
				break;

				case stateLib.TOOL_UNCOMMENTER:
				if (sLine.IndexOf(GlobalState.StringLib.node_color_uncomment) != -1) return GlobalState.StringLib.node_color_uncomment;
				else if (sLine.IndexOf(GlobalState.StringLib.node_color_incorrect_uncomment) != -1) return GlobalState.StringLib.node_color_incorrect_uncomment;
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
			sReturn = GlobalState.StringLib.node_color_print + node.InnerText + stringLib.CLOSE_COLOR_TAG;
			break;
			case stringLib.NODE_NAME_WARP:
			sReturn = GlobalState.StringLib.node_color_warp + node.InnerText + stringLib.CLOSE_COLOR_TAG;
			break;
			case stringLib.NODE_NAME_RENAME:
			sReturn = GlobalState.StringLib.node_color_rename + node.InnerText + stringLib.CLOSE_COLOR_TAG;
			break;
			case stringLib.NODE_NAME_QUESTION:
			sReturn = GlobalState.StringLib.node_color_question + node.InnerText + stringLib.CLOSE_COLOR_TAG;
			break;
			case stringLib.NODE_NAME_VARIABLE_COLOR:
			sReturn = GlobalState.StringLib.node_color_rename + node.InnerText + stringLib.CLOSE_COLOR_TAG;
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
								sReturn += GlobalState.StringLib.node_color_uncomment + commentOpenSymbol + sNewParts[i] + commentCloseSymbol + stringLib.CLOSE_COLOR_TAG;
								sReturn += "\n";
							}
						}
						else {
							sReturn = GlobalState.StringLib.node_color_uncomment + commentOpenSymbol + node.InnerText + commentCloseSymbol + stringLib.CLOSE_COLOR_TAG;
						}
						break;
						case "false":
						if (sNewParts.Length != 1 && commentStyle == "single") {
							// multiple lines using single-line commenting style.
							sReturn = "";
							for (int i = 0 ; i < sNewParts.Length ; i++) {
								sReturn += GlobalState.StringLib.node_color_incorrect_uncomment + commentOpenSymbol + sNewParts[i] + commentCloseSymbol + stringLib.CLOSE_COLOR_TAG;
								sReturn += "\n";
							}
						}
						else {
							sReturn = GlobalState.StringLib.node_color_incorrect_uncomment + commentOpenSymbol + node.InnerText + commentCloseSymbol + stringLib.CLOSE_COLOR_TAG;
						}
						break;
						default:
						break;
					}
					break;
					case "robobug":
					sReturn = node.InnerText + GlobalState.StringLib.node_color_comment + commentOpenSymbol + commentCloseSymbol + stringLib.CLOSE_COLOR_TAG;
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
