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
            Color circleColor = Color.red;
            if (objectToMove.Try(current + speed * Time.deltaTime))
            {
                current += speed * Time.deltaTime;
                circleColor = Color.green;
            }

            DebugDraw.Circle(transform.position, circleColor, 0.5f + 0.25f * Mathf.Sin(Time.time));

            if (current >= 1f)
                Destroy(this);
        }
    }
}
