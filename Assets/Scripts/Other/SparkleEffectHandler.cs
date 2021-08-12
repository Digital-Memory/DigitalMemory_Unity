using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SparkleEffectHandler : MonoBehaviour
{
    public static System.Action<Attacher> OnAttacherDetach;
    public static System.Action<Attacher> OnAttacherAttach;

    [SerializeField] List<Attacher> emptyAttachers = new List<Attacher>();

    [SerializeField] ParticleSystem particleSystem;
    [SerializeField] AudioSource sparkAudio;

    private void OnEnable()
    {
        OnAttacherAttach += UnregisterAttacher;
        OnAttacherDetach += RegisterAttacher;

        StartCoroutine(TeleportSparkRoutine());
    }

    private void OnDisable()
    {
        OnAttacherAttach -= UnregisterAttacher;
        OnAttacherDetach -= RegisterAttacher;

        StopAllCoroutines();
    }

    private void RegisterAttacher(Attacher attacher)
    {
        if (!emptyAttachers.Contains(attacher))
            emptyAttachers.Add(attacher);
    }

    private void UnregisterAttacher(Attacher attacher)
    {
        if (emptyAttachers.Contains(attacher))
            emptyAttachers.Remove(attacher);
    }

    private IEnumerator TeleportSparkRoutine()
    {
        int index = -1;
        while (true)
        {
            index++;
            if (index >= emptyAttachers.Count)
                index = 0;

            if (emptyAttachers.Count > 0)
            {
                if (!emptyAttachers[index].isActiveAndEnabled) Debug.LogError("teleport to disabled attacher...");
                transform.position = emptyAttachers[index].transform.position;
                transform.localScale = emptyAttachers[index].transform.localScale * 0.5f;

                PlaySparkEffect();
            }

            yield return new WaitForSeconds(Mathf.Max(5f / Mathf.Max((float)emptyAttachers.Count, 1f), 1f));
        }
    }

    private void PlaySparkEffect()
    {
        particleSystem.Play();
        sparkAudio.pitch = UnityEngine.Random.Range(0.6f, 1.4f);
        sparkAudio.Play();
    }
}
