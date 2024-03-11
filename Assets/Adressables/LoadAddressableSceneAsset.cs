using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public interface IAddressableSceneLoader
{
    public void LoadAddressableScene();
}
public class LoadAddressableSceneAsset : MonoBehaviour, IAddressableSceneLoader
{
    public AssetReference scene;

    public void LoadAddressableScene() => LoadSceneFromPublicField();

    public void LoadSceneFromPublicField()
    {
        LoadSceneFromReference(scene);
    }

    public static void LoadSceneFromReference(AssetReference scene)
    {
        if (scene != null)
            Addressables.LoadSceneAsync(scene, UnityEngine.SceneManagement.LoadSceneMode.Single);
        else
            throw new Exception("scene public field not set");
    }

}
