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

  public string ColorizeText(string sText, string language = "python") {
    Debug.Log("ColorizeText: test string: " + sText);
    // Turn all comments and their following text green. Remove all color tags from following text.
    Regex rgxStringLiteral = new Regex("(\"|\')(.*)(\"|\')");
    string patternCommentPython = @"(\/\/|\s#|\n#|#)(.*)";
    string patternCommentCpp = @"(\/\/|\*\/)(.*)";
    string patternKeywordPython = @"(\W|^)(else if|class|print|not|or|and|def|include|bool|auto|double|int|struct|break|else|long|switch|case|enum|register|typedef|char|extern|return|union|continue|for|signed|void|do|if|static|while|default|goto|sizeof|volatile|const|float|short|unsigned)(\W|$)";
    string patternKeywordCpp = @"(\\W|^)(else if|class|print|not|or|and|def|include|bool|auto|double|int|struct|break|else|long|switch|case|enum|register|typedef|char|extern|return|union|continue|for|signed|void|do|if|static|while|default|goto|sizeof|volatile|const|float|short|unsigned)(\W|$)";
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
    string sReturnString = "";

    Regex rgxComment = new Regex(patternComment);
    Regex rgxKeyword = new Regex(patternKeyword);
    Regex rgxInclude = new Regex(patternInclude);

    Match mStringLiteral, mComment, mKeyword, mInclude;
    mStringLiteral = rgxStringLiteral.Match(sText);
    mComment = rgxComment.Match(sText);
    mKeyword = rgxKeyword.Match(sText);
    mInclude = rgxInclude.Match(sText);

    if (mInclude.Success) {
      Debug.Log("ColorizeText: Found Include");
      sReturnString = stringLibrary.syntax_color_include + mInclude.Groups[1] + mInclude.Groups[2] + stringLib.CLOSE_COLOR_TAG;
    }
    else if (mStringLiteral.Success && !mComment.Success) {
      Debug.Log("ColorizeText: Found String Literal");
      sReturnString = stringLibrary.syntax_color_string + mStringLiteral.Groups[1] + mStringLiteral.Groups[2]  + mStringLiteral.Groups[3] + stringLib.CLOSE_COLOR_TAG;
      if (mKeyword.Success) {
        Debug.Log("ColorizeText: Found keyword and string literal.");
        // Found a keyword as well as a string
        Regex rgxOutsideKeyword = new Regex(patternKeyword + "(?=(?:[^\"]|\"[^\"]*\")*$)");
        Match mOutsideKeyword = rgxOutsideKeyword.Match(sText);
        sReturnString = mOutsideKeyword.Groups[1] + stringLibrary.syntax_color_keyword + mOutsideKeyword.Groups[2] + stringLib.CLOSE_COLOR_TAG + mOutsideKeyword.Groups[3];
      }
    }
    // Comment found in this substring
    else if (mComment.Success) {
      Debug.Log("ColorizeText: Found Comment.");
      sReturnString = stringLibrary.syntax_color_comment + mComment.Groups[1] + mComment.Groups[2] + stringLib.CLOSE_COLOR_TAG;
      Debug.Log("sReturnString being set to: " + sReturnString);
    }
    // Keyword found in this substring
    else if (mKeyword.Success) {
      Debug.Log("ColorizeText: Found keyword.");
      sReturnString = mKeyword.Groups[1] + stringLibrary.syntax_color_keyword + mKeyword.Groups[2] + stringLib.CLOSE_COLOR_TAG + mKeyword.Groups[3];
    }
    else {
      Debug.Log("ColorizeText: Found no patterns.");
      sReturnString = sText;
    }
    Debug.Log("ColorizeText: processedString: " + sReturnString);
    return sReturnString;
  }

  public string DecolorizeText(string sText) {
    Debug.Log("DecolorizeText: Decolorizing");
    Regex rgx = new Regex("(?s)(.*)(<color=#.{8}>)(.*)(</color>)(.*)");
    sText = rgx.Replace(sText, "$1$3$5");
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

}
