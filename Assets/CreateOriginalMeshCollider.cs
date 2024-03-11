using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateOriginalMeshCollider : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        if (GetComponent<MeshCollider>())
        {
            // Delete Meshcollider
            DestroyImmediate(GetComponent<MeshCollider>());
            Debug.Log("Deleted" + name + "Meshcollider!");
        }

        MeshCollider collider = gameObject.AddComponent<MeshCollider>();
        // Print out the name of the object added to add Meshcollider
        Debug.Log("" + collider.gameObject.name + "creates a Meshcollider!");

        collider.cookingOptions = MeshColliderCookingOptions.None;
        collider.convex = false;
        collider.sharedMesh = gameObject.GetComponent<MeshFilter>().mesh;
    }

}
