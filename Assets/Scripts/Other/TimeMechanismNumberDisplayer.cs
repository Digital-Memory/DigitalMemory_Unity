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
    private void OnEnable()
    {
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        material = new Material(meshRenderer.material);
        meshRenderer.material = material;
    }

    [Button]
    private void Test2()
    {
        MoveTo(2, overshoot: 0);
    }

    [Button]
    private void Test4()
    {
        MoveTo(4, overshoot: 1);
    }

    [Button]
    private void Test6()
    {
        MoveTo(6, overshoot: 3);
    }

    [Button]
    private void Test8()
    {
        MoveTo(8, overshoot: 10);
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
