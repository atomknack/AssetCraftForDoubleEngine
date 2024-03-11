using System;
using System.Collections;
using System.Collections.Generic;
using DoubleEngine;
using DoubleEngine.Atom;
using UnityEngine;

public static partial class DebugDrawer
{
    public static void DrawEdges(ReadOnlySpan<Vec2D> v2, ReadOnlySpan<EdgeIndexed> singleEdges, Color colorStart, Color colorEnd, float duration = 0f, float lineWidth = 0.1f)
    {
        for (int i = 0; i < singleEdges.Length; i++)
        {
            var edge = singleEdges[i];
                DebugLine.DrawLine(
                    v2[edge.start].ToVector2(),
                    v2[edge.end].ToVector2(),
                    colorStart,
                    colorEnd,
                    duration,
                    lineWidth);
        }
    }

    
    public static void DrawEdges(Vector2[] v2s, EdgeIndexed[] singleEdges, Matrix4x4? m = null, Color[] polyColors = null)
    {
        IndexedEdgePoly poly = IndexedEdgePoly.DebugIndexedEdgePolyFromSingleEdges(singleEdges, v2s.ToArrayVec2D());
        for (int i = 0; i < poly.subPolygons.Length; i++)
        {
            Color color = Color.white;
            if (polyColors != null && polyColors.Length > i)
                color = polyColors[i];
            Vector2[] polyVector2 = IndexedEdgePoly.GetCorners(poly, i, v2s);// ToArrayVector2(v2s, i);
            DebugDrawer.DrawPolygonContour(polyVector2.ConvertXYtoXYZArray(0), m, color, Color.black, 0, 0.1f);
        }
    }

    public static void DrawPolygonContour(
        this Span<Vec2D> corners,
        double newZ = 0d,
        MatrixD4x4? m = null,
        Color? edgeStart = null,
        Color? edgeEnd = null,
        double duration = 0d,
        double lineWidth = 0.1d
        ) => DrawPolygonContour(
            corners.ToArray().ConvertXYtoXYZArray(newZ).ToArrayVector3().AsSpan(), 
            m.HasValue?m.Value.ToMatrix4x4():null, 
            edgeStart, 
            edgeEnd, 
            (float)duration, 
            (float)lineWidth);
    public static void DrawPolygonContour(
        this Span<Vector2> corners, 
        float newZ = 0f, 
        Matrix4x4? m = null, 
        Color? edgeStart = null, 
        Color? edgeEnd = null, 
        float duration = 0f, 
        float lineWidth =0.1f
        ) => DrawPolygonContour(
            corners.ToArray().ConvertXYtoXYZArray(newZ).AsSpan(),
            m,
            edgeStart,
            edgeEnd,
            duration,
            lineWidth);
    public static void DrawPolygonContour(
    this Span<Vector3> corners,
    Matrix4x4? m = null,
    Color? edgeStart = null,
    Color? edgeEnd = null,
    float duration = 0f,
    float lineWidth = 0.1f
    )
    {
        if (edgeStart == null)
            edgeStart = Color.white;
        if (edgeEnd == null)
            edgeEnd = Color.black;
        Vector3 prevPoint = corners[corners.Length - 1];

        for (int i = 0; i < corners.Length; i++)
        {
            Vector3 point = corners[i];
            if (m == null)
                DebugLine.DrawLine(
                    prevPoint,
                    point,
                    edgeStart.Value,
                    edgeEnd.Value,
                    duration,
                    lineWidth);
            else
                DebugLine.DrawLine(
                    m.Value.MultiplyPoint(prevPoint),
                    m.Value.MultiplyPoint(point),
                    edgeStart.Value,
                    edgeEnd.Value,
                    duration,
                    lineWidth);
            prevPoint = point;
        }

    }
}
