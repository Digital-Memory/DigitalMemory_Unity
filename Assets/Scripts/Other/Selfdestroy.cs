using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selfdestroy : MonoBehaviour
{
    public bool StartCountdownOnStart = false;
    public float delay;

    // Start is called before the first frame update
    void Start()
    {
        if (StartCountdownOnStart)
            DestroyDelayed(delay);
    }

    public void DestroyDelayed(float delay)
    {
        if (delay > 0f)
            Invoke("Destroy",delay);
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
