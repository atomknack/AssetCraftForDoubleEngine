//based on https://stackoverflow.com/questions/43897078/c-sharp-equivalent-of-pythons-range-with-step/43897739#43897739
//rewritten to while loop
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// To make use Range like in python add "using static ExtensionMethods" in your cs file
public static partial class ExtensionMethods //_RangeLikeInPython
{
    public static void LogTestRange()
    {
        Debug.Log("Test Range(0, 10, 3) should return [0,3,6,9]");
        foreach (int i in Range(0, 10, 3)) Debug.Log(i); 

        Debug.Log("Test Range(4, -3, -1) should return [4,3,2,1,0,-1,-2]");
        foreach (int i in Range(4, -3, -1)) Debug.Log(i);

        Debug.Log("Test Range(5) should return [0,1,2,3,4]");
        foreach (int i in Range(5)) Debug.Log(i);

        Debug.Log("Test Range(2, 5) should return [2,3,4]");
        foreach (int i in Range(2, 5)) Debug.Log(i);

        Debug.Log("Test range(1, -3, 2) should return []");
        foreach (int i in Range(1, -3, 2)) Debug.Log(i); 
        
    }

    public static IEnumerable<int> Range(int start, int stop, int step = 1)
    {
        if (step == 0)
            throw new ArgumentException(nameof(step));

        return RangeIterator(start, stop, step);
    }

    public static IEnumerable<int> Range(int stop) => RangeIterator(0, stop, 1);

    private static IEnumerable<int> RangeIterator(int start, int stop, int step)
    {
        int x = start;

        while (!((step < 0 && x <= stop) || (0 < step && stop <= x)))
        {
            yield return x;
            x += step;
        }
        
    }
}
