using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.Burst.CompilerServices;
using UnityEditor;
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
    [SerializeField]
    UnitController unitTarget;
    [SerializeField]
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
        attackRange = monStat.attackRange;
        moveSpeed = 2.0f;
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

    public void OnDamage(int att, int knockBack = 0)
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
                TowerAttackRange(1.75f);
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
                Debug.Log("공격끄읏");
                SetMonsterState(MonsterState.Idle);

                attackCoolTime = 1.0f;

            }


        }
    }


    void MonsterDie()
    {
        if (monsterColl.enabled)
        {
            SetMonsterState(MonsterState.Die);
            monsterColl.enabled = false;
            GameObject.Destroy(gameObject, 3.0f);

        }
    }


    public void OnAttack()
    {

        if (monsterClass == MonsterClass.Warrior)
        {

            if (!towerAttack)
            {

                if (unitTarget != null)
                    CriticalAttack(unitTarget);
                
            }
            else
                CriticalAttack(playerTowerCtrl);
            

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

    bool CriticalCheck()
    {
        //유닛공격력을 받아서 크리티컬확률을 받아서 확률에 맞으면 크리공격
        //아니면 일반 공격
        int rand = UnityEngine.Random.Range(0, 101);
        if (rand <= monStat.criticalRate)
            return true;

        return false;


    }


    void CriticalAttack(UnitController uniCtrl)
    {
        if (CriticalCheck())//true면 크리티컬데미지 false면 일반데미지
        {
            Debug.Log("크리티컬!!!!");
            int attack = att * 2;
            uniCtrl.OnDamage(attack, monStat.knockBackForce);      //크리티컬이면 데미지2배에 넉백까지
            UnitEffectAndSound(unitTarget.transform.position, "CriticalSound", "HitEff");

        }
        else  //노크리티컬이면 일반공격
        {
            Debug.Log("일반공격...");

            uniCtrl.OnDamage(att);        //넉백은 없이
            UnitEffectAndSound(unitTarget.transform.position, "WarriorAttack", "HitEff");

        }
    }

    void CriticalAttack(PlayerTower tower)
    {
        if (CriticalCheck())//true면 크리티컬데미지 false면 일반데미지
        {
            int attack = att * 2;
            tower.TowerDamage(attack);      //크리티컬이면 데미지2배 타워는 2배만
            UnitEffectAndSound(tower.transform.position, "CriticalSound", "HitEff");

        }
        else  //노크리티컬이면 일반공격
        {
            tower.TowerDamage(att);        //넉백은 없이
            UnitEffectAndSound(tower.transform.position, "WarriorAttack", "HitEff");

        }
    }


    void UnitEffectAndSound(Vector3 pos, string soundPath, string effPath)
    {
        Managers.Sound.Play($"Sounds/Effect/{soundPath}");
        GameObject eff = Managers.Resource.Load<GameObject>($"Prefabs/Effect/{effPath}");
        Vector2 randomPos = RandomPosSetting(pos);

        if (eff != null)
            Instantiate(eff, randomPos, Quaternion.identity);
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

            if (distance < attackRange)
            {
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

            if (distance < attackRange)
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


    Vector2 RandomPosSetting(Vector3 pos)
    {
        randomX = UnityEngine.Random.Range(-0.5f, 0.5f);
        randomY = UnityEngine.Random.Range(-0.5f, 0.5f);
        Vector2 randomPos = pos;
        randomPos.x += randomX;
        randomPos.y += randomY;

        return randomPos;
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

            state = MonsterState.Idle;

            transform.position = transform.position;

        }
    }


    public override void AttackDelay()
    {
        if (state == MonsterState.Die)
            return;


        if (attackCoolTime > 0.0f)
        {
            attackCoolTime -= Time.deltaTime;
            if (attackCoolTime <= .0f)
            {
                if (towerDist < 1.75f * 1.75f)
                    SetMonsterState(MonsterState.Attack);
                else
                    SetMonsterState(MonsterState.Run);
                if (towerAttack)
                    towerAttack = false;
            }

        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(pos.position, boxSize);
    }
}
