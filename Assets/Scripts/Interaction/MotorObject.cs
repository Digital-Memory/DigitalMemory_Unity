using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotorObject : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] bool defineObjectToMoveManually = false;
    [ShowIf("defineObjectToMoveManually")]
    [SerializeField] MovingObject objectToMove;
    float current = 0f;

    private void OnEnable ()
    {
        if (!defineObjectToMoveManually)
        objectToMove = GetComponent<MovingObject>();

        if (objectToMove == null)
            Debug.LogError("no moving object found..." + gameObject.name);
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
