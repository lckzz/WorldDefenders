using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class EliteShamanController : EliteMonsterController
{
    //��������
    Transform magicPos;

    //��������


    public override void Init()
    {
        base.Init();

        monStat = new MonsterStat();


        monStat = Managers.Data.monsterDict[GlobalData.g_EliteShamanID];


        att = monStat.att;
        hp = monStat.hp;
        maxHp = hp;
        knockbackForce = monStat.knockBackForce;
        attackRange = monStat.attackRange;
        moveSpeed = 2.0f;

        magicPos = transform.Find("MagicPos");
        Skills.AddSkill<DarkPowerSkill>();
        skilldialogs = new string[dialogCount];
        for (int ii = 0; ii < dialogCount; ii++)
        {
            if (ii == 0)
                skilldialogs[ii] = Skills.GetSkill<DarkPowerSkill>().SkillData.skillDialog1;
            else if (ii == 1)
                skilldialogs[ii] = Skills.GetSkill<DarkPowerSkill>().SkillData.skillDialog2;


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

    public override void EnemySensor()
    {
        base.EnemySensor();

        for (int i = 0; i < unitCtrls.Count; i++)   //�ִ� Ÿ�ټ�
        {
            if (i == 0)
                skillenemyList.Add(unitCtrls[i]);

            else if (i < 3)
            {
                skillenemyList.Add(unitCtrls[unitCtrls.Count - i]);

            }
        }

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

    public override void OnAttack()
    {
        //�θ�Ŭ���������� ������ �������̶� ���� ����Ἥ �����Լ��� �������̵��Ѵ�.
        GameObject obj = Resources.Load<GameObject>("Prefabs/Weapon/ShamanMagicShot");

        if (unitTarget != null)
        {

            if (obj != null)
            {
                GameObject magicBall = Instantiate(obj, magicPos.position, Quaternion.identity, this.transform);
                magicBall.TryGetComponent(out ShamanMagicAttackCtrl magicCtrl);
                magicCtrl.Init();
            }
        }
        else
        {

            if (obj != null)
            {
                GameObject magicBall = Instantiate(obj, magicPos.position, Quaternion.identity, this.transform);
                magicBall.TryGetComponent(out ShamanMagicAttackCtrl magicCtrl);
                magicCtrl.Init();

            }
        }




    }
    
}
