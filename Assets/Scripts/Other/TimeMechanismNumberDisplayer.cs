using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class TimeMechanismNumberDisplayer : MonoBehaviour
{
    [SerializeField]
    float dullness = 0.5f;
    [SerializeField]
    float maxSpeed = 25f;
    [SerializeField]
    AudioSource audioSource;
    [SerializeField]
    AudioClip click;

    Material material;
    float currentNumber = 0;
    float localPitchOffset = 0.1f;
    [SerializeField] float currentVelocity = 0;
    private void Awake()
    {
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        material = new Material(meshRenderer.material);
        meshRenderer.material = material;
    }

    public void MoveTo(int targetNumber, int overshoot = 5)
    {
        float ceil = Mathf.Ceil(Mathf.Round(currentNumber) / 10f) * 10f;
        //Debug.Log($" ceil {ceil} + overshoot {overshoot} * 10 => {targetNumber}");
        float targetNumberAdded = ceil + (overshoot * 10) + targetNumber;

        localPitchOffset = Random.Range(-0.25f, 0.25f);

        StopAllCoroutines();
        StartCoroutine(MoveToRoutine(targetNumberAdded));
    }

    private IEnumerator MoveToRoutine(float targetNumber)
    {
        float distance = float.MaxValue;

        audioSource.Play();

        while (distance > 0.01f)
        {
            distance = Mathf.Abs(currentNumber - targetNumber);

            currentNumber = Mathf.SmoothDamp(currentNumber, targetNumber, ref currentVelocity, dullness, maxSpeed);

            material.SetFloat("Number", currentNumber);

            audioSource.pitch = Remap(currentVelocity, 0, 25, 0.5f, 2 + localPitchOffset);
            audioSource.volume = Mathf.Clamp(Remap(currentVelocity, 1, 5, 0, 0.25f), 0, 0.25f);

            yield return null;
        }

        audioSource.Stop();
    }

    private float Remap(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
}
