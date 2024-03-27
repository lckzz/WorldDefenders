using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class EliteWarriorController : EliteMonsterController
{

    //����Ʈ������ �˱�
    private Transform swordPos;

    //����Ʈ������ �˱�
    private readonly string skillDialogSubKey = "eliteWarriorSkillDialog";
    private readonly string appearDialogSubKey = "eliteWarriorAppear";


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
        Skills.AddSkill<SwordDanceSkill>();
        coolTime = Skills.activeSkillList[0].SkillData.skillCoolTime;

        swordPos = transform.Find("SwordPos");
        speechBubble.SpeechBubbleOn(monsterAppearTitleKey, appearDialogSubKey, appearProbability);
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
        if (Managers.Game.GameEndResult())       //������ �������� ����
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
        if (skillOn)  //��ų���̸�
        {
            if (Skills.activeSkillList.Count > 0)
            {
                if (startCoolTimeCo != null)
                    StopCoroutine(startCoolTimeCo);

                Skills.activeSkillList[0].UseSkill(this,skillenemyList);     //��ų ���
                SpeechchBubbleOn(skillTitleKey, skillDialogSubKey,skillProbability);
                startCoolTimeCo = StartCoroutine(UnitSkillCoolTime(coolTime));              //��ų���� ��Ÿ�ӽ��� �ڷ�ƾ

            }
        }
    }
}
