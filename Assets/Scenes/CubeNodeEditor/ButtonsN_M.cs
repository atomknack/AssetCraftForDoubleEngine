using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class ButtonsN_M : MonoBehaviour
{
    public Button ButtonN { private set; get; }
    public UnityEvent ButtonNClick;
    public Button ButtonM { private set; get; }
    public UnityEvent ButtonMClick;

    private VisualElement rootUI;


    private void OnEnable()
    {
        VisualElement rootUI = GetComponent<UIDocument>().rootVisualElement;

        ButtonN = rootUI.Q<Button>("ButtonN");
        ButtonN.RegisterCallback<ClickEvent>(ev => ButtonNClick.Invoke());

        ButtonM = rootUI.Q<Button>("ButtonM");
        ButtonM.RegisterCallback<ClickEvent>(ev => ButtonMClick.Invoke());
    }
}
