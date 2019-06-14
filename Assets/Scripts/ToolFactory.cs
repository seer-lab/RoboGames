using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using System.Text.RegularExpressions;

public abstract class ToolFactory 
{
    protected int lineNumber;
    protected XmlNode childnode;
    protected string path;
    protected float initialLineY;
    protected float linespacing;
    protected ToolFactory(XmlNode node, int line)
    {
        lineNumber = line;
        childnode = node;
        path = "Prefabs/";
        initialLineY = 3f; 
        linespacing = 0.825f; 
    }
    public abstract Tools GetScript();
    public abstract GameObject GetGameObject(); 

}

public class PrinterFactory : ToolFactory
{
    printer tool; 
    public PrinterFactory(XmlNode node, int line)
        :base(node, line)
    {
        tool = new printer();
        int lineS = line + 1;
        GlobalState.correctLine[stateLib.TOOL_PRINTER_OR_QUESTION] += lineS.ToString() + " ";
    }
    public override Tools GetScript()
    {
        tool.DisplayText = childnode.Attributes[stringLib.XML_ATTRIBUTE_TEXT].Value;
        tool.Index = lineNumber;
        if (childnode.Attributes[stringLib.XML_ATTRIBUTE_TOOL] != null)
        {
            string toolatt = childnode.Attributes[stringLib.XML_ATTRIBUTE_TOOL].Value;
            string[] toolcounts = toolatt.Split(',');
            for (int i = 0; i < toolcounts.Length; i++)
            {
                tool.tools[i] = int.Parse(toolcounts[i]);
            }
            GlobalState.level.Tasks[1]++; 
        }
        return tool; 
    }
    public override GameObject GetGameObject()
    {
        GameObject newoutput = GameObject.Instantiate(Resources.Load<GameObject>(path + "printer"));
        GlobalState.level.TaskOnLine[lineNumber, stateLib.TOOL_PRINTER_OR_QUESTION]++;
        tool = newoutput.GetComponent<printer>();
        GetScript();
        //GlobalState.level.Tasks[1]++;
        return newoutput; 
    }

}

public class WarperFactory: ToolFactory
{
    warper tool; 
    public WarperFactory(XmlNode node, int line)
        :base(node, line)
    {
        tool = new warper();
        int lineS = line + 1;
        GlobalState.correctLine[stateLib.TOOL_WARPER_OR_RENAMER] += lineS.ToString() + " "; 
    }
    public override Tools GetScript()
    {
        tool.Filename = childnode.Attributes[stringLib.XML_ATTRIBUTE_FILE].Value;
        tool.Index = lineNumber;
        if (childnode.Attributes[stringLib.XML_ATTRIBUTE_TOOL] != null)
        {
            string toolatt = childnode.Attributes[stringLib.XML_ATTRIBUTE_TOOL].Value;
            string[] toolcounts = toolatt.Split(',');
            for (int i = 0; i < toolcounts.Length; i++)
            {
                tool.tools[i] = int.Parse(toolcounts[i]);
            }
        }
        if (childnode.Attributes[stringLib.XML_ATTRIBUTE_LINE] != null)
        {
            tool.WarpToLine = childnode.Attributes[stringLib.XML_ATTRIBUTE_LINE].Value;
        }

        GlobalState.level.Tasks[2]++;
        return tool; 
    }
    public override GameObject GetGameObject()
    {
        GameObject newWarp = GameObject.Instantiate(Resources.Load<GameObject>(path + "warper"));
        GlobalState.level.TaskOnLine[lineNumber, stateLib.TOOL_WARPER_OR_RENAMER]++;
        tool = newWarp.GetComponent<warper>();
        GetScript();
        return newWarp;
    }
}

public class GenericBugFactory: ToolFactory
{
    GenericBug tool;
    public GenericBugFactory(XmlNode node, int line)
        :base(node, line)
    {
        tool = new GenericBug(); 
    }
    public override Tools GetScript()
    {
        throw new NotImplementedException();
    }
    public override GameObject GetGameObject()
    {
        throw new NotImplementedException();
    }
}

public class QuestionFactory: ToolFactory
{
    question tool;
    stringLib stringLibrary; 
    public QuestionFactory(XmlNode node, int line)
        :base(node, line)
    {
        tool = new question();
        stringLibrary = new stringLib();
        int lineS = line + 1;
        GlobalState.correctLine[stateLib.TOOL_PRINTER_OR_QUESTION] += lineS.ToString() + " "; 
    }
    public override Tools GetScript()
    {
        tool.DisplayText = childnode.Attributes[stringLib.XML_ATTRIBUTE_TEXT].Value + "\n";
        tool.expected = childnode.Attributes[stringLib.XML_ATTRIBUTE_ANSWER].Value;
        tool.Index = lineNumber;
        GlobalState.level.Tasks[1]++;
        Regex rgx = new Regex("(.*)(" + stringLibrary.node_color_question + ")(.*)(</color>)(.*)");
        string thisQuestionInnerText = rgx.Replace(GlobalState.level.Code[tool.Index], "$2$3$4");
        tool.innertext = thisQuestionInnerText;
        return tool; 
    }
    public override GameObject GetGameObject()
    {
        GameObject question = GameObject.Instantiate(Resources.Load<GameObject>(path + "question"));
        GlobalState.level.TaskOnLine[lineNumber, stateLib.TOOL_PRINTER_OR_QUESTION]++;
        tool = question.GetComponent<question>();
        GetScript();
        return question;
    }
}

public class RenamerFactory : ToolFactory
{
    rename tool; 
    public RenamerFactory(XmlNode node, int lineNumber)
        :base(node, lineNumber)
    {
        tool = new rename();
        int lineS = lineNumber + 1;
        GlobalState.correctLine[stateLib.TOOL_WARPER_OR_RENAMER] += lineS.ToString() + " "; 
    }
    public override Tools GetScript()
    {
        stringLib stringLibrary = new stringLib(); 
        tool.DisplayText = childnode.Attributes[stringLib.XML_ATTRIBUTE_TEXT].Value + "\n";
        tool.correct = childnode.Attributes[stringLib.XML_ATTRIBUTE_CORRECT].Value;
        //tool.groupid = int.Parse(childnode.Attributes[stringLib.XML_ATTRIBUTE_GROUPID].Value);
        try
        {
            tool.oldname = childnode.Attributes[stringLib.XML_ATTRIBUTE_OLDNAME].Value;

        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
            tool.oldname = childnode.InnerText;
        }
        tool.Index = lineNumber;
        string options = childnode.Attributes[stringLib.XML_ATTRIBUTE_OPTIONS].Value;
        string[] optionsArray = options.Split(',');
        for (int i = 0; i < optionsArray.Length; i++)
        {
            tool.options.Add(optionsArray[i]);
        }
        GlobalState.level.Tasks[2]++;

        Regex rgx = new Regex(@"(^| |\>)(" + tool.oldname + ")(;| )");
        GlobalState.level.Code[tool.Index] = rgx.Replace(GlobalState.level.Code[tool.Index], "$1" + stringLibrary.node_color_rename + tool.oldname + stringLib.CLOSE_COLOR_TAG + "$3");
        //Debug.Log("property text = " + tool.innertext);
        return tool; 
    }
    public override GameObject GetGameObject()
    {
        GameObject newRenamer = GameObject.Instantiate(Resources.Load<GameObject>(path + "renamer"));
        GlobalState.level.TaskOnLine[lineNumber, stateLib.TOOL_WARPER_OR_RENAMER]++;
        tool = newRenamer.GetComponent<rename>();
        GetScript();
        return newRenamer; 
    }
}

public class BreakpointFactory: ToolFactory
{
    Breakpoint tool; 
    public BreakpointFactory(XmlNode node, int line)
        :base(node, line)
    {
        tool = new Breakpoint();
        int lineS = line + 1;
        GlobalState.correctLine[stateLib.TOOL_CATCHER_OR_CONTROL_FLOW] += lineS.ToString() + " "; 
    }
    public override Tools GetScript()
    {
        tool.values = childnode.Attributes[stringLib.XML_ATTRIBUTE_TEXT].Value;
        Debug.Log(childnode.Attributes[stringLib.XML_ATTRIBUTE_TEXT].Value);
        tool.Index = lineNumber;
        if (childnode.Attributes[stringLib.XML_ATTRIBUTE_TOOL] != null)
        {
            string toolatt = childnode.Attributes[stringLib.XML_ATTRIBUTE_TOOL].Value;
            string[] toolcounts = toolatt.Split(',');
            for (int i = 0; i < toolcounts.Length; i++)
            {
                tool.tools[i] = int.Parse(toolcounts[i]);
            }
        }
        GlobalState.level.Tasks[4]++; 
        return tool; 
    }
    public override GameObject GetGameObject()
    {
        GameObject newBreakpoint = GameObject.Instantiate(Resources.Load<GameObject>(path + "breakpoint"));
        GlobalState.level.TaskOnLine[lineNumber, stateLib.TOOL_UNCOMMENTER]++;
        tool = newBreakpoint.GetComponent<Breakpoint>();
        GetScript();
        return newBreakpoint; 
    }
}
public class PrizeBugFactory: ToolFactory
{
    PrizeBug tool; 
    public PrizeBugFactory(XmlNode node, int line)
        :base(node, line)
    {
        tool = new PrizeBug(); 
    }
    public override Tools GetScript()
    {
        string[] bonuses = childnode.Attributes[stringLib.XML_ATTRIBUTE_BONUSES].Value.Split(',');
        for (int i = 0; i < stateLib.NUMBER_OF_TOOLS; i++)
        {
            tool.bonus[i] += int.Parse(bonuses[i]);
        }
        return tool; 
    }
    public override GameObject GetGameObject()
    {
        GameObject bug = Resources.Load<GameObject>(path + "prizebug");
        throw new NotImplementedException(); 
       // bug.transform.localScale += new Vector3(bugscale * (bugsize - 1), bugscale * (bugsize - 1), 0);
    }
}
public class BeaconFactory: ToolFactory
{
    beacon tool; 
    public BeaconFactory(XmlNode node, int line)
        :base(node, line)
    {
        tool = new beacon();
        int lineS = line + 1; 
        GlobalState.correctLine[stateLib.TOOL_CATCHER_OR_CONTROL_FLOW ] += lineS.ToString() + " ";
    }
    public override Tools GetScript()
    {
        tool.Index = lineNumber;
        if (childnode.Attributes[stringLib.XML_ATTRIBUTE_FLOWORDER].Value != "")
        {
            string[] flowOrder = childnode.Attributes[stringLib.XML_ATTRIBUTE_FLOWORDER].Value.Split(',');
            for (int i = 0; i < flowOrder.Length; i++)
            {
                tool.flowOrder.Add(int.Parse(flowOrder[i]));
                GlobalState.level.Tasks[0]++;
            }
        }
        return tool; 
    }
    public override GameObject GetGameObject()
    {
        GameObject beacon = GameObject.Instantiate(Resources.Load<GameObject>(path + "beacon"));
        GlobalState.level.TaskOnLine[lineNumber, stateLib.TOOL_CATCHER_OR_CONTROL_FLOW]++;
        tool = beacon.GetComponent<beacon>();
        GetScript();
        return beacon; 
    }
}
public class VariableColorFactory: ToolFactory
{
    VariableColor tool; 
    public VariableColorFactory(XmlNode node, int line)
        :base(node, line)
    {
        tool = new VariableColor();
    }
    public override Tools GetScript()
    {
        stringLib stringLibrary = new stringLib(); 
        tool.groupid = int.Parse(childnode.Attributes[stringLib.XML_ATTRIBUTE_GROUPID].Value);
        tool.Index = lineNumber;
        tool.oldname = childnode.InnerText;
        //Debug.Log("oldname for new variable object = " + tool.oldname);
        Regex varrgx = new Regex(@"(^| |\t|\>)(" + tool.oldname + ")(;| )");
        GlobalState.level.Code[tool.Index] = varrgx.Replace(GlobalState.level.Code[tool.Index], "$1" + stringLibrary.node_color_rename + tool.oldname + stringLib.CLOSE_COLOR_TAG + "$3");
        varrgx = new Regex("(.*)(" + stringLibrary.node_color_rename + ")(\\w)(</color>)(.*)");
        string thisVarnamenInnerText = varrgx.Replace(GlobalState.level.Code[tool.Index], "$2$3$4");
        tool.innertext = thisVarnamenInnerText;
        return tool; 
    }
    public override GameObject GetGameObject()
    {
        GameObject variableColor = GameObject.Instantiate(Resources.Load<GameObject>(path + "variablecolor"));
        tool = variableColor.GetComponent<VariableColor>();
        GetScript();
        return variableColor; 
    }
}
public class CommentFactory: ToolFactory
{
    public int Entity;
    public CommentFactory(XmlNode node, int line)
        :base(node, line)
    {
        int lineS = line + 1;
        GlobalState.correctLine[stateLib.TOOL_COMMENTER ] += lineS.ToString() + " ";

    }
    public override Tools GetScript()
    {
        CommentTypeFactory factory;
        switch (childnode.Attributes[stringLib.XML_ATTRIBUTE_TYPE].Value)
        {
            case "robobug":
                factory = new BugCommentFactory(childnode, lineNumber);
                return factory.GetScript();
            case "description":
                factory = new DescriptionCommentFactory(childnode, lineNumber);
                return factory.GetScript();
            case "code":
                factory = new CodeCommentFactory(childnode, lineNumber);
                return factory.GetScript();
            default: return null;
        }
    }
    public override GameObject GetGameObject()
    {
        GameObject newComment = GameObject.Instantiate(Resources.Load<GameObject>(path + "comment"));
        CommentTypeFactory factory; 
        switch (childnode.Attributes[stringLib.XML_ATTRIBUTE_TYPE].Value)
        {
            case "robobug":
                factory = new BugCommentFactory(childnode, lineNumber);
                factory.ApplyScript(newComment);
                Entity = factory.Entity;
                 GlobalState.level.TaskOnLine[lineNumber, stateLib.TOOL_COMMENTER]++;
                break;
            case "description": 
                factory = new DescriptionCommentFactory(childnode, lineNumber);
                factory.ApplyScript(newComment);
                Entity = factory.Entity;
                GlobalState.level.TaskOnLine[lineNumber, stateLib.TOOL_COMMENTER]++;
                break;
            case "code":
                factory = new CodeCommentFactory(childnode, lineNumber);
                 GlobalState.level.TaskOnLine[lineNumber, stateLib.TOOL_UNCOMMENTER]++;
                factory.ApplyScript(newComment);
                Entity = factory.Entity;
                break;
            default: break; 
        }
    
        return newComment; 
    }

}
