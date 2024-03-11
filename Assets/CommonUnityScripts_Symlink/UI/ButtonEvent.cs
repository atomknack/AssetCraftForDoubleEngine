using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class ButtonEvent : MonoBehaviour
{
    public string ButtonName;
    public UnityEvent buttonJSONClick;
    private Button _button;
    private VisualElement _rootUI;


    private void OnEnable()
    {
        VisualElement _rootUI = GetComponent<UIDocument>().rootVisualElement;
        _button = _rootUI.Q<Button>(ButtonName);
        _button.RegisterCallback<ClickEvent>(ev => buttonJSONClick.Invoke());

    }
}
