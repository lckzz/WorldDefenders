using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class EliteCavalryController : EliteMonsterController
{
    GameObject miniPortal1;
    GameObject miniPortal2;

    public GameObject MiniPortal1 { get { return miniPortal1; } }
    public GameObject MiniPortal2 { get { return miniPortal2; } }

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
        skilldialogs = new string[dialogCount];
        for (int ii = 0; ii < dialogCount; ii++)
        {
            if (ii == 0)
                skilldialogs[ii] = Skills.GetSkill<SkeletonSummonSkill>().SkillData.skillDialog1;
            else if (ii == 1)
                skilldialogs[ii] = Skills.GetSkill<SkeletonSummonSkill>().SkillData.skillDialog2;


        }


        for (int ii = 0;ii < this.transform.childCount;ii++)
        {
            if (this.transform.GetChild(ii).name.Contains("PortalPos1"))
                miniPortal1 = transform.GetChild(ii).gameObject;

            if(this.transform.GetChild(ii).name.Contains("PortalPos2"))
                miniPortal2 = transform.GetChild(ii).gameObject;

        }

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
        Debug.Log("�׽�Ʈ");
        if (skillOn)  //��ų���̸�
        {
            if (Skills.activeSkillList.Count > 0)
            {
                Debug.Log("�߽�");
                Skills.activeSkillList[0].UseSkill(this);     //��ų ���
                SpeechchBubbleOn();
            }
        }
    }


}
