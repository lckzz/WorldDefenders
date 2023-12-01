using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class EliteMonsterController : Unit
{
    [SerializeField]
    protected MonsterClass monsterClass;
    [SerializeField]
    private EliteMonsterState state = EliteMonsterState.Run;
    private bool isSkil = false;
    [SerializeField] protected bool skillOn = false;     //스킬 발동판단


    protected List<Unit> unitCtrls = new List<Unit>();
    [SerializeField] protected Unit unitTarget;
    [SerializeField] protected PlayerTower playerTowerCtrl;
    protected List<Unit> skillenemyList = new List<Unit>();


    [SerializeField] protected GameObject appearDust;


    protected MonsterStat monStat;

    protected float coolTime = 20.0f;

    readonly string warriorHitSound = "WarriorAttack";
    readonly string warriorCriticalSound = "CriticalSound";
    readonly string warriorHitEff = "HitEff";

    protected readonly string monsterAppearTitleKey = "monsterAppearDialog";
    protected readonly string monsterDieTitleKey = "monsterDieDialog";
    protected readonly string skillTitleKey = "skillDialog";

    protected readonly string monsterDieSubKey = "eliteMonsterDie";

    protected readonly int appearProbability = 50;       //30프로확률로 등장하면서 말풍선
    protected readonly int dieProbability = 70;
    protected readonly int skillProbability = 100;




    private Debuff debuff;

    public Debuff Debuff { get { return debuff; } }

    public SkillBook Skills { get; protected set; }

    public Unit UnitCtrl { get { return unitTarget; } }
    public PlayerTower PlayerTowerCtrl { get { return playerTowerCtrl; } }
    public EliteMonsterState MonState { get { return state; } }

    Coroutine startCoolTime;

    public override void OnEnable()
    {
        if (sp != null && myColl != null)
        {
            //오브젝트 풀에서 생성되면 초기화 시켜줘야함
            isDie = false;
            isRun = false;
            hp = maxHp;
            SetMonsterState(EliteMonsterState.Run);
            sp.color = new Color32(255, 255, 255, 255);
            myColl.enabled = true;
            appearDust?.SetActive(true);
            unitTarget = null;
            playerTowerCtrl = null;
        }

    }

    public override void Init()
    {
        base.Init();



        spawnPosX = 20.0f;

        Skills = gameObject.GetComponent<SkillBook>();

        TryGetComponent<Collider2D>(out myColl);
        TryGetComponent<Debuff>(out debuff);

        SetMonsterState(EliteMonsterState.Run);
        startCoolTime = StartCoroutine(UnitSKillCoolTime(coolTime));
        appearDust?.SetActive(true);

        
    }



    public override void EnemySensor()      //적감지
    {
        //Debug.Log(isTargeting);
        //Debug.Log($"타겟팅{isTageting}");
        #region 타겟구현


        UnitSense();
        UnitDistanceAsending();
        #endregion

    }

    void UnitSense()
    {

        skillenemyList.Clear();
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



            //체크박스안에 들어온 콜라이더중에서 현재 유닛과의 거리가 제일 가까운 것을 골라내기
            for (int ii = 0; ii < enemyColls2D.Length; ii++)
            {
                if (enemyColls2D[ii].gameObject.layer == LayerMask.NameToLayer("Unit"))
                {
                    UnitController unitctrl;
                    enemyColls2D[ii].TryGetComponent<UnitController>(out unitctrl);
                    unitCtrls.Add(unitctrl);

                    //unitCtrls[ii] = unitctrl;

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
    void UnitDistanceAsending()
    {
        if (unitCtrls.Count > 0)
        {

            if (unitCtrls.Count > 1)
            {
                for (int i = 0; i < unitCtrls.Count; i++)
                {
                    for (int j = i + 1; j < unitCtrls.Count; j++)
                    {
                        if (j == unitCtrls.Count)
                            break;

                        float distA = (unitCtrls[i].transform.position - this.transform.position).sqrMagnitude;
                        float distB = (unitCtrls[j].transform.position - this.transform.position).sqrMagnitude;
                        if (distA * distA > distB * distB)
                        {
                            Unit unitTemp = unitCtrls[i];
                            unitCtrls[i] = unitCtrls[j];
                            unitCtrls[j] = unitTemp;

                        }

                    }


                }



            }



            for (int ii = 0; ii < skillenemyList.Count; ii++)
            {
                if (skillenemyList[ii].IsDie)
                    skillenemyList.RemoveAt(ii);
            }


            if (unitCtrls.Count != 0)
            {
                unitTarget = unitCtrls[0];
            }


        }
    }

    protected void TowerSensor()
    {
        //타워는 유닛이 없다면 그때 감지를하고 공격추격이나 공격을 할 수 있다.

        towerColl = Physics2D.OverlapBox(pos.position, boxSize, 0, LayerMask.GetMask("Tower"));
        //Debug.Log(towerColl?.name);
        if (towerColl != null)
            towerColl.TryGetComponent(out playerTowerCtrl);


    }

    protected void MonsterStateCheck()
    {
        switch (state)
        {
            case EliteMonsterState.Idle:
                {
                    MonsterIdle();
                    break;
                }
            case EliteMonsterState.Run:
                {
                    MonsterMove();
                    break;
                }
            case EliteMonsterState.Trace:
                {
                    MonsterTrace();
                    break;
                }
            case EliteMonsterState.Attack:
                {


                    UnitAttack();
                    break;
                }
            case EliteMonsterState.Skill:
                {
                    MonsterSkill();
                    break;
                }
            case EliteMonsterState.KnockBack:
                {
                    ApplyKnockBack(new Vector2(1.0f, 1.0f), damageKnockBack);
                    break;
                }
            case EliteMonsterState.Die:
                {
                    MonsterDie();
                    break;
                }

        }

    }
    protected void SetMonsterState(EliteMonsterState state)
    {
        if (isDie)
            return;

        this.state = state;
        switch (this.state)
        {
            case EliteMonsterState.Idle:
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
            case EliteMonsterState.Run:
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
            case EliteMonsterState.Attack:
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
                    break;
                }
            case EliteMonsterState.Skill:
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
            case EliteMonsterState.KnockBack:
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
            case EliteMonsterState.Die:
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
            SetMonsterState(EliteMonsterState.Trace);

        if (IsTargetOn())
            return;


        rigbody.transform.position += Vector3.left * moveSpeed * Time.fixedDeltaTime;




    }


    void MonsterTrace()
    {
        if (!IsTargetOn())
        {
            SetMonsterState(EliteMonsterState.Run);
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


        if (unitTarget != null)
        {
            if (unitTarget.gameObject.layer == LayerMask.NameToLayer("Unit"))
            {
                if (unitTarget is UnitController unitCtrl)
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

        if (unitCtrls.Count > 0)
        {
            if (skillOn)      //스킬On이면
            {
                SetMonsterState(EliteMonsterState.Skill);
                return;
            }
        }


        if (anim.GetCurrentAnimatorStateInfo(0).IsName("NormalAttack"))
        {

            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)
            {
                SetMonsterState(EliteMonsterState.Idle);

                attackCoolTime = 1.0f;

            }


        }
    }


    public virtual void MonsterSkill() { }


    void MonsterDie()
    {
        if (myColl.enabled)
        {

            speechBubble.SpeechBubbuleOn(monsterDieTitleKey, monsterDieSubKey,dieProbability);

            SetMonsterState(EliteMonsterState.Die);
            myColl.enabled = false;
            StartCoroutine(Util.DestroyTime(gameObject, 3.0f));


        }
    }

    public override void OnDamage(int att, int knockBack = 0, bool criticalCheck = false)
    {

        if (hp > 0)
        {
            hp -= att;
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
                SetMonsterState(EliteMonsterState.KnockBack);
                damageKnockBack = knockBack;
            }


            if (hp <= 0)
            {
                hp = 0;
                SetMonsterState(EliteMonsterState.Die);

            }
        }
    }

    public override void OnAttack()
    {


        if (unitTarget != null)
        {
            float dist = (unitTarget.transform.position - this.gameObject.transform.position).sqrMagnitude;
            if (dist < monStat.attackRange * monStat.attackRange)
                CriticalAttack(unitTarget,warriorHitSound,warriorCriticalSound, warriorHitEff);

        }


        else if(playerTowerCtrl != null)
            CriticalAttack(playerTowerCtrl,warriorHitSound,warriorCriticalSound, warriorHitEff);






    }



    public override bool CriticalCheck()
    {
        //유닛공격력을 받아서 크리티컬확률을 받아서 확률에 맞으면 크리공격
        //아니면 일반 공격
        int rand = UnityEngine.Random.Range(0, 101);
        if (rand <= monStat.criticalRate)
            return true;

        return false;


    }


    public override void CriticalAttack(Unit uniCtrl, string soundPath,string criticalSoundPath, string hitPath)
    {
        if (CriticalCheck())//true면 크리티컬데미지 false면 일반데미지
        {
            Debug.Log("크리티컬!!!!");
            int attack = att * 2;
            uniCtrl.OnDamage(attack, monStat.knockBackForce,true);      //크리티컬이면 데미지2배에 넉백까지
            Managers.Resource.ResourceEffectAndSound(unitTarget.transform.position, criticalSoundPath, hitPath);

        }
        else  //노크리티컬이면 일반공격
        {
            Debug.Log("일반공격...");

            uniCtrl.OnDamage(att);        //넉백은 없이
            Managers.Resource.ResourceEffectAndSound(unitTarget.transform.position, soundPath, hitPath);

        }
    }

    public override void CriticalAttack(Tower tower, string soundPath, string criticalSoundPath, string hitPath)
    {
        if (CriticalCheck())//true면 크리티컬데미지 false면 일반데미지
        {
            int attack = att * 2;
            tower.TowerDamage(attack);      //크리티컬이면 데미지2배 타워는 2배만
            Managers.Resource.ResourceEffectAndSound(tower.transform.position, criticalSoundPath, hitPath);

        }
        else  //노크리티컬이면 일반공격
        {
            tower.TowerDamage(att);        //넉백은 없이
            Managers.Resource.ResourceEffectAndSound(tower.transform.position, soundPath, hitPath);

        }
    }

    public virtual void OnSkill() { }



    void ApplyKnockBack(Vector2 dir, float force)
    {
        if (transform.position.x >= spawnPosX)
        {
            SetMonsterState(EliteMonsterState.Idle);
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
            rigbody.velocity = new Vector2(1, 0) * knockBackSpeed;
            knockbackTime += Time.fixedDeltaTime;
            yield return null;
        }

        //knockBackAccleration = 10.0f;

        while (knockBackSpeed > 0.0f)   //속도 감소
        {
            knockBackSpeed -= (knockBackAccleration * 0.5f) * Time.fixedDeltaTime;

            Vector2 velo = new Vector2(knockBackSpeed, rigbody.velocity.y);
            rigbody.velocity = velo;
            yield return null;
        }
        //rigbody.velocity *= 0.5f;

        yield return wfs; // 넉백 지속 시간

        SetMonsterState(EliteMonsterState.Idle);
        attackCoolTime = 0.5f;
        knockbackStart = false;



    }



    void Trace<T>(T obj) where T : UnityEngine.Component
    {
        Vector3 vec = obj.gameObject.transform.position - this.transform.position;
        float distance = vec.sqrMagnitude;
        Vector3 dir = vec.normalized;


        if (distance < attackRange * attackRange)
        {
            SetMonsterState(EliteMonsterState.Attack);
        }
        else
        {
            rigbody.transform.position += dir * moveSpeed * Time.fixedDeltaTime;
            SetMonsterState(EliteMonsterState.Trace);

        }
    }



    public override void AttackDelay()
    {
        if (state == EliteMonsterState.Die)
            return;


        if (attackCoolTime > 0.0f)
        {
            attackCoolTime -= Time.fixedDeltaTime;
            if (attackCoolTime <= .0f)
            {
                if (unitTarget != null)
                {
                    Vector3 vec = unitTarget.gameObject.transform.position - this.transform.position;
                    traceDistance = vec.sqrMagnitude;

                    if (traceDistance < attackRange * attackRange)
                        SetMonsterState(EliteMonsterState.Attack);
                    else
                        SetMonsterState(EliteMonsterState.Run);
                }
                else if(playerTowerCtrl != null)
                {
                    Vector3 vec = playerTowerCtrl.gameObject.transform.position - this.transform.position;
                    traceDistance = vec.sqrMagnitude;

                    if (traceDistance < attackRange * attackRange)
                        SetMonsterState(EliteMonsterState.Attack);
                    else
                    {
                        SetMonsterState(EliteMonsterState.Run);
                        
                    }
                }
                else
                    SetMonsterState(EliteMonsterState.Run);

            }

        }
    }

    public void SpeechchBubbleOn(string speechTitleKey , string speechSubKey,int probaility)
    {
        speechBubble.SpeechBubbuleOn(speechTitleKey,speechSubKey, probaility);


    }



    IEnumerator UnitSKillCoolTime(float coolTime)
    {
        WaitForSeconds wfs = new WaitForSeconds(coolTime);

        while (true)
        {
            if (!skillOn)     //스킬이 안돌았다면
            {
                yield return wfs; //쿨타임 대기

                skillOn = true;      //스킬 사용가능!  스킬사용하면 다시 false로
                Debug.Log("스킬온!@!@");

            }


            yield return null;

        }


    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(pos.position, boxSize);
    }

}
