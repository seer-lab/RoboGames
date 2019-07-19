using System;
using System.Xml;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class ActionFactory
{
    protected XmlNode childnode;
    protected int lineNumber, column;
    protected TextColoration color;
    protected List<Action> actions;
    protected CodeProperties props; 
    /// <summary>
    /// Constructor for Abstract Class ActionFactory
    /// </summary>
    /// <param name="node">The block of text associated with the Action</param>
    /// <param name="nodeLine">The line where the Action can be found in relation to all the lines of code.</param>
    /// <param name="properties">The current properties (line spacing etc) that the game is following</param>
    protected ActionFactory(XmlNode node, int nodeLine, CodeProperties properties)
    {
        this.props = properties; 
        color = new TextColoration();
        actions = new List<Action>();
        childnode = node;
        string[] lines = childnode.InnerText.Split('\n');
    }
    /// <summary>
    /// Stores and removes the action from the code. This function will do 
    /// all the logic associated with text cleaning.
    /// </summary>
    /// <param name="text">Target text to be evaluated</param>
    /// <param name="line">The line number in the code where this text is found</param>
    /// <returns></returns>
    public abstract bool HandleParams(string text, int line);
    public abstract Action GetAction();
    public virtual List<Action> GetActions(){return actions;}
}
public class DialogFactory : ActionFactory
{
    int order = 0;
    string ans;
    public DialogFactory(XmlNode node, int line, CodeProperties props)
        : base(node, line, props)
    {
    }
    public override bool HandleParams(string text, int line)
    {
        if (!text.Contains("@")) return false;
        Regex paramRgx = new Regex(stringLib.DIALOG_REGEX);   // Finds the value in between "@" eg. @Hello, World!@ => Hello, World!
        string values = paramRgx.Match(text).Value;
        GlobalState.level.Code[line] = GlobalState.level.Code[line].Replace("@" + values + "@", "");
        column = text.IndexOf("@");
        actions.Add(new Action(props, ActionType.Dialog, line, column, values));
        return true;
    }
    public override List<Action> GetActions()
    {
        return actions;
    }
    public override Action GetAction()
    {
        return new Action(props, ActionType.Dialog, lineNumber, column, ans);
    }
}
public class SwitchFactory : ActionFactory
{
    public SwitchFactory(XmlNode node, int line, CodeProperties props)
        : base(node, line, props)
    {

    }
    public override bool HandleParams(string text, int line)
    {
        if (text.Contains("???"))
        {
            int count = 0;
            int a= 0;  
            while ((a = text.IndexOf("???", a)) != -1)
            {
                actions.Add(new Action(props, ActionType.SwitchTool, line, a));
                a += ("???").Length;              
                count++;
            }
            column = count; 
            GlobalState.level.Code[line] = GlobalState.level.Code[line].Replace("???", "");      
            return true;
        }
        return false;
    }
    public override Action GetAction(){
        return new Action(props, ActionType.SwitchTool, lineNumber, column); 
    }
}
public class FireFactory : ActionFactory
{
    public FireFactory(XmlNode node, int line, CodeProperties props)
        : base(node, line, props)
    {

    }
    public override bool HandleParams(string text, int line)
    {
        if (text.Contains("!!!"))
        {
            int count = 0;
            int a= 0;  
            while ((a = text.IndexOf("!!!", a)) != -1)
            {
                actions.Add(new Action(props, ActionType.Throw, line, a));
                a += ("!!!").Length;
                count++;
            }
            column = count;
            GlobalState.level.Code[line] = GlobalState.level.Code[line].Replace("!!!", "");
            return true;
        }
        return false;
    }
    public override List<Action> GetActions()
    {
        return actions;
    }
    public override Action GetAction()
    {
        return new Action(props, ActionType.Throw, lineNumber, column);
    }
}