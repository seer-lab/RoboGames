using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof (XMLParser))]
public class XMLChecker : Editor{
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        if(GUILayout.Button("Check XML")) {
            //XMLParser xMLParser = new XMLParser();
        }
    }
}
