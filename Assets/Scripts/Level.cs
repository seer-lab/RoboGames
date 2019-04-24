using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level 
{
    public string Language { get; set; }
    public string NextLevel { get; set; }
    public string FileName { get; set; }
    public bool Warp { get; set; }
    public string[] Code { get; set; }
    public float Time { get; set; }
    public int[] Tasks;
    public int[] CompletedTasks; 

}
