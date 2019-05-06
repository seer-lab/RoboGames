using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class XMLParser : MonoBehaviour {
    private string filename;
    private bool isSucesses = false;

    XMLParser(string filename) {
        this.filename = filename;
    }


    public bool checkXML() {

        isSucesses = true;
        Debug.Log("Checking file " + filename);
        XmlReaderSettings settings = new XmlReaderSettings();
        return false;
    }
}