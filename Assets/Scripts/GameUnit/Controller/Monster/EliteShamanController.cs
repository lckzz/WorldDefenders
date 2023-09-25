using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class EliteShamanController : EliteMonsterController
{
    //��������
    Transform magicPos;
    float distacne = 9999999;
    float dis;
    //��������


    public override void Init()
    {
        base.Init();

        monStat = new MonsterStat();


        monStat = Managers.Data.monsterDict[GlobalData.g_BowSkeletonID];


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
    void Update()
    {
        EnemySensor();
        TowerSensor();
        MonsterStateCheck();
        MonsterVictory();

    }

    public override void EnemySensor()
    {
        base.EnemySensor();

        for (int i = 0; i < unitCtrls.Length; i++)   //�ִ� Ÿ�ټ�
        {
            if (i == 0)
                skillenemyList.Add(unitCtrls[i]);

            else if (i < 3)
            {
                skillenemyList.Add(unitCtrls[unitCtrls.Length - i]);

            }
        }
        //for(var i = 0; i < unitCtrls.Length;i++)
        //{
        //    dis = ((Vector2)unitCtrls[i].transform.position - (Vector2)this.gameObject.transform.position).sqrMagnitude;

        //    if (unitCtrls[i].gameObject.activeInHierarchy)
        //    {
        //        if(unitCtrls[i].IsDie == false)
        //        {
        //            if(dis < distacne)
        //            {
        //                unitCtrlsOrder.Add(unitCtrls[i]);
        //                distacne = dis;
        //            }
        //        }
        //    }

        //}

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
                if (unitTarget.gameObject.layer == LayerMask.NameToLayer("Unit") && unitTarget is UnitController ctrl)
                {
                    magicCtrl.SetType(ctrl, null);

                }
                else if (unitTarget.gameObject.layer == LayerMask.NameToLayer("SpecialUnit") && unitTarget is SpecialUnitController special)
                {
                    magicCtrl.SetType(special, null);

                }
            }
        }
        else
        {

            if (obj != null)
            {
                GameObject magicBall = Instantiate(obj, magicPos.position, Quaternion.identity, this.transform);
                magicBall.TryGetComponent(out ShamanMagicAttackCtrl magicCtrl);
                magicCtrl.SetType(null, playerTowerCtrl);
            }
        }




    }
    //public void OnSkill()
    //{
    //    if (skillOn)  //��ų���̸�
    //    {
    //        if (Skills.activeSkillList.Count > 0)
    //        {
    //            Debug.Log("�߽�");
    //            Skills.activeSkillList[0].UseSkill(this, skillenemyList);     //��ų ���
    //            SpeechchBubbleOn();
    //        }
    //    }
    //}


    //public void SpeechchBubbleOn()
    //{

    //    if (speechBubbleObj.activeSelf == false)
    //        speechBubbleObj.SetActive(true);


    //    if (speechBubbleObj.activeSelf == true && speechBBCtrl != null)
    //    {
    //        randomIdx = Random.Range(0, 2);


    //        speechBBCtrl.GetSpeechString(skilldialogs[randomIdx]);
    //    }

    //}
}
