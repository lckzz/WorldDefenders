using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class MagicianController : SpecialUnitController
{

    //마법사 전용
    Transform magicPos;
    Vector3 normalMagicPos = new Vector3(-0.9f, 0.16f, 0.0f);
    //마법사 전용

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        EnemySensor();
        UnitStateCheck();

        
    }

    public override void Init()
    {
        base.Init();
        unitStat = new UnitStat();

        unitStat = Managers.Data.magicDict[Managers.Game.UnitMagicianLv];




        hp = unitStat.hp;
        att = unitStat.att;
        knockbackForce = unitStat.knockBackForce;
        attackRange = unitStat.attackRange;
        coolTime = 20.0f;

        moveSpeed = 2.5f;
        maxHp = hp;


        magicPos = transform.Find("MagicPos");
        Skills.AddSkill<MeteroSkill>();
        skilldialogs = new string[dialogCount];
        for (int ii = 0; ii < dialogCount; ii++)
        {
            if (ii == 0)
                skilldialogs[ii] = Skills.GetSkill<MeteroSkill>().SkillData.skillDialog1;
            else if (ii == 1)
                skilldialogs[ii] = Skills.GetSkill<MeteroSkill>().SkillData.skillDialog2;


        }

    }

    public override void OnAttack()
    {
        GameObject obj = Managers.Resource.Load<GameObject>("Prefabs/Weapon/MagicShot");

        if (monTarget != null)
        {

            if (obj != null)
            {
                GameObject magicBall = Managers.Resource.Instantiate(obj, magicPos.position, Quaternion.identity, this.transform);
                magicBall.TryGetComponent(out MagicAttackCtrl magicCtrl);
                magicCtrl.Init();
            }
        }
        else
        {

            if (obj != null)
            {
                GameObject magicBall = Managers.Resource.Instantiate(obj, magicPos.position, Quaternion.identity, this.transform);
                magicBall.TryGetComponent(out MagicAttackCtrl magicCtrl);
                magicCtrl.Init();

            }
        }
    }


    public override void UnitSkill()
    {

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("SkillAttack"))
        {
            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)
            {

                SetUnitState(SpecialUnitState.Idle);

                skillOn = false;
                attackCoolTime = 1.0f;


            }


        }
    }


    public void OnSkill()
    {
        if (skillOn)  //스킬온이면
        {
            if (Skills.activeSkillList.Count > 0)
            {
                Debug.Log("발싸");
                Skills.activeSkillList[0].UseSkill(this, skillMonList);     //스킬 사용
                SpeechchBubbleOn();
            }
        }
    }


    public void SpeechchBubbleOn()
    {

        if (speechBubbleObj.activeSelf == false)
            speechBubbleObj.SetActive(true);


        if (speechBubbleObj.activeSelf == true && speechBBCtrl != null)
        {
            randomIdx = Random.Range(0, 2);


            speechBBCtrl.SetSpeechString(skilldialogs[randomIdx]);
        }

    }
}
