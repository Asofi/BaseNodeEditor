using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using UnityEditor;

public class NodeEditor : EditorWindow {
    List<BaseNode> _windows = new List<BaseNode>();
    Vector2 _mousePos;
    BaseNode _selectedNode;
    bool _makeTransitionMode = false;

    [MenuItem("Window/Node Editor")]
    static void ShowEditor() {
        NodeEditor editor = EditorWindow.GetWindow<NodeEditor>();
    }

    void OnGUI() {
        Event e = Event.current;

        _mousePos = e.mousePosition;

        bool clickedOnWindow = false;
        int selectIndex = -1;

        for (int i = 0; i < _windows.Count; i++) {
            if (_windows[i].WindowRect.Contains(_mousePos)) {
                selectIndex = i;
                clickedOnWindow = true;
                break;
            }
        }

        if (e.button == 1 && !_makeTransitionMode) {
            if (e.type == EventType.MouseDown) {
                if (!clickedOnWindow) {
                    GenericMenu menu = new GenericMenu();

                    menu.AddItem(new GUIContent("Add Input Node"), false, ContextCallback, "inputNode");
                    menu.AddItem(new GUIContent("Add Output Node"), false, ContextCallback, "outputNode");
                    menu.AddItem(new GUIContent("Add Calculation Node"), false, ContextCallback, "calcNode");
                    menu.AddItem(new GUIContent("Add Comparison Node"), false, ContextCallback, "compNode");

                    menu.ShowAsContext();
                    e.Use();
                } else {
                    GenericMenu menu = new GenericMenu();

                    menu.AddItem(new GUIContent("Make Transition"), false, ContextCallback, "makeTransition");
                    menu.AddSeparator("");
                    menu.AddItem(new GUIContent("Delete Node"), false, ContextCallback, "deleteNode");

                    menu.ShowAsContext();
                    e.Use();
                }
            }
        } else if (e.button == 0 && e.type == EventType.MouseDown && _makeTransitionMode) {
            if (clickedOnWindow && !_windows[selectIndex].Equals(_selectedNode)) {
                _windows[selectIndex].SetInput((BaseInputNode) _selectedNode, _mousePos);
                _makeTransitionMode = false;
                _selectedNode = null;
            }

            if (!clickedOnWindow) {
                _makeTransitionMode = false;
                _selectedNode = null;
            }

            e.Use();
        } else if (e.button == 0 && e.type == EventType.MouseDown && !_makeTransitionMode) {
            if (clickedOnWindow) {
                BaseInputNode nodeToChange = _windows[selectIndex].ClickedOnInput(_mousePos);

                if (nodeToChange) {
                    _selectedNode = nodeToChange;
                    _makeTransitionMode = true;
                }
            }
        }

        if (_makeTransitionMode && _selectedNode) {
            Rect mouseRect = new Rect(e.mousePosition.x, e.mousePosition.y, 10, 10);

            DrawNodeCurve(_selectedNode.WindowRect, mouseRect);

            Repaint();
        }

        foreach (BaseNode baseNode in _windows) {
            baseNode.DrawCurves();
        }

        BeginWindows();

        for (int i = 0; i < _windows.Count; i++) {
            _windows[i].WindowRect = GUI.Window(i, _windows[i].WindowRect, DrawNodeWindow, _windows[i].WindowTitle);
        }

        EndWindows();
    }

    void DrawNodeWindow(int id) {
        _windows[id].DrawWindow();
        GUI.DragWindow();
    }

    void ContextCallback(object obj) {
        string clb = obj.ToString();
        
        bool clickedOnWindow = false;
        int selectIndex = -1;

        for (int i = 0; i < _windows.Count; i++) {
            if (_windows[i].WindowRect.Contains(_mousePos)) {
                selectIndex = i;
                clickedOnWindow = true;
                break;
            }
        }

        switch (clb) {
            case "inputNode":
                InputNode inputNode = new InputNode();
                inputNode.WindowRect = new Rect(_mousePos.x, _mousePos.y, 200, 150);

                _windows.Add(inputNode);
                break;
            case "outputNode":
                OutputNode outputNode = new OutputNode();
                outputNode.WindowRect = new Rect(_mousePos.x, _mousePos.y, 200, 100);

                _windows.Add(outputNode);
                break;
            case "calcNode":
                CalcNode calcNode = new CalcNode();
                calcNode.WindowRect = new Rect(_mousePos.x, _mousePos.y, 200, 150);

                _windows.Add(calcNode);
                break;
            case "compNode":
                ComparisonNode compNode = new ComparisonNode();
                compNode.WindowRect = new Rect(_mousePos.x, _mousePos.y, 200, 150);

                _windows.Add(compNode);
                break;
            case "makeTransition":
                if (clickedOnWindow) {
                    _selectedNode = _windows[selectIndex];
                    _makeTransitionMode = true;
                }

                break;
            case "deleteNode":
                if (clickedOnWindow) {
                    BaseNode selectedNode  = _windows[selectIndex];
                    _windows.RemoveAt(selectIndex);

                    foreach (BaseNode baseNode in _windows) {
                        baseNode.NodeDeleted(selectedNode);
                    }
                }

                break;
        }
    }

    public static void DrawNodeCurve(Rect start, Rect end) {
        Vector3 startPos = new Vector3(start.x + start.width/2, start.y + start.height/2, 0);
        Vector3 endPos = new Vector3(end.x + end.width/2, end.y + end.height/2, 0);
        Vector3 startTan = startPos + Vector3.right * 50;
        Vector3 endTan = endPos + Vector3.left * 50;
        Color shadowCol = new Color(0,0,0,.06f);

        for (int i = 0; i < 3; i++) {
            Handles.DrawBezier(startPos, endPos, startTan, endTan, shadowCol, null, (i+1) * 5);
        }
        
        Handles.DrawBezier(startPos, endPos, startTan, endTan, Color.black, null, 1);
    }
}