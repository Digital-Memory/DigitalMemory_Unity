using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using NaughtyAttributes;

public class TestingKit : MonoBehaviour
{
    [SerializeField] InventoryObjectData[] inventoryObjects;

    private void OnGUI()
    {
        //int indexMax = inventoryObjects.Length - 1;
        //int index = 0;
        //
        //while (index <= indexMax)
        //{
        //    InventoryObjectData obj = inventoryObjects[index];
        //    if (GUI.Button(new Rect(10, 10 + 30 * index, 120, 20), obj.name))
        //        Game.UIHandler.InventoryAdder.MoveToInventory(obj, Input.mousePosition);
        //
        //    index++;
        //}
    }

    [Button]
    private void CountVerts()
    {
        int totalVerts = 0;
        List<KeyValuePair<string, int>> hitList = new List<KeyValuePair<string, int>>();

        foreach (MeshFilter mf in FindObjectsOfType(typeof(MeshFilter)))
        {
            int verts = mf.mesh.vertexCount;
            totalVerts += verts;
            hitList.Add(new KeyValuePair<string, int>( mf.name, verts));
        }

        StringBuilder sb = new StringBuilder();

        foreach (KeyValuePair<string,int> item in hitList.OrderByDescending(pairs => pairs.Value))
        {
            sb.Append($"{item.Key} : {item.Value} ({(((float)item.Value) / ((float)totalVerts)) * 100}%)\n");
        }

    }

    [SerializeField] Material materialToSearchFor;
    [SerializeField] MeshRenderer[] renderers;

    [Button]
    private void FindMaterialReferences()
    {
        List<MeshRenderer> meshRenderers = new List<MeshRenderer>();

        foreach (MeshRenderer renderer in FindObjectsOfType<MeshRenderer>())
        {
            if (renderer.sharedMaterials.Contains(materialToSearchFor))
                meshRenderers.Add(renderer);
        }

        renderers = meshRenderers.ToArray();
    }

}
