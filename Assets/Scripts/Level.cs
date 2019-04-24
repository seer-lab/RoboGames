using System.Collections;
using System.Collections.Generic;
using System.Xml; 
using UnityEngine;

public class Level 
{
    public string Language { get; set; }
    public string NextLevel { get; set; }
    public string FileName { get; set; }
    public bool Warp { get; set; }
    public string[] Code { get; set; }
    public string[] Tags { get; set; }
    public float Time { get; set; }
    public int[] Tasks;
    public int[] CompletedTasks; 
    public int[,] TaskOnLine { get; set; }
    public string IntroText { get; set; }
    public string ExitText { get; set; }
    public int LineCount { get; set; }

    public XmlNodeList CodeNodes { get; set; }
    public IList<XmlNode> NodeList { get; set; }
    public XmlNode LevelNode { get; set; }
}
