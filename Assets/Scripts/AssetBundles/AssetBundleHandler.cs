using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class AssetBundleHandler : Singleton<AssetBundleHandler>
{
    [SerializeField] string remoteAssetPath;
    [SerializeField] string localAssetPath;
    [SerializeField] Material defaultImageMaterial;

    List<string> bundlesBeeingLoaded = new List<string>();
    Dictionary<string, AssetBundle> loadedBundles = new Dictionary<string, AssetBundle>();
    List<UnhandeledRequest> unhandeledRequests = new List<UnhandeledRequest>();

    public void AddMaterialFromAssetBundleTexture(MeshRenderer meshRendererToAddTo, string bundleName, string textureName)
    {
        var request = new MaterialAssignmentRequest { bundleName = bundleName, textureName = textureName, meshRenderer = meshRendererToAddTo, defaultMaterial = defaultImageMaterial };

        if (loadedBundles.ContainsKey(bundleName))
        {
            request.TryHandle(loadedBundles[bundleName]);
        }
        else
        {
            unhandeledRequests.Add(request);

            if (!bundlesBeeingLoaded.Contains(bundleName))
            {
                bundlesBeeingLoaded.Add(bundleName);
                StartCoroutine(LoadBundle(bundleName, OnBundleLoaded));
            }
        }
    }

    private void OnBundleLoaded(AssetBundle loadedBundle)
    {
        Debug.Log($"Loaded Bundle { loadedBundle.name } number of open requests: {unhandeledRequests.Count}.");
        bundlesBeeingLoaded.Remove(loadedBundle.name);
        loadedBundles.Add(loadedBundle.name, loadedBundle);
        TryHandleRequests(loadedBundle);
    }

    private void TryHandleRequests(AssetBundle loadedBundle)
    {
        for (int i = 0; i < unhandeledRequests.Count; i++)
        {
            UnhandeledRequest unhandeledRequest = unhandeledRequests[i];
            if (unhandeledRequest != null && unhandeledRequest.TryHandle(loadedBundle))
            {
                unhandeledRequests.RemoveAt(i);
                i--;
            }
        }

        Debug.Log($"Tried handling all Requests. number left: {unhandeledRequests.Count}");
    }

    IEnumerator LoadBundle(string bundleName, UnityAction<AssetBundle> callback)
    {

#if UNITY_WEBGL && !UNITY_EDITOR
        string path = remoteAssetPath + bundleName.ToLower();
    UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(path);
    yield return request.SendWebRequest();

    if (request.isNetworkError || request.isHttpError)
    {
        Debug.LogError(request.error);
    }
    else
    {
        // Get downloaded asset bundle
        callback(DownloadHandlerAssetBundle.GetContent(request));
    }
#else

        string path = localAssetPath + bundleName.ToLower();
        var assetBundleCreateRequest = AssetBundle.LoadFromFileAsync(path);
        yield return assetBundleCreateRequest;
        callback(assetBundleCreateRequest.assetBundle);
#endif
    }

    [Button]
    public void UnloadAssetBundles()
    {
        AssetBundle.UnloadAllAssetBundles(true);
        loadedBundles.Clear();
    }
}

public abstract class UnhandeledRequest
{
    public string bundleName;
    public abstract bool TryHandle(AssetBundle assetBundle);
}

public class MaterialAssignmentRequest : UnhandeledRequest
{
    public string textureName;
    public MeshRenderer meshRenderer;
    public Material defaultMaterial;

    public override bool TryHandle(AssetBundle assetBundle)
    {
        if (bundleName == assetBundle.name)
        {
            return AssignMaterial(assetBundle);
        }

        return false;
    }
    public bool AssignMaterial(AssetBundle assetBundle)
    {
        Texture2D tex = assetBundle.LoadAsset<Texture2D>(textureName);
        if (tex != null && meshRenderer != null)
        {
            meshRenderer.materials[1] = new Material(defaultMaterial);
            meshRenderer.materials[1].mainTexture = tex;
            return true;
        }

        return false;
    }
}