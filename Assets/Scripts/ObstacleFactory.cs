

using System.Xml;
using System.Text.RegularExpressions; 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Creates All Obstacles
/// </summary>
public abstract class ObstacleFactory
{
    protected int lineNumber, column;
    protected XmlNode childnode;
    /// <summary>
    /// </summary>
    /// <param name="node">XML Node to be parsed for obstacles</param>
    /// <param name="nodeLine">the line where the node exists</param>
    protected ObstacleFactory(XmlNode node, int nodeLine)
    {
        childnode = node; 
        string[] lines = childnode.InnerText.Split('\n');
        int row = 0, col = 0;
        for (int i = 0; i < lines.Length; i++)
        {
            if (lines[i].Contains("$O"))
            {
                row = nodeLine + i;
                col = lines[i].IndexOf("$O");
                break; 
            }
        }
        lineNumber = row; 
        column = col; 
    }
    /// <summary>
    /// Gets a list of the parameters in the specification
    /// </summary>
    /// <param name="type">The type of the obstacle</param>
    /// <returns>string array of all the params</returns>
    protected string[] GetParams(string type){
        string rgx = @"((?<=\$O" + type + @").+(?=\$))"; 
        Regex paramRgx = new Regex(rgx); 
        string answers = paramRgx.Match(GlobalState.level.Code[lineNumber]).Value; 
        return answers.Split(' '); 
    }
    public abstract GameObject GetGameObject(); 
}

public class FirewallFactory: ObstacleFactory{
    public FirewallFactory(XmlNode node, int nodeLine)
        :base(node, nodeLine)
    {
        GlobalState.obstacleLine[stateLib.OBSTACLE_FIREWALL] += nodeLine.ToString() + " ";

    }
    public override GameObject GetGameObject(){
        GameObject firewall = Resources.Load<GameObject>("Prefabs/Firewall"); 
        firewall = GameObject.Instantiate(firewall); 
        Firewall script = firewall.GetComponent<Firewall>(); 
        script.Index = lineNumber; 
        int dmg = 0; 
        int.TryParse(GetParams("Firewall")[1], out dmg);
        script.Damage = dmg;
        return firewall;  
    }
}
