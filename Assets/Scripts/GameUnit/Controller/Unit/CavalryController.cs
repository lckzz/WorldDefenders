using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class CavalryController : SpecialUnitController
{

    private readonly string skillDialogSubKey = "cavalrySkillDialog";
    private readonly string appearDialogSubkey = "cavalryAppear";

    public override void OnEnable()
    {
        base.OnEnable();
        if (sp != null && myColl != null)
            speechBubble.SpeechBubbuleOn(appearDialogSubkey, appearDialogSubkey, appearProbability);

    }

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
        speechBubble.SpeechBubbuleOn(appearDialogSubkey, appearDialogSubkey, appearProbability);

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
                SpeechchBubbleOn(skillTitleKey,skillDialogSubKey,skillProbaility);
            }
        }
    }



}
