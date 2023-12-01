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

public class UnitController : Unit
{

    [SerializeField]
    private UnitClass unitClass;
    [SerializeField]
    protected UnitState state = UnitState.Run;        //�׽�Ʈ���̶� �� 


    protected List<Unit> monCtrls = new List<Unit>();  //�����ȿ� ���� ������ �������� ��Ƶ�
    [SerializeField] protected Unit monTarget;  //���͵��� �������߿��� ���� ���ְ� ����� ���������� �޾ƿ�
    [SerializeField] protected MonsterPortal monsterPortal;


    //��ó ���� ����
    protected Transform posTr;

    //��ó ���� ����

    protected UnitStat unitStat;

    public int KnockBackForce { get { return knockbackForce; } }

    public Unit Monctrl { get { return monTarget; } }

    public MonsterPortal MonsterPortal { get { return monsterPortal; } }

    public UnitState UniState { get { return state; } }

    public UnitClass UniClass { get { return unitClass; } }



    private readonly string warriorHitSound = "WarriorAttack";
    private readonly string warriorCriticalSound = "CriticalSound";
    private readonly string warriorHitEff = "HitEff";
    private readonly string appearTitleKey = "unitAppearDialog";
    private readonly string dieTitleKey = "unitDieDialog";


    public override void Init()
    {

        base.Init();
        spawnPosX = -9.2f;

        unitStat = new UnitStat();

        if (unitClass == UnitClass.Warrior)
        {
            unitStat = Managers.Data.warriorDict[Managers.Game.UnitWarriorLv];
        }
        else if (unitClass == UnitClass.Archer)
        {
            unitStat = Managers.Data.archerDict[Managers.Game.UnitArcherLv];

        }
        else if (unitClass == UnitClass.Spear)
        {
            unitStat = Managers.Data.spearDict[Managers.Game.UnitSpearLv];

        }
        else if (unitClass == UnitClass.Priest)
        {
            unitStat = Managers.Data.priestDict[Managers.Game.UnitPriestLv];

        }



        hp = unitStat.hp;
        att = unitStat.att;
        knockbackForce = unitStat.knockBackForce;
        attackRange = unitStat.attackRange;

        moveSpeed = 2.5f;
        maxHp = hp;


        //rig = this.GetComponent<Rigidbody2D>();
        //anim = GetComponent<Animator>();


        if (unitClass == UnitClass.Archer)
            posTr = transform.Find("ArrowPos");
        else if(unitClass == UnitClass.Priest)
            posTr = transform.Find("MagicPos");
        else
            posTr = null;

        SetUnitState(UnitState.Run);

    }

    public override void OnEnable()
    {
        if(sp != null && myColl != null)
        {
            //������Ʈ Ǯ���� �����Ǹ� �ʱ�ȭ ���������
            isDie = false;
            isRun = false;
            hp = maxHp;
            SetUnitState(UnitState.Run);
            sp.color = new Color32(255, 255, 255, 255);
            myColl.enabled = true;
            monTarget = null;
        }

    }
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Managers.Game.GameEndResult())       //������ �������� ����
            return;

        //UnitVictory();
        EnemySensor();
        UnitStateCheck();
        //UnitVictory();

        //if (Input.GetKeyDown(KeyCode.Q))
        //{
        //    state = UnitState.KnockBack;

        //}

    }




    public override void EnemySensor()      //������
    {

        #region Ÿ�ٱ���

        UnitSense();
        UnitDistanceAsending();




#endregion

    }

    //���ֵ��� ����
    void UnitSense()
    {
        //Ÿ�ٸ���Ʈ �ʱ�ȭ����
        monCtrls.Clear();
        enemyColls2D = Physics2D.OverlapBoxAll(pos.position, boxSize, 0, LayerMask.GetMask("Monster") | LayerMask.GetMask("EliteMonster"));
        if (enemyColls2D != null)
        {
            if (enemyColls2D.Length <= 0)
            {

                TowerSensor();
                //�ڽ��� �ݶ��̴��� �ƹ��͵� ������
                if (monTarget != null)  //������ ���� Ÿ������ ���������
                {
                    monTarget = null;
                    return;
                }
            }
            else
            {
                monsterPortal = null;
            }


            //üũ�ڽ��ȿ� ���� �ݶ��̴��߿��� ���� ���ְ��� �Ÿ��� ���� ����� ���� ��󳻱�
            for (int ii = 0; ii < enemyColls2D.Length; ii++)
            {
                if (enemyColls2D[ii].gameObject.layer == LayerMask.NameToLayer("Monster"))
                {
                    MonsterController monctrl;
                    enemyColls2D[ii].TryGetComponent<MonsterController>(out monctrl);
                    monCtrls.Add(monctrl);

                }
                else if (enemyColls2D[ii].gameObject.layer == LayerMask.NameToLayer("EliteMonster"))
                {
                    EliteMonsterController elite;
                    enemyColls2D[ii].TryGetComponent<EliteMonsterController>(out elite);
                    monCtrls.Add(elite);

                }
            }


        }
    }

    //������ ���ֵ��� �ش� ĳ���Ϳ��� �Ÿ� ��������
    void UnitDistanceAsending()
    {
        if (monCtrls.Count > 0)
        {
            float disMin = 0;
            int min = 0;


            if (monCtrls.Count > 1)
            {
                for (int i = 0; i < monCtrls.Count; i++)
                {
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


            if (monCtrls.Count != 0)
            {
                monTarget = monCtrls[min];
            }

        }
    }


    void TowerSensor()
    {
        towerColl = Physics2D.OverlapBox(pos.position, boxSize, 0, LayerMask.GetMask("MonsterPortal"));
        if (towerColl != null)
            towerColl.TryGetComponent(out monsterPortal);
    }




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

    protected virtual bool IsTargetOn()
    {
        if (monTarget == null && monsterPortal == null)
            return false;


        if(monTarget != null)
        {
            if(monTarget.gameObject.layer == LayerMask.NameToLayer("Monster") && monTarget is MonsterController monsterCtrl)
            {
                if (monsterCtrl.MonState == MonsterState.Die)
                    return false;

                if (!monTarget.gameObject.activeInHierarchy)
                    return false;
            }

            else if(monTarget.gameObject.layer == LayerMask.NameToLayer("EliteMonster") && monTarget is EliteMonsterController elite)
            {
                if (elite.MonState == Define.EliteMonsterState.Die)
                    return false;

                if (!monTarget.gameObject.activeInHierarchy)
                    return false;
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

                SetUnitState(UnitState.Idle);

                attackCoolTime = 1.0f;


            }


        }
    }

    void UnitDie()
    {
        if (myColl.enabled)
        {
            SetUnitState(UnitState.Die);
            myColl.enabled = false;
            StartCoroutine(Util.DestroyTime(gameObject,5.0f));
        }
    }


   

    public override void OnDamage(int att,int knockBack = 0, bool criticalCheck = false)
    {


        if (hp > 0)
        {
            hp -= att;
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




    public override void OnHeal(int heal)
    {
        if(hp > 0)
        {
            unitHUDHp?.SpawnHUDText(heal.ToString(), (int)Define.UnitDamageType.Team);
            hp += heal;

        }

        if (hp >= maxHp)
            hp = maxHp;
    }


    public override void OnAttack()    //�ִϸ��̼� �̺�Ʈ �Լ�
    {
        if(unitClass == UnitClass.Warrior)
        {

            if (monTarget != null)
            {
                float dist = (monTarget.transform.position - this.gameObject.transform.position).magnitude;
                if (dist < unitStat.attackRange + 0.5f)
                    CriticalAttack(monTarget,warriorHitSound,warriorCriticalSound, warriorHitEff);
            }
            else if(monsterPortal != null)
                CriticalAttack(monsterPortal, warriorHitSound, warriorCriticalSound ,warriorHitEff);
            

        }

        else if(unitClass == UnitClass.Archer)
        {

            if(monTarget != null)
            {
                GameObject obj = Managers.Resource.Load<GameObject>("Prefabs/Weapon/UnitArrow");

                if (obj != null)
                {
                    Managers.Sound.Play("Sounds/Effect/Bow");
                    GameObject arrow = Managers.Resource.Instantiate(obj, posTr.position, Quaternion.identity, this.transform);
                    arrow.TryGetComponent(out ArrowCtrl arrowCtrl);
                    arrowCtrl.Init();
                    //if(monTarget is MonsterController monsterCtrl)
                    //{
                    //    arrowCtrl.SetType(monsterCtrl, null);

                    //}
                    //else if(monTarget is EliteMonsterController elite)
                    //{
                    //    arrowCtrl.SetType(elite, null);

                    //}
                }
            }
            
            else if(monsterPortal != null)
            {
                GameObject obj = Resources.Load<GameObject>("Prefabs/Weapon/UnitArrow");

                if (obj != null)
                {
                    Managers.Sound.Play("Sounds/Effect/Bow");
                    GameObject arrow = Managers.Resource.Instantiate(obj, posTr.position, Quaternion.identity, this.transform);
                    arrow.TryGetComponent(out ArrowCtrl arrowCtrl);
                    arrowCtrl.Init();
                }
            }

            


        
        }

        if (unitClass == UnitClass.Spear)
        {

            if (monTarget != null)
            {
                float dist = (monTarget.transform.position - this.gameObject.transform.position).magnitude;
                Debug.Log(dist);
                Debug.Log(unitStat.attackRange);

                if (dist < unitStat.attackRange + 0.5f)
                    CriticalAttack(monTarget, warriorHitSound, warriorCriticalSound, warriorHitEff);
            }

            else if (monsterPortal != null)
                CriticalAttack(monsterPortal, warriorHitSound, warriorCriticalSound, warriorHitEff);

        }

    }


    public override bool CriticalCheck()
    {
        //���ְ��ݷ��� �޾Ƽ� ũ��Ƽ��Ȯ���� �޾Ƽ� Ȯ���� ������ ũ������
        //�ƴϸ� �Ϲ� ����
        int rand = UnityEngine.Random.Range(0, 101);
        if (rand <= unitStat.criticalRate)
            return true;

        return false;


    }


    public override void CriticalAttack(Unit monCtrl,string soundPath, string criticalSoundPath, string hitPath)
    {
        if (CriticalCheck())//true�� ũ��Ƽ�õ����� false�� �Ϲݵ�����
        {
            int attack = att * 2;
            monCtrl.OnDamage(attack, unitStat.knockBackForce,true);      //ũ��Ƽ���̸� ������2�迡 �˹����
            Managers.Resource.ResourceEffectAndSound(monTarget.transform.position, criticalSoundPath, hitPath);

        }
        else  //��ũ��Ƽ���̸� �Ϲݰ���
        {
            
            monCtrl.OnDamage(att);        //�˹��� ����
            Managers.Resource.ResourceEffectAndSound(monTarget.transform.position, soundPath, hitPath);

        }
    }

    public override void CriticalAttack(Tower monPortal, string soundPath, string criticalSoundPath, string hitPath)
    {
        if (CriticalCheck())//true�� ũ��Ƽ�õ����� false�� �Ϲݵ�����
        {
            int attack = att * 2;
            monPortal.TowerDamage(attack);      //ũ��Ƽ���̸� ������2�� Ÿ���� 2�踸
            Managers.Resource.ResourceEffectAndSound(monPortal.transform.position, criticalSoundPath, hitPath);

        }
        else  //��ũ��Ƽ���̸� �Ϲݰ���
        {
            monPortal.TowerDamage(att);        //�˹��� ����
            Managers.Resource.ResourceEffectAndSound(monPortal.transform.position, soundPath, hitPath);

        }
    }



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
            StartCoroutine(RestoreGravityAfterKnockback(force));

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


    public void AddObserver(IHpObserver observer)
    {

    }

    public void RemoveObserver(IHpObserver observer)
    {

    }

    public void NotifyToObserver(IHpObserver observer)
    {

    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(pos.position, boxSize);
    }
}
