using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof (XMLParser))]
public class XMLChecker : Editor{
    public override void OnInspectorGUI() {
        XMLParser xmlParser = (XMLParser)target;

        base.OnInspectorGUI();

        if(GUILayout.Button("Check XML")) {
            xmlParser.checkXML();
        }
    }
}
