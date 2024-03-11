using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using DoubleEngine;

public class Vector2Comparer_1em5 : IEqualityComparer<Vector2>
{
    private const float EPSILON = 2e-5f; //0,00002
    private const float MULTIPLIER = 1e3f; //1000
    public bool Equals(Vector2 a, Vector2 b)
    {
        //Debug.Log($"{a:f9} {b:f9} {VectorUtil.CloseEnoughByEach(a, b, EPSILON)}");
        return a.CloseByEach(b, EPSILON);
    }

    public int GetHashCode(Vector2 obj)
    {
        int hash = FLOATtoRoundedINTdirtyFastHash(obj.x);
        hash = ((hash << 9) ^ hash) ^ FLOATtoRoundedINTdirtyFastHash(obj.y);
        return hash;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)] 
    private int FLOATtoRoundedINTdirtyFastHash(float f) => Mathf.RoundToInt(f * MULTIPLIER);
}
