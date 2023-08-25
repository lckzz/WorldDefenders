using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterUnitUI : UnitUI
{
    private Unit uniCtrl = null;
    [SerializeField] private MonsterController monCtrl;
    private EliteMonsterController eliteMonCtrl;
    // Start is called before the first frame update
    void Start()
    {
        if(uniCtrl == null)
        {
            this.gameObject.TryGetComponent(out uniCtrl);
        }


        if(uniCtrl as MonsterController)
        {
            monCtrl = (MonsterController)uniCtrl;
            ComponentInit<MonsterController>(out monCtrl);

        }
        else if (uniCtrl as EliteMonsterController)
        {
            eliteMonCtrl = (EliteMonsterController)uniCtrl;
            ComponentInit<EliteMonsterController>(out eliteMonCtrl);

        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHp(hpSpeed);
        if(monCtrl != null)
        {
            if(monCtrl.IsDie)
            {
                if(monCtrl != null)
                    UnitDeadSpAlpha<MonsterController>(monCtrl, spr);
                else
                    UnitDeadSpAlpha<EliteMonsterController>(eliteMonCtrl, spr);


            }
        }

    }

    

    public override void UpdateHp(float _hpspeed)
    {
        if(hpbar != null)
        {

            if(monCtrl != null)
                hp = monCtrl.hpPercent();
            else
                hp = eliteMonCtrl.hpPercent();


            
            if (hpbar.fillAmount > hp)
                hpbar.fillAmount -= Time.deltaTime * _hpspeed;

        }
    }
}
