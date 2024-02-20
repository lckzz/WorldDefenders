using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.UI.CanvasScaler;


public enum UnitClass
{
    Warrior,
    Archer,
    Spear,
    Priest,
    Magician,
    Cavalry,
    Count
}


public enum UnitState
{
    Idle,
    Run,
    Trace,
    Attack,
    KnockBack,
    Die
}

public abstract class UnitController : UnitBase
{


    [SerializeField]
    protected UnitState state = UnitState.Run;        //테스트용이라 런 

    public int KnockBackForce { get { return knockbackForce; } }

    public UnitState UniState { get { return state; } }


    protected readonly int appearProbability = 25;       //25프로확률로 등장하면서 말풍선
    protected readonly int dieProbability = 25;

    public override void Init()
    {
        base.Init();
        spawnPosX = -9.2f;

    }


    private void OnDisable()
    {
  
    }

    public override void OnEnable()
    {

        if (myColl != null)
        {

            isDie = false;
            isRun = false;
            //오브젝트 풀에서 생성되면 초기화 시켜줘야함

            hp = maxHp;


            SetUnitState(UnitState.Run);


            myColl.enabled = true;
            monTarget = null;

        }

    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if (Managers.Game.GameEndResult())       //게임이 끝났으면 리턴
            return;


        EnemySensor();
        UnitStateCheck();


    }




    public override void EnemySensor()      //적감지
    {

        #region 타겟구현

        UnitSense();
        UnitDistanceAsending();




#endregion

    }

    //유닛들을 감지


    void UnitStateCheck()
    {
        switch (state)
        {
            case UnitState.Idle:
                {
                    UnitIdle();
                    break;
                }
            case UnitState.Run:
                {
                    UnitMove();
                    break;
                }
            case UnitState.Trace:
                {
                    UnitTrace();
                    break;
                }
            case UnitState.Attack:
                {
                    UnitAttack();
                    break;
                }
            case UnitState.KnockBack:
                {
                    ApplyKnockBack(new Vector2(-1.0f,1.0f),damageKnockBack);
                    break;
                }
            case UnitState.Die:
                {
                    UnitDie();
                    break;
                }

        }

    }


    protected void SetUnitState(UnitState state)
    {
        if (isDie)
            return;

        this.state = state;

        switch(this.state)
        {
            case UnitState.Idle:
                {
                    if (isAtt)
                    {
                        isAtt = false;
                        anim.SetBool("Attack", isAtt);
                    }
                    if(isRun)
                    {
                        isRun = false;
                        anim.SetBool("Run", isRun);
                    }

  
                    break;
                }
            case UnitState.Run:
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
                    break;
                }
            case UnitState.Attack:
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
                    break;
                }
            case UnitState.KnockBack:
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
                    break;
                }
            case UnitState.Die:
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


    protected virtual void UnitMove()
    {
        if (monTarget != null || monsterPortal != null)
            SetUnitState(UnitState.Trace);

        if (IsTargetOn())
            return;


        rigbody.transform.position += Vector3.right * moveSpeed * Time.fixedDeltaTime;




    }


    protected virtual void UnitTrace()
    {
        if (!IsTargetOn())
        {
            SetUnitState(UnitState.Run);
            return;
        }


        if (monTarget != null)
            Trace(monTarget);

        else if(monTarget == null)
            Trace(monsterPortal);
    }




    void UnitAttack()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)
            {

                SetUnitState(UnitState.Idle);

                attackCoolTime = 1.0f;


            }


        }
    }

    void UnitDie()
    {
        if (myColl.enabled)
        {
            speechBubble.SpeechBubbleOn(dieTitleKey, dieDialogSubKey, dieProbability);
            SetUnitState(UnitState.Die);
            myColl.enabled = false;
            StartCoroutine(DestroyTime(gameObject, 3.0f));
            onDead?.Invoke();
        }
    }






   

    public override void OnDamage(int att,int knockBack = 0, bool criticalCheck = false)
    {


        if (hp > 0)
        {
            hp -= att;
            NotifyToHpObserver();       //체력이 바뀌어서 옵저버들에게 체력이 바꼇다는걸 알리고 보내기

            //넉백이 안통하는 존에 있다면 넉백수치를 0으로 만들어준다.
            if (NoKnockBackValid())
                knockBack = 0;


            if (att > 0)
            {
                if (criticalCheck)
                    unitHUDHp?.SpawnHUDText(att.ToString(), (int)Define.UnitDamageType.Critical);
                else
                    unitHUDHp?.SpawnHUDText(att.ToString(), (int)Define.UnitDamageType.Enemy);
            }



            if (knockBack > 0)
            {
                SetUnitState(UnitState.KnockBack);
                damageKnockBack = knockBack;
            }


            if (hp <= 0)
            {
                hp = 0;
                SetUnitState(UnitState.Die);
            }
        }
    }










    //public override bool CriticalCheck()
    //{
    //    //유닛공격력을 받아서 크리티컬확률을 받아서 확률에 맞으면 크리공격
    //    //아니면 일반 공격
    //    int rand = UnityEngine.Random.Range(0, 101);
    //    if (rand <= unitStat.criticalRate)
    //        return true;

    //    return false;


    //}


    //public override void CriticalAttack(Unit monCtrl,string soundPath, string criticalSoundPath, string hitPath)
    //{
    //    if (CriticalCheck())//true면 크리티컬데미지 false면 일반데미지
    //    {
    //        int attack = att * 2;
    //        monCtrl.OnDamage(attack, unitStat.knockBackForce,true);      //크리티컬이면 데미지2배에 넉백까지
    //        Managers.Resource.ResourceEffectAndSound(monTarget.transform.position, criticalSoundPath, hitPath);

    //    }
    //    else  //노크리티컬이면 일반공격
    //    {
            
    //        monCtrl.OnDamage(att);        //넉백은 없이
    //        Managers.Resource.ResourceEffectAndSound(monTarget.transform.position, soundPath, hitPath);

    //    }
    //}

    //public override void CriticalAttack(Tower monPortal, string soundPath, string criticalSoundPath, string hitPath)
    //{
    //    if (CriticalCheck())//true면 크리티컬데미지 false면 일반데미지
    //    {
    //        int attack = att * 2;
    //        monPortal.TowerDamage(attack);      //크리티컬이면 데미지2배 타워는 2배만
    //        Managers.Resource.ResourceEffectAndSound(monPortal.transform.position, criticalSoundPath, hitPath);

    //    }
    //    else  //노크리티컬이면 일반공격
    //    {
    //        monPortal.TowerDamage(att);        //넉백은 없이
    //        Managers.Resource.ResourceEffectAndSound(monPortal.transform.position, soundPath, hitPath);

    //    }
    //}



    #region 넉백
    void ApplyKnockBack(Vector2 dir , float force)
    {
        if(transform.position.x < spawnPosX)        //스폰지점보다 밑에 있으면
        {
            SetUnitState(UnitState.Idle); //넉백당하지않음
            attackCoolTime = 1.0f;

            return;
        }


        if (!knockbackStart)
        {
            dir.y = 0;
            knockbackStart = true;
            if (knockBackCo != null)
                StopCoroutine(knockBackCo);

            knockBackCo = StartCoroutine(RestoreGravityAfterKnockback(force));

        }

        
    }


    IEnumerator RestoreGravityAfterKnockback(float force)
    {
        WaitForSeconds wfs = new WaitForSeconds(knockbackDuration);
        float knockBackSpeed = 0.0f;
        float knockBackAccleration = force;            //힘

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

        SetUnitState(UnitState.Idle);
        attackCoolTime = 0.5f;

        knockbackStart = false;



    }

    #endregion

    protected void Trace<T>(T obj) where T : UnityEngine.Component 
    {

        Vector3 vec = obj.gameObject.transform.position - this.transform.position;
        traceDistance = vec.sqrMagnitude;
        Vector3 dir = vec.normalized;

        if (traceDistance < Mathf.Pow(attackRange,2))
        {
            SetUnitState(UnitState.Attack);
        }
        else
        {
            rigbody.transform.position += dir * moveSpeed * Time.fixedDeltaTime;
            SetUnitState(UnitState.Trace);

        }


    }



    public override void AttackDelay()
    {
        if (state == UnitState.Die)
            return;

        if (attackCoolTime > 0.0f)
        {
            attackCoolTime -= Time.fixedDeltaTime;
            if (attackCoolTime <= .0f)
            {
                if (monTarget != null)
                {
                    Vector3 vec = monTarget.gameObject.transform.position - this.transform.position;
                    traceDistance = vec.sqrMagnitude;


                    if (traceDistance < Mathf.Pow(attackRange, 2))
                        SetUnitState(UnitState.Attack);
                    else
                        SetUnitState(UnitState.Run);
                }
                else if(monsterPortal != null)
                {

                    Vector3 vec = monsterPortal.gameObject.transform.position - this.transform.position;
                    traceDistance = vec.sqrMagnitude;

                    if (traceDistance < Mathf.Pow(attackRange, 2))
                        SetUnitState(UnitState.Attack);
                    else
                        SetUnitState(UnitState.Run);
                }
                else
                    SetUnitState(UnitState.Run);

            }

        }
    }


    //public void AddObserver(IHpObserver observer)
    //{

    //}

    //public void RemoveObserver(IHpObserver observer)
    //{

    //}

    //public void NotifyToObserver(IHpObserver observer)
    //{

    //}


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(pos.position, boxSize);
    }
}
