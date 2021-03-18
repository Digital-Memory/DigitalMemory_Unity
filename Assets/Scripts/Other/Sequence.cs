using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sequence : MonoBehaviour
{
    public Sequence before, after;
    public float StartDelay = 1f, StartReversedDelay = 1f;
    public List<int> TimeDots;
}
