using Atom;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using DoubleEngine;
using DoubleEngine.Atom;
//using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(MeshFragmentVector3))]
//[CustomPropertyDrawer(typeof(MeshFragment?))]
public class MeshFragment_PropertyDrawer : PropertyDrawer
{
    public bool foldVectors;
    public bool foldTriangles;
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)

    {
        MeshFragmentVector3 fragment = (MeshFragmentVector3)EditorHelper.GetTargetObjectOfProperty(property);
        float extraHeight = 40.0f;
        if (fragment.Vertices != null && foldVectors)
            extraHeight += fragment.Vertices.Length * 20;
        if (fragment.Triangles != null && foldTriangles)
            extraHeight += fragment.Triangles.Length * 20;
        return base.GetPropertyHeight(property, label) + extraHeight;
    }
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        int r = 0;
        Rect NextRect()
        {
            var newRect = new Rect(position.x, position.y + (r * 20), position.width, 20);
            r++;
            return newRect;
        }
        MeshFragmentVector3 fragment = (MeshFragmentVector3)EditorHelper.GetTargetObjectOfProperty(property);
        EditorGUI.BeginProperty(position, label, property);
        EditorGUI.PrefixLabel(NextRect(), GUIUtility.GetControlID(FocusType.Passive), label);
        EditorGUI.indentLevel++;
        if (fragment == null)
        {
            EditorGUI.LabelField(NextRect(), "MeshFragment = null");
        }
        else
        {
            if (fragment.Vertices != null)
            {
                if (foldVectors = EditorGUI.Foldout(NextRect(), foldVectors, "Vertices"))
                {
                    EditorGUI.indentLevel++;
                    //EditorGUI.BeginDisabledGroup(true);
                    for (var i = 0; i < fragment.Vertices.Length; i++)
                    {
                        //SerializedProperty ElementProperty = Property.GetArrayElementAtIndex(I);
                        EditorGUI.Vector3Field(NextRect(), $"{i}", fragment.Vertices[i]);
                    }
                    //EditorGUI.EndDisabledGroup();
                    EditorGUI.indentLevel--;
                }
            }
            else EditorGUI.LabelField(NextRect(), "vertices = null");

            if (fragment.Triangles != null)
            {
                if (foldTriangles = EditorGUI.Foldout(NextRect(), foldTriangles, "Triangles"))
                {
                    EditorGUI.indentLevel++;
                    //EditorGUI.BeginDisabledGroup(true);
                    for (var i = 0; i < fragment.Triangles.Length; i++)
                    {
                        //SerializedProperty ElementProperty = Property.GetArrayElementAtIndex(I);
                        EditorGUI.IntField(NextRect(), $"{i}", fragment.Triangles[i]);
                    }
                    //EditorGUI.EndDisabledGroup();
                    EditorGUI.indentLevel--;
                }
            }
            else EditorGUI.LabelField(NextRect(), "Triangles = null");
        }
        EditorGUI.indentLevel--;
        EditorGUI.EndProperty();
    }

    /*public override VisualElement CreatePropertyGUI(SerializedProperty property)
    {
        MeshFragment fragment = (MeshFragment)EditorHelper.GetTargetObjectOfProperty(property);
        var container = new VisualElement();
        var s = "";
        if (fragment.vertices != null)
            s += $"{fragment.vertices.Length} ";
        if (fragment.triangles != null)
            s += $"{fragment.triangles.Length} ";
        var text = new TextElement();

        text.text = s;
        container.Add(text);

        return container;
    }*/
}