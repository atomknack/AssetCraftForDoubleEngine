using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class NeighbourSelector : MonoBehaviour
{
    public Button Next { private set; get; }
    public UnityEvent NextButtonClick;
    public Button Prev { private set; get; }
    public UnityEvent PrevButtonClick;

    //public Button CreateConnectionNode { private set; get; }
    //public UnityEvent CreateConnectionNodeButtonClick;
    //public ListView NeighborList { private set; get; }

    private VisualElement rootUI;


    private void OnEnable()
    {
        rootUI = GetComponent<UIDocument>().rootVisualElement;

        Next = rootUI.Q<Button>("next-button");
        Next.RegisterCallback <ClickEvent>(ev => NextButtonClick.Invoke());

        Prev = rootUI.Q<Button>("prev-button");
        Prev.RegisterCallback<ClickEvent>(ev => PrevButtonClick.Invoke());

        //CreateConnectionNode = rootUI.Q<Button>("CreateConnectionNode");
        //CreateConnectionNode.RegisterCallback<ClickEvent>(ev => CreateConnectionNodeButtonClick.Invoke());

        //NeighborList = rootUI.Q<ListView>("neigborTypes-list");
        //AddExampleListItems();

    }

    /*
    private void AddExampleListItems()
    {
        List<string> listData =  new List<string> { "Joe", "Larry", "Richard", "Barry", "Linda", "Carol", "Ilea", "Bob" }; //new List<string>();

        Func<VisualElement> makeItem = () =>
        {
            var box = new VisualElement();
            box.style.flexDirection = FlexDirection.Row;
            box.style.flexGrow = 1f;
            box.style.flexShrink = 0f;
            box.style.flexBasis = 0f;
            box.Add(new Label());
            box.Add(new Button(() => { }) { text = "Button" });
            return box;
        };

        Action<VisualElement, int> bindItem = (e, i) =>
        {
            (e.ElementAt(0) as Label).text = (string)listData[i];
            (e.ElementAt(1) as Button).text = $"B {(string)listData[i]} B";
        };

        //itemsSource is generic IList so data any kind of data objects inside it can be used
        NeighborList.itemsSource = listData.Select(s => s.ToUpper()).ToList();
        //NeighborList.itemsSource = listData.Select(s => s.Length).ToList<int>();

        NeighborList.makeItem = makeItem;
        NeighborList.bindItem = bindItem;

        NeighborList.AddToSelection(0);

        NeighborList.onItemsChosen += obj => Debug.Log($"{obj.ToString()} nl");
        NeighborList.onSelectionChange += objects => Debug.Log($"{objects} nl ch");
        NeighborList.onSelectionChange += objects => Debug.Log($"{objects.First()} nl ch first {objects.First().GetType()}");

    }
    */
}
