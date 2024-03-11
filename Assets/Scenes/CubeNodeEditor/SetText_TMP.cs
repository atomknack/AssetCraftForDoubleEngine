using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class SetText_TMP : MonoBehaviour
{
    public string textField;
    private TMP_Text text;
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    private void Init()
    {
        if(text == null)
            text = GetComponent<TMP_Text>();
    }

    public void SetText(string textString)
    {
        Init();
        text.text = textString;
    }

    public void SetTextFromField()
    {
        Init();
        text.text = textField;
    }
}
