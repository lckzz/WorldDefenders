using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using static Define;

public class SkeletonKingController : EliteMonsterController
{
    //미니 포탈
    
    private GameObject miniPortal1;
    //private GameObject miniPortal2;
    //private GameObject miniPortal3;
    //미니 포탈

    private float skeletonKingFristSkillCoolTime = 3.0f;

    private readonly string skillDialogSubKey = "skeletonKingSkillDialog";
    private readonly string appearDialogSubKey = "skeletonKingAppear";

    public GameObject MiniPortal1 { get { return miniPortal1; } }
    //public GameObject MiniPortal2 { get { return miniPortal2; } }
    //public GameObject MiniPortal3 { get { return miniPortal3; } }



    public override void OnEnable()
    {
        base.OnEnable();
        if (myColl != null)
        {
            Init();
            speechBubble.SpeechBubbleOn(monsterAppearTitleKey, appearDialogSubKey, appearProbability);

        }

    }

    public override void Init()
    {
        base.Init();

        Skills.ClearSkill();
        Skills.AddSkill<SkeletonSummonSkill>();

        coolTime = skeletonKingFristSkillCoolTime;            //현재 장착중인 스킬의 쿨타임적용 (스켈레톤킹은 처음 쿨타임만 5초고 그 후 데이터의 쿨타임을 따라감)



        speechBubble.SpeechBubbleOn(monsterAppearTitleKey, appearDialogSubKey, appearProbability);

        for (int ii = 0; ii < this.transform.childCount; ii++)
        {
            if (this.transform.GetChild(ii).name.Contains("PortalPos"))
                miniPortal1 = transform.GetChild(ii).gameObject;

            //if (this.transform.GetChild(ii).name.Contains("PortalPos2"))
            //    miniPortal2 = transform.GetChild(ii).gameObject;

        }

        startCoolTimeCo = StartCoroutine(UnitSkillCoolTime(coolTime));



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
        MonsterStateCheck();


    }

    public override void MonsterSkill()
    {

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("SkillAttack"))
        {
            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)
            {

                SetMonsterState(EliteMonsterState.Idle);

                skillOn = false;
                attackCoolTime = 1.0f;


            }


        }
    }


    public override void OnSkill()
    {
        if (skillOn)  //스킬온이면
        {
            if (Skills.activeSkillList.Count > 0)
            {
                if (startCoolTimeCo != null)
                    StopCoroutine(startCoolTimeCo);

                if(coolTime != Skills.activeSkillList[0].SkillData.skillCoolTime)       //현재 쿨타임이 데이터의 쿨타임과 다르다면
                    coolTime = Skills.activeSkillList[0].SkillData.skillCoolTime;       //쿨타임을 넣어줌

                Managers.Sound.Play("Effect/Monster/EliteCavarlySkill");
                Skills.activeSkillList[0].UseSkill(this);     //스킬 사용
                SpeechchBubbleOn(skillTitleKey, skillDialogSubKey, skillProbability);       //스킬사용시 말풍선온 (확률적으로)
                startCoolTimeCo = StartCoroutine(UnitSkillCoolTime(coolTime));              //스킬사용시 쿨타임시작 코루틴

            }
        }
    }
}
