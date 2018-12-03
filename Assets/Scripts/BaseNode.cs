using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;

using UnityEngine;

using UnityEditor;

public abstract class BaseNode : ScriptableObject {
    public Rect WindowRect;
    public bool HasInput = false;
    public string WindowTitle = "";

    public virtual void DrawWindow() {
        WindowTitle = EditorGUILayout.TextField("Title", WindowTitle);
    }

    public abstract void DrawCurves();

    public virtual void SetInput(BaseInputNode input, Vector2 clickPos) { }

    public virtual void NodeDeleted(BaseNode node) { }

    public virtual BaseInputNode ClickedOnInput(Vector2 pos) {
        return null;
    }
}