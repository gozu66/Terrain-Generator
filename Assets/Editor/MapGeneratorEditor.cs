using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomEditor (typeof (MapGenerator))]
public class MapGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        MapGenerator mapGen = (MapGenerator)target;

        if(DrawDefaultInspector())
        {
            if(mapGen.autoUpdate)
            {
                mapGen.drawMapInEditor();
            }
        }

        if(GUILayout.Button("Generate"))
        {
            mapGen.drawMapInEditor();
        }
    }
}