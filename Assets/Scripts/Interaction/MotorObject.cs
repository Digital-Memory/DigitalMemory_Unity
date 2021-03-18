using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotorObject : MonoBehaviour
{
    MovingObject objectToMove;
    float current = 0f;
    [SerializeField] float speed;

    private void OnEnable ()
    {
        objectToMove = GetComponent<MovingObject>();

        if (objectToMove == null)
            Debug.LogWarning("no moving object found... please add one to " + gameObject.name);
    }

    private void Update()
    {
        if (objectToMove != null)
        {
            if (objectToMove.Try(current + speed * Time.deltaTime))
            {
                current += speed * Time.deltaTime;
            }

            if (current >= 1f)
                Destroy(this);
        }
    }
}
