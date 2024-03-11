using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using DoubleEngine;
using DoubleEngine.Atom;
using Collections.Pooled;

namespace Atom
{


    [RequireComponent(typeof(UIDocument))]
    [RequireComponent(typeof(DrawSingleFlatNode))]
    public class Deleter_CreateAndUpdateList : MonoBehaviour
    {
        public ListView flatNodesListView;

        private VisualElement rootUI;

        private FlatNode[] _items;
        LookUpTable<int> _flatNodeUsedByMeshes;
        private int _index = -1;

        DrawSingleFlatNode _flatNodeDrawer;

        public void ApplyDoubleRoundFilterToSelectedMesh()
        {
            CheckThatFlatNodeSelected();
            FlatNodes.ApplyFilterToBase(_index, 10);
            FlatNodes.SaveToJsonFile();
            CleanSelection();
            UpdateBaseList();
        }

        public void ApplyDeleteNodeFilterToSelectedMesh()
        {
            CheckThatFlatNodeSelected();
            FlatNodes.ApplyFilterToBase(_index, 32);
            FlatNodes.SaveToJsonFile();
            CleanSelection();
            UpdateBaseList();
        }

        public void ExportSelectedMesh()
        {
            CheckThatFlatNodeSelected();
            string today = DateTime.Now.Date.ToString("yyyyMMdd");
            SimpleFileBrowser.FileBrowser.ShowSaveDialog(
                x => JsonHelpers.SaveToJsonFile(
                   /* MeshFragmentVec2D.CreateMeshFragmentAsIsWithoutArraysCopying(
                        _items[_index].form.Vertices.ConvertXYZtoXYArray(), 
                        _items[_index].form.Triangles.ToArray()
                        ), */
                   _items[_index].form,
                    x[0]), 
                ()=>Debug.Log($"Save canceled"), 
                SimpleFileBrowser.FileBrowser.PickMode.Files,
                initialFilename: $"{today}_mesh2d_id{_items[_index].id}.json");
        }

        public void CheckThatFlatNodeSelected()
        {
            if (_index == -1)
            {
                Debug.LogError("No flat node selected");
                throw new Exception($"{_index}");
            }
        }

        private void OnEnable()
        {
            rootUI = GetComponent<UIDocument>().rootVisualElement;

            flatNodesListView = rootUI.Q<ListView>("flatNodes-list");

            _flatNodeDrawer = GetComponent<DrawSingleFlatNode>();

            UpdateBaseList();
        }

        private void SelectedNodeToChange(int index)
        {
            _index = index;
            Debug.Log($"SelectedPairNodesToChange {index}");

            _flatNodeDrawer.UpdateFlatNode(_items[index]);
        }


        private void CleanSelection()
        {
            Debug.Log($"Clean Selection called");
            _index = -1;
        }


        private void UpdateBaseList()
        {
            InitFlatNodes();

            Action<VisualElement, int> bindItem = (e, i) =>
            {
                (e.ElementAt(0) as Label).text = $"id: {_items[i].id}, used by: {_flatNodeUsedByMeshes.GetValues(_items[i].id).Length} meshes";
            };

            flatNodesListView.itemsSource = _items;
            flatNodesListView.makeItem = makeListItem;
            flatNodesListView.bindItem = bindItem;

            flatNodesListView.onSelectedIndicesChange += x => { if (x.Any()) SelectedNodeToChange(x.First()); else CleanSelection(); };

            flatNodesListView.AddToSelection(0);
        }

        private void InitFlatNodes()
        {
            FlatNodes.LoadFromJsonFile();
            _items = FlatNodes.AllDefaultNodes.ToArray();
            if(_flatNodeUsedByMeshes is not null)
                _flatNodeUsedByMeshes.Dispose();
            _flatNodeUsedByMeshes = new LookUpTable<int>(_items.Length);

            ThreeDimensionalCellMeshes.StaticConstructorInitIntactMeshesFromJson();
            int cellMeshesCount = ThreeDimensionalCellMeshes.GetCount();
            for (short i = 0; i < cellMeshesCount; i++)
            {
                DeshelledCubeMesh deshelled = ThreeDimensionalCellMeshes.GetDeshelled(i);
                _flatNodeUsedByMeshes.AddItem(deshelled.XNegativeFlatNode(Grid6SidesCached.Default).id, i);
                _flatNodeUsedByMeshes.AddItem(deshelled.YNegativeFlatNode(Grid6SidesCached.Default).id, i);
                _flatNodeUsedByMeshes.AddItem(deshelled.ZNegativeFlatNode(Grid6SidesCached.Default).id, i);
                _flatNodeUsedByMeshes.AddItem(deshelled.XPositiveFlatNode(Grid6SidesCached.Default).id, i);
                _flatNodeUsedByMeshes.AddItem(deshelled.YPositiveFlatNode(Grid6SidesCached.Default).id, i);
                _flatNodeUsedByMeshes.AddItem(deshelled.ZPositiveFlatNode(Grid6SidesCached.Default).id, i);
            }
        }

        private readonly Func<VisualElement> makeListItem = () =>
        {
            var box = new VisualElement();
            box.style.flexDirection = FlexDirection.Row;
            box.style.flexGrow = 1f;
            box.style.flexShrink = 0f;
            box.style.flexBasis = 0f;
            box.Add(new Label());
            return box;
        };

}

}