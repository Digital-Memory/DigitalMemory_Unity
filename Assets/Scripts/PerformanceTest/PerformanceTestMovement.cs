using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace performanceTest
{
    public class PerformanceTestMovement : MonoBehaviour
    {
        void Update()
        {
            transform.Rotate(Vector3.up, Random.Range(45f, -45f) * Time.deltaTime);
            transform.Translate(transform.forward * Time.deltaTime);
        }
    }
}
