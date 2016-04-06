using UnityEngine;
using System.Collections;
using System;

public class BaseInputNode : BaseNode {
    public override void DrawCurves()
    {
        
    }

    public virtual string getResult()
    {
        return "None";
    }
}
