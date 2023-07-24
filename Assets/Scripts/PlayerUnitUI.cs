using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnitUI : UnitUI
{
    private UnitController unitCtrl;
    // Start is called before the first frame update
    void Start()
    {
        ComponentInit<UnitController>(out unitCtrl);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHp(minHp);
    }


    public override void UpdateHp(float hpspeed)
    {
        if (hpbar != null)
        {
            float hp = unitCtrl.hpPercent();
            if (hpbar.fillAmount > hp)
                hpbar.fillAmount -= Time.deltaTime * hpspeed;

        }
    }
}
