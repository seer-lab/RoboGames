using System.Collections;
using System.Collections.Generic;
using System.IO; 
using UnityEngine;
using System.Xml; 

public class LevelManager
{
    public List<string> levels;
    public List<string> passed;
    private string filepath; 

    public LevelManager()
    {
        filepath = (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor) ? @"\" : @"/";
        levels = new List<string>();
        passed = new List<string>();
        SetTitle(); 
    }
    void SetTitle()
    {
        TextMesh title = GameObject.Find("Description").GetComponent<TextMesh>();
        foreach (XmlNode codenode in GlobalState.level.CodeNodes)
        {
            if (codenode.Name == stringLib.NODE_NAME_DESCRIPTION)
            {
                title.text = codenode.InnerText;
            }
        }
    }
    public void SaveGame()
    {
        levels.Clear();
        passed.Clear();
        string lfile = GlobalState.GameMode + "leveldata" + filepath + "levels.txt";

        StreamReader sr = File.OpenText(lfile);
        string line;
        while ((line = sr.ReadLine()) != null)
        {
            string[] data = line.Split(' ');
            levels.Add(data[0]);
            passed.Add(data[1]);
        }
        sr.Close();
        passed[levels.IndexOf(GlobalState.CurrentONLevel)] = "1";
        StreamWriter sw = File.CreateText(GlobalState.GameMode + "leveldata" + filepath + "levels.txt");
        for (int i = 0; i < levels.Count; i++)
        {
            sw.WriteLine(levels[i] + " " + passed[i]);
        }
        sw.Close();
    } 
}
