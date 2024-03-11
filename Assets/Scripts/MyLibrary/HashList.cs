using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Not Tested
[Obsolete("Not thread safe")]
public class HashList //Indexed List Of Unique Items
{
    private List<int> _list = new List<int>();
    private Dictionary<int, int> _hash = new Dictionary<int, int>();
    private int _count = 0;
    public int this[int index] { get { return _list[index]; } }
    public int Position(int item) => _hash[item];
    public bool Contains(int item) => _hash.ContainsKey(item);
    public void Add(int item)
    {
        if (_hash.ContainsKey(item) == false)
        {
            _list.Add(item);
            _hash.Add(item, _count);
            _count++;
        }
    }
    public int[] ToArray() => _list.ToArray();
}
