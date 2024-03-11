using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


public class ModalDialog : MonoBehaviour
{
    public delegate void voidDelegate();

    private VisualElement _rootUI;
    //private VisualElement _background;

    private readonly List<Button> _buttons = new();
    private int _buttonIndex;
    private Button _okButton;
    private string _okButtonText = "Ok";
    private int _okButtonIndex;
    private Button _cancelButton;
    private string _cancelButtonText = "Cancel";
    private int _cancelButtonIndex;

    private static ModalDialog _instance = null;
    private string _header = "";
    private string _modalText = "";
    private voidDelegate _onOk = null;
    private voidDelegate _onCancel = null;

    private static ModalDialog Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = Instantiate(Resources.Load<GameObject>("UIDialogModalPrefab")).GetComponent<ModalDialog>();
                DontDestroyOnLoad(_instance.gameObject);
                _instance.gameObject.SetActive(false);
            }

            return _instance;
        }
    }

    public static bool ShowModal(string header = "", string modalText = "", 
        voidDelegate onOk = null, voidDelegate onCancel = null, 
        string okText = "Ok", string cancelText = "Cancel",
        int uiDocumentSortOrder = 1000)
    {
        if (Instance.gameObject.activeSelf)
        {
            Debug.Log("Must be only one modal dialog at time");
            //Instance.CancelAction();
            return false;
        }

        Instance._header = header;
        Instance._modalText = modalText;
        Instance._onOk = onOk;
        Instance._onCancel = onCancel;
        Instance._okButtonText = okText;
        Instance._cancelButtonText = cancelText;
        Instance.gameObject.GetComponent<UIDocument>().sortingOrder = uiDocumentSortOrder;
        Instance.gameObject.SetActive(true);

        return true;
    }

    private void Awake()
    {
        if (!_instance)
        {
            _instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void CloseModal()
    {
        Instance._onOk = null;
        Instance._onCancel = null;
        Instance.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        _buttons.Clear();
        _buttonIndex = 0;

        _rootUI = GetComponent<UIDocument>().rootVisualElement;
        //_background = _rootUI.Q<VisualElement>("background");

        _okButton = _rootUI.Q<Button>("okButton");
        _okButton.text = _okButtonText;
        _buttons.Add(_okButton);
        _okButtonIndex = _buttons.Count - 1;

        _cancelButton = _rootUI.Q<Button>("cancelButton");
        _cancelButton.text = _cancelButtonText;
        _buttons.Add(_cancelButton);
        _cancelButtonIndex = _buttons.Count - 1;

        //VisualElement modalForm = _rootUI.Q<VisualElement>("modalForm");
        _rootUI.Q<Label>("headerLabel").text = _header;
        _rootUI.Q<Label>("modalText").text = _modalText;

        _rootUI.RegisterCallback<KeyDownEvent>(Modal_KeyDown, TrickleDown.TrickleDown);
        //_rootUI.RegisterCallback<KeyDownEvent>(Modal_KeyDown, TrickleDown.NoTrickleDown);

        _rootUI.RegisterCallback<KeyUpEvent>(Modal_KeyUp, TrickleDown.TrickleDown);
        //_rootUI.RegisterCallback<KeyUpEvent>(Modal_KeyUp, TrickleDown.NoTrickleDown);

        _rootUI.RegisterCallback<MouseDownEvent>(SetButtonsFocus, TrickleDown.TrickleDown);
        _rootUI.RegisterCallback<ClickEvent>(SetButtonsFocus, TrickleDown.TrickleDown);


        _okButton.RegisterCallback<ClickEvent>(OkClicked);
        _okButton.RegisterCallback<KeyDownEvent>(OkPressed);

        _cancelButton.RegisterCallback<ClickEvent>(CancelClicked);
        _cancelButton.RegisterCallback<KeyDownEvent>(CancelPressed);

        _rootUI.schedule.Execute(() => SetButtonsFocus(_okButtonIndex));
        
    }
    private void SetButtonsFocus(MouseDownEvent ev)
    {
        //Debug.Log($"SetButtonsFocus mouseDownEvent {ev.pressedButtons} {ev}");
        if (ev.pressedButtons > 1)
        {
            ev.PreventDefault();
            ev.StopImmediatePropagation();
            ev.StopPropagation();
            SetButtonsFocus();
        }
        SetButtonsFocus();
    }

    private void SetButtonsFocus(ClickEvent ev)
    {
        //Debug.Log($"SetButtonsFocus ClickEvent {ev.pressedButtons} {ev}");
        if (ev.pressedButtons > 1)
        {
            ev.PreventDefault();
            ev.StopImmediatePropagation();
            ev.StopPropagation();
            SetButtonsFocus();
        }
    }
    private void SetButtonsFocus() => _buttons[_buttonIndex].Focus();
    private void SetButtonsFocus(int index) { _buttonIndex = index; SetButtonsFocus(); }

    private void OnDisable()
    {
        _rootUI.UnregisterCallback<KeyDownEvent>(Modal_KeyDown, TrickleDown.TrickleDown);
        //_rootUI.UnregisterCallback<KeyDownEvent>(Modal_KeyDown, TrickleDown.NoTrickleDown);
        _rootUI.UnregisterCallback<KeyUpEvent>(Modal_KeyUp, TrickleDown.TrickleDown);
        //_rootUI.UnregisterCallback<KeyUpEvent>(Modal_KeyUp, TrickleDown.NoTrickleDown);
        _rootUI.UnregisterCallback<MouseDownEvent>(SetButtonsFocus, TrickleDown.TrickleDown);
        _rootUI.UnregisterCallback<ClickEvent>(SetButtonsFocus, TrickleDown.TrickleDown);


        _okButton.UnregisterCallback<ClickEvent>(OkClicked);
        _okButton.UnregisterCallback<KeyDownEvent>(OkPressed);

        _cancelButton.UnregisterCallback<ClickEvent>(CancelClicked);
        _cancelButton.UnregisterCallback<KeyDownEvent>(CancelPressed);
    }
    private void OkClicked(ClickEvent ev) { 
        //Debug.Log("Ok clicked"); 
        SetButtonsFocus(_okButtonIndex); 
        OkAction(); }
    private void OkPressed(KeyDownEvent evt) {
        //Debug.Log("Ok something pressed"); 
        if (evt.keyCode == KeyCode.Return)
            OkAction();
        else 
            Modal_KeyDown(evt);
    }
    private void OkAction() { 
        //Debug.Log("Ok Activated"); 
        _onOk?.Invoke(); 
        CloseModal(); }


    private void CancelClicked(ClickEvent ev) { 
        //Debug.Log("Cancel clicked"); 
        SetButtonsFocus(_cancelButtonIndex); 
        CancelAction(); }
    private void CancelPressed(KeyDownEvent evt) { 
        //Debug.Log("Cancel something pressed"); 
        if (evt.keyCode == KeyCode.Return) 
            CancelAction();
        else 
            Modal_KeyDown(evt);
    }
    public void CancelAction() { 
        //Debug.Log("Cancel Activated"); 
        _onCancel?.Invoke(); 
        CloseModal(); }


    private void IncreaseButtonIndex()
    {
        _buttonIndex++;
        if (_buttonIndex >= _buttons.Count)
            _buttonIndex = 0;
    }
    private void DecreaseButtonIndex()
    {
        _buttonIndex--;
        if (_buttonIndex <0)
            _buttonIndex = _buttons.Count-1;
    }

    private void Modal_KeyUp(KeyUpEvent ev)
    {
        ev.PreventDefault();
        ev.StopPropagation();
        ev.StopImmediatePropagation();
        SetButtonsFocus();
        //_rootUI.schedule.Execute(() => { SetButtonsFocus(); }).ExecuteLater(0);
        _rootUI.schedule.Execute(() => SetButtonsFocus()).Every(5).ForDuration(100);
        //_rootUI.schedule.Execute(() => { SetButtonsFocus(); }).ExecuteLater(5);
        _rootUI.schedule.Execute(() => SetButtonsFocus()).ExecuteLater(200);
    }
    private void Modal_KeyDown(KeyDownEvent ev)
    {
        //Debug.Log($"KeyDownEvent {ev}");

        if (ev.keyCode == KeyCode.Tab || ev.keyCode == KeyCode.D || ev.keyCode == KeyCode.RightArrow)
            IncreaseButtonIndex();

        if ( ev.keyCode == KeyCode.A || ev.keyCode == KeyCode.LeftArrow)
            DecreaseButtonIndex();

        if (ev.keyCode == KeyCode.Escape)
        {
            //evt.PreventDefault();
            //evt.StopImmediatePropagation();
            CancelAction();
            return;
        }
        if (ev.keyCode == KeyCode.Return)
        {
            //Debug.Log("return pressed");
            return;
        }  

        ev.PreventDefault();
        ev.StopPropagation();
        ev.StopImmediatePropagation();
        SetButtonsFocus();
        //_rootUI.schedule.Execute(() => { SetButtonsFocus(); }).ExecuteLater(0);
        _rootUI.schedule.Execute(() =>SetButtonsFocus()).Every(5).ForDuration(100);
        //_rootUI.schedule.Execute(() => { SetButtonsFocus(); }).ExecuteLater(5);
        _rootUI.schedule.Execute(() => SetButtonsFocus()).ExecuteLater(200);

        /*if (evt.keyCode == KeyCode.Tab || evt.character == '\t')
        {
            evt.PreventDefault();
            evt.StopImmediatePropagation();
            SetButtonsFocus();
        }*/

    }
}
