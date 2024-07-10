using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GenericScriptTest))]
[CanEditMultipleObjects]
public class GenericScriptTestEditor : Editor
{
    //Settings
    
    // Connections
    
    // State Variables
    
    // Start is called before the first frame update
    void Start()
    {
        //InitConnections();
        //InitState();
    }
    void InitConnections(){
    }
    void InitState(){
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if (GUILayout.Button("Launch Test"))
        {
            ((GenericScriptTest)target).LaunchTest();
        }
    }
}

