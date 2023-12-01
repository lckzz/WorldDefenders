using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class EliteCavalryController : EliteMonsterController
{
    private GameObject miniPortal1;
    private GameObject miniPortal2;

    private readonly string skillDialogSubKey = "eliteCavalrySkillDialog";
    private readonly string appearDialogSubKey = "eliteCavalryAppear";


    public GameObject MiniPortal1 { get { return miniPortal1; } }
    public GameObject MiniPortal2 { get { return miniPortal2; } }


    public override void OnEnable()
    {
        base.OnEnable();
        if (sp != null && myColl != null)
            speechBubble.SpeechBubbuleOn(monsterAppearTitleKey, appearDialogSubKey, appearProbability);

    }

    public override void Init()
    {
        base.Init();

        monStat = new MonsterStat();


        monStat = Managers.Data.monsterDict[GlobalData.g_EliteCavalryID];


        att = monStat.att;
        hp = monStat.hp;
        maxHp = hp;
        knockbackForce = monStat.knockBackForce;
        attackRange = monStat.attackRange;
        moveSpeed = 2.0f;

        Skills.AddSkill<SkeletonSummonSkill>();



        for (int ii = 0;ii < this.transform.childCount;ii++)
        {
            if (this.transform.GetChild(ii).name.Contains("PortalPos1"))
                miniPortal1 = transform.GetChild(ii).gameObject;

            if(this.transform.GetChild(ii).name.Contains("PortalPos2"))
                miniPortal2 = transform.GetChild(ii).gameObject;

        }


        speechBubble.SpeechBubbuleOn(monsterAppearTitleKey, appearDialogSubKey,appearProbability);

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
                Debug.Log("발싸");
                Skills.activeSkillList[0].UseSkill(this);     //스킬 사용
                SpeechchBubbleOn(skillTitleKey,skillDialogSubKey,skillProbability);
            }
        }
    }


}
