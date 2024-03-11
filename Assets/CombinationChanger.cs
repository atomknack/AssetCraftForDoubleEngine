using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DoubleEngine;
using DoubleEngine.Atom;

[RequireComponent(typeof(DrawFlatNodes))]
[RequireComponent(typeof(VerticesChanger))]
public class CombinationChanger : MonoBehaviour
{
    DrawFlatNodes _drawFlatNodes;
    EdgeDrawer _edgeDrawer;

    VerticesChanger _verticeChanger;
    FlatNode _baseNode;
    FlatNode _complimentary;
    private void OnEnable()
    {
        _drawFlatNodes = GetComponent<DrawFlatNodes>();
        _verticeChanger = GetComponent<VerticesChanger>();
        _edgeDrawer = FindObjectOfType<EdgeDrawer>(); //bad
    }
    public void UpdateFlatNodes(FlatNode newBaseNode, FlatNode newComplimentary)
    {
        _drawFlatNodes.UpdateFlatNodes(newBaseNode, newComplimentary);
        _verticeChanger.UpdateFlatNodes(newBaseNode, newComplimentary);

        if (_edgeDrawer != null) 
            _edgeDrawer.fn = newComplimentary; 

        if (_baseNode!=null && _complimentary!=null)
            if (newBaseNode!=_baseNode || newComplimentary!=_complimentary)
                ModalDialog.ShowModal("Save complimentary pair", $"{_baseNode} \n{_complimentary}",
                    () => Debug.Log("Modal chosen OK"), () => Debug.Log("Modal chosen Cancel"), "kinda save it", "forget it", 10000);

        _baseNode = newBaseNode;
        _complimentary = newComplimentary;
    }
    public void CleanFlatNodes()
    {
        //Debug.Log($"Clean SelectedPairNodesToChange");
        _drawFlatNodes.CleanFlatNodes();
        _verticeChanger.CleanFlatNodes();

        if (_edgeDrawer != null)
            _edgeDrawer.fn = null;

        if (_baseNode != null && _complimentary != null)
            ModalDialog.ShowModal("Save complimentary pair before forget", $"{_baseNode} \n {_complimentary}",
                () => Debug.Log("Modal chosen OK"), () => Debug.Log("Modal chosen Cancel"), "kinda save it", "forget it", 10000);

        _baseNode = null;
        _complimentary=null;
    }

}
