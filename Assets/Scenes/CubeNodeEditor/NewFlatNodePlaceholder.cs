using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.AddressableAssets;
//using UnityEngine.SceneManagement;
using DoubleEngine;
using DoubleEngine.Atom;

public class NewFlatNodePlaceholder : MonoBehaviour
{
    //public AssetReference currentScene;
    public MeshFragmentVec3D fragment
    {
        get { return _fragment; }
        set { SetFragment(value);}
    }
    [SerializeField]
    private MeshFragmentVec3D _fragment = null;

    public void SetFragment(MeshFragmentVec3D fragment)
    {
        if (fragment == null)
        {
            _fragment = null;
            GetComponent<MeshFilter>().sharedMesh = null;
        }
        else
        {
            var mesh = new Mesh();
            _fragment = fragment;
            mesh.vertices = _fragment.Vertices.ToArrayVector3();
            mesh.triangles = _fragment.Triangles.ToArray();
            GetComponent<MeshFilter>().sharedMesh = mesh;
        }
    }

    public void AddAndSaveFlatNodeAsIs()
    {
        ExportFlatNodeToJson.SaveNewFlatNodeToJson(fragment.JoinedClosestVerticesIfNeeded());
        fragment = null;
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //Addressables.LoadSceneAsync(currentScene, UnityEngine.SceneManagement.LoadSceneMode.Single);
    }
}
