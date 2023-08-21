using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalUI : UnitUI
{
    private MonsterPortal monPortal;

    // Start is called before the first frame update
    void Start()
    {
        ComponentInit(out monPortal);

    }

    // Update is called once per frame
    void Update()
    {
        UpdateHp(hpSpeed);

    }


    public override void UpdateHp(float hpspeed)
    {
        if (hpbar != null)
        {
            
            float hp = monPortal.hpPercent();


            if (hpbar.fillAmount > hp)
                hpbar.fillAmount -= Time.deltaTime * hpspeed;

        }
    }
}
