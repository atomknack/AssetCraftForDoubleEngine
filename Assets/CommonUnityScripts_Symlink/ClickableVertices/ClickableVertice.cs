/// Author Pavel Ivanov
/// Used in ClickableVerticeSpherePrefab
/// Not for separate use
/// To add clickable vertices to scene: 
/// 1. add empty object to use as base
/// 2. and VerticeDrawerClicker to created base empty object
/// 3. all vertices spawned by VerticeDrawerClicker will be children of base

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[RequireComponent(typeof(MeshRenderer))]
public class ClickableVertice : MonoBehaviour
{
    [SerializeField] private Material _unSelected;
    [SerializeField] private Material _selected;
    private MeshRenderer _renderer;
    private ClickDelegate _click = null;

    public delegate void ClickDelegate(int ClickableVerticeId);
    public int ClickableVerticeId = -1;

  
    public void Set(Vector3 position, ClickDelegate click, int id,  Material unSelected = null, string gameobjectName = null)
    {
        if (gameobjectName != null)
        {
            gameObject.name = gameobjectName;
        }
        _click = click;
        ClickableVerticeId = id;
        transform.position = position;
        if (unSelected != null)
        {
            _unSelected = unSelected;
            if (_renderer!=null)
                _renderer.sharedMaterial = _unSelected;
        }
            
    }

    /*private void OnDisable()
    {
        _click = null;
    }*/

    public void Click()
    {
        if (_click != null)
            _click(ClickableVerticeId);
    }

    public void Select()
    {
        _renderer.sharedMaterial = _selected;
    }

    public void unSelect()
    {
        _renderer.sharedMaterial = _unSelected;
    }

    private void Start()
    {
        _renderer = GetComponent<MeshRenderer>();
        _renderer.sharedMaterial = _unSelected;
    }
}
