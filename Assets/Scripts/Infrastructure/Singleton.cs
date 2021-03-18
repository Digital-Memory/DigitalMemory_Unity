using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    protected static T instance;

    public static T CreateBlind() {
        var go = new GameObject(typeof(T).ToString());
        return go.AddComponent<T>();
    }

    public static T LoadResourceFromName(string name) {
        Singleton<T> prefab = Resources.Load<Singleton<T>>("Singletons/"+name);
        return (T)Instantiate(prefab, Game.GetInstance().transform);
    }

    protected virtual void Start () {
        Game g = Game.GetInstance();
    }

    public virtual void Init()
    {
        //
    }

    public static T GetInstance(bool usePrefab = false, string prefabName = "") {
    
        if (instance == null)
        {
            instance = FindObjectOfType<T>();

            if (instance == null)
            {
                Debug.Log("spawn new instance of type " + typeof(T).ToString());
                if (usePrefab)
                {
                    string name = (prefabName == "") ? typeof(T).ToString() : prefabName;
                    instance = LoadResourceFromName(name);
                }
                else
                {
                    instance = CreateBlind();
                }
            }
        }

        return instance;
    }
}
