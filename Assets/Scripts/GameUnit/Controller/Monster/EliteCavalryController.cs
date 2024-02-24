using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class EliteCavalryController : EliteMonsterController
{ 
    private readonly string skillDialogSubKey = "eliteCavalrySkillDialog";
    private readonly string appearDialogSubKey = "eliteCavalryAppear";

    [SerializeField] private GameObject accObj;
    private SPUM_HorseSpriteList spumHorseList;
    private List<SpriteRenderer> allSpriteRenderList = new List<SpriteRenderer>();
    private List<SpriteRenderer> horseSRs = new List<SpriteRenderer>();



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
        Skills.AddSkill<AttackAfterImageSkill>();
        coolTime = Skills.activeSkillList[0].SkillData.skillCoolTime;

        speechBubble.SpeechBubbleOn(monsterAppearTitleKey, appearDialogSubKey,appearProbability);

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

                Managers.Sound.Play("Effect/Monster/EliteCavarlySkill");
                Skills.activeSkillList[0].UseSkill(this);     //��ų ���
                SpeechchBubbleOn(skillTitleKey,skillDialogSubKey,skillProbability);
                startCoolTimeCo = StartCoroutine(UnitSkillCoolTime(coolTime));              //��ų���� ��Ÿ�ӽ��� �ڷ�ƾ

            }
        }
    }


   

}
