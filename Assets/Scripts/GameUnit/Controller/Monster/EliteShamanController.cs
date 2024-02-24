using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class EliteShamanController : EliteMonsterController
{
    //매직포스
    private Transform magicPos;
    private readonly string skillDialogSubKey = "eliteShamanSkillDialog";
    private readonly string appearDialogSubKey = "eliteShamanAppear";

    //매직포스
    private int maxSkillTargetCount = 5;

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
        Skills.AddSkill<DarkPowerSkill>();
        coolTime = Skills.activeSkillList[0].SkillData.skillCoolTime;


        magicPos = transform.Find("MagicPos");

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
        if (Managers.Game.GameEndResult())       //게임이 끝났으면 리턴
            return;


        EnemySensor();
        MonsterStateCheck();


    }

    public override void EnemySensor()
    {
        base.EnemySensor();



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
        Managers.Sound.Play("Effect/MeleeSound");

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

    public override void OnSkill()
    {
        if (skillOn)  //스킬온이면
        {
            if (Skills.activeSkillList.Count > 0)
            {
                skillenemyList.Clear();
                for (int i = 0; i < unitCtrls.Count; i++)   //최대 타겟수
                {
                    if (i == 0)
                        skillenemyList.Add(unitCtrls[i]);

                    else if (i < maxSkillTargetCount)
                    {
                        skillenemyList.Add(unitCtrls[unitCtrls.Count - i]);

                    }
                }

                if (startCoolTimeCo != null)
                    StopCoroutine(startCoolTimeCo);


                Skills.activeSkillList[0].UseSkill(this,skillenemyList);     //스킬 사용
                SpeechchBubbleOn(skillTitleKey, skillDialogSubKey,skillProbability);
                startCoolTimeCo = StartCoroutine(UnitSkillCoolTime(coolTime));              //스킬사용시 쿨타임시작 코루틴

            }
        }
    }

    public void ShamanSkillSound()
    {
        //애니메이션 이벤트 함수
        Managers.Sound.Play("Effect/Monster/EliteShamanSkill");
    }

}
