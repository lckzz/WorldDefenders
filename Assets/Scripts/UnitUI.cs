using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitUI : MonoBehaviour
{
    [SerializeField]
    protected Image hpbar;
    protected float minHp = .0f;
    protected float hpSpeed = .3f;

    protected T ComponentInit<T>(out T compo) where T : Component
    {
        if(TryGetComponent<T>(out compo))
            return compo;
        else
            return null;
    }

    public virtual void UpdateHp(float hpspeed)
    {

    }


}
