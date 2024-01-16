using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PriestController : UnitController
{
    //UnitController���� ������Ʈ �����͸� �ʱ�ȭ�� �ϴ� �س���
    private Collider2D[] unitColls2D;

    [SerializeField] private Unit unitTarget;
    [SerializeField] private List<Unit> unitCtrls = new List<Unit>();

    private Transform posTr;


    string priestSound = "PriestHeal";
    string priestHitEff = "PriestEff";
    string prieshHealEff = "HealEff";




    public override void Init()
    {
        base.Init();
        unitStat = Managers.Data.priestDict[Managers.Game.UnitPriestLv];

        hp = unitStat.hp;
        att = unitStat.att;
        knockbackForce = unitStat.knockBackForce;
        attackRange = unitStat.attackRange;

        moveSpeed = 2.5f;
        maxHp = hp;

        posTr = transform.Find("MagicPos");
        SetUnitState(UnitState.Run);

    }

    void Start()
    {
        Init();
    }



    public override void EnemySensor()
    {


        unitColls2D = Physics2D.OverlapBoxAll(pos.position, boxSize, 0, LayerMask.GetMask("Unit") | LayerMask.GetMask("SpecialUnit"));  //�ڽ����� �Ʊ����� �޾ƿ´�.
        if (unitColls2D != null)
        {
            if (unitTarget != null)  //������ ���ֵ��� Ÿ������ ���������
                unitTarget = null;
            
            
            if (monTarget != null) //����Ÿ���� �����ִٸ�
                monTarget = null;

            unitCtrls.Clear();   //�ٽ� �����Ҷ� �޾Ҵ� ������ ������ �ʱ�ȭ�����ش�.
            //������ �ڽ����� ������ֵ��߿��� hp���� �޾Ƽ� hp�� �ִ�ġ�� �ƴ� ���ֵ��� �����ؾ��Ѵ�.
            for (int ii = 0; ii < unitColls2D.Length; ii++)
            {

                if (unitColls2D[ii].gameObject.layer == LayerMask.NameToLayer("Unit"))
                {
                    UnitController unitCtrl;
                    unitColls2D[ii].TryGetComponent<UnitController>(out unitCtrl);
                    if(unitCtrl != null)
                        unitCtrls.Add(unitCtrl);
                }

                if(unitColls2D[ii].gameObject.layer == LayerMask.NameToLayer("SpecialUnit"))
                {
                    SpecialUnitController unitCtrl;
                    unitColls2D[ii].TryGetComponent<SpecialUnitController>(out unitCtrl);
                    if (unitCtrl != null)
                        unitCtrls.Add(unitCtrl);
                }
            }


        }




        if (unitCtrls.Count > 0)
        {
            for (int i = 0; i < unitCtrls.Count; i++)
            {
                //�ڽ��ȿ����� ü�¾��� ĳ���� �ִٸ� �� ������ ����
                if (unitCtrls[i].Hp < unitCtrls[i].MaxHp)
                    unitTarget = unitCtrls[i];

            }
        }


       

        if (unitTarget == null)  //������Ʈ �ڽ��ȿ� ������ ���ٸ� ���� Ÿ������ ���
        {
            base.EnemySensor();
        }
    }

    protected override void UnitMove()
    {
        if (monTarget != null || unitTarget != null || monsterPortal != null)
            SetUnitState(UnitState.Trace);

        if (IsTargetOn())
            return;

        if (rigbody == null)
            TryGetComponent(out rigbody);

        rigbody.transform.position += Vector3.right * moveSpeed * Time.deltaTime;




    }


    protected override bool IsTargetOn()
    {
        if (unitTarget == null && monTarget == null && monsterPortal == null)
            return false;


        if (monTarget != null)
        {
            if (monTarget.gameObject.layer == LayerMask.NameToLayer("Monster") && monTarget is MonsterController monsterCtrl)
            {
                if (monsterCtrl.MonState == MonsterState.Die)
                    return false;

                if (!monTarget.gameObject.activeInHierarchy)
                    return false;
            }

            else if (monTarget.gameObject.layer == LayerMask.NameToLayer("EliteMonster") && monTarget is EliteMonsterController elite)
            {
                if (elite.MonState == Define.EliteMonsterState.Die)
                    return false;

                if (!monTarget.gameObject.activeInHierarchy)
                    return false;
            }

        }



        return true;
    }

    protected override void UnitTrace()
    {
        if (!IsTargetOn())
        {
            SetUnitState(UnitState.Run);
            return;
        }


        if (unitTarget != null)  //����Ÿ���� ������ ������ ���ؼ� �߰�
            Trace(unitTarget);
        else if (monTarget != null)
            Trace(monTarget);

        else if (unitTarget == null && monTarget == null)
            Trace(monsterPortal);
    }

    public override void CriticalAttack(Unit unit, string soundPath, string criticalSoundPath, string hitPath)
    {
        //������Ʈ ��

        if(unit is UnitController unitCtrl)  //�����̶�� ����� �������
        {

            int heal = (int)(att * 0.5f);     //������ �⺻���ݷ��� ���丷(ũ���� ������ �Ȱ���)
            unitCtrl.OnHeal(att);
            Managers.Resource.ResourceEffectAndSound(unitTarget.transform.position, soundPath, hitPath);

            
        }

        else if(unit is SpecialUnitController spUnitCtrl)
        {
            int heal = (int)(att * 0.5f);     //������ �⺻���ݷ��� ���丷(ũ���� ������ �Ȱ���)
            spUnitCtrl.OnHeal(att);
            Managers.Resource.ResourceEffectAndSound(unitTarget.transform.position, soundPath, hitPath);
        }

        else if(unit is MonsterController monsterCtrl)
        {
            if (CriticalCheck())//true�� ũ��Ƽ�õ����� false�� �Ϲݵ�����
            {
                int attack = att * 2;
                monsterCtrl.OnDamage(attack, unitStat.knockBackForce,true);      //ũ��Ƽ���̸� ������2�迡 �˹����
                Managers.Resource.ResourceEffectAndSound(monTarget.transform.position, soundPath, hitPath);

            }
            else  //��ũ��Ƽ���̸� �Ϲݰ���
            {

                monsterCtrl.OnDamage(att);        //�˹��� ����
                Managers.Resource.ResourceEffectAndSound(monTarget.transform.position, soundPath, hitPath);

            }
        }


    }


    public override void OnAttack()
    {
        if (unitTarget != null)
        {
            float dist = (unitTarget.transform.position - this.gameObject.transform.position).magnitude;
            if (dist < unitStat.attackRange + 0.5f)
                CriticalAttack(unitTarget, priestSound, priestSound, prieshHealEff);
        }

        else if (monTarget != null)
             CriticalAttack(monTarget, priestSound, priestSound, priestHitEff);

        else if(monsterPortal != null)
            CriticalAttack(monsterPortal, priestSound, priestSound, priestHitEff);
    }

    public override void AttackDelay()
    {
        if (state == UnitState.Die)
            return;

        if (attackCoolTime > 0.0f)
        {
            attackCoolTime -= Time.deltaTime;
            if (attackCoolTime <= .0f)
            {
                if(unitTarget != null)
                {
                    Vector3 vec = unitTarget.gameObject.transform.position - this.transform.position;
                    traceDistance = vec.sqrMagnitude;
                    if (traceDistance < attackRange * attackRange)
                        SetUnitState(UnitState.Attack);
                    else
                        SetUnitState(UnitState.Run);
                }

                else if (monTarget != null)
                {
                    Vector3 vec = monTarget.gameObject.transform.position - this.transform.position;
                    traceDistance = vec.sqrMagnitude;
                    if (traceDistance < attackRange * attackRange)
                        SetUnitState(UnitState.Attack);
                    else
                        SetUnitState(UnitState.Run);
                }
                else if(monsterPortal != null)
                {
                    Vector3 vec = monsterPortal.gameObject.transform.position - this.transform.position;
                    traceDistance = vec.sqrMagnitude;
                    if (traceDistance < attackRange * attackRange)
                        SetUnitState(UnitState.Attack);
                    else
                        SetUnitState(UnitState.Run);
                }
                else
                    SetUnitState(UnitState.Run);


            }

        }
    }

}
