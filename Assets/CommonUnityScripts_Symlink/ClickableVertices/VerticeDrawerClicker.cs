/// Author Pavel Ivanov


/// To add clickable vertices to scene:
/// 1. add empty object to use as base
/// 2. and VerticeDrawerClicker to created base empty object
/// 3. all vertices spawned by VerticeDrawerClicker will be children of base

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VerticeDrawerClicker : MonoBehaviour
{
    private readonly List<ClickableVertice> _allClickableVertices = new();
    private readonly List<ClickableVertice> _selectedVertices = new();

    public delegate void OnSelectionChangeDelegate (int[] clickedIds);
    public OnSelectionChangeDelegate OnSelectionChange = null;
    public ClickableVertice.ClickDelegate OnAnyVerticeClick = null;
    public void AddVertice(Vector3 v, ClickableVertice.ClickDelegate _1, int id, Material unselectedMaterial, string gameobjectName)
    {
        ClickableVertice.ClickDelegate onVerticeClick = OnVerticeClicked;
        onVerticeClick += _1;

        GameObject vertice = Instantiate(Resources.Load<GameObject>("ClickableVerticeSpherePrefab"));
        ClickableVertice bcv = vertice.GetComponent<ClickableVertice>();
        bcv.Set(v, onVerticeClick, id, unselectedMaterial, gameobjectName);
        bcv.transform.SetParent(transform, false);
        _allClickableVertices.Add(bcv);
    }
    public void UnSelectAll()
    {
        foreach (var vertice in _selectedVertices)
            vertice.unSelect();
        _selectedVertices.Clear();
        CallSelectionChangeDelegate();
    }
    public void ClearAndRemoveAll()
    {
        //Debug.Log($"ClearAll called");
        UnSelectAll();
        //Debug.Log($"all clickable vertices to clear: {_allClickableVertices.Count}");
        foreach (var v in _allClickableVertices)
        {
            //Debug.Log($"destroy {v.ClickableVerticeId}");
            Destroy(v.gameObject);
        }
        _allClickableVertices.Clear();
        OnSelectionChange = null;
        OnAnyVerticeClick = null;
    }

    private void CallSelectionChangeDelegate()
    {
        int[] selected = _selectedVertices.Select(x => x.ClickableVerticeId).ToArray();
        if (OnSelectionChange != null)
        {
            OnSelectionChange(selected);
        }
    }
    private void OnVerticeClicked(int publicId)
    {
        if (OnAnyVerticeClick!= null)
            OnAnyVerticeClick(publicId);
        Debug.Log($"vertice clicker drawer OnVerticeClicked {publicId} called");
        CallSelectionChangeDelegate();

    }
    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
        {
            if (hit.collider is SphereCollider)
            {
                if (hit.collider.gameObject.GetComponent<ClickableVertice>() is ClickableVertice bcv)
                    CheckClick(bcv);
            }
            else
            {
                Debug.Log("Raycast hit not a MeshCollider");
            }
        }
    }
    private void CheckClick(ClickableVertice bcv)
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (_selectedVertices.Contains(bcv))
            {
                //RemoveSelectedVertice(bcv);
                bcv.unSelect();
                _selectedVertices.Remove(bcv);
                bcv.Click();
            }
            else
            {
                bcv.Select();
                _selectedVertices.Add(bcv);
                bcv.Click();
            }

        }
    }

}
