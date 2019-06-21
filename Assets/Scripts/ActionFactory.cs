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
    protected ActionFactory(XmlNode node, int nodeLine, CodeProperties properties)
    {
        this.props = properties; 
        color = new TextColoration();
        actions = new List<Action>();
        childnode = node;
        string[] lines = childnode.InnerText.Split('\n');
        /* 
        int row = 0, col = 0;
        for (int i = 0; i < lines.Length; i++)
        {
            if (HandleParams(lines[i], nodeLine + i))
            {
                row = nodeLine + i;
            }
        }
        lineNumber = row;
        */
    }
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
        Regex paramRgx = new Regex(@"((?<=\@).+(?=\@))");
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