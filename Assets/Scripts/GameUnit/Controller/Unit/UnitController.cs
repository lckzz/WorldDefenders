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
    protected UnitState state = UnitState.Run;        //�׽�Ʈ���̶� �� 

    public int KnockBackForce { get { return knockbackForce; } }

    public UnitState UniState { get { return state; } }


    protected readonly int appearProbability = 25;       //25����Ȯ���� �����ϸ鼭 ��ǳ��
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
            //������Ʈ Ǯ���� �����Ǹ� �ʱ�ȭ ���������

            hp = maxHp;


            SetUnitState(UnitState.Run);


            myColl.enabled = true;
            monTarget = null;

        }

    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if (Managers.Game.GameEndResult())       //������ �������� ����
            return;


        EnemySensor();
        UnitStateCheck();


    }




    public override void EnemySensor()      //������
    {

        #region Ÿ�ٱ���

        UnitSense();
        UnitDistanceAsending();




#endregion

    }

    //���ֵ��� ����


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
            NotifyToHpObserver();       //ü���� �ٲ� �������鿡�� ü���� �ٲ��ٴ°� �˸��� ������

            //�˹��� �����ϴ� ���� �ִٸ� �˹��ġ�� 0���� ������ش�.
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
    //    //���ְ��ݷ��� �޾Ƽ� ũ��Ƽ��Ȯ���� �޾Ƽ� Ȯ���� ������ ũ������
    //    //�ƴϸ� �Ϲ� ����
    //    int rand = UnityEngine.Random.Range(0, 101);
    //    if (rand <= unitStat.criticalRate)
    //        return true;

    //    return false;


    //}


    //public override void CriticalAttack(Unit monCtrl,string soundPath, string criticalSoundPath, string hitPath)
    //{
    //    if (CriticalCheck())//true�� ũ��Ƽ�õ����� false�� �Ϲݵ�����
    //    {
    //        int attack = att * 2;
    //        monCtrl.OnDamage(attack, unitStat.knockBackForce,true);      //ũ��Ƽ���̸� ������2�迡 �˹����
    //        Managers.Resource.ResourceEffectAndSound(monTarget.transform.position, criticalSoundPath, hitPath);

    //    }
    //    else  //��ũ��Ƽ���̸� �Ϲݰ���
    //    {
            
    //        monCtrl.OnDamage(att);        //�˹��� ����
    //        Managers.Resource.ResourceEffectAndSound(monTarget.transform.position, soundPath, hitPath);

    //    }
    //}

    //public override void CriticalAttack(Tower monPortal, string soundPath, string criticalSoundPath, string hitPath)
    //{
    //    if (CriticalCheck())//true�� ũ��Ƽ�õ����� false�� �Ϲݵ�����
    //    {
    //        int attack = att * 2;
    //        monPortal.TowerDamage(attack);      //ũ��Ƽ���̸� ������2�� Ÿ���� 2�踸
    //        Managers.Resource.ResourceEffectAndSound(monPortal.transform.position, criticalSoundPath, hitPath);

    //    }
    //    else  //��ũ��Ƽ���̸� �Ϲݰ���
    //    {
    //        monPortal.TowerDamage(att);        //�˹��� ����
    //        Managers.Resource.ResourceEffectAndSound(monPortal.transform.position, soundPath, hitPath);

    //    }
    //}



    #region �˹�
    void ApplyKnockBack(Vector2 dir , float force)
    {
        if(transform.position.x < spawnPosX)        //������������ �ؿ� ������
        {
            SetUnitState(UnitState.Idle); //�˹����������
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
        float knockBackAccleration = force;            //��

        float knockbackTime = 0.0f;
        float maxKnockBackTime = 0.3f;

        while (knockbackTime < maxKnockBackTime)  //�ӵ� ����
        {
            knockBackSpeed += knockBackAccleration * Time.fixedDeltaTime;
            rigbody.velocity = new Vector2(-1, 0) * knockBackSpeed;
            knockbackTime += Time.fixedDeltaTime;
            yield return null;
        }

        //knockBackAccleration = 10.0f;

        while (knockBackSpeed > 0.0f)   //�ӵ� ����
        {
            knockBackSpeed -= (knockBackAccleration * 0.5f) * Time.fixedDeltaTime;

            Vector2 velo = new Vector2(-knockBackSpeed, rigbody.velocity.y);
            rigbody.velocity = velo;
            yield return null;
        }
        //rigbody.velocity *= 0.5f;

        yield return wfs; // �˹� ���� �ð�

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
