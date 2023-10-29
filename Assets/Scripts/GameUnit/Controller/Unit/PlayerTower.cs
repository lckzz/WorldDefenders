using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTower : Tower
{
    [SerializeField]
    private TowerState twState = TowerState.Idle;

    private int m_level = 1;

    TowerStat towerStat = new TowerStat();

    // Start is called before the first frame update
    void Start()
    {
        towerStat = Managers.Data.towerDict[GlobalData.g_PlayerLevel];
        base.Init(towerStat.hp ,towerStat.level);       //나중에 JSon으로 받은 타워의 데이터에서 hp와 level을 넣어줌
    }

    // Update is called once per frame
    void Update()
    {
        //if(Managers.Game.State != GameState.GameFail)
        //    Managers.Game.State = GameState.GameFail;

    }

    public float GetSetHp { get { return hp; } set { if (value > 0) hp = value; } }
    public float GetMaxHp { get { return maxHp; } }



    public override float hpPercent()
    {
        return hp / maxHp;
    }

    public override void TowerDamage(int att)
    {
        if (hp > 0)
        {
            hp -= att;

            if (hp <= 0)
            {
                twState = TowerState.Destroy;
                TowerDestroy();
                hp = 0;

            }
        }
    }



    protected override void TowerDestroy()
    {
        if(twState == TowerState.Destroy)
        {
            Managers.Game.ResultState(Define.StageStageType.Defeat);

            //GameManager.instance.state = GameState.GameFail;
        }
    }

}
