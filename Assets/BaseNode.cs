﻿using UnityEngine;
using UnityEditor;
using System.Collections;

public abstract class BaseNode : ScriptableObject {

    public Rect windowRect;

    public bool hasInputs = false;
    public string windowTitle = "";
    public virtual void DrawWindow()
    {
        windowTitle = EditorGUILayout.TextField("Title", windowTitle);
    }

    public abstract void DrawCurves();

    public virtual void SetInput(BaseInputNode input, Vector2 clickPosition)
    {

    }

    public virtual void NodeDeleted(BaseNode node)
    {

    }

    public virtual BaseInputNode ClickedOnInput(Vector2 pos)
    {
        return null;
    }
}
