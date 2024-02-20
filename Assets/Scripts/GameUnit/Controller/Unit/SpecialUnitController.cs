using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static Define;

public class SpecialUnitController : UnitBase
{

    [SerializeField]
    protected Define.SpecialUnitState state = Define.SpecialUnitState.Run;

    protected bool isSkil = false;
    protected bool isAttacking = false;
    [SerializeField] protected bool skillOn = false;     //스킬 발동판단


    protected List<Unit> skillMonList = new List<Unit>();


    protected float coolTime = 20.0f;

    protected readonly string warriorHitSound = "WarriorAttack";
    protected readonly string warriorCriticalSound = "CriticalSound";
    protected readonly string warriorHitEff = "HitEff";




    protected readonly string skillTitleKey = "skillDialog";

    protected readonly int appearProbability = 50;
    protected readonly int dieProbaility = 70;
    protected readonly int skillProbaility = 100;


    [SerializeField] protected GameObject appearDust;



    public SkillBook Skills { get; protected set; }


    public SpecialUnitState UniState { get { return state; } }

    protected Coroutine startCoolTimeCo;

    public override void OnEnable()
    {
        if (myColl != null)
        {
            //오브젝트 풀에서 생성되면 초기화 시켜줘야함
            isDie = false;
            hp = maxHp;
            SetUnitState(SpecialUnitState.Run);
            myColl.enabled = true;
            appearDust?.SetActive(true);

            if (startCoolTimeCo != null)
                StopCoroutine(startCoolTimeCo);

            skillOn = false;

        }

    }

    public override void Init()
    {
        base.Init();
        spawnPosX = -9.2f;
        Skills = gameObject.GetComponent<SkillBook>();


        SetUnitState(SpecialUnitState.Run);

        startCoolTimeCo = StartCoroutine(UnitSkillCoolTime(coolTime));
        appearDust?.SetActive(true);



    }



    public override void EnemySensor()      //적감지
    {
        //Debug.Log(isTargeting);
        //Debug.Log($"타겟팅{isTageting}");
        #region 타겟구현
        if (state == SpecialUnitState.Attack || state == SpecialUnitState.Skill)
            return;

        UnitSense();
        UnitDistanceAsending();






        #endregion

    }
    //유닛들을 감지
    protected override void UnitSense()
    {
        monCtrls.Clear();
        skillMonList.Clear();
        base.UnitSense();
    }

    protected override void UnitDistanceAsending()
    {
        if (monCtrls.Count > 0)
        {
            float disMin = 0;
            int min = 0;


            if (monCtrls.Count > 1)
            {

                for (int i = 0; i < monCtrls.Count; i++)
                {

                    if (i < 3)
                        skillMonList.Add(monCtrls[i]);


                    if (i == 0 && monCtrls.Count > 1)
                    {
                        float distA = (monCtrls[i].transform.position - this.transform.position).sqrMagnitude;
                        float distB = (monCtrls[i + 1].transform.position - this.transform.position).sqrMagnitude;

                        if (distA * distA > distB * distB)
                        {
                            disMin = distB * distB;
                            min = i + 1;

                        }
                        else
                        {
                            disMin = distA * distA;
                            min = i;


                        }
                    }

                    else if (i < monCtrls.Count - 1)
                    {
                        float distB = (monCtrls[i + 1].transform.position - this.transform.position).sqrMagnitude;

                        if (disMin > distB * distB)
                        {
                            disMin = distB * distB;
                            min = i + 1;


                        }


                    }

                }
            }

            if (monCtrls.Count == 1)
            {
                skillMonList.Add(monCtrls[0]);

            }

            for (int ii = 0; ii < skillMonList.Count; ii++)
            {
                if (skillMonList[ii].IsDie)
                    skillMonList.RemoveAt(ii);
            }


            if (monCtrls.Count != 0)
            {
                monTarget = monCtrls[min];
                //skillMonList.Add(monTarget);

            }


        }

    }


    public void TowerSensor()
    {
        towerColl = Physics2D.OverlapBox(pos.position, boxSize, 0, LayerMask.GetMask("MonsterPortal"));
        if (towerColl != null)
            towerColl.TryGetComponent(out monsterPortal);
    }

 
    public void UnitStateCheck()
    {
        switch (state)
        {
            case SpecialUnitState.Idle:
                {
                    UnitIdle();
                    break;
                }
            case SpecialUnitState.Run:
                {
                    UnitMove();
                    break;
                }
            case SpecialUnitState.Trace:
                {
                    UnitTrace();
                    break;
                }
            case SpecialUnitState.Attack:
                {
                    UnitAttack();
                    break;
                }
            case SpecialUnitState.Skill:
                {
                    UnitSkill();   
                    break;
                }
            case SpecialUnitState.KnockBack:
                {
                    ApplyKnockBack(new Vector2(-1.0f, 1.0f), damageKnockBack);
                    break;
                }

            case SpecialUnitState.Die:
                {
                    UnitDie();
                    break;
                }

        }

    }

    protected void SetUnitState(SpecialUnitState state)
    {

        this.state = state;

        switch (this.state)
        {
            case SpecialUnitState.Idle:
                {
                    if (isAtt)
                    {
                        isAtt = false;
                        anim.SetBool("Attack", isAtt);
                    }
                    if (isRun)
                    {
                        isRun = false;
                        anim.SetBool("Run", isRun);
                    }
                    if (isSkil)
                    {
                        isSkil = false;
                        anim.SetBool("SkillAttack", isSkil);
                    }
                    break;
                }
            case SpecialUnitState.Run:
                {
                    if (!isRun)
                    {
                        isRun = true;
                        anim.SetBool("Run", isRun);

                    }
                    if (isAtt)
                    {
                        isAtt = false;
                        anim.SetBool("Attack", isAtt);

                    }
                    if (isSkil)
                    {
                        isSkil = false;
                        anim.SetBool("SkillAttack", isSkil);
                    }
                    break;
                }
            case SpecialUnitState.Attack:
                {
                    if (isRun)
                    {
                        isRun = false;
                        anim.SetBool("Run", isRun);

                    }

                    if (!isAtt)
                    {
                        isAtt = true;
                        anim.SetBool("Attack", isAtt);

                    }
                    if (isSkil)
                    {
                        isSkil = false;
                        anim.SetBool("SkillAttack", isSkil);
                    }

                    if (monCtrls.Count > 0)
                    {
                        if (skillOn)      //스킬On이면
                        {
                            SetUnitState(SpecialUnitState.Skill);
                            return;
                        }
                    }

                    break;
                }
            case SpecialUnitState.Skill:
                {

                    if (isRun)
                    {
                        isRun = false;
                        anim.SetBool("Run", isRun);

                    }
                    if (isAtt)
                    {
                        isAtt = false;
                        anim.SetBool("Attack", isAtt);
                    }
                    if (!isSkil)
                    {

                        isSkil = true;
                        anim.SetBool("SkillAttack", isSkil);
                    }
                    break;
                }
            case SpecialUnitState.KnockBack:
                {
                    if (isRun)
                    {
                        isRun = false;
                        anim.SetBool("Run", isRun);

                    }
                    if (isAtt)
                    {
                        isAtt = false;
                        anim.SetBool("Attack", isAtt);

                    }
                    if (isSkil)
                    {
                        isSkil = false;
                        anim.SetBool("SkillAttack", isSkil);
                    }
                    break;
                }
            case SpecialUnitState.Die:
                {
                    if (!isDie)
                    {
                        isDie = true;
                        anim.SetTrigger("Die");

                    }
                    break;
                }
        }
    }


    void UnitIdle()
    {
        AttackDelay();

    }


    void UnitMove()
    {

        if (monTarget != null || monsterPortal != null)
            SetUnitState(SpecialUnitState.Trace);



        if (IsTargetOn())
            return;

        rigbody.transform.position += Vector3.right * moveSpeed * Time.fixedDeltaTime;




    }

    void UnitTrace()
    {
        if (!IsTargetOn())
        {
            SetUnitState(SpecialUnitState.Run);
            return;
        }

        if (monTarget != null)
            Trace(monTarget);

        else if (monsterPortal != null)
            Trace(monsterPortal);
    }

    //bool IsTargetOn()
    //{
    //    if (monTarget == null && monsterPortal == null)
    //        return false;


    //    if (monTarget != null)
    //    {
    //        if (monTarget.gameObject.layer == LayerMask.NameToLayer("Monster") && monTarget is MonsterController monsterCtrl)
    //        {
    //            if (monsterCtrl.MonState == MonsterState.Die)
    //                return false;

    //            if (!monTarget.gameObject.activeInHierarchy)
    //                return false;
    //        }

    //        else if (monTarget.gameObject.layer == LayerMask.NameToLayer("EliteMonster") && monTarget is EliteMonsterController elite)
    //        {
    //            if (elite.MonState == Define.EliteMonsterState.Die)
    //                return false;

    //            if (!monTarget.gameObject.activeInHierarchy)
    //                return false;
    //        }
    //    }



    //    return true;
    //}

    void UnitAttack()
    {


        if (anim.GetCurrentAnimatorStateInfo(0).IsName("NormalAttack"))
        {

            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)
            { 
                SetUnitState(SpecialUnitState.Idle);
                attackCoolTime = 1.0f;
            }


        }
    }


    public virtual void UnitSkill()
    {
        //여기서 각각의 유닛들의 스킬들을 구현하면 너무 낭비되는 메모리가 생김
        //왜냐면 구현하기 위한 게임오브젝트들을 미리 선언하기 때문
        //쿨타임이 돌면 스킬매니저에서 해당 유닛에 맞는 스킬을 가져온다.
        //공격했을 때 공격박스에 있는 유닛들을 가져와야함
        //타겟유닛리스트를 매개변수로 넣는다.
        //Managers.Skill.ActiveSkillUse(skillMonList,);



    }

    void UnitDie()
    {
        if (myColl.enabled)
        {
            speechBubble.SpeechBubbleOn(dieTitleKey, dieDialogSubKey, dieProbaility);
            myColl.enabled = false;
            StartCoroutine(DestroyTime(gameObject, 3.0f));
            SetUnitState(SpecialUnitState.Die);
            onDead?.Invoke();
        }
    }



    public override void OnDamage(int att, int knockBack = 0, bool criticalCheck = false)
    {

        if (hp > 0)
        {
            hp -= att;
            NotifyToHpObserver();       //체력이 바뀌어서 옵저버들에게 체력이 바꼇다는걸 알리고 보내기

            //넉백이 안통하는 존에 있다면 넉백수치를 0으로 만들어준다.
            if (NoKnockBackValid())
                knockBack = 0;

            if (att > 0)   //받은 데미지가 0보다 클때만 데미지 표시
            {
                if (criticalCheck)
                    unitHUDHp?.SpawnHUDText(att.ToString(), (int)Define.UnitDamageType.Critical);
                else
                    unitHUDHp?.SpawnHUDText(att.ToString(), (int)Define.UnitDamageType.Enemy);
            }


            if (knockBack > 0)
            {
                SetUnitState(SpecialUnitState.KnockBack);
                damageKnockBack = knockBack;
            }


            if (hp <= 0)
            {
                hp = 0;
                SetUnitState(SpecialUnitState.Die);
            }
        }
    }







    //public override void OnAttack()    //애니메이션 이벤트 함수
    //{

    //}





    //public override void CriticalAttack(Unit monCtrl, string soundPath, string criticlaSoundPath, string hitPath)
    //{
    //    if (CriticalCheck())//true면 크리티컬데미지 false면 일반데미지
    //    {
    //        int attack = att * 2;
    //        monCtrl.OnDamage(attack, unitStat.knockBackForce,true);      //크리티컬이면 데미지2배에 넉백까지
    //        Managers.Resource.ResourceEffectAndSound(monTarget.transform.position,warriorCriticalSound,warriorHitEff);

    //    }
    //    else  //노크리티컬이면 일반공격
    //    {

    //        monCtrl.OnDamage(att);        //넉백은 없이
    //        Managers.Resource.ResourceEffectAndSound(monTarget.transform.position, warriorHitSound, warriorHitEff);

    //    }
    //}

    //public override void CriticalAttack(Tower monPortal, string soundPath,string criticlaSoundPath, string hitPath)
    //{
    //    if (CriticalCheck())//true면 크리티컬데미지 false면 일반데미지
    //    {
    //        int attack = att * 2;
    //        monPortal.TowerDamage(attack);      //크리티컬이면 데미지2배 타워는 2배만
    //        Managers.Resource.ResourceEffectAndSound(monPortal.transform.position,  warriorCriticalSound, warriorHitEff);

    //    }
    //    else  //노크리티컬이면 일반공격
    //    {
    //        monPortal.TowerDamage(att);        //넉백은 없이
    //        Managers.Resource.ResourceEffectAndSound(monPortal.transform.position, warriorHitSound, warriorHitEff);

    //    }
    //}



    #region 넉백
    void ApplyKnockBack(Vector2 dir, float force)
    {
        if(transform.position.x < spawnPosX)
        {
            SetUnitState(SpecialUnitState.Idle);
            attackCoolTime = 1.0f;

            return;

        }

        if (!knockbackStart)
        {
            dir.y = 0;
            knockbackStart = true;
            StartCoroutine(RestoreGravityAfterKnockback(force));

        }
    }


    IEnumerator RestoreGravityAfterKnockback(float force)
    {
        WaitForSeconds wfs = new WaitForSeconds(knockbackDuration);
        float knockBackSpeed = 0.0f;
        float knockBackAccleration = force * 0.5f;            //힘

        float knockbackTime = 0.0f;
        float maxKnockBackTime = 0.3f;

        while (knockbackTime < maxKnockBackTime)  //속도 증가
        {
            knockBackSpeed += knockBackAccleration * Time.fixedDeltaTime;
            rigbody.velocity = new Vector2(-1, 0) * knockBackSpeed;
            knockbackTime += Time.fixedDeltaTime;
            yield return null;
        }

        //knockBackAccleration = 10.0f;

        while (knockBackSpeed > 0.0f)   //속도 감소
        {
            knockBackSpeed -= (knockBackAccleration * 0.5f) * Time.fixedDeltaTime;

            Vector2 velo = new Vector2(-knockBackSpeed, rigbody.velocity.y);
            rigbody.velocity = velo;
            yield return null;
        }
        //rigbody.velocity *= 0.5f;

        yield return wfs; // 넉백 지속 시간

        SetUnitState(SpecialUnitState.Idle);
        attackCoolTime = 0.5f;
        knockbackStart = false;



    }

    #endregion

    void Trace<T>(T obj) where T : UnityEngine.Component
    {
        Vector3 vec = obj.gameObject.transform.position - this.transform.position;
        traceDistance = vec.sqrMagnitude;
        Vector3 dir = vec.normalized;

        if (traceDistance < Mathf.Pow(attackRange, 2))
        {
            SetUnitState(SpecialUnitState.Attack);
        }
        else
        {
            rigbody.transform.position += dir * moveSpeed * Time.fixedDeltaTime;
            SetUnitState(SpecialUnitState.Trace);
        }

        //if (unitClass == UnitClass.Magician)
        //{
        //    if (traceDistance < attackRange * attackRange)
        //    {
        //        SetUnitState(SpecialUnitState.Attack);
        //    }
        //    else
        //    {
        //        rigbody.transform.position += dir * moveSpeed * Time.fixedDeltaTime;
        //        SetUnitState(SpecialUnitState.Trace);
        //    }

        //}

        //else if (unitClass == UnitClass.Cavalry)
        //{
        //    if (traceDistance < attackRange * attackRange)
        //    {

        //        SetUnitState(SpecialUnitState.Attack);
        //    }
        //    else
        //    {
        //        rigbody.transform.position += dir * moveSpeed * Time.fixedDeltaTime;
        //        SetUnitState(SpecialUnitState.Trace);
        //    }

        //}

    }

    public override void AttackDelay()
    {
        if (state == SpecialUnitState.Die)
            return;

        if (attackCoolTime > 0.0f)
        {
            attackCoolTime -= Time.fixedDeltaTime;
            if (attackCoolTime <= .0f)
            {
                if(monTarget != null)
                {
                    Vector3 vec = monTarget.gameObject.transform.position - this.transform.position;
                    traceDistance = vec.sqrMagnitude;
                    if (traceDistance < attackRange * attackRange)
                    {
                        SetUnitState(SpecialUnitState.Attack);
                    }
                    else
                        SetUnitState(SpecialUnitState.Run);
                }
                else if (monsterPortal != null)
                {
                    Vector3 vec = monsterPortal.gameObject.transform.position - this.transform.position;
                    traceDistance = vec.sqrMagnitude;

                    if (traceDistance < attackRange * attackRange)
                        SetUnitState(SpecialUnitState.Attack);
                    else
                        SetUnitState(SpecialUnitState.Run);
                }
                else
                    SetUnitState(SpecialUnitState.Run);





            }

        }
    }

    

    protected IEnumerator UnitSkillCoolTime(float coolTime)
    {
        float cool = coolTime;

        while(true)
        {
            if(!skillOn)     //스킬이 안돌았다면
            {
                cool -= Time.deltaTime;

                if (cool <= 0.0f)        //0보다 작아지면
                {
                    cool = 0.0f;
                    skillOn = true;      //스킬 사용가능!  스킬사용하면 다시 false로

                    yield break;        //쿨타임이 다돌면 코루틴 종료
                }

            }


            yield return null;

        }


    }

    //public void SpeechchBubbleOn(string speechTitleKey, string speechSubKey, int probaility)
    //{
    //    speechBubble.SpeechBubbuleOn(speechTitleKey, speechSubKey, probaility);


    //}

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(pos.position, boxSize);
    }

}
