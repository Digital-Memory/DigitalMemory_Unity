using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selfdestroy : MonoBehaviour
{
    public bool StartCountdownOnStart = false;
    [SerializeField] private float delay;

    // Start is called before the first frame update
    void Start()
    {
        if (StartCountdownOnStart)
            Destroy(delay);
    }

    public void Destroy(float delay)
    {
        if (!(delay < 0f))
            Destroy(gameObject, delay);
    }
}
