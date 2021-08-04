using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class StartFlow : MonoBehaviour
{
    [SerializeField] StartBoxTopInteractable[] startBoxTops;
    [SerializeField] Rigidbody[] sides;
    [SerializeField] GameObject box;
    [SerializeField] CinemachineVirtualCamera centerCam, sideCam;
    [SerializeField] StartTextDisplayer startTextDisplayer;
    [SerializeField] Image blend;
    [SerializeField] AudioClip clatterSound;
    int topsMoved = 0;
    bool confirmed;
    Material[] boxMaterials;

    private void OnEnable()
    {
        foreach (StartBoxTopInteractable top in startBoxTops)
        {
            top.OnMove.AddListener(OnMoveBoxTop);
        }

        List<Material> boxMats = new List<Material>();

        foreach (MeshRenderer mr in box.GetComponentsInChildren<MeshRenderer>())
        {
            mr.material = new Material(mr.material);
            boxMats.Add(mr.material);
        }

        boxMaterials = boxMats.ToArray();

        Fade(In: true);
    }

    private void Fade(bool In = false, bool Out = false)
    {
        if (In != Out)
            StartCoroutine(FadeRoutine(In));
    }

    private IEnumerator FadeRoutine(bool In)
    {
        float target = In ? 0 : 1;
        float alpha = In ? 1 : 0; ;

        while (In ? (alpha > target) : (alpha < target))
        {
            alpha = Mathf.MoveTowards(alpha, target, Time.deltaTime);
            blend.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        blend.raycastTarget = !In;
    }

    private void OnMoveBoxTop(StartBoxTopInteractable top)
    {
        top.OnMove.RemoveAllListeners();
        topsMoved++;

        if (topsMoved > 1)
        {
            StartCoroutine(StartRoutine());
        }
    }

    private IEnumerator StartRoutine()
    {
        //let all walls fall
        foreach (Rigidbody side in sides)
        {
            side.isKinematic = false;
        }
        

        //switch camera
        sideCam.gameObject.SetActive(true);
        centerCam.gameObject.SetActive(false);

        yield return new WaitForSeconds(1.5f);
        Game.SoundPlayer.Play(clatterSound);

        //fade out walls
        float matAlpha = 1;
        while (matAlpha > 0)
        {
            matAlpha -= Time.deltaTime;
            foreach (Material mat in boxMaterials)
            {
                mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, matAlpha);
            }
            yield return null;
        }
        Destroy(box);

        confirmed = false;
        startTextDisplayer.Display("Vor einigen Jahren erz‰hlte mir meine Groﬂmutter eine Geschichte.");
        while (!confirmed)
            yield return null;

        confirmed = false;
        startTextDisplayer.Display("Ihre Geschichte.");
        while (!confirmed)
            yield return null;

        confirmed = false;
        startTextDisplayer.Display("Sie beginnt zu einer anderen Zeit, in einem anderen Land.");
        while (!confirmed)
            yield return null;

        confirmed = false;
        startTextDisplayer.Display("Jetzt erz‰hle ich sie dir, bist du bereit?", customConfirm: true);
        while (!confirmed)
            yield return null;

        FadeOut();

        yield return new WaitForSeconds(1);

        Game.LevelHandler.TryLoad(1);
    }

    private void FadeOut()
    {
        Destroy(startTextDisplayer.gameObject);
        Fade(Out: true);
    }

    public void Confirm()
    {
        confirmed = true;
    }

    private void OnDestroy()
    {
        foreach (Material mat in boxMaterials)
        {
            mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, 1);
        }
    }
}
