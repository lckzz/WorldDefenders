using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class EliteShamanController : EliteMonsterController
{
    //��������
    private Transform magicPos;
    private readonly string skillDialogSubKey = "eliteShamanSkillDialog";
    private readonly string appearDialogSubKey = "eliteShamanAppear";

    //��������
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
        if (Managers.Game.GameEndResult())       //������ �������� ����
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
        //�θ�Ŭ���������� ������ �������̶� ���� ����Ἥ �����Լ��� �������̵��Ѵ�.
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
        if (skillOn)  //��ų���̸�
        {
            if (Skills.activeSkillList.Count > 0)
            {
                skillenemyList.Clear();
                for (int i = 0; i < unitCtrls.Count; i++)   //�ִ� Ÿ�ټ�
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


                Skills.activeSkillList[0].UseSkill(this,skillenemyList);     //��ų ���
                SpeechchBubbleOn(skillTitleKey, skillDialogSubKey,skillProbability);
                startCoolTimeCo = StartCoroutine(UnitSkillCoolTime(coolTime));              //��ų���� ��Ÿ�ӽ��� �ڷ�ƾ

            }
        }
    }

    public void ShamanSkillSound()
    {
        //�ִϸ��̼� �̺�Ʈ �Լ�
        Managers.Sound.Play("Effect/Monster/EliteShamanSkill");
    }

}
