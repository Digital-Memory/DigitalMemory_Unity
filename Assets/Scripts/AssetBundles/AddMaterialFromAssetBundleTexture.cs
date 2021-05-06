using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class AddMaterialFromAssetBundleTexture : MonoBehaviour
{
    [ShowIf("TextureNameIsEmpty")]
    [ValidateInput("TextureNameIsNotEmpty", "Select a texture Refenrence and Hit SetReferences.")]
    [SerializeField] Texture2D texture2D;

    [ShowIf("BundleNameIsEmpty")]
    [ValidateInput("BundleNameIsNotEmpty", "Select a bundle Refenrence and Hit SetReferences.")]
    [Dropdown("CreateDropdown")]
    [SerializeField] string bundle;

     bool TextureNameIsEmpty() { return !TextureNameIsNotEmpty(); }
     bool BundleNameIsEmpty() { return !BundleNameIsNotEmpty(); }
    private bool TextureNameIsNotEmpty() { return textureName != ""; }
    private bool BundleNameIsNotEmpty() { return bundleName != ""; }


    [SerializeField] string bundleName = "";
    [SerializeField] string textureName = "";

    [Button]

    private void SetReferences()
    {
        if (texture2D != null)
            textureName = texture2D.name;

        if (bundle != "")
            bundleName = bundle;
    }


    private DropdownList<string> CreateDropdown()
    {
        return AssetBundleDropdownCreator.GetAssetBundleDropdown();
    }

    private void Start()
    {
        AddMaterial();
    }

    [Button]
    private void AddMaterial()
    {
        Game.AssetBundleHandler.AddMaterialFromAssetBundleTexture(GetComponent<MeshRenderer>(), bundleName, textureName);
    }
}
