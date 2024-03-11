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
    [RequireComponent(typeof(DrawSingleThreeDimensionalCellMesh))]
    public class ThreeDimensionalCellMeshesDeleter_CreateAndUpdateList : MonoBehaviour
    {
        public ListView flatNodesListView;

        private VisualElement rootUI;

        private FlatNode[] _flatNodes;
        private DeshelledCubeMesh[] _deshelledMeshes;
        LookUpTable<int> _meshesUseFlatnodes;
        private short _index = -1;

        DrawSingleThreeDimensionalCellMesh _cellMeshDrawer;

        /*
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
        */

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

            _cellMeshDrawer = GetComponent<DrawSingleThreeDimensionalCellMesh>();

            UpdateBaseList();
        }

        private void SelectedListItemChanged(short index)
        {
            _index = index;
            Debug.Log($"SelectedPairNodesToChange {index}");

            _cellMeshDrawer.UpdateMesh(ThreeDimensionalCellMeshes.GetUnityMesh(index));
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
                (e.ElementAt(0) as Label).text = $"id: {i}, uses flatnode ids: {String.Join(' ',_meshesUseFlatnodes.GetValues(i).ToArray())}";
            };

            flatNodesListView.itemsSource = _deshelledMeshes;
            flatNodesListView.makeItem = makeListItem;
            flatNodesListView.bindItem = bindItem;

            flatNodesListView.onSelectedIndicesChange += x => { if (x.Any()) SelectedListItemChanged((short)x.First()); else CleanSelection(); };

            flatNodesListView.AddToSelection(0);
        }

        private void InitFlatNodes()
        {
            FlatNodes.LoadFromJsonFile();
            _flatNodes = FlatNodes.AllDefaultNodes.ToArray();

            ThreeDimensionalCellMeshes.StaticConstructorInitIntactMeshesFromJson();
            if (_meshesUseFlatnodes is not null)
                _meshesUseFlatnodes.Dispose();
            int cellMeshesCount = ThreeDimensionalCellMeshes.GetCount();
            _deshelledMeshes = new DeshelledCubeMesh[cellMeshesCount];
            
            _meshesUseFlatnodes = new LookUpTable<int>(cellMeshesCount);

            for (short i = 0; i < cellMeshesCount; i++)
            {
                DeshelledCubeMesh deshelled = ThreeDimensionalCellMeshes.GetDeshelled(i);
                _deshelledMeshes[i] = deshelled;
                _meshesUseFlatnodes.AddItem(i, deshelled.XNegativeFlatNode(Grid6SidesCached.Default).id);
                _meshesUseFlatnodes.AddItem(i, deshelled.YNegativeFlatNode(Grid6SidesCached.Default).id);
                _meshesUseFlatnodes.AddItem(i, deshelled.ZNegativeFlatNode(Grid6SidesCached.Default).id);
                _meshesUseFlatnodes.AddItem(i, deshelled.XPositiveFlatNode(Grid6SidesCached.Default).id);
                _meshesUseFlatnodes.AddItem(i, deshelled.YPositiveFlatNode(Grid6SidesCached.Default).id);
                _meshesUseFlatnodes.AddItem(i, deshelled.ZPositiveFlatNode(Grid6SidesCached.Default).id);
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