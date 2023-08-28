using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicianController : SpecialUnitController
{

    //마법사 전용
    Transform magicPos;
    Vector3 normalMagicPos = new Vector3(-0.9f, 0.16f, 0.0f);
    //마법사 전용

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        TowerSensor();
        EnemySensor();
        UnitStateCheck();

        if(Input.GetKeyDown(KeyCode.P))
        {
            if(Skills.activeSkillList.Count > 0)
            {
                Skills.activeSkillList[0].UseSkill(this, skillMonList);

            }
        }

    }

    public override void Init()
    {
        base.Init();
        unitStat = new UnitStat();

        unitStat = Managers.Data.magicDict[GlobalData.g_UnitMagicianLv];




        hp = unitStat.hp;
        att = unitStat.att;
        knockbackForce = unitStat.knockBackForce;
        attackRange = unitStat.attackRange;

        moveSpeed = 2.5f;
        maxHp = hp;


        magicPos = transform.Find("MagicPos");
        Skills.AddSkill<MeteroSkill>();


    }

    public override void OnAttack()
    {
        GameObject obj = Resources.Load<GameObject>("Prefabs/Weapon/MagicShot");

        if (monTarget != null)
        {

            if (obj != null)
            {
                GameObject magicBall = Instantiate(obj, magicPos.position, Quaternion.identity, this.transform);
                magicBall.TryGetComponent(out MagicAttackCtrl magicCtrl);
                magicCtrl.SetType(monTarget, null);
            }
        }
        else
        {

            if (obj != null)
            {
                GameObject magicBall = Instantiate(obj, magicPos.position, Quaternion.identity, this.transform);
                magicBall.TryGetComponent(out MagicAttackCtrl magicCtrl);
                magicCtrl.SetType(null, monsterPortal);
            }
        }
    }
}
