using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using static Define;

public class EliteWarriorController : EliteMonsterController
{

    //엘리트워리어 검기
    private Transform swordPos;

    //엘리트워리어 검기
    private readonly string skillDialogSubKey = "eliteWarriorSkillDialog";
    private readonly string appearDialogSubKey = "eliteWarriorAppear";


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


        monStat = Managers.Data.monsterDict[GlobalData.g_EliteWarriorID];


        att = monStat.att;
        hp = monStat.hp;
        maxHp = hp;
        knockbackForce = monStat.knockBackForce;
        attackRange = monStat.attackRange;
        moveSpeed = 2.0f;

        swordPos = transform.Find("SwordPos");
        Skills.AddSkill<SwordDanceSkill>();
        speechBubble.SpeechBubbuleOn(monsterAppearTitleKey, appearDialogSubKey, appearProbability);


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
                SpeechchBubbleOn(skillTitleKey, skillDialogSubKey,skillProbability);
            }
        }
    }
}
