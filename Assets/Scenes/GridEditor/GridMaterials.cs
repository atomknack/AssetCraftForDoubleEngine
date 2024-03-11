using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using DoubleEngine;
using DoubleEngine.Atom;

public class GridMaterials
{
    /*
    private static readonly Lazy<GridMaterials> lazy =
        new Lazy<GridMaterials>(() => new GridMaterials());
    public static byte DefaultMaterial = ThreeDimensionalCell.DefaultMaterial;

    private Material[] _unityMaterials;
    private readonly Color32[] _unityColors; 
public static GridMaterials Instance => lazy.Value;

    private GridMaterials()
    {
        _unityMaterials = new Material[] {
            Resources.Load<Material>("Materials/Blackish"),
            Resources.Load<Material>("Materials/WhiteTwinkle"), 
            Resources.Load<Material>("Materials/GreenSlowTrinkle"),
            Resources.Load<Material>("Materials/BlueSlowTwinkle"),
            Resources.Load<Material>("Materials/Red"),
        };

        _unityColors = new Color32[] { Color.gray, Color.white, Color.green, Color.blue, Color.red, Color.cyan };

    }

    public byte NextMaterial(byte materialIndex) => materialIndex.NextByteCyclic(_unityMaterials.Length);
    public Material GetUnityMaterial(byte materialIndex) =>_unityMaterials[materialIndex];

    internal ReadOnlySpan<Color32> GetMaterialColors() => _unityColors;*/
}
