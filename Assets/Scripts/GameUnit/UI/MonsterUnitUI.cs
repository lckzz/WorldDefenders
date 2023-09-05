using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterUnitUI : UnitUI
{
    private Unit uniCtrl = null;
    //[SerializeField] private MonsterController monCtrl;
    //[SerializeField] private EliteMonsterController eliteMonCtrl;
    // Start is called before the first frame update
    void Start()
    {
        if(uniCtrl == null)
        {
            this.gameObject.TryGetComponent(out uniCtrl);
        }


        //if(uniCtrl as MonsterController)
        //{
        //    monCtrl = (MonsterController)uniCtrl;
        //    ComponentInit<MonsterController>(out monCtrl);

        //}
        //else if (uniCtrl as EliteMonsterController)
        //{
        //    eliteMonCtrl = (EliteMonsterController)uniCtrl;
        //    ComponentInit<EliteMonsterController>(out eliteMonCtrl);

        //}
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHp(hpSpeed);
        if(uniCtrl != null)
        {
            if(uniCtrl.IsDie)
            {

                if(uniCtrl != null)
                    UnitDeadSpAlpha<Unit>(uniCtrl, spr);



            }
        }

    }

    

    public override void UpdateHp(float _hpspeed)
    {
        if(hpbar != null)
        {

            if(uniCtrl != null)
                hp = uniCtrl.hpPercent();


            
            if (hpbar.fillAmount > hp)
                hpbar.fillAmount -= Time.deltaTime * _hpspeed;

        }
    }
}
