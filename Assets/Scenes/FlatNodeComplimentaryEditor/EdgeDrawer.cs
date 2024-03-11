using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DoubleEngine;
using DoubleEngine.Atom;

public class EdgeDrawer : MonoBehaviour
{
    public FlatNode fn;

    void Update()
    {
        if (fn != null)
            DebugDrawer.DrawEdges(fn.Transformed.Vertices.ToArrayVector3().ConvertXYZtoXYArray(), 
                fn.singleEdges, 
                transform.localToWorldMatrix, 
                new Color[] { Color.green, Color.blue, Color.red, Color.yellow, Color.white }
                );
        //DrawEdges(fn, null, new Color[] { Color.green, Color.blue, Color.red, Color.yellow, Color.white });
    }

}
