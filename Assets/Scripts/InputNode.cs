using UnityEditor;
using UnityEngine;

public class InputNode : BaseInputNode {

    InputType _inputType;

    public enum InputType {
        Number,
        Random
    }

    string _randomFrom = "";
    string _randomTo = "";
    
    string _inputValue = "";

    public InputNode() {
        WindowTitle = "Input Node";
    }

    public override void DrawWindow() {
        base.DrawWindow();

        _inputType = (InputType) EditorGUILayout.EnumPopup("Input type: ", _inputType);
        
        if (_inputType == InputType.Number) {
            _inputValue = EditorGUILayout.TextField("Value", _inputValue);
        } else {
            if (_inputType == InputType.Random) {
                _randomFrom = EditorGUILayout.TextField("From", _randomFrom);
                _randomTo = EditorGUILayout.TextField("To", _randomTo);

                if (GUILayout.Button("Calculate Random")) {
                    CalculateRandom();
                }
            }
        }
    }

    public override void DrawCurves() {
        
    }

    void CalculateRandom() {
        float from = 0;
        float to = 0;

        float.TryParse(_randomFrom, out from);
        float.TryParse(_randomTo, out to);

        int random = (int)Random.Range(from, to + 1);

        _inputValue = random.ToString();
    }

    public override string GetResult() {
        return _inputValue;
    }
}
