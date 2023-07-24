using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterUnitUI : UnitUI
{
    private MonsterController monCtrl;


    // Start is called before the first frame update
    void Start()
    {
        ComponentInit<MonsterController>(out monCtrl);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHp(hpSpeed);
        if(monCtrl != null)
        {
            if(monCtrl.IsDie)
            {
                UnitDeadSpAlpha<MonsterController>(monCtrl, spr);

            }
        }

    }

    

    public override void UpdateHp(float _hpspeed)
    {
        if(hpbar != null)
        {
            float hp = monCtrl.hpPercent();
            if (hpbar.fillAmount > hp)
                hpbar.fillAmount -= Time.deltaTime * _hpspeed;

        }
    }
}
