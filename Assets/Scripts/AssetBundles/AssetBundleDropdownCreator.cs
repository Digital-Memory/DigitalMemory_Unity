using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using UnityEditor;

public class AssetBundleDropdownCreator : MonoBehaviour
{
    public static DropdownList<string> GetAssetBundleDropdown()
    {
        DropdownList<string> dropdown = new DropdownList<string>();
#if UNITY_EDITOR
        string[] names = AssetDatabase.GetAllAssetBundleNames();
        foreach (string name in names)
            dropdown.Add(name, name);

#endif
        return dropdown;
    }
}
