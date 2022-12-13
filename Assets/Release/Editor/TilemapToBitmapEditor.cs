using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TilemapToBitmap))]
public class TilemapToBitmapEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var script = (TilemapToBitmap)target;
        if(GUILayout.Button("Adjust 8x8 Tiles", GUILayout.Height(40)))
        {
            script.Adjust8x8Tiles();
        }
        if(GUILayout.Button("Adjust 16x16 Tiles", GUILayout.Height(40)))
        {
            script.Adjust16x16Tiles();
        }
        if(GUILayout.Button("Export", GUILayout.Height(40)))
        {
            script.ExportTilemap();
        }
         
    }
}
