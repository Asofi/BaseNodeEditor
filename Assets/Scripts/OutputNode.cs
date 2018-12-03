using UnityEditor;
using UnityEngine;

public class OutputNode : BaseNode {

    string _result = "";
    
    BaseInputNode _inputNode;
    Rect _inputNodeRect;

    public OutputNode() {
        WindowTitle = "Output Node";
        HasInput = true;
    }
    
    public override void DrawWindow() {
        base.DrawWindow();

        Event e = Event.current;
        string inputOneTittle = "None";
        if (_inputNode) {
            inputOneTittle = _inputNode.GetResult();
        }
        
        GUILayout.Label("Input 1: " + inputOneTittle);

        if (e.type == EventType.Repaint) {
            _inputNodeRect = GUILayoutUtility.GetLastRect();
        }
        
        GUILayout.Label("Result: " + _result);
    }
    
    public override void DrawCurves() {
        if (_inputNode) {
            Rect rect = WindowRect;
            rect.x += _inputNodeRect.x;
            rect.y += _inputNodeRect.y + _inputNodeRect.height / 2;
            rect.width = 1;
            rect.height = 1;
            
            NodeEditor.DrawNodeCurve(_inputNode.WindowRect, rect);
        }
    }

    public override void NodeDeleted(BaseNode node) {
        if (node.Equals(_inputNode)) {
            _inputNode = null;
        }
    }

    public override BaseInputNode ClickedOnInput(Vector2 pos) {
        BaseInputNode retVal = null;

        pos.x -= WindowRect.x;
        pos.y -= WindowRect.y;

        if (_inputNodeRect.Contains(pos)) {
            retVal = _inputNode;
            _inputNode = null;
        }

        return retVal;
    }

    public override void SetInput(BaseInputNode input, Vector2 clickPos) {
        clickPos.x -= WindowRect.x;
        clickPos.y -= WindowRect.y;
        
        if (_inputNodeRect.Contains(clickPos)) {
            _inputNode = input;
        }
    }
}
