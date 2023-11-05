using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class CavalryController : SpecialUnitController
{




    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        EnemySensor();
        UnitStateCheck();
    }


    public override void Init()
    {
        base.Init();
        unitStat = new UnitStat();

        unitStat = Managers.Data.cavarlyDict[Managers.Game.UnitCarlvlry];




        hp = unitStat.hp;
        att = unitStat.att;
        knockbackForce = unitStat.knockBackForce;
        attackRange = unitStat.attackRange;
        coolTime = 20.0f;

        moveSpeed = 2.5f;
        maxHp = hp;


        Skills.AddSkill<SwordSummonSkill>();
        skilldialogs = new string[dialogCount];
        for (int ii = 0; ii < dialogCount; ii++)
        {
            if (ii == 0)
                skilldialogs[ii] = Skills.GetSkill<SwordSummonSkill>().SkillData.skillDialog1;
            else if (ii == 1)
                skilldialogs[ii] = Skills.GetSkill<SwordSummonSkill>().SkillData.skillDialog2;


        }

    }



    public override void OnAttack()
    {
        if (monTarget != null)
        {
            float dist = (monTarget.transform.position - this.gameObject.transform.position).magnitude;
            if (dist < unitStat.attackRange + 0.5f)
                CriticalAttack(monTarget, warriorHitSound, warriorCriticalSound, warriorHitEff);

        }


        else if(monsterPortal != null)
            CriticalAttack(monsterPortal, warriorHitSound, warriorCriticalSound, warriorHitEff);
    }


    public override void UnitSkill()
    {

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("SkillAttack"))
        {
            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)
            {

                SetUnitState(SpecialUnitState.Idle);

                skillOn = false;
                attackCoolTime = 1.0f;


            }


        }
    }


    public void OnSkill()
    {
        if (skillOn)  //스킬온이면
        {
            if (Skills.activeSkillList.Count > 0)
            {
                Debug.Log("스킬사용");
                Skills.activeSkillList[0].UseSkill(this, skillMonList);     //스킬 사용
                SpeechchBubbleOn();
            }
        }
    }


    public void SpeechchBubbleOn()
    {

        if (speechBubbleObj.activeSelf == false)
            speechBubbleObj.SetActive(true);


        if (speechBubbleObj.activeSelf == true && speechBBCtrl != null)
        {
            randomIdx = Random.Range(0, 2);


            speechBBCtrl.GetSpeechString(skilldialogs[randomIdx]);
        }

    }
}
