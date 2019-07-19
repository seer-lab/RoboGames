using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using UnityEngine;
using System.IO;

// TODO : Actually implement this in code

/// <summary>
/// A class that validates the XML
/// </summary>

public class XMLParser : MonoBehaviour {
    public string filedirectory;
    public bool isSucesses = false;
    public static string[] files{get;set;}
    public string schemafile;
    public static int i{get; set;}
    public void checkXML(){
        files = Directory.GetFiles(this.filedirectory, "*.xml");
        schemafile = Directory.GetFiles(this.filedirectory, "*.xsd")[0];

        //Sets the schema file
        XmlSchemaSet schemaset = new XmlSchemaSet();
        schemaset.Add(null, schemafile);

        //Sets the setting
        XmlReaderSettings settings = new XmlReaderSettings();
        settings.ValidationType = ValidationType.Schema;
        settings.Schemas = schemaset;
        settings.ValidationEventHandler += new ValidationEventHandler(ValidationCallBack);

        //Read through the file and produce any error
        for(i =0 ; i < files.Length; i++){
            try{
            XmlReader reader = XmlReader.Create(files[i], settings);
            while(reader.Read());
            }catch(Exception e){
                string errMessage = "XML File: " + files[i] + " " + e.Message;
                Debug.LogError(errMessage);
            }
        }
    }

    static void ValidationCallBack(object sender, ValidationEventArgs e) {
        string errMessage = "XML File: " + files[i] + " " + e.Message;
        switch (e.Severity) {
            case XmlSeverityType.Error:
                Debug.LogWarning(errMessage);
                break;
            case XmlSeverityType.Warning:
                Debug.LogError(errMessage);
                break;
        }

    }
}