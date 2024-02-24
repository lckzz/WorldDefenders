using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using static Define;

public class SkeletonKingController : EliteMonsterController
{
    //�̴� ��Ż
    
    private GameObject miniPortal1;
    //private GameObject miniPortal2;
    //private GameObject miniPortal3;
    //�̴� ��Ż

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

        coolTime = skeletonKingFristSkillCoolTime;            //���� �������� ��ų�� ��Ÿ������ (���̷���ŷ�� ó�� ��Ÿ�Ӹ� 5�ʰ� �� �� �������� ��Ÿ���� ����)



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

                if(coolTime != Skills.activeSkillList[0].SkillData.skillCoolTime)       //���� ��Ÿ���� �������� ��Ÿ�Ӱ� �ٸ��ٸ�
                    coolTime = Skills.activeSkillList[0].SkillData.skillCoolTime;       //��Ÿ���� �־���

                Managers.Sound.Play("Effect/Monster/EliteCavarlySkill");
                Skills.activeSkillList[0].UseSkill(this);     //��ų ���
                SpeechchBubbleOn(skillTitleKey, skillDialogSubKey, skillProbability);       //��ų���� ��ǳ���� (Ȯ��������)
                startCoolTimeCo = StartCoroutine(UnitSkillCoolTime(coolTime));              //��ų���� ��Ÿ�ӽ��� �ڷ�ƾ

            }
        }
    }
}
