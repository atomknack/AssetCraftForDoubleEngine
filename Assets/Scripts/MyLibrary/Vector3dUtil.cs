using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DoubleEngine;
using DoubleEngine.Atom;

public static class Vector3dUtil
{
	/*
	public static bool IsVerticeOnEdges(Vector3[] vertices, EdgeIndexed[] edges, Vector3 toCheck, out EdgeIndexed edgeContainingPoint)// out int EdgeStart, out int EdgeEnd)
	{
		for (var i = 0; i < edges.Length; i ++)
		{
			if (EdgeVector3.PointBelongsToEdge(vertices[edges[i].start], vertices[edges[i].end], toCheck))
			{
				edgeContainingPoint = edges[i];
				return true;
			}
		}
		edgeContainingPoint = new EdgeIndexed(); //point is not on any edge, just fill up out variable, so compiler wont complain
		return false;
	}
	public static bool IsVerticeOnAnyTriangleEdge(Vector3[] vertices, int[] triangles, Vector3 toCheck, out EdgeIndexed edgeContainingPoint)//, out int EdgeStart, out int EdgeEnd)
	{
		for (var i = 0; i < triangles.Length; i += 3)
		{
			if (EdgeVector3.PointBelongsToEdge(vertices[triangles[i]], vertices[triangles[i + 1]], toCheck))
			{
				edgeContainingPoint = new EdgeIndexed(triangles[i], triangles[i + 1]);
				return true;
			}
			if (EdgeVector3.PointBelongsToEdge(vertices[triangles[i + 1]], vertices[triangles[i + 2]], toCheck))
			{
				edgeContainingPoint = new EdgeIndexed(triangles[i + 1], triangles[i + 2]);
				return true;
			}
			if (EdgeVector3.PointBelongsToEdge(vertices[triangles[i]], vertices[triangles[i + 2]], toCheck))
			{
				edgeContainingPoint = new EdgeIndexed(triangles[i], triangles[i + 2]);
				return true;
			}
		}
		edgeContainingPoint = new EdgeIndexed(); //point is not on any edge, just fill up out variable, so compiler wont complain
		return false;
	}
	*/
	/*
	public static bool PointBelongsToLineSegment(Vector3 lineStart, Vector3 lineEnd, Vector3 Point) //Need testing
	{
		Vector3 lineVector = lineEnd - lineStart;
		Vector3 PointInLineStartCoords = Point - lineStart;
		float ProjectionPosition = Vector3.Dot(PointInLineStartCoords, lineVector) / Vector3.SqrMagnitude(lineVector);
		if (ProjectionPosition < 0 || ProjectionPosition > 1)
			return false; //Point is outside of line segment, it may or may not belong to some other part of line

		Vector3 ProjectedPoint = lineStart + (lineVector * ProjectionPosition);
		if (Vector3.SqrMagnitude(Point - ProjectedPoint) >= 0.0000001f)
			return false; //Point is not belongs to any part of line

		return true;
	}

	public static Vector3 ProjectPointOnLine(Vector3 lineStart, Vector3 lineEnd, Vector3 Point) //Need testing
    {
		Vector3 lineVector = lineEnd - lineStart;
		Vector3 PointInLineStartCoords = Point - lineStart;
		float ProjectionPosition = Vector3.Dot(PointInLineStartCoords, lineVector) / Vector3.SqrMagnitude(lineVector);
		return lineStart + (lineVector * ProjectionPosition);
	}

	*/
}
