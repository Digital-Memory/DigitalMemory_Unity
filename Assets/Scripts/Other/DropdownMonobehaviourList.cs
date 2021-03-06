using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropdownMonobehaviourList<T> : DropdownList<T>
{
    public static DropdownList<T> FromObjectsOfType(T[] objects, bool AddNullOption = false)
    {
        DropdownList<T> list = new DropdownList<T>();

        if (AddNullOption)
            list.Add("NULL", default);

        foreach (var item in objects)
        {
            if (item != null)
                list.Add(item.ToString(),item);
        }

        return list;
    }
}
