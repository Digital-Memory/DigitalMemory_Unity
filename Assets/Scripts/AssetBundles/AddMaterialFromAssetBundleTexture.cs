using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class AddMaterialFromAssetBundleTexture : MonoBehaviour
{
    [OnValueChanged("SetReferences")]
    [ValidateInput("InputIsValid", "Select a texture Refenrence and Hit SetReferences.")]
    [SerializeField] Texture2D texture2D;

    private bool InputIsValid() { return textureName != "" && bundleName != ""; }


    [SerializeField] string bundleName = "";
    [SerializeField] string textureName = "";

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
    private void TestTexture () {
        if (InputIsValid() && texture2D != null) {
            MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
            Material material = meshRenderer.sharedMaterial;
            material.mainTexture = texture2D;
            meshRenderer.material = material;
        } else {
            Debug.LogError("Selected invalid texture for texture test.");
        }
    }

    [Button]
    private void ResetTexture()
    {
            MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
            Material material = meshRenderer.sharedMaterial;
            material.mainTexture = null;
           meshRenderer.material = material;

    }

    [Button]
    private void AdaptWidthToHeightAndRatio() {

        if (InputIsValid() && texture2D != null)
        {
            float height = transform.localScale.x;
            transform.localScale = new Vector3(height, transform.localScale.y, (height / texture2D.width) * texture2D.height);
        }
        else
        {
            Debug.LogError("Selected invalid texture for width adaptation.");
        }
    }

#endif

    private void AddMaterial()
    {
        Game.AssetBundleHandler.AddMaterialFromAssetBundleTexture(GetComponent<MeshRenderer>(), bundleName, textureName);
    }
}
