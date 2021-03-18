using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceHandler : Singleton<SequenceHandler>
{
    [SerializeField] Sequence current;
    public event System.Action<Sequence> OnStartNewSeqeunce;

    protected override void Start()
    {
        base.Start();
        StartNewSequence(startFromBeginning: true);
    }
    internal void TryPlayBefore()
    {
        if (current.before != null)
            StartCoroutine(ChangeSequenceDelayed(current, current.before, startFromBeginning: false));
    }

    internal void TryPlayAfter()
    {
        if (current.after != null)
            StartCoroutine(ChangeSequenceDelayed(current, current.after));
    }

    IEnumerator ChangeSequenceDelayed(Sequence from, Sequence to, bool startFromBeginning = true)
    {
        yield return new WaitForSeconds(startFromBeginning ? to.StartDelay : to.StartReversedDelay);

        to.gameObject.SetActive(true);
        from.gameObject.SetActive(false);

        current = to;
        StartNewSequence(startFromBeginning);
    }

    private void StartNewSequence(bool startFromBeginning)
    {
        Debug.Log("START NEW SEQUENCE: " + current.name);
        OnStartNewSeqeunce?.Invoke(current);
    }
}
