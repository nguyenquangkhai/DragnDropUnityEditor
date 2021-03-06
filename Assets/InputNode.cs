﻿using UnityEngine;
using System.Collections;
using UnityEditor;
using System;

public class InputNode : BaseInputNode {
    public enum InputType
    {
        Number, 
        Randomization
    }

    private InputType inputType;
    private string randomFrom = "";
    private string randomTo = "";

    private string inputValue = "";

    public InputNode()
    {
        windowTitle = "Input Node";
    }

    public override void DrawWindow()
    {
        base.DrawWindow();

        inputType = (InputType)EditorGUILayout.EnumPopup("Input Type: ", inputType);

        if( inputType == InputType.Number)
        {
            inputValue = EditorGUILayout.TextField("Value: ", inputValue);
        }
        else
        {
            randomFrom = EditorGUILayout.TextField("From: ", randomFrom);
            randomTo = EditorGUILayout.TextField("To: ", randomTo);

            if(GUILayout.Button("Calculate Random"))
            {
                calculateRandom();
            }

            inputValue = EditorGUILayout.TextField("Value: ", inputValue);
        }
    }

    public override void DrawCurves()
    {
        
    }

    private void calculateRandom()
    {
        float rFrom = 0;
        float rTo = 0;

        float.TryParse(randomFrom, out rFrom);
        float.TryParse(randomTo, out rTo);

        int randFrom = (int)(rFrom * 10);
        int randTo = (int)(rTo * 10);

        int selected = UnityEngine.Random.Range(randFrom, randTo + 1);

        float selectedValue = selected / 10;

        inputValue = selectedValue.ToString();
    }

    public override string getResult()
    {
        return inputValue.ToString();

    }
}
