using System;
using System.Collections;
using System.Collections.Generic;

using UnityEditor;

using UnityEngine;

public class ComparisonNode : BaseInputNode {
    BaseInputNode _inputOne;
    Rect _inputOneRect;

    BaseInputNode _inputTwo;
    Rect _inputTwoRect;

    ComparisonType _comparisonType;

    public enum ComparisonType {
        Greater,
        Less,
        Equal
    }

    public ComparisonNode() {
        WindowTitle = "Comparison Node";
        HasInput = true;
    }

    public override void DrawWindow() {
        base.DrawCurves();
        Event e = Event.current;

        _comparisonType = (ComparisonType) EditorGUILayout.EnumPopup("Comparison Type", _comparisonType);

        string inputOneTitle = "None";

        if (_inputOne) {
            inputOneTitle = _inputOne.GetResult();
        }

        GUILayout.Label("Input 1: " + inputOneTitle);

        if (e.type == EventType.Repaint) {
            _inputOneRect = GUILayoutUtility.GetLastRect();
        }

        string inputTwoTitle = "None";

        if (_inputTwo) {
            inputTwoTitle = _inputTwo.GetResult();
        }

        GUILayout.Label("Input 2: " + inputTwoTitle);

        if (e.type == EventType.Repaint) {
            _inputTwoRect = GUILayoutUtility.GetLastRect();
        }
    }

    public override void SetInput(BaseInputNode input, Vector2 clickPos) {
        clickPos.x -= WindowRect.x;
        clickPos.y -= WindowRect.y;

        if (_inputOneRect.Contains(clickPos)) {
            _inputOne = input;
        } else {
            if (_inputTwoRect.Contains(clickPos)) {
                _inputTwo = input;
            }
        }
    }

    public override void DrawCurves() {
        if (_inputOne) {
            Rect rect = WindowRect;
            rect.x += _inputOneRect.x;
            rect.y += _inputOneRect.y + _inputOneRect.height / 2;
            rect.width = 1;
            rect.height = 1;

            NodeEditor.DrawNodeCurve(_inputOne.WindowRect, rect);
        }

        if (_inputTwo) {
            Rect rect = WindowRect;
            rect.x += _inputTwoRect.x;
            rect.y += _inputTwoRect.y + _inputTwoRect.height / 2;
            rect.width = 1;
            rect.height = 1;

            NodeEditor.DrawNodeCurve(_inputTwo.WindowRect, rect);
        }
    }

    public override string GetResult() {
        float inputOneValue = 0;
        float inputTwoValue = 0;

        if (_inputOne) {
            float.TryParse(_inputOne.GetResult(), out inputOneValue);
        }

        if (_inputTwo) {
            float.TryParse(_inputTwo.GetResult(), out inputTwoValue);
        }

        string result = "false";

        switch (_comparisonType) {
            case ComparisonType.Greater:
                result = (inputOneValue > inputTwoValue).ToString();
                break;
            case ComparisonType.Less:
                result = (inputOneValue < inputTwoValue).ToString();
                break;
            case ComparisonType.Equal:
                result = (inputOneValue == inputTwoValue).ToString();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        return result;
    }

    public override BaseInputNode ClickedOnInput(Vector2 pos) {
        BaseInputNode retVal = null;

        pos.x -= WindowRect.x;
        pos.y -= WindowRect.y;

        if (_inputOneRect.Contains(pos)) {
            retVal = _inputOne;
            _inputOne = null;
        } else {
            if (_inputTwoRect.Contains(pos)) {
                retVal = _inputTwo;
                _inputTwo = null;
            }
        }

        return retVal;
    }

    public override void NodeDeleted(BaseNode node) {
        if (node.Equals(_inputOne)) {
            _inputOne = null;
        }
        
        if (node.Equals(_inputTwo)) {
            _inputTwo = null;
        }
    }
}
