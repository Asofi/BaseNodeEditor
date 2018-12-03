using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using UnityEditor;

using UnityEngine;

public class CalcNode : BaseInputNode {
    BaseInputNode _inputOne;
    Rect _inputOneRect;

    BaseInputNode _inputTwo;
    Rect _inputTwoRect;

    CalculationType _calculationType;

    public enum CalculationType {
        Addition,
        Substraction,
        Multiplication,
        Division
    }

    public CalcNode() {
        WindowTitle = "Calculation Node";
        HasInput = true;
    }

    public override void DrawWindow() {
        base.DrawCurves();
        Event e = Event.current;

        _calculationType = (CalculationType) EditorGUILayout.EnumPopup("Calculation Type", _calculationType);

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

        switch (_calculationType) {
            case CalculationType.Addition:
                result = (inputOneValue + inputTwoValue).ToString();
                break;
            case CalculationType.Substraction:
                result = (inputOneValue - inputTwoValue).ToString();
                break;
            case CalculationType.Multiplication:
                result = (inputOneValue * inputTwoValue).ToString();
                break;
            case CalculationType.Division:
                result = (inputOneValue / inputTwoValue).ToString();
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