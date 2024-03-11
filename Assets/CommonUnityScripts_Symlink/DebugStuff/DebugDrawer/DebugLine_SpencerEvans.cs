/* Author: Spencer Evans
 * 
 * Description:
 * DebugLine.cs provides 2 useful functions for debugging : DrawLine and DrawRay. These functions are almost identical to those provided in the Unity 3D 
 * engine Debug class (http://docs.unity3d.com/Documentation/ScriptReference/Debug.html), except in the following ways:
 * 1. The lines are drawn in the Game window, not the Scene editor window.
 * 2. A "width" option is provided - allowing the user to choose the width in pixels of the line drawn.
 * 3. The "depthTest" parameter in the Debug.DrawLine() and Debug.DrawRay() functions is unavailable; all lines are obscured by objects closer to the camera.
 * 
 * Usage:
 * 1. Create a new folder titled "Plugins", and move it to your project's "/Assets" folder (if you haven't already).
 * 2. Create and place "DebugLine.cs" inside the "/Plugins" folder.
 * 3. Call the DebugLine.DrawLine() or DebugLine.DrawRay() functions just as you would with the Debug class.
 * 
 * Notes:
 * 1. You must have a Camera in your scene tagged "MainCamera" for these debug functions to work properly.
 * 2. You do NOT need Unity Pro for this to work.
 */

using UnityEngine;
using System.Collections;

public class DebugLine : MonoBehaviour
{
	//used to make calls from both Update and FixedUpdate function properly
	public float destroy_time = float.MaxValue;
	public float fixed_destroy_time = float.MaxValue;

	//draw ray functions - http://docs.unity3d.com/Documentation/ScriptReference/Debug.DrawRay.html
	public static void DrawRay(Vector3 start, Vector3 dir)
	{
		DebugLine.DrawLine(start, start + dir, Color.white);
	}
	public static void DrawRay(Vector3 start, Vector3 dir, Color color, float duration = 0, float width = 1)
	{
		DebugLine.DrawLine(start, start + dir, color, null, duration, width);
	}

	//draw line functions - http://docs.unity3d.com/Documentation/ScriptReference/Debug.DrawLine.html
	public static void DrawLine(Vector3 start, Vector3 end)
	{
		DebugLine.DrawLine(start, end, Color.white);
	}
	public static void DrawLine(Vector3 start, Vector3 end, Color color, Color? nullableEndColor=null, float duration = 0, float width = 1)
	{
		//early out if there is no Camera.main to calculate the width
		if (!Camera.main)
			return;

		Color endColor;
		if (nullableEndColor.HasValue)
			endColor = nullableEndColor.Value;
		else
			endColor = color;

		GameObject line = new GameObject("debug_line");

		//set the params of the line renderer - http://docs.unity3d.com/Documentation/ScriptReference/LineRenderer.html
		LineRenderer lineRenderer = line.AddComponent<LineRenderer>();
		//P lineRenderer.material = new Material(Shader.Find("Mobile/Particles/Additive"));
		//P https://stackoverflow.com/questions/46033156/unity-shader-returns-a-null-when-using-shader-find
		//P Edit->Project Settings->Graphics Then in the inspector where it says "Always Included Shaders" add "UI/Unlit/Text"
		lineRenderer.material = new Material(Shader.Find("UI/Unlit/Text"));
		lineRenderer.SetPosition(0, start);
		lineRenderer.SetPosition(1, end);
		//P lineRenderer.SetColors(color, color);
		lineRenderer.startColor = color;
		lineRenderer.endColor = endColor;
		//P lineRenderer.SetWidth(DebugLine.CalcPixelHeightAtDist((start - Camera.main.transform.position).magnitude) * width,
		//P 					   DebugLine.CalcPixelHeightAtDist((end - Camera.main.transform.position).magnitude) * width);
		lineRenderer.startWidth = DebugLine.CalcPixelHeightAtDist((start - Camera.main.transform.position).magnitude) * width * 4;
		lineRenderer.endWidth = DebugLine.CalcPixelHeightAtDist((end - Camera.main.transform.position).magnitude) * width * 4;

		//add the MonoBehaviour instance
		DebugLine debugLine = line.AddComponent<DebugLine>();

		//set the time to expire - default is 0 (will only draw for one update cycle)
		//use Update or FixedUpdate depending on the time of the invoking call
		if (Time.deltaTime == Time.fixedDeltaTime)
			debugLine.fixed_destroy_time = Time.fixedTime + duration;
		else debugLine.destroy_time = Time.time + duration;
	}

	//utility function to calculate the world height of a single pixel given the following info:
	// - Camera.main.fov
	// - Camera.main.pixelHeight
	// - distance to the camera
	public static float CalcPixelHeightAtDist(float dist)
	{
		//early out if there is no Camera.main to calculate the width
		if (!Camera.main)
			return 0;

		//http://docs.unity3d.com/Documentation/Manual/FrustumSizeAtDistance.html
		float frustumHeight = 2 * dist * Mathf.Tan(Camera.main.fieldOfView * 0.5f * Mathf.Deg2Rad);

		return frustumHeight / Camera.main.pixelHeight;
	}

	//check to see if should be destroyed yet
	public void Update()
	{
		if (Time.time > destroy_time)
			Destroy(transform.gameObject);
	}
	public void FixedUpdate()
	{
		if (Time.fixedTime > fixed_destroy_time)
			Destroy(transform.gameObject);
	}
}