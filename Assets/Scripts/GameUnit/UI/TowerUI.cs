using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerUI : UnitUI
{
    private PlayerTower playerTower;
    
    // Start is called before the first frame update
    void Start()
    {
        ComponentInit(out playerTower);

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
            float hp = playerTower.hpPercent();


            if (hpbar.fillAmount > hp)
                hpbar.fillAmount -= Time.deltaTime * hpspeed;

        }
    }
}
