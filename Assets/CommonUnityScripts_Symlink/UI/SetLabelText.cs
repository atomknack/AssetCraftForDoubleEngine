using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class SetLabelText : MonoBehaviour
{
    public string LabelName;
    public string Text;
    private Label _label;
    private VisualElement _rootUI;

    public void SetLabelTextFromPublicField()
    {
        _label.text = Text;
    }

    public void SetText(string text)
    {
        _label.text = text;
    }

    private void OnEnable()
    {
        VisualElement _rootUI = GetComponent<UIDocument>().rootVisualElement;
        _label = _rootUI.Q<Label>(LabelName);

    }
}
