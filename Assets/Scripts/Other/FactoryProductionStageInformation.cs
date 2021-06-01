using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class FactoryProductionStageInformation : MonoBehaviour
{
    private FactoryProductionAnimatingObject production;
    public ProdctionStage Stage;
    [Range(0,1)]
    [OnValueChanged("PreviewChangeTime")]
    public float Time;

    private void PreviewChangeTime()
    {
#if UNITY_EDITOR

        if (production == null)
            production = GetComponent<FactoryProductionAnimatingObject>();

        production.PreviewStage(Time);
    }

    private void Reset()
    {
        production = GetComponent<FactoryProductionAnimatingObject>();
#endif
    }
}
