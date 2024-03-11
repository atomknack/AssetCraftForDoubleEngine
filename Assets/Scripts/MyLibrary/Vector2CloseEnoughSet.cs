using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Vector2CloseEnoughSet : HashSet<Vector2>
{
    private static readonly Vector2Comparer_1em5 _comparerInstance = new Vector2Comparer_1em5();
    public static Vector2CloseEnoughSet NewHashSet() => new Vector2CloseEnoughSet(_comparerInstance);
    private Vector2CloseEnoughSet(Vector2Comparer_1em5 comparer): base(comparer) { }
}
