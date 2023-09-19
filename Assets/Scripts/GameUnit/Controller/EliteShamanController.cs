using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class EliteShamanController : EliteMonsterController
{
    //매직포스
    Transform magicPos;

    //매직포스


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

        magicPos = transform.Find("MagicPos");
        //Skills.AddSkill<SwordDanceSkill>();
        //skilldialogs = new string[dialogCount];
        //for (int ii = 0; ii < dialogCount; ii++)
        //{
        //    if (ii == 0)
        //        skilldialogs[ii] = Skills.GetSkill<SwordDanceSkill>().SkillData.skillDialog1;
        //    else if (ii == 1)
        //        skilldialogs[ii] = Skills.GetSkill<SwordDanceSkill>().SkillData.skillDialog2;


        //}

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
        //부모클래스에서의 공격은 근접용이라 새로 덮어써서 공격함수를 오버라이드한다.

        //if (unitTarget != null)
        //{
        //    float dist = (unitTarget.transform.position - this.gameObject.transform.position).sqrMagnitude;
        //    if (dist < monStat.attackRange * monStat.attackRange)
        //        CriticalAttack(unitTarget);
        //    else
        //    {
        //        if (towerDist < monStat.attackRange * monStat.attackRange)
        //            CriticalAttack(playerTowerCtrl);
        //    }

        //}


        //else
        //    CriticalAttack(playerTowerCtrl);






    }
    //public void OnSkill()
    //{
    //    if (skillOn)  //스킬온이면
    //    {
    //        if (Skills.activeSkillList.Count > 0)
    //        {
    //            Debug.Log("발싸");
    //            Skills.activeSkillList[0].UseSkill(this, skillenemyList);     //스킬 사용
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
