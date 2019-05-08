using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using UnityEngine;
using System.IO;

public class XMLParser : MonoBehaviour {
    private string filename;
    private bool isSucesses = false;

    XMLParser(string filename) {
        this.filename = filename;
        if (!File.Exists(filename)) {
            Debug.Log("File Does not exists");
        }
    }


    public bool checkXML() {

        isSucesses = true;
        Debug.Log("Checking file " + filename);
        XmlReaderSettings settings = new XmlReaderSettings();
        XmlReader reader = XmlReader.Create(filename, settings);

        try {

            XmlDocument document = new XmlDocument();
            document.Load(reader);
            ValidationEventHandler eventHandler = new ValidationEventHandler(ValidationEventHandler);
            document.Validate(eventHandler);
            return true;
        }
        catch (Exception ex){
            Debug.Log("Xml not well formed");
            Debug.Log(ex.Message);
        }
        return false;
    }


    static void ValidationEventHandler(object sender, ValidationEventArgs e) {
        switch (e.Severity) {
            case XmlSeverityType.Error:
                Console.WriteLine("Error: {0}", e.Message);
                break;
            case XmlSeverityType.Warning:
                Console.WriteLine("Warning {0}", e.Message);
                break;
        }

    }
}