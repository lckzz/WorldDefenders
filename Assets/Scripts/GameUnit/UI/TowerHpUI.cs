using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerHpUI : HpUIBase
{
    private Tower tower;

    // Start is called before the first frame update
    protected override void Start()
    {
        //ComponentInit(out tower);
        base.Start();
        tower = hpSubject as Tower;
        tower.onDead += HpUIOff;

    }


    private void OnDestroy()
    {
        if(tower != null)
            tower.onDead -= HpUIOff;
    }
    // Update is called once per frame
    //void Update()
    //{
    //    UpdateHp(hpSpeed);

    //}


    //public override void UpdateHp(float hpspeed)
    //{
    //    if (hpbar != null)
    //    {

    //        float hp = monPortal.hpPercent();


    //        if (hpbar.fillAmount > hp)
    //            hpbar.fillAmount -= Time.deltaTime * hpspeed;

    //    }
    //}
}
