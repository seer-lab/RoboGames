using System;
using System.Xml;
using System.Text.RegularExpressions; 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class ActionFactory{
    protected XmlNode childnode; 
    protected int lineNumber, column;
    protected TextColoration color; 
    protected List<Action> actions; 
    protected ActionFactory(XmlNode node, int nodeLine){
        color = new TextColoration();  
        actions = new List<Action>(); 
        childnode = node; 
        string[] lines = childnode.InnerText.Split('\n');
        int row = 0, col = 0;
        for (int i = 0; i < lines.Length; i++)
        {
            if (HandleParams(lines[i], nodeLine + i)){
                row = nodeLine + i; 
            } 
        }
        lineNumber = row; 
    }
    public abstract bool HandleParams(string text, int line); 
    public abstract Action GetAction(); 
    public abstract List<Action> GetActions(); 
}
public class DialogFactory: ActionFactory{
    int order = 0; 
    string ans; 
    public DialogFactory(XmlNode node, int line)
        :base(node, line)
    {
    }
    public override bool HandleParams(string text, int line){
        if (!text.Contains("@")) return false; 
        Regex paramRgx = new Regex(@"((?<=\@).+(?=\@))"); 
        string values = paramRgx.Match(text).Value; 
        GlobalState.level.Code[line] = color.ColorizeText(GlobalState.level.Code[line].Replace("@" + values + "@", ""), GlobalState.level.Language); 
        column = text.IndexOf("@");  
        actions.Add(new Action(ActionType.Dialog, line, column, values)); 
        return true; 
    }
    public override List<Action> GetActions(){
        return actions; 
    }
    public override Action GetAction(){
        return new Action(ActionType.Dialog, lineNumber, column, ans); 
    }
}
public class FireFactory: ActionFactory{
    public FireFactory(XmlNode node, int line)
        :base(node, line)
    {

    }
    public override bool HandleParams(string text, int line){
        if (text.Contains("!!!")){
            column = text.IndexOf("!!!"); 
            GlobalState.level.Code[line] = color.ColorizeText(GlobalState.level.Code[line].Replace("!!!", ""), GlobalState.level.Language); 
            actions.Add(new Action(ActionType.Throw, line, column)); 
        }
        return false; 
    }
    public override List<Action> GetActions(){
        return actions; 
    }
    public override Action GetAction(){
        return new Action(ActionType.Throw, lineNumber, column); 
    }
}