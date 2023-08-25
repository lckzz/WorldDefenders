using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static Define;

public class SpecialUnitController : Unit
{
    [SerializeField]
    private UnitClass unitClass;
    [SerializeField]
    private Define.SpecialUnitState state = Define.SpecialUnitState.Run;

    private bool isSkil = false;

    MonsterController[] monCtrls;  //범위안에 들어온 몬스터의 정보들을 모아둠
    [SerializeField] MonsterController monTarget;  //몬스터들의 정보들중에서 제일 유닛과 가까운 몬스터정보를 받아옴
    [SerializeField] MonsterPortal monsterPortal;

    UnitStat unitStat;

    //마법사 전용
    Transform magicPos;
    Vector3 normalMagicPos = new Vector3(-0.9f, 0.16f, 0.0f);
    //마법사 전용

    public MonsterController Monctrl { get { return monTarget; } }

    public MonsterPortal MonsterPortal { get { return monsterPortal; } }

    public SpecialUnitState UniState { get { return state; } }



    public override void Init()
    {
        base.Init();

        unitStat = new UnitStat();

        if (unitClass == UnitClass.Magician)
            unitStat = Managers.Data.magicDict[GlobalData.g_UnitMagicianLv];
        



        hp = unitStat.hp;
        att = unitStat.att;
        knockbackForce = unitStat.knockBackForce;
        attackRange = unitStat.attackRange;

        moveSpeed = 2.5f;
        maxHp = hp;


        //rig = this.GetComponent<Rigidbody2D>();
        //anim = GetComponent<Animator>();
        monsterPortal = GameObject.FindObjectOfType<MonsterPortal>();

        if (unitClass == UnitClass.Magician)
            magicPos = transform.Find("MagicPos");
        else
            magicPos = null;


        SetUnitState(SpecialUnitState.Run);

    }

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        TowerSensor();
        EnemySensor();
        UnitStateCheck();
    }

    public override void EnemySensor()      //적감지
    {
        //Debug.Log(isTargeting);
        //Debug.Log($"타겟팅{isTageting}");
        #region 타겟구현

        enemyColls2D = Physics2D.OverlapBoxAll(pos.position, boxSize, 0, LayerMask.GetMask("Monster"));
        if (enemyColls2D != null)
        {
            Debug.Log(enemyColls2D.Length);
            if (enemyColls2D.Length <= 0)
            {
                //박스안 콜라이더가 아무것도 없으면
                if (monTarget != null)  //이전에 몬스터 타겟팅이 잡혓더라면
                {
                    monTarget = null;
                    return;
                }
            }

            monCtrls = new MonsterController[enemyColls2D.Length];
            //체크박스안에 들어온 콜라이더중에서 현재 유닛과의 거리가 제일 가까운 것을 골라내기
            for (int ii = 0; ii < enemyColls2D.Length; ii++)
                enemyColls2D[ii].TryGetComponent<MonsterController>(out monCtrls[ii]);


        }


        if (monCtrls.Length > 0)
        {
            float disMin = 0;
            int min = 0;


            if (monCtrls.Length > 1)
            {
                for (int i = 0; i < monCtrls.Length; i++)
                {
                    if (i == 0 && monCtrls.Length > 1)
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

                    else if (i < monCtrls.Length - 1)
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


            if (monCtrls.Length != 0)
            {
                monTarget = monCtrls[min];
            }


        }





        #endregion

    }


    void TowerSensor()
    {
        //타워를 최우선적으로 타격하고 거리를 계속해서 계산해서 일정거리안에 들어오면 타워 공격
        if (monsterPortal == null)
            return;

        towerVec = monsterPortal.gameObject.transform.position - this.transform.position;
        towerDist = towerVec.sqrMagnitude;
        towerDir = towerVec.normalized;

        if (towerDist < 15.0f * 15.0f)
        {
            if (!towerTrace)
            {
                towerTrace = true;
                SetUnitState(SpecialUnitState.Trace);


            }

            if (unitClass == UnitClass.Magician)
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
                SetUnitState(SpecialUnitState.Attack);
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

    void UnitStateCheck()
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

    void SetUnitState(SpecialUnitState state)
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
                        anim.SetBool("SkillAttack", isAtt);
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
                        anim.SetBool("SkillAttack", isAtt);
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
                        anim.SetBool("SkillAttack", isAtt);
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
                        anim.SetBool("SkillAttack", isAtt);
                    }
                    break;
                }
            case SpecialUnitState.KnockBack:
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
                        anim.SetBool("SkillAttack", isAtt);
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
        if (monTarget != null)
            SetUnitState(SpecialUnitState.Trace);

        if (monTarget == null)
            if (towerTrace)
                towerTrace = false;

        if (IsTargetOn())
            return;


        rigbody.transform.position += Vector3.right * moveSpeed * Time.deltaTime;




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

        else if (monTarget == null)
            Trace(monsterPortal);
    }

    bool IsTargetOn()
    {
        if (monTarget == null && towerTrace == false)
            return false;


        if (monTarget != null)
        {
            if (monTarget.MonState == MonsterState.Die)
                return false;

            if (!monTarget.gameObject.activeInHierarchy)
                return false;
        }



        return true;
    }

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

    void UnitDie()
    {
        if (myColl.enabled)
        {
            SetUnitState(SpecialUnitState.Die);
            myColl.enabled = false;
            GameObject.Destroy(gameObject, 5.0f);

        }
    }


    public override void OnDamage(int att, int knockBack = 0)
    {

        if (hp > 0)
        {
            hp -= att;
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







    public override void OnAttack()    //애니메이션 이벤트 함수
    {
        if (unitClass == UnitClass.Magician)
        {
            GameObject obj = Resources.Load<GameObject>("Prefabs/Weapon/MagicShot");

            if (monTarget != null)
            {

                if (obj != null)
                {
                    GameObject magicBall = Instantiate(obj, magicPos.position, Quaternion.identity, this.transform);
                    magicBall.TryGetComponent(out MagicAttackCtrl magicCtrl);
                    magicCtrl.SetType(monTarget, null);
                }
            }
            else
            {

                if (obj != null)
                {
                    GameObject magicBall = Instantiate(obj, magicPos.position, Quaternion.identity, this.transform);
                    magicBall.TryGetComponent(out MagicAttackCtrl magicCtrl);
                    magicCtrl.SetType(null, monsterPortal);
                }
            }

        }



    }


    public override bool CriticalCheck()
    {
        //유닛공격력을 받아서 크리티컬확률을 받아서 확률에 맞으면 크리공격
        //아니면 일반 공격
        int rand = UnityEngine.Random.Range(0, 101);
        if (rand <= unitStat.criticalRate)
            return true;

        return false;


    }


    public override void CriticalAttack(Unit monCtrl)
    {
        if (CriticalCheck())//true면 크리티컬데미지 false면 일반데미지
        {
            int attack = att * 2;
            monCtrl.OnDamage(attack, unitStat.knockBackForce);      //크리티컬이면 데미지2배에 넉백까지
            UnitEffectAndSound(monTarget.transform.position, "CriticalSound", "HitEff");

        }
        else  //노크리티컬이면 일반공격
        {

            monCtrl.OnDamage(att);        //넉백은 없이
            UnitEffectAndSound(monTarget.transform.position, "WarriorAttack", "HitEff");

        }
    }

    public override void CriticalAttack(Tower monPortal)
    {
        if (CriticalCheck())//true면 크리티컬데미지 false면 일반데미지
        {
            int attack = att * 2;
            monPortal.TowerDamage(attack);      //크리티컬이면 데미지2배 타워는 2배만
            UnitEffectAndSound(monPortal.transform.position, "CriticalSound", "HitEff");

        }
        else  //노크리티컬이면 일반공격
        {
            monPortal.TowerDamage(att);        //넉백은 없이
            UnitEffectAndSound(monPortal.transform.position, "WarriorAttack", "HitEff");

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


    #region 넉백
    void ApplyKnockBack(Vector2 dir, float force)
    {
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
        force = force * 0.5f;               //스페셜유닛은 넉백의 힘이 반감됨
        float knockBackAccleration = 25.0f;            //힘

        float knockbackTime = 0.0f;
        float maxKnockBackTime = 0.3f;

        while (knockbackTime < maxKnockBackTime)  //속도 증가
        {
            knockBackSpeed += knockBackAccleration * Time.deltaTime;
            rigbody.velocity = new Vector2(-1, 0) * knockBackSpeed;
            knockbackTime += Time.deltaTime;
            yield return null;
        }

        //knockBackAccleration = 10.0f;

        while (knockBackSpeed > 0.0f)   //속도 감소
        {
            knockBackSpeed -= (knockBackAccleration * 0.5f) * Time.deltaTime;

            Vector2 velo = new Vector2(-knockBackSpeed, rigbody.velocity.y);
            rigbody.velocity = velo;
            yield return null;
        }
        //rigbody.velocity *= 0.5f;

        yield return wfs; // 넉백 지속 시간

        SetUnitState(SpecialUnitState.Run);
        knockbackStart = false;



    }

    #endregion

    void Trace<T>(T obj) where T : UnityEngine.Component
    {

        Vector3 vec = obj.gameObject.transform.position - this.transform.position;
        traceDistance = vec.magnitude;
        Vector3 dir = vec.normalized;
        if (unitClass == UnitClass.Magician)
        {
            Debug.Log(traceDistance);
            Debug.Log(attackRange);
            if (traceDistance < attackRange)
            {

                SetUnitState(SpecialUnitState.Attack);
            }
            else
            {
                rigbody.transform.position += dir * moveSpeed * Time.deltaTime;
                SetUnitState(SpecialUnitState.Trace);
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

    void UnitVictory()
    {
        if (Managers.Game.State == GameState.GameVictory)
        {
            isRun = false;
            anim.SetBool("Run", isRun);
            isAtt = false;
            anim.SetBool("Attack", isAtt);

        }
    }



    public override void AttackDelay()
    {
        if (state == SpecialUnitState.Die)
            return;

        if (attackCoolTime > 0.0f)
        {
            attackCoolTime -= Time.deltaTime;
            if (attackCoolTime <= .0f)
            {

                //if(monTarget != null)
                //{
                //    distance = (monTarget.transform.position - this.transform.position).sqrMagnitude;
                //    if (distance < attackRange * attackRange)
                //        SetUnitState(UnitState.Attack);
                //    else
                //        SetUnitState(UnitState.Run);
                //}
                //else
                //    SetUnitState(UnitState.Run);

                if(monTarget != null)
                {
                    Vector3 vec = monTarget.gameObject.transform.position - this.transform.position;
                    traceDistance = vec.sqrMagnitude;
                    if (traceDistance < attackRange * attackRange)
                        SetUnitState(SpecialUnitState.Attack);
                    else
                        SetUnitState(SpecialUnitState.Run);
                }
                else
                {
                    if (towerDist < attackRange * attackRange)
                        SetUnitState(SpecialUnitState.Attack);
                    else
                        SetUnitState(SpecialUnitState.Run);
                }

   
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
