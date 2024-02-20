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
        if (myColl != null)
            SpeechBubbleOn(appearTitleKey, appearDialogSubkey, appearProbability);

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

        magicPos = transform.Find("MagicPos");
        Skills.AddSkill<MeteroSkill>();
        coolTime = Skills.activeSkillList[0].SkillData.skillCoolTime;
        SpeechBubbleOn(appearTitleKey, appearDialogSubkey, appearProbability);

    }

    public override void OnAttack()
    {
        GameObject obj = Managers.Resource.Load<GameObject>("Prefabs/Weapon/MagicShot");

        Managers.Sound.Play("Effect/MeleeSound");
        if (monTarget != null)
        {

            if (obj != null)
            {
                GameObject magicBall = Managers.Resource.Instantiate(obj, magicPos.position, Quaternion.identity);
                magicBall.TryGetComponent(out MagicAttackCtrl magicCtrl);
                magicCtrl.Init(this);
            }
        }
        else
        {

            if (obj != null)
            {
                GameObject magicBall = Managers.Resource.Instantiate(obj, magicPos.position, Quaternion.identity);
                magicBall.TryGetComponent(out MagicAttackCtrl magicCtrl);
                magicCtrl.Init(this);

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
                if (startCoolTimeCo != null)
                    StopCoroutine(startCoolTimeCo);


                Skills.activeSkillList[0].UseSkill(this, skillMonList);     //��ų ���
                SpeechBubbleOn(skillTitleKey,skillDialogSubKey,skillProbaility);
                startCoolTimeCo = StartCoroutine(UnitSkillCoolTime(coolTime));              //��ų���� ��Ÿ�ӽ��� �ڷ�ƾ

            }
        }
    }

    public void MagicianSkillSound()
    {
        //�ִϸ��̼� �̺�Ʈ �Լ�
        Managers.Sound.Play("Effect/MagicianSkill");

    }



}
