using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LODGroupFromChildren : MonoBehaviour
{
    private void Reset () {
        LOD[] lod = new LOD[] { new LOD(0.9f, GetComponentsInChildren<MeshRenderer>()) };
        LODGroup group = gameObject.AddComponent<LODGroup>();

        group.SetLODs(lod);
        Destroy(this);
    }
}
