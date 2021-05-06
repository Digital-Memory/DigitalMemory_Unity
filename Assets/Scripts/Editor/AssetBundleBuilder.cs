using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using NaughtyAttributes;

public class AssetBundleBuilder : Editor
{
    [MenuItem("Assets/BuildAssetBundles")]
    static void BuildAllBundles()
    {
        BuildPipeline.BuildAssetBundles(@"C:\data\projects\01_Repositorys\DigitalMemoryAssetBundles", BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.WebGL);
    }

    
}
