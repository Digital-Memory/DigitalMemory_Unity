using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu]
public class PullableImageData : ScriptableObject
{
    [OnValueChanged("SetReferences")]
    [ValidateInput("InputIsValid", "Select a texture Refenrence and Hit SetReferences.")]
    [SerializeField] Texture2D texture2D;

    private bool InputIsValid() { return textureName != "" && bundleName != ""; }


    public string bundleName = "";
    public string textureName = "";

    [ResizableTextArea]
    public string Text;

#if UNITY_EDITOR
    private void SetReferences()
    {
        if (texture2D != null)
        {
            textureName = texture2D.name;

            string path = AssetDatabase.GetAssetPath(texture2D);

            bundleName = AssetDatabase.GetImplicitAssetBundleName(path);

            if (bundleName == "")
            {
                Debug.LogError("The Asset you selected seems not to be part of any bundle.");
            }
            else
                Debug.Log($"update asset references path: {path} / bundle name: {bundleName}.");
        }
    }

    [Button]
    private void Refresh()
    {
        SetReferences();
    }
#endif
}