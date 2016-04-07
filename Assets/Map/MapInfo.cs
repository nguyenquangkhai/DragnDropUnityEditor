using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public class MapInfo : ScriptableObject {

    public Rect windowRect;
    public List<MapInfo> relativeMap = new List<MapInfo>();
    public bool hasInputs = false;
    public string windowTitle = "";
    public void DrawCurves()
    {
        if (relativeMap.Count > 0)
        {
            foreach(MapInfo info in relativeMap)
            {
                MapEditor.DrawNodeCurve(info.windowRect, windowRect);
            }
        }
    }

    public void DrawWindow()
    {
        windowTitle = EditorGUILayout.TextField("Title", windowTitle);

        GUILayout.Label("Total input:" + relativeMap.Count);

        foreach(MapInfo map in relativeMap)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.TextField(map.windowTitle);
            if (GUILayout.Button("-"))
            {
                relativeMap.Remove(map);
            }
            EditorGUILayout.EndHorizontal();
        }
    }

    public void SetInput(MapInfo input, Vector2 clickPosition)
    {
        if (windowRect.Contains(clickPosition))
        {
            if(relativeMap.Contains(input) == false)
                relativeMap.Add(input);
        }
    }

    public virtual MapInfo ClickedOnInput(Vector2 pos)
    {
        return null;
    }

    public void MapDeleted(MapInfo map)
    {
        relativeMap.Remove(map);
    }
}
