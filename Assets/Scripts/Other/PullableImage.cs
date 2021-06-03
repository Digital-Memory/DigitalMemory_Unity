using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PullableImage : MonoBehaviour
{
    [SerializeField] [Expandable] PullableImageData imageData;

    IPullableImageDisplay photoMeshRenderer;

    private void OnEnable()
    {
        photoMeshRenderer = GetComponentInChildren<IPullableImageDisplay>();
        photoMeshRenderer.DisplayText(imageData.Text);
        Game.AssetBundleHandler.AddMaterialFromAssetBundleTexture(photoMeshRenderer.MeshRenderer,imageData.bundleName, imageData.textureName);
    }
}
