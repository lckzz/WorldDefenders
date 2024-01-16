using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PriestController : UnitController
{
    //UnitController에서 프리스트 데이터를 초기화를 일단 해놓음
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


        unitColls2D = Physics2D.OverlapBoxAll(pos.position, boxSize, 0, LayerMask.GetMask("Unit") | LayerMask.GetMask("SpecialUnit"));  //박스안의 아군들을 받아온다.
        if (unitColls2D != null)
        {
            if (unitTarget != null)  //이전에 유닛들의 타겟팅이 잡혓더라면
                unitTarget = null;
            
            
            if (monTarget != null) //몬스터타겟이 남아있다면
                monTarget = null;

            unitCtrls.Clear();   //다시 감지할때 받았던 유닛의 정보를 초기화시켜준다.
            //힐러는 박스안의 모든유닛들중에서 hp값을 받아서 hp가 최대치가 아닌 유닛들을 감지해야한다.
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
                //박스안에서의 체력없는 캐릭이 있다면 그 유닛을 선택
                if (unitCtrls[i].Hp < unitCtrls[i].MaxHp)
                    unitTarget = unitCtrls[i];

            }
        }


       

        if (unitTarget == null)  //프리스트 박스안에 유닛이 없다면 몬스터 타겟으로 잡기
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


        if (unitTarget != null)  //유닛타겟이 있으면 유닛을 향해서 추격
            Trace(unitTarget);
        else if (monTarget != null)
            Trace(monTarget);

        else if (unitTarget == null && monTarget == null)
            Trace(monsterPortal);
    }

    public override void CriticalAttack(Unit unit, string soundPath, string criticalSoundPath, string hitPath)
    {
        //프리스트 용

        if(unit is UnitController unitCtrl)  //유닛이라면 딜대신 힐줘야함
        {

            int heal = (int)(att * 0.5f);     //힐량은 기본공격력의 반토막(크리는 터져도 똑같음)
            unitCtrl.OnHeal(att);
            Managers.Resource.ResourceEffectAndSound(unitTarget.transform.position, soundPath, hitPath);

            
        }

        else if(unit is SpecialUnitController spUnitCtrl)
        {
            int heal = (int)(att * 0.5f);     //힐량은 기본공격력의 반토막(크리는 터져도 똑같음)
            spUnitCtrl.OnHeal(att);
            Managers.Resource.ResourceEffectAndSound(unitTarget.transform.position, soundPath, hitPath);
        }

        else if(unit is MonsterController monsterCtrl)
        {
            if (CriticalCheck())//true면 크리티컬데미지 false면 일반데미지
            {
                int attack = att * 2;
                monsterCtrl.OnDamage(attack, unitStat.knockBackForce,true);      //크리티컬이면 데미지2배에 넉백까지
                Managers.Resource.ResourceEffectAndSound(monTarget.transform.position, soundPath, hitPath);

            }
            else  //노크리티컬이면 일반공격
            {

                monsterCtrl.OnDamage(att);        //넉백은 없이
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
