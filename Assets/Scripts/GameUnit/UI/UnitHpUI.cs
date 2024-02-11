using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitHpUI : HpUIBase
{
    private Unit unitCtrl;

    protected override void Start()
    {
        //ComponentInit(out tower);
        base.Start();
        unitCtrl = hpSubject as Unit;
        unitCtrl.onDead += HpUIOff;

    }


    private void OnDestroy()
    {
        if(unitCtrl != null)
            unitCtrl.onDead -= HpUIOff;
    }




}
