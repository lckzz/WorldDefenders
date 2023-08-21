using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.Burst.CompilerServices;
using UnityEngine;
using static Define;

enum MonsterClass
{
    Warrior,
    Archer
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

public class MonsterController : Unit
{
    [SerializeField]
    private Animator anim;
    [SerializeField]
    private MonsterClass monsterClass;
    [SerializeField]
    private MonsterState state = MonsterState.Run;        //테스트용이라 런 



    UnitController[] unitCtrls;
    UnitController unitTarget;
    PlayerTower playerTowerCtrl;

    Rigidbody2D rigbody;

    Collider2D monsterColl;     //죽었을 때 콜라이더 끄기위한 변수

    //아처 전용
    Transform arrowPos;

    //아처 전용

    MonsterStat monStat;






    public bool IsDie { get { return isDie; } }
    public float Hp { get { return hp; } }
    public int Att { get { return att; } }
    public int KnockBackForce { get { return knockbackForce; } }



    public UnitController UnitCtrl { get { return unitTarget; } }
    public PlayerTower PlayerTowerCtrl { get { return playerTowerCtrl; } }
    public MonsterState MonState { get { return state; } }




    void Init()
    {
        monStat = new MonsterStat();

        if (monsterClass == MonsterClass.Warrior)
            monStat = Managers.Data.normalSkeleton[GlobalData.g_NormalSkeletonID];
        else if (monsterClass == MonsterClass.Archer)
            monStat = Managers.Data.bowSkeleton[GlobalData.g_BowSkeletonID];


        att = monStat.att;
        hp = monStat.hp;
        knockbackForce = monStat.knockBackForce;
        moveSpeed = 2.0f;
        archerAttDis = 5.0f;
        maxHp = hp;

        playerTowerCtrl = GameObject.FindObjectOfType<PlayerTower>();

        TryGetComponent<Animator>(out anim);
        TryGetComponent<Collider2D>(out monsterColl);
        TryGetComponent<Rigidbody2D>(out rigbody);

        if (monsterClass == MonsterClass.Archer)
            arrowPos = transform.Find("ArrowPos");
        else
            arrowPos = null;


        SetMonsterState(MonsterState.Run);

    }

    // Start is called before the first frame update
    void Start()
    {
        Init();

    }

    // Update is called once per frame
    void Update()
    {

        if (Managers.Game.State == GameState.GamePlaying)
        {
            EnemySensor();
            TowerSensor();
            MonsterStateCheck();

        }

        MonsterVictory();

    }



    public float hpPercent()
    {
        return hp / maxHp;
    }

    public void OnDamage(int att,int knockBack = 0)
    {

        if(hp > 0)
        {
            hp -= att;

            if(knockBack > 0)
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





    public override void EnemySensor()      //적감지
    {
        //Debug.Log(isTargeting);
        //Debug.Log($"타겟팅{isTageting}");
        #region 타겟구현

        coll2d = Physics2D.OverlapBoxAll(pos.position, boxSize, 0, LayerMask.GetMask("Unit"));

        
        if (coll2d != null)
        {

            if (coll2d.Length <= 0)
            {
                //박스안 콜라이더가 아무것도 없으면
                if (unitTarget != null)  //이전에 몬스터 타겟팅이 잡혓더라면
                {
                    unitTarget = null;
                    return;
                }
            }

            unitCtrls = new UnitController[coll2d.Length];
            //체크박스안에 들어온 콜라이더중에서 현재 유닛과의 거리가 제일 가까운 것을 골라내기
            for (int ii = 0; ii < coll2d.Length; ii++)
                coll2d[ii].TryGetComponent<UnitController>(out unitCtrls[ii]);


        }

        if (unitCtrls.Length > 0)
        {
            float disMin = 0;
            int min = 0;


            if (unitCtrls.Length > 1)
            {
                for (int i = 0; i < unitCtrls.Length; i++)
                {
                    if (i == 0 && unitCtrls.Length > 1)
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

                    else if (i < unitCtrls.Length - 1)
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


            if (unitCtrls.Length != 0)
            {
                unitTarget = unitCtrls[min];
            }


        }





        #endregion

    }

    void TowerSensor()
    {
        //타워를 최우선적으로 타격하고 거리를 계속해서 계산해서 일정거리안에 들어오면 타워 공격

        towerVec = playerTowerCtrl.gameObject.transform.position - this.transform.position;
        towerDist = towerVec.sqrMagnitude;
        towerDir = towerVec.normalized;

        if (towerDist < 15.0f * 15.0f)
        {
            if (!towerTrace)
            {
                towerTrace = true;
                SetMonsterState(MonsterState.Trace);

            }

            if (monsterClass == MonsterClass.Warrior)
            {

                TowerAttackRange(1.5f);
            }

            else if (monsterClass == MonsterClass.Archer)
            {
                TowerAttackRange(6.5f);
            }
        }
        else
        {
            if (towerTrace)
            {
                towerTrace = false;

            }
        }
    }

    void TowerAttackRange(float distance)
    {

        if (towerDist < distance * distance)
        {
            if (!towerAttack)
            {
                towerAttack = true;
                SetMonsterState(MonsterState.Attack);
            }
        }
        else
        {

            if (towerAttack)
            {
                towerAttack = false;
            }
        }
    }


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


    void SetMonsterState(MonsterState state)
    {
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
        if (unitTarget != null)
            SetMonsterState(MonsterState.Trace);

        if (unitTarget == null)
            if (towerTrace)
                towerTrace = false;


        if (IsTargetOn())
            return;


        rigbody.transform.position += Vector3.left * moveSpeed * Time.deltaTime;




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

        else if (unitTarget == null)
            Trace(playerTowerCtrl);
    }

    bool IsTargetOn()
    {


        
        if (unitTarget == null && towerTrace == false)
            return false;


        if(unitTarget != null)
        {
            if (unitTarget.UniState == UnitState.Die)
                return false;

            if (!unitTarget.gameObject.activeInHierarchy)
                return false;
        }



        return true;
    }


    void UnitAttack()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {

            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)
            {

                attackCoolTime = 1.0f;
                SetMonsterState(MonsterState.Idle);
                if (towerAttack)
                    towerAttack = false;
            }


        }
    }


    void MonsterDie()
    {
        if (monsterColl.enabled)
        {
            state = MonsterState.Die;
            monsterColl.enabled = false;
            GameObject.Destroy(gameObject, 5.0f);

        }
    }


    public void OnAttack()
    {

        if (monsterClass == MonsterClass.Warrior)
        {

            if (!towerAttack)
            {

                if (unitTarget != null)
                {
                    unitTarget.OnDamage(att, knockbackForce);
                    Managers.Sound.Play("Sounds/Effect/WarriorAttack");
                    GameObject eff = Managers.Resource.Load<GameObject>("Prefabs/Effect/HitEff");
                    if (eff != null)
                        Instantiate(eff, unitTarget.transform.position, Quaternion.identity);

                }
            }
            else
            {
                playerTowerCtrl.TowerDamage(att);
                Managers.Sound.Play("Sounds/Effect/WarriorAttack");
                GameObject eff = Managers.Resource.Load<GameObject>("Prefabs/Effect/HitEff");
                if (eff != null)
                    Instantiate(eff, playerTowerCtrl.transform.position, Quaternion.identity);
            }

        }

        else if (monsterClass == MonsterClass.Archer)
        {
            if (!towerAttack)
            {
                if (unitTarget != null)
                {
                    GameObject obj = Resources.Load<GameObject>("Prefabs/Weapon/MonsterArrow");

                    if (obj != null)
                    {
                        Managers.Sound.Play("Sounds/Effect/Bow");
                        GameObject arrow = Instantiate(obj, arrowPos.position, Quaternion.identity, this.transform);
                        arrow.TryGetComponent(out MonsterArrowCtrl arrowCtrl);
                        arrowCtrl.SetType(unitTarget, null);
                    }
                }
            }
            else
            {
                GameObject obj = Resources.Load<GameObject>("Prefabs/Weapon/MonsterArrow");

                if (obj != null)
                {
                    Managers.Sound.Play("Sounds/Effect/Bow");
                    GameObject arrow = Instantiate(obj, arrowPos.position, Quaternion.identity, this.transform);
                    arrow.TryGetComponent(out MonsterArrowCtrl arrowCtrl);
                    arrowCtrl.SetType(null, playerTowerCtrl);
                }
            }

        }
    }


    void ApplyKnockBack(Vector2 dir, float force)
    {
        dir.y = 0;

        rigbody.gravityScale = 0.0f;
        rigbody.velocity = Vector2.zero;
        rigbody.AddForce(dir * force, ForceMode2D.Impulse);
        

        StartCoroutine(RestoreGravityAfterKnockback());
    }


    float knockbackDuration = 0.5f;
    IEnumerator RestoreGravityAfterKnockback()
    {
        yield return new WaitForSeconds(knockbackDuration); // 넉백 지속 시간


        while (rigbody.velocity.magnitude > 0.1f)
        {
            rigbody.velocity *= 0.96f;
            yield return null;
        }
        //rigbody.velocity *= 0.5f;


        SetMonsterState(MonsterState.Run);
    }



    void Trace<T>(T obj) where T : UnityEngine.Component
    {

        Vector3 vec = obj.gameObject.transform.position - this.transform.position;
        float distance = vec.magnitude;
        Vector3 dir = vec.normalized;
        if (monsterClass == MonsterClass.Archer)
        {

            if (distance < archerAttDis)
            {
                Debug.Log("asdsadsadsadasdwqeqe");
                SetMonsterState(MonsterState.Attack);
            }
            else
            {
                rigbody.transform.position += dir * moveSpeed * Time.deltaTime;
                SetMonsterState(MonsterState.Trace);
            }

        }
        else if (monsterClass == MonsterClass.Warrior)
        {

            if (distance < 1.5f)
            {
                SetMonsterState(MonsterState.Attack);
            }
            else
            {
                rigbody.transform.position += dir * moveSpeed * Time.deltaTime;
                SetMonsterState(MonsterState.Trace);

            }

        }







    }


    void MonsterVictory()
    {
        //Managers.Game.state = GameState.GameFail;


        if(Managers.Game.State == GameState.GameFail)
        {
            isRun = false;
            anim.SetBool("Run", isRun);
            isAtt = false;
            anim.SetBool("Attack", isAtt);
            isTargeting = false;

            state = MonsterState.Idle;

            transform.position = transform.position;

        }
    }


    public override void AttackDelay()
    {
        Debug.Log("테으슽");
        if (attackCoolTime > 0.0f)
        {
            attackCoolTime -= Time.deltaTime;
            if (attackCoolTime <= .0f)
            {
                SetMonsterState(MonsterState.Run);
            }

        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(pos.position, boxSize);
    }
}
