using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using static Define;

public class EliteWarriorController : EliteMonsterController
{

    //엘리트워리어 검기
    Transform swordPos;

    //엘리트워리어 검기


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
        skilldialogs = new string[dialogCount];
        for (int ii = 0; ii < dialogCount; ii++)
        {
            if (ii == 0)
                skilldialogs[ii] = Skills.GetSkill<SwordDanceSkill>().SkillData.skillDialog1;
            else if (ii == 1)
                skilldialogs[ii] = Skills.GetSkill<SwordDanceSkill>().SkillData.skillDialog2;


        }
      
    }


    // Start is called before the first frame update
    void Start()
    {
        Init();
        Debug.Log(speechBubbleObj);


    }

    // Update is called once per frame
    void Update()
    {
        if (Managers.Game.GameEndResult())       //게임이 끝났으면 리턴
            return;


        EnemySensor();
        TowerSensor();
        MonsterStateCheck();
        //MonsterVictory();

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
