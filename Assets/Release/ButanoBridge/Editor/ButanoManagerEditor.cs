using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ButanoManager))]
public class ButanoManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var script = (ButanoManager)target;
 
        if(GUILayout.Button("Export All", GUILayout.Height(40)))
        {
            script.ExportAll();
        }
    }
}
