using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class Matrix4x4Util
{
    public static Matrix4x4? Apply(this Matrix4x4? m, Matrix4x4 lhs) => m == null ? lhs : m.Value*lhs;
}
