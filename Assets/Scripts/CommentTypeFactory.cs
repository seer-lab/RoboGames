using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

/// <summary>
/// Handles the differences in various kinds of comments in the game. Used only by CommentFactory
/// </summary>
public abstract class CommentTypeFactory 
{
    protected XmlNode childnode;
    protected int lineNumber;
    public int Entity; 
    public CommentTypeFactory(XmlNode node, int line)
    {
        childnode = node;
        lineNumber = line; 
    }
    /// <summary>
    /// All Comments have a few variables that are initialized similarly. These
    /// are done here. 
    /// </summary>
    /// <param name="propertyHandler">The instance of Comment</param>
    protected virtual void Initalize(comment propertyHandler)
    {
        propertyHandler.Index = lineNumber;
        propertyHandler.groupid = int.Parse(childnode.Attributes[stringLib.XML_ATTRIBUTE_GROUPID].Value);
        try
        {
            propertyHandler.commentStyle = childnode.Attributes[stringLib.XML_ATTRIBUTE_COMMENT_STYLE].Value;
        }
        catch
        {
            propertyHandler.commentStyle = "single";
        }

        propertyHandler.Initialize(); 
    }
    /// <summary>
    /// Creates a Script version. 
    /// </summary>
    /// <returns></returns>
    public abstract comment GetScript();
    /// <summary>
    /// Applies the script to an object. Is used primarily over GetScript()
    /// </summary>
    /// <param name="obj">The GameObject to apply the script to</param>
    public abstract void ApplyScript(GameObject obj); 
}

public class BugCommentFactory : CommentTypeFactory
{
    BugComment propertyHandler;
    public BugCommentFactory(XmlNode node, int line)
        : base(node, line)
    {
        propertyHandler = new BugComment();
    }
    public override void ApplyScript(GameObject obj)
    {
        propertyHandler = obj.AddComponent<BugComment>() as BugComment;
        GetScript();
        GlobalState.level.Tasks[3]++;

    }
    public override comment GetScript()
    {
        Initalize(propertyHandler);
        propertyHandler.entityType = stateLib.ENTITY_TYPE_ROBOBUG_COMMENT;
        Entity = stateLib.ENTITY_TYPE_ROBOBUG_COMMENT;
        propertyHandler.errmsg = childnode.Attributes[stringLib.XML_ATTRIBUTE_TEXT].Value;
        if (childnode.Attributes[stringLib.XML_ATTRIBUTE_TOOL] != null)
        {           
            string toolatt = childnode.Attributes[stringLib.XML_ATTRIBUTE_TOOL].Value;
            string[] toolcounts = toolatt.Split(',');
            for (int i = 0; i < toolcounts.Length; i++)
            {
                propertyHandler.tools[i] = int.Parse(toolcounts[i]);

            }
            
        }
        return propertyHandler;
    }
}


public class DescriptionCommentFactory: CommentTypeFactory
{
    public DescriptionCommentFactory(XmlNode node, int line)
        :base(node, line)
    {
    }
    public override void ApplyScript(GameObject obj)
    {
        if (childnode.Attributes[stringLib.XML_ATTRIBUTE_CORRECT].Value == "true")
        {
            CorrectComment propertyHandler = obj.AddComponent<CorrectComment>() as CorrectComment;
            propertyHandler.entityType = stateLib.ENTITY_TYPE_CORRECT_COMMENT;
            Entity = stateLib.ENTITY_TYPE_CORRECT_COMMENT;
            GlobalState.level.Tasks[3]++;
            Debug.Log("Added Task"); 
            Initalize(propertyHandler); 
        }
        else if (childnode.Attributes[stringLib.XML_ATTRIBUTE_CORRECT].Value == "false")
        {
            IncorrectComment propertyHandler = obj.AddComponent<IncorrectComment>() as IncorrectComment; 
            propertyHandler.entityType = stateLib.ENTITY_TYPE_INCORRECT_COMMENT;         
            Entity = stateLib.ENTITY_TYPE_INCORRECT_COMMENT;
            Initalize(propertyHandler);
        }
    }
    public override comment GetScript()
    {
        comment propertyHandler; 
        if (childnode.Attributes[stringLib.XML_ATTRIBUTE_CORRECT].Value == "true")
        {
            propertyHandler = new CorrectComment();
            propertyHandler.entityType = stateLib.ENTITY_TYPE_CORRECT_COMMENT;
            Entity = stateLib.ENTITY_TYPE_CORRECT_COMMENT; 
            GlobalState.level.Tasks[3]++;
        }
        else if (childnode.Attributes[stringLib.XML_ATTRIBUTE_CORRECT].Value == "false")
        {
            propertyHandler = new IncorrectComment(); 
            propertyHandler.entityType = stateLib.ENTITY_TYPE_INCORRECT_COMMENT;
            Entity = stateLib.ENTITY_TYPE_INCORRECT_COMMENT;
        }
        else
        {
            throw new Exception("Error: Description comment is neither true or false"); 
        }

        Initalize(propertyHandler);
        return propertyHandler; 
    }

}

public class CodeCommentFactory: CommentTypeFactory
{
    public CodeCommentFactory(XmlNode node, int line)
        :base(node, line)
    {
    }
    public override void ApplyScript(GameObject obj)
    {
        if (childnode.Attributes[stringLib.XML_ATTRIBUTE_CORRECT].Value == "true")
        {
            CorrectUncomment propertyHandler = obj.AddComponent<CorrectUncomment>() as CorrectUncomment;
            propertyHandler.entityType = stateLib.ENTITY_TYPE_CORRECT_UNCOMMENT;
            Entity = stateLib.ENTITY_TYPE_CORRECT_UNCOMMENT; 
            GlobalState.level.Tasks[4]++;
            Initalize(propertyHandler);
        }
        else if (childnode.Attributes[stringLib.XML_ATTRIBUTE_CORRECT].Value == "false")
        {
            IncorrentUncomment propertyHandler = obj.AddComponent<IncorrentUncomment>() as IncorrentUncomment; 
            propertyHandler.entityType = stateLib.ENTITY_TYPE_INCORRECT_UNCOMMENT;
            Entity = stateLib.ENTITY_TYPE_INCORRECT_UNCOMMENT;
            Initalize(propertyHandler); 
        }
    }
    public override comment GetScript()
    {
        comment propertyHandler;
        if (childnode.Attributes[stringLib.XML_ATTRIBUTE_CORRECT].Value == "true")
        {
            propertyHandler = new CorrectUncomment();
            propertyHandler.entityType = stateLib.ENTITY_TYPE_CORRECT_UNCOMMENT;
            Entity = stateLib.ENTITY_TYPE_CORRECT_UNCOMMENT; 
            GlobalState.level.Tasks[4]++;
        }
        else if (childnode.Attributes[stringLib.XML_ATTRIBUTE_CORRECT].Value == "false")
        {
            propertyHandler = new IncorrectComment();
            propertyHandler.entityType = stateLib.ENTITY_TYPE_INCORRECT_UNCOMMENT;
            Entity = stateLib.ENTITY_TYPE_INCORRECT_UNCOMMENT;
        }
        else
        {
            throw new Exception("Error: Code comment is neither true or false");
        }
        return propertyHandler; 
    }
}