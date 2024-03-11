using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//[CreateAssetMenu(fileName = "New Generic ScriptableObjectEvent", menuName = "ScriptableObjects/Events")]
[System.Serializable]
public class ScriptableObjectEventGeneric1<T> : ScriptableObject
{
    private event UnityAction<T> action;

    // Start is called before the first frame update
    public void AddListener(UnityAction<T> listener)
    {
        action -= listener; //because we don't want duplicates, if we don't have listener in action this will do nothing :)
        action += listener;
    }
    public void RemoveListener(UnityAction<T> listener)
    {
        action -= listener;
    }

    public void RemoveAllListeners()
    {
        action = null;
    }

    public void Invoke(T value)
    {
        action?.Invoke(value);
    }
}
