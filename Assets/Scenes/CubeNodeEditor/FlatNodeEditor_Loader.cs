using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using DoubleEngine;
using DoubleEngine.Atom;

public class FlatNodeEditor_Loader : MonoBehaviour, IAddressableSceneLoader
{
    public AssetReference scene;
    public NewFlatNodePlaceholder fragmentSource;

    public void LoadAddressableScene() => LoadFlatNodeEditor();

    // Start is called before the first frame update
    public void LoadFlatNodeEditor()
    {
        MeshFragmentVec3D fragment;
        Debug.Log(fragmentSource);
        fragment = fragmentSource.fragment;
        Debug.Log(fragment);
        //FlatNodes.LoadFromJsonFile();

        /*if (fragment!=null)
        {
            FlatNodes.CreateNewAndAdd((MeshFragment)fragmentSource.fragment);
            FlatNodes.SaveToJsonFile();
            Debug.Log("Added new FlatNode");
        } else
        {
            FlatNodes.SaveToJsonFile();
            Debug.Log("Saved without new FlatNode");
        }*/

        if (scene != null)
        {
            Debug.Log("Loading new scene");
            FlatNodeEditorStartFragment flatnodeFragment = (FlatNodeEditorStartFragment)FindObjectOfType(typeof(FlatNodeEditorStartFragment));
            flatnodeFragment.Fragment = fragment;
            Addressables.LoadSceneAsync(scene, UnityEngine.SceneManagement.LoadSceneMode.Single);
        }
        else
        {
            throw new System.ArgumentException("No second scene");
        }

    }
}
