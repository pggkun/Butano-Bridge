using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ButanoSettings))]
public class ButanoSettingsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var script = (ButanoSettings)target;
 
        if(GUILayout.Button("Browse Include Folder", GUILayout.Height(40)))
        {
            script.ProjectIncludeFolder = EditorUtility.SaveFolderPanel("Choose your project include folder", "", "");
        }

        if(GUILayout.Button("Browse GFX Folder", GUILayout.Height(40)))
        {
            script.ProjectGFXFolder = EditorUtility.SaveFolderPanel("Choose your project graphics folder", "", "");
        }
         
    }
}
