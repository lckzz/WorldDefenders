using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnitUI : UnitUI, IHpObserver
{
    private Unit unitCtrl;
    // Start is called before the first frame update
    void Start()
    {
        ComponentInit<Unit>(out unitCtrl);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHp(hpSpeed);
        if(unitCtrl != null)
        {
            if(unitCtrl.IsDie)
            {
                UnitDeadSpAlpha<Unit>(unitCtrl, spr);
            }
        }
    }


    public override void UpdateHp(float hpspeed)
    {
        if (hpbar != null)
        {
            float hp = unitCtrl.hpPercent();

            if (hpbar.fillAmount > hp)
                hpbar.fillAmount -= Time.deltaTime * hpspeed;

            if (hpbar.fillAmount <= hp)
                hpbar.fillAmount += Time.deltaTime * hpspeed;

        }
    }

    public void Notified(int hp)  //옵저버 디자인패턴 적용해보는중 -> hp값을 받아오는거
    {
        
    }
}
