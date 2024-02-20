using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.Burst.CompilerServices;
using UnityEditor;
using UnityEngine;
using static Define;

public enum MonsterClass
{
    Warrior,
    Archer,
    Spear,
    EliteWarrior,
    EliteCavalry,
    EliteShaman,
    SkeletonKing,
    Count
}

public enum MonsterState
{
    Idle,
    Run,
    Trace,
    Attack,
    KnockBack,
    Die
}

public class MonsterController : MonsterBase
{

    [SerializeField]
    private MonsterState state = MonsterState.Run;        //테스트용이라 런 

    public int DropGold { get ; protected set; }
    //public int DropCost { get; protected set; }

    public int KnockBackForce { get { return knockbackForce; } }

    public MonsterState MonState { get { return state; } }



    protected readonly string appearTitleKey = "monsterAppearDialog";
    protected readonly string dieTitleKey = "monsterDieDialog";
    private readonly string dieDialogSubKey = "monsterDie";

    protected readonly int appearProbability = 25;       //25프로확률로 등장하면서 말풍선
    protected readonly int dieProbability = 25;


    protected Dictionary<Stage, Dictionary<MonsterClass, MonsterStat>> monsterStatDict;


    public override void Init()
    {
        base.Init();

        monsterStatDict = new Dictionary<Stage, Dictionary<MonsterClass, MonsterStat>>
        {
            { Stage.West, new Dictionary<MonsterClass, MonsterStat>
            {
                { MonsterClass.Warrior,Managers.Data.monsterDict[Managers.Game.MonsterTypeIdDict[MonsterType.NormalSkeleton]] },
                { MonsterClass.Archer,Managers.Data.monsterDict[Managers.Game.MonsterTypeIdDict[MonsterType.NormalBowSkeleton]] },
                { MonsterClass.Spear,Managers.Data.monsterDict[Managers.Game.MonsterTypeIdDict[MonsterType.SpearSkeleton]] },

            } },
            { Stage.South, new Dictionary<MonsterClass, MonsterStat>
            {
                { MonsterClass.Warrior,Managers.Data.monsterDict[Managers.Game.MonsterTypeIdDict[MonsterType.MidSkeleton]] },
                { MonsterClass.Archer,Managers.Data.monsterDict[Managers.Game.MonsterTypeIdDict[MonsterType.MidBowSkeleton]] },
                { MonsterClass.Spear,Managers.Data.monsterDict[Managers.Game.MonsterTypeIdDict[MonsterType.MidSpearSkeleton]] },

            } },
            { Stage.East, new Dictionary<MonsterClass, MonsterStat>
            {
                { MonsterClass.Warrior,Managers.Data.monsterDict[Managers.Game.MonsterTypeIdDict[MonsterType.HighSkeleton]] },
                { MonsterClass.Archer,Managers.Data.monsterDict[Managers.Game.MonsterTypeIdDict[MonsterType.HighBowSkeleton]] },
                { MonsterClass.Spear,Managers.Data.monsterDict[Managers.Game.MonsterTypeIdDict[MonsterType.HighSpearSkeleton]] },

            } },
            { Stage.Boss, new Dictionary<MonsterClass, MonsterStat>
            {
                { MonsterClass.Warrior,Managers.Data.monsterDict[Managers.Game.MonsterTypeIdDict[MonsterType.HighSkeleton]] },
                { MonsterClass.Archer,Managers.Data.monsterDict[Managers.Game.MonsterTypeIdDict[MonsterType.HighBowSkeleton]] },
                { MonsterClass.Spear,Managers.Data.monsterDict[Managers.Game.MonsterTypeIdDict[MonsterType.HighSpearSkeleton]] },

            } }

        };

        monStat = monsterStatDict[Managers.Game.CurStageType][monsterClass];
        att = monStat.att;
        hp = monStat.hp;
        maxHp = hp;
        knockbackForce = monStat.knockBackForce;
        attackRange = monStat.attackRange;
        moveSpeed = 2.0f;
        DropGold = monStat.dropGold;
        DropCost = monStat.dropCost;


        spawnPosX = 18.0f;

    }

    public override void OnEnable()
    {
        base.OnEnable();

        if (myColl != null)
        {
            SetMonsterState(MonsterState.Run);

        }

    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if (Managers.Game.GameEndResult())       //게임이 끝났으면 리턴
            return;

        EnemySensor();
        MonsterStateCheck();

    }

    public override void EnemySensor()      //적감지
    {
        #region 타겟구현

        UnitSense();
        UnitDistanceAsending();

        #endregion

    }

    //유닛들을 감지
    protected override void UnitSense()
    {
        unitCtrls.Clear();
        enemyColls2D = Physics2D.OverlapBoxAll(pos.position, boxSize, 0, LayerMask.GetMask("Unit") | LayerMask.GetMask("SpecialUnit"));


        if (enemyColls2D != null)
        {

            if (enemyColls2D.Length <= 0)
            {
                TowerSensor();   //몬스터가 아무도 없다면 타워센서를 킨다.


                //박스안 콜라이더가 아무것도 없으면
                if (unitTarget != null)  //이전에 몬스터 타겟팅이 잡혓더라면
                {
                    unitTarget = null;
                    return;
                }
            }
            else
            {
                playerTowerCtrl = null;

            }

            //체크박스안에 들어온 콜라이더중에서 현재 유닛과의 거리가 제일 가까운 것을 골라내기
            for (int ii = 0; ii < enemyColls2D.Length; ii++)
            {
                if (enemyColls2D[ii].gameObject.layer == LayerMask.NameToLayer("Unit"))
                {
                    UnitController unitctrl;
                    enemyColls2D[ii].TryGetComponent<UnitController>(out unitctrl);
                    unitCtrls.Add(unitctrl);


                }
                else if (enemyColls2D[ii].gameObject.layer == LayerMask.NameToLayer("SpecialUnit"))
                {
                    SpecialUnitController specialUnit;
                    enemyColls2D[ii].TryGetComponent<SpecialUnitController>(out specialUnit);
                    unitCtrls.Add(specialUnit);


                }
            }


        }
    }
    //감지한 유닛들의 해당 캐릭터와의 거리 오름차순
    protected override void UnitDistanceAsending()
    {
        if (unitCtrls.Count > 0)
        {
            float disMin = 0;
            int min = 0;


            if (unitCtrls.Count > 1)
            {
                for (int i = 0; i < unitCtrls.Count; i++)
                {
                    if (i == 0 && unitCtrls.Count > 1)
                    {
                        float distA = (unitCtrls[i].transform.position - this.transform.position).sqrMagnitude;
                        float distB = (unitCtrls[i + 1].transform.position - this.transform.position).sqrMagnitude;

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

                    else if (i < unitCtrls.Count - 1)
                    {
                        float distB = (unitCtrls[i + 1].transform.position - this.transform.position).sqrMagnitude;

                        if (disMin > distB * distB)
                        {
                            disMin = distB * distB;
                            min = i + 1;
                        }


                    }

                }
            }


            if (unitCtrls.Count != 0)
            {
                unitTarget = unitCtrls[min];
            }


        }


    }


    //void TowerSensor()
    //{
    //    //타워는 유닛이 없다면 그때 감지를하고 공격추격이나 공격을 할 수 있다.

    //    towerColl = Physics2D.OverlapBox(pos.position, boxSize, 0, LayerMask.GetMask("Tower"));


    //    if (towerColl != null)
    //        towerColl.TryGetComponent(out playerTowerCtrl);
        
    //}





    void MonsterStateCheck()
    {
        switch (state)
        {
            case MonsterState.Idle:
                {
                    MonsterIdle();
                    break;
                }
            case MonsterState.Run:
                {
                    MonsterMove();
                    break;
                }
            case MonsterState.Trace:
                {
                    MonsterTrace();
                    break;
                }
            case MonsterState.Attack:
                {
                    

                    UnitAttack();
                    break;
                }
            case MonsterState.KnockBack:
                {
                    ApplyKnockBack(new Vector2(1.0f, 1.0f), damageKnockBack);
                    break;
                }
            case MonsterState.Die:
                {
                    MonsterDie();
                    break;
                }

        }

    }


    protected void SetMonsterState(MonsterState state)
    {
        if (isDie)
            return;

        this.state = state;
        switch (this.state)
        {
            case MonsterState.Idle:
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
                    break;
                }
            case MonsterState.Run:
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
            case MonsterState.Attack:
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
            case MonsterState.KnockBack:
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
            case MonsterState.Die:
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


    void MonsterIdle()
    {
        AttackDelay();

    }


    void MonsterMove()
    {
        if (unitTarget != null || playerTowerCtrl != null)
            SetMonsterState(MonsterState.Trace);

        if (IsTargetOn())
            return;


        rigbody.transform.position += Vector3.left * moveSpeed * Time.fixedDeltaTime;




    }


    void MonsterTrace()
    {
        if (!IsTargetOn())
        {
            SetMonsterState(MonsterState.Run);
            return;
        }


        if (unitTarget != null)
            Trace(unitTarget);

        else if (playerTowerCtrl != null)
            Trace(playerTowerCtrl);
    }

    bool IsTargetOn()
    {
        if (unitTarget == null && playerTowerCtrl == null)
            return false;


        if(unitTarget != null)
        {
            if (unitTarget.gameObject.layer == LayerMask.NameToLayer("Unit"))
            {
                if(unitTarget is UnitController unitCtrl)
                {
                    if (unitCtrl.UniState == UnitState.Die)
                        return false;

                    if (!unitTarget.gameObject.activeInHierarchy)
                        return false;
                }

            }
            else if (unitTarget.gameObject.layer == LayerMask.NameToLayer("SpecialUnit"))
            {
                if (unitTarget is SpecialUnitController specialUnit)
                {
                    if (specialUnit.UniState == Define.SpecialUnitState.Die)
                        return false;

                    if (!unitTarget.gameObject.activeInHierarchy)
                        return false;
                }

            }



        }



        return true;
    }


    void UnitAttack()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {

            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)
            {
                SetMonsterState(MonsterState.Idle);

                attackCoolTime = 1.0f;

            }


        }
    }


    void MonsterDie()
    {
        if (myColl.enabled)
        {
            if (unitTarget != null)
                unitTarget = null;
            if (playerTowerCtrl != null)
                playerTowerCtrl = null;

            int randidx = 0;
            randidx = UnityEngine.Random.Range(0, 2);
            if (randidx == 0)
                Managers.Sound.Play("Effect/Monster/MonsterDie1");
            else
                Managers.Sound.Play("Effect/Monster/MonsterDie2");



            Debuff?.DebuffDestory();

            speechBubble.SpeechBubbleOn(dieTitleKey, dieDialogSubKey, dieProbability);

            SetMonsterState(MonsterState.Die);
            myColl.enabled = false;


            dropItem?.Drop(this.gameObject.transform.position);
            StartCoroutine(DestroyTime(gameObject, 3.0f));

            StartCoroutine(MonsterDieDropText());
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


            if(att > 0)
            {
                if (criticalCheck)
                    unitHUDHp?.SpawnHUDText(att.ToString(), (int)Define.UnitDamageType.Critical);
                else
                    unitHUDHp?.SpawnHUDText(att.ToString(), (int)Define.UnitDamageType.Enemy);
            }
  


            if (knockBack > 0)
            {
                SetMonsterState(MonsterState.KnockBack);
                damageKnockBack = knockBack;
            }


            if (hp <= 0)
            {
                hp = 0;
                SetMonsterState(MonsterState.Die);

            }
        }
    }


    public override void OnAttack() { }


    //public override bool CriticalCheck()
    //{
    //    //유닛공격력을 받아서 크리티컬확률을 받아서 확률에 맞으면 크리공격
    //    //아니면 일반 공격
    //    int rand = UnityEngine.Random.Range(0, 101);
    //    if (rand <= monStat.criticalRate)
    //        return true;

    //    return false;


    //}


    //public override void CriticalAttack(Unit uniCtrl, string soundPath, string criticalSoundPath,  string hitPath)
    //{
    //    if (CriticalCheck())//true면 크리티컬데미지 false면 일반데미지
    //    {
    //        int attack = att * 2;
    //        uniCtrl.OnDamage(attack, monStat.knockBackForce,true);      //크리티컬이면 데미지2배에 넉백까지
    //        Managers.Resource.ResourceEffectAndSound(unitTarget.transform.position, criticalSoundPath, hitPath);

    //    }
    //    else  //노크리티컬이면 일반공격
    //    {
    //        uniCtrl.OnDamage(att);        //넉백은 없이
    //        Managers.Resource.ResourceEffectAndSound(unitTarget.transform.position, soundPath, hitPath);

    //    }
    //}

    //public override void CriticalAttack(Tower tower, string soundPath, string criticalSoundPath, string hitPath)
    //{
    //    if (CriticalCheck())//true면 크리티컬데미지 false면 일반데미지
    //    {
    //        int attack = att * 2;
    //        tower.TowerDamage(attack);      //크리티컬이면 데미지2배 타워는 2배만
    //        Managers.Resource.ResourceEffectAndSound(tower.transform.position, criticalSoundPath, hitPath);

    //    }
    //    else  //노크리티컬이면 일반공격
    //    {

    //        tower.TowerDamage(att);        //넉백은 없이
    //        Managers.Resource.ResourceEffectAndSound(tower.transform.position, soundPath, hitPath);

    //    }
    //}



    void ApplyKnockBack(Vector2 dir, float force)
    {
        if(transform.position.x >= spawnPosX)
        {
            SetMonsterState(MonsterState.Idle);
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
        Debug.Log(force);
        float knockBackAccleration = force;            //힘

        float knockbackTime = 0.0f;
        float maxKnockBackTime = 0.3f;

        while (knockbackTime < maxKnockBackTime)  //속도 증가
        {
            knockBackSpeed += knockBackAccleration * Time.fixedDeltaTime;
            rigbody.velocity = new Vector2(1, 0) * knockBackSpeed;
            knockbackTime += Time.fixedDeltaTime;
            yield return null;
        }

        //knockBackAccleration = 10.0f;
        
        while(knockBackSpeed > 0.0f)   //속도 감소
        {

            knockBackSpeed -= (knockBackAccleration * 0.25f) * Time.fixedDeltaTime;

            Vector2 velo = new Vector2(knockBackSpeed, rigbody.velocity.y);
            rigbody.velocity = velo;
            yield return null;
        }
        //rigbody.velocity *= 0.5f;

        yield return wfs; // 넉백 지속 시간

        SetMonsterState(MonsterState.Idle);
        attackCoolTime = 0.5f;

        knockbackStart = false;



    }






    void Trace<T>(T obj) where T : UnityEngine.Component
    {

        Vector2 vec = obj.gameObject.transform.position - this.transform.position;
        float distance = vec.sqrMagnitude;
        Vector3 dir = vec.normalized;

        if (distance < attackRange * attackRange)
        {
            SetMonsterState(MonsterState.Attack);
        }
        else
        {
            rigbody.transform.position += dir * moveSpeed * Time.fixedDeltaTime;
            SetMonsterState(MonsterState.Trace);
        }

    }




    public override void AttackDelay()
    {
        if (state == MonsterState.Die)
            return;


        if (attackCoolTime > 0.0f)
        {
            attackCoolTime -= Time.fixedDeltaTime;
            if (attackCoolTime <= .0f)
            {
                if (unitTarget != null)
                {
                    Vector2 vec = unitTarget.gameObject.transform.position - this.transform.position;
                    traceDistance = vec.sqrMagnitude;

                    if (traceDistance < attackRange * attackRange)
                        SetMonsterState(MonsterState.Attack);
                    else
                        SetMonsterState(MonsterState.Run);
                }
                else if(playerTowerCtrl != null)
                {

                    Vector2 vec = playerTowerCtrl.gameObject.transform.position - this.transform.position;
                    traceDistance = vec.sqrMagnitude;

                    if (traceDistance < attackRange * attackRange)
                        SetMonsterState(MonsterState.Attack);
                    else
                        SetMonsterState(MonsterState.Run);
                }
                else  //타워나 유닛타겟에 아무것도 잡히지 않았다면
                    SetMonsterState(MonsterState.Run);
                


            }

        }
    }


    //public void Notified(int att, float speed)
    //{
    //    this.att = att;
    //    this.moveSpeed = speed;

    //}


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(pos.position, boxSize);
    }


}
