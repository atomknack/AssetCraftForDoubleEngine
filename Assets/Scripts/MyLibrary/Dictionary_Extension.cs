using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class Dictionary_Extension
{
    //Not tested //TODO Test method
public static bool AbsentKey<TKey,TValue>(this Dictionary<TKey,TValue> dict, TKey key)
    {
        return dict.ContainsKey(key) == false;
    }
}
