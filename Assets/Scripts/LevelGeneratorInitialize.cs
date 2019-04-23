using UnityEngine;
using UnityEngine.UI; 
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Text;
using System.Xml;
using System.Text.RegularExpressions;
using System;

public partial class LevelGenerator: MonoBehaviour{
    private void LoadPanels(){
        string path = "Sprites/panel-"; 
        for (int i = 0; i < panels.Length; i++){
            panels[i] = Resources.Load<Sprite>(path+(i+2).ToString()); 
        }
    }
}

