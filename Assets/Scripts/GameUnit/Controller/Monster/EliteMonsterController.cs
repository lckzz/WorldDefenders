using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class EliteMonsterController : MonsterBase
{

    [SerializeField]
    private EliteMonsterState state = EliteMonsterState.Run;
    private bool isSkil = false;
    private bool isSummon = false;      //��ȯ�� ����Ʈ�������� �Ǵ��ϴ� ����
    [SerializeField] protected bool skillOn = false;     //��ų �ߵ��Ǵ�
    [SerializeField] protected GameObject appearDust;

    protected List<Unit> skillenemyList = new List<Unit>();


    protected float coolTime = 20.0f;

    readonly string warriorHitSound = "WarriorAttack";
    readonly string warriorCriticalSound = "CriticalSound";
    readonly string warriorHitEff = "HitEff";

    protected readonly string monsterAppearTitleKey = "monsterAppearDialog";
    protected readonly string monsterDieTitleKey = "monsterDieDialog";
    protected readonly string skillTitleKey = "skillDialog";

    protected readonly string monsterDieSubKey = "eliteMonsterDie";

    protected readonly int appearProbability = 50;       //30����Ȯ���� �����ϸ鼭 ��ǳ��
    protected readonly int dieProbability = 70;
    protected readonly int skillProbability = 100;


    protected Dictionary<MonsterClass, MonsterStat> eliteMonsterStatDict;


    public bool IsSummon { get { return isSummon; } set { isSummon = value; } }
    public SkillBook Skills { get; protected set; }

    public EliteMonsterState MonState { get { return state; } }

    protected Coroutine startCoolTimeCo;

    public override void OnEnable()
    {
        base.OnEnable();
        

        if (myColl != null)
        {
            if(startCoolTimeCo != null)
                StopCoroutine(startCoolTimeCo);
            skillOn = false;
            isSummon = false;

            SetMonsterState(EliteMonsterState.Run);
        }

    }

    public override void Init()
    {
        base.Init();


        Debug.Log(Managers.Game.MonsterTypeIdDict[MonsterType.SkeletonKing]);
        eliteMonsterStatDict = new Dictionary<MonsterClass, MonsterStat>
        {
            {MonsterClass.EliteWarrior, Managers.Data.monsterDict[Managers.Game.MonsterTypeIdDict[MonsterType.EliteWarrior]] },
            {MonsterClass.EliteShaman, Managers.Data.monsterDict[Managers.Game.MonsterTypeIdDict[MonsterType.EliteShaman]] },
            {MonsterClass.EliteCavalry, Managers.Data.monsterDict[Managers.Game.MonsterTypeIdDict[MonsterType.EliteCavalry]] },
            {MonsterClass.SkeletonKing, Managers.Data.monsterDict[Managers.Game.MonsterTypeIdDict[MonsterType.SkeletonKing]] }


        };

        monStat = eliteMonsterStatDict[monsterClass];           //�ش� ���ֿ� ����� ����Ŭ������ �־��ָ� �´� ���ͽ����� ������

        //���� ������ ������ �޾Ƽ� ����
        att = monStat.att;
        hp = monStat.hp;
        maxHp = hp;
        knockbackForce = monStat.knockBackForce;
        attackRange = monStat.attackRange;
        moveSpeed = 2.0f;
        DropCost = monStat.dropCost;


        spawnPosX = 20.0f;



        Skills = gameObject.GetComponent<SkillBook>();

        SetMonsterState(EliteMonsterState.Run);


        if (startCoolTimeCo != null)
            StopCoroutine(startCoolTimeCo);

        if(!isSummon)
            appearDust?.SetActive(true);


    }



    public override void EnemySensor()      //������
    {
        #region Ÿ�ٱ���


        UnitSense();
        UnitDistanceAsending();
        #endregion

    }

    protected override void UnitSense()
    {

        skillenemyList.Clear();
        unitCtrls.Clear();
        enemyColls2D = Physics2D.OverlapBoxAll(pos.position, boxSize, 0, LayerMask.GetMask("Unit") | LayerMask.GetMask("SpecialUnit"));


        if (enemyColls2D != null)
        {

            if (enemyColls2D.Length <= 0)
            {
                TowerSensor();   //���Ͱ� �ƹ��� ���ٸ� Ÿ�������� Ų��.


                //�ڽ��� �ݶ��̴��� �ƹ��͵� ������
                if (unitTarget != null)  //������ ���� Ÿ������ ���������
                {
                    unitTarget = null;
                    return;
                }
            }



            //üũ�ڽ��ȿ� ���� �ݶ��̴��߿��� ���� ���ְ��� �Ÿ��� ���� ����� ���� ��󳻱�
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
    protected override void UnitDistanceAsending()
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
            if (skillOn)      //��ųOn�̸�
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

            speechBubble.SpeechBubbleOn(monsterDieTitleKey, monsterDieSubKey,dieProbability);

            SetMonsterState(EliteMonsterState.Die);
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
        float knockBackAccleration = force * 0.5f;            //��

        float knockbackTime = 0.0f;
        float maxKnockBackTime = 0.3f;

        while (knockbackTime < maxKnockBackTime)  //�ӵ� ����
        {
            knockBackSpeed += knockBackAccleration * Time.fixedDeltaTime;
            rigbody.velocity = new Vector2(1, 0) * knockBackSpeed;
            knockbackTime += Time.fixedDeltaTime;
            yield return null;
        }

        //knockBackAccleration = 10.0f;

        while (knockBackSpeed > 0.0f)   //�ӵ� ����
        {
            knockBackSpeed -= (knockBackAccleration * 0.5f) * Time.fixedDeltaTime;

            Vector2 velo = new Vector2(knockBackSpeed, rigbody.velocity.y);
            rigbody.velocity = velo;
            yield return null;
        }
        //rigbody.velocity *= 0.5f;

        yield return wfs; // �˹� ���� �ð�

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
        speechBubble.SpeechBubbleOn(speechTitleKey,speechSubKey, probaility);


    }



    protected IEnumerator UnitSkillCoolTime(float coolTime)
    {

        float cool = coolTime;

        while (true)
        {
            if (!skillOn)     //��ų�� �ȵ��Ҵٸ�
            {
                cool -= Time.deltaTime;

                if(cool <= 0.0f)        //0���� �۾�����
                {
                    cool = 0.0f;
                    skillOn = true;      //��ų ��밡��!  ��ų����ϸ� �ٽ� false��

                    yield break;        //��Ÿ���� �ٵ��� �ڷ�ƾ ����
                }

            }


            yield return null;

        }


    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(pos.position, boxSize);
    }

    //public void Notified(int att, float speed)
    //{
    //    this.att = att;
    //    this.moveSpeed = speed;
    //}
}
