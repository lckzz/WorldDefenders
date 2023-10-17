using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class EliteShamanController : EliteMonsterController
{
    //매직포스
    Transform magicPos;
    float distacne = 9999999;
    float dis;
    //매직포스


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
        if (Managers.Game.GameEndResult())       //게임이 끝났으면 리턴
            return;


        EnemySensor();
        TowerSensor();
        MonsterStateCheck();


    }

    public override void EnemySensor()
    {
        base.EnemySensor();

        for (int i = 0; i < unitCtrls.Count; i++)   //최대 타겟수
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
        //부모클래스에서의 공격은 근접용이라 새로 덮어써서 공격함수를 오버라이드한다.
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
