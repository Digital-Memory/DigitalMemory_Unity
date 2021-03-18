using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRespawnHandler : MonoBehaviour
{
    private const int RESPAWN_DISTANCE_TO_CENTER = 20;
    private void OnTriggerExit(Collider other)
    {
        other.attachedRigidbody.position = Game.CameraController.GetUpVector() * RESPAWN_DISTANCE_TO_CENTER;
        Rigidbody rigidbody = other.GetComponent<Rigidbody>();
        if (rigidbody != null)
            rigidbody.velocity = Vector3.zero;
    }
}
