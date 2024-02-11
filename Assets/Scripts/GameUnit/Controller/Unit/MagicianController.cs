using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class MagicianController : SpecialUnitController
{

    //������ ����
    Transform magicPos;
    Vector3 normalMagicPos = new Vector3(-0.9f, 0.16f, 0.0f);

    private readonly string skillDialogSubKey = "magicianSkillDialog";
    private readonly string appearDialogSubkey = "magicianAppear";



    //������ ����

    public override void OnEnable()
    {
        base.OnEnable();
        if (sp != null && myColl != null)
            speechBubble.SpeechBubbuleOn(appearDialogSubkey, appearDialogSubkey, appearProbability);

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
        speechBubble.SpeechBubbuleOn(appearTitleKey, appearDialogSubkey, appearProbability);

    }

    public override void OnAttack()
    {
        GameObject obj = Managers.Resource.Load<GameObject>("Prefabs/Weapon/MagicShot");

        Managers.Sound.Play("Effect/MeleeSound");
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
        if (skillOn)  //��ų���̸�
        {
            if (Skills.activeSkillList.Count > 0)
            {
                Skills.activeSkillList[0].UseSkill(this, skillMonList);     //��ų ���
                SpeechchBubbleOn(skillTitleKey,skillDialogSubKey,skillProbaility);
            }
        }
    }

    public void MagicianSkillSound()
    {
        //�ִϸ��̼� �̺�Ʈ �Լ�
        Managers.Sound.Play("Effect/MagicianSkill");

    }



}
