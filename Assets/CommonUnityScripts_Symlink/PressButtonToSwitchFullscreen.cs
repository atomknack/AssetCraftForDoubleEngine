using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressButtonToSwitchFullscreen : MonoBehaviour
{
    public KeyCode fullscreenButton = KeyCode.F11;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(fullscreenButton))
        {
            Screen.fullScreen = !Screen.fullScreen;
        }
    }
}
