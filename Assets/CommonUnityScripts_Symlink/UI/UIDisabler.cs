using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class UIDisabler : MonoBehaviour
{
    public string[] DisablElements;
    // Start is called before the first frame update
    void Start()
    {
        if (DisablElements != null)
        {
            VisualElement _rootUI = GetComponent<UIDocument>().rootVisualElement;
            foreach (string element in DisablElements)
            {
                var found = _rootUI.Q<VisualElement>(element);
                if (found != null)
                    found.SetEnabled(false);
            }
        }
    }

}
