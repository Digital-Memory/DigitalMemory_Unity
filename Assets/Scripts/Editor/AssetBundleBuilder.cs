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
        string path = EditorUtility.OpenFolderPanel("Where should the ab go Max Bredlau?", "", "");
        BuildPipeline.BuildAssetBundles(path, BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.WebGL);
        EditorUtility.DisplayDialog("Build Complete", "Congrats Max Bredlau, you build the Assets Bundles", "Ok. cool.", "Age?");
    }

    
}
