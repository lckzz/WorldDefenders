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
        if (myColl != null)
            SpeechBubbleOn(appearTitleKey, appearDialogSubkey, appearProbability);

    }

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (Managers.Game.GameEndResult())       //게임이 끝났으면 리턴
            return;



        EnemySensor();
        UnitStateCheck();
    }


    public override void Init()
    {
        base.Init();


        Skills.AddSkill<SwordSummonSkill>();
        coolTime = Skills.activeSkillList[0].SkillData.skillCoolTime;
        SpeechBubbleOn(appearTitleKey, appearDialogSubkey, appearProbability);

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
                if (startCoolTimeCo != null)
                    StopCoroutine(startCoolTimeCo);

                Debug.Log(Skills.activeSkillList[0].name);
                Skills.activeSkillList[0].UseSkill(this, skillMonList);     //스킬 사용
                SpeechBubbleOn(skillTitleKey,skillDialogSubKey,skillProbaility);
                startCoolTimeCo = StartCoroutine(UnitSkillCoolTime(coolTime));              //스킬사용시 쿨타임시작 코루틴

            }
        }
    }


 



}
