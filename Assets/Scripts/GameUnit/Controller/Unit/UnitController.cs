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



    string warriorHitSound = "WarriorAttack";
    string warriorCriticalSound = "CriticalSound";
    string warriorHitEff = "HitEff";



    public override void Init()
    {

        base.Init();
        spawnPosX = -9.2f;

        unitStat = new UnitStat();

        if (unitClass == UnitClass.Warrior)
        {
            unitStat = Managers.Data.warriorDict[GlobalData.g_UnitWarriorLv];
            towerAttackRange = 2.0f;
        }
        else if (unitClass == UnitClass.Archer)
        {
            unitStat = Managers.Data.archerDict[GlobalData.g_UnitArcherLv];
            towerAttackRange = 6.0f;

        }
        else if (unitClass == UnitClass.Spear)
        {
            unitStat = Managers.Data.spearDict[GlobalData.g_UnitSpearLv];
            towerAttackRange = 2.0f;

        }
        else if (unitClass == UnitClass.Priest)
        {
            unitStat = Managers.Data.priestDict[GlobalData.g_UnitPriestLv];
            towerAttackRange = 6.5f;

        }



        hp = unitStat.hp;
        att = unitStat.att;
        knockbackForce = unitStat.knockBackForce;
        attackRange = unitStat.attackRange;

        moveSpeed = 2.5f;
        maxHp = hp;


        //rig = this.GetComponent<Rigidbody2D>();
        //anim = GetComponent<Animator>();
        monsterPortal = GameObject.FindObjectOfType<MonsterPortal>();


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
        }

    }
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        if (Managers.Game.GameEndResult())       //������ �������� ����
            return;


        //UnitVictory();
        TowerSensor();
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

        monCtrls.Clear();
        enemyColls2D = Physics2D.OverlapBoxAll(pos.position, boxSize, 0, LayerMask.GetMask("Monster") | LayerMask.GetMask("EliteMonster"));
        if (enemyColls2D != null)
        {
            if (enemyColls2D.Length <= 0)
            {
                //�ڽ��� �ݶ��̴��� �ƹ��͵� ������
                if (monTarget != null)  //������ ���� Ÿ������ ���������
                {
                    monTarget = null;
                    return;
                }
            }


            //üũ�ڽ��ȿ� ���� �ݶ��̴��߿��� ���� ���ְ��� �Ÿ��� ���� ����� ���� ��󳻱�
            for(int ii = 0; ii < enemyColls2D.Length;ii++)
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


#endregion

    }

    void TowerSensor()
    {
        //Ÿ���� ������ ������ �ְ� Ÿ������ �Ÿ��� ����ؼ� üũ
        if (monsterPortal == null)
            return;

        towerVec = monsterPortal.gameObject.transform.position - this.transform.position;
        towerDist = towerVec.sqrMagnitude;
        towerDir = towerVec.normalized;

        if (towerDist < 15.0f * 15.0f)      //Ÿ������ �����Ÿ��� ������ �ƹ��� ������ Ÿ������
        {
            if (!towerTrace)  //������ �ٷ� Ÿ��������
            {
                towerTrace = true;
                SetUnitState(UnitState.Trace);
        

            }

            TowerAttackRange(towerAttackRange);

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
                SetUnitState(UnitState.Attack);
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
        if (monTarget != null)
            SetUnitState(UnitState.Trace);

        if (monTarget == null)
            if (towerTrace)
                towerTrace = false;

        if (IsTargetOn())
            return;


        rigbody.transform.position += Vector3.right * moveSpeed * Time.deltaTime;




    }


    protected virtual void UnitTrace()
    {
        if (!IsTargetOn())
        {
            SetUnitState(UnitState.Run);
            return;
        }

        if (towerDist < towerAttackRange * towerAttackRange)
        {
            SetUnitState(UnitState.Attack);
            return;
        }

        if (monTarget != null)
            Trace(monTarget);

        else if(monTarget == null)
            Trace(monsterPortal);
    }

    protected virtual bool IsTargetOn()
    {
        if (monTarget == null && towerTrace == false)
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
            StartCoroutine(Util.UnitDieTime(gameObject,5.0f));
        }
    }


   

    public override void OnDamage(int att,int knockBack = 0, bool criticalCheck = false)
    {

        if (hp > 0)
        {
            hp -= att;

            if(att > 0)
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
                else
                {
                    if (towerDist < unitStat.attackRange * unitStat.attackRange) 
                        CriticalAttack(monsterPortal, warriorHitSound, warriorCriticalSound ,warriorHitEff);

                }
            }

            
            else
                CriticalAttack(monsterPortal, warriorHitSound, warriorCriticalSound ,warriorHitEff);
            

        }

        else if(unitClass == UnitClass.Archer)
        {

            if(!towerAttack)
            {
                if(monTarget != null)
                {
                    GameObject obj = Managers.Resource.Load<GameObject>("Prefabs/Weapon/UnitArrow");

                    if (obj != null)
                    {
                        Managers.Sound.Play("Sounds/Effect/Bow");
                        GameObject arrow = Managers.Resource.Instantiate(obj, posTr.position, Quaternion.identity, this.transform);
                        arrow.TryGetComponent(out ArrowCtrl arrowCtrl);
                        if(monTarget.gameObject.layer == LayerMask.NameToLayer("Monster") && monTarget is MonsterController monsterCtrl)
                        {
                            arrowCtrl.SetType(monsterCtrl, null);

                        }
                        else if(monTarget.gameObject.layer == LayerMask.NameToLayer("EliteMonster") && monTarget is EliteMonsterController elite)
                        {
                            arrowCtrl.SetType(elite, null);

                        }
                    }
                }
            }
            else
            {
                GameObject obj = Resources.Load<GameObject>("Prefabs/Weapon/UnitArrow");

                if (obj != null)
                {
                    Managers.Sound.Play("Sounds/Effect/Bow");
                    GameObject arrow = Managers.Resource.Instantiate(obj, posTr.position, Quaternion.identity, this.transform);
                    arrow.TryGetComponent(out ArrowCtrl arrowCtrl);
                    arrowCtrl.SetType(null, monsterPortal);
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
                else
                {
                    if (towerDist < unitStat.attackRange * unitStat.attackRange)
                        CriticalAttack(monsterPortal, warriorHitSound, warriorCriticalSound, warriorHitEff);

                }
            }


            else
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


    //protected void UnitEffectAndSound(Vector3 pos, string soundPath, string effPath)
    //{
    //    Managers.Sound.Play($"Sounds/Effect/{soundPath}");
    //    GameObject eff = Managers.Resource.Load<GameObject>($"Prefabs/Effect/{effPath}");
    //    Vector2 randomPos = RandomPosSetting(pos);

    //    if (eff != null)
    //    {
    //        if(unitClass == UnitClass.Priest)
    //            Instantiate(eff, pos, Quaternion.identity);
    //        else
    //            Instantiate(eff, randomPos, Quaternion.identity);

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
            StartCoroutine(RestoreGravityAfterKnockback(force));

        }

        
    }


    IEnumerator RestoreGravityAfterKnockback(float force)
    {
        WaitForSeconds wfs = new WaitForSeconds(knockbackDuration);
        float knockBackSpeed = 0.0f;
        float knockBackAccleration = 25.0f;            //��

        float knockbackTime = 0.0f;
        float maxKnockBackTime = 0.3f;

        while (knockbackTime < maxKnockBackTime)  //�ӵ� ����
        {
            knockBackSpeed += knockBackAccleration * Time.deltaTime;
            rigbody.velocity = new Vector2(-1, 0) * knockBackSpeed;
            knockbackTime += Time.deltaTime;
            yield return null;
        }

        //knockBackAccleration = 10.0f;

        while (knockBackSpeed > 0.0f)   //�ӵ� ����
        {
            knockBackSpeed -= (knockBackAccleration * 0.5f) * Time.deltaTime;

            Vector2 velo = new Vector2(-knockBackSpeed, rigbody.velocity.y);
            rigbody.velocity = velo;
            yield return null;
        }
        //rigbody.velocity *= 0.5f;

        yield return wfs; // �˹� ���� �ð�

        SetUnitState(UnitState.Run);
        knockbackStart = false;



    }

    #endregion

    protected void Trace<T>(T obj) where T : UnityEngine.Component 
    {

        Vector3 vec = obj.gameObject.transform.position - this.transform.position;
        traceDistance = vec.magnitude;
        Vector3 dir = vec.normalized;


        if (traceDistance < attackRange)
        {
            SetUnitState(UnitState.Attack);
        }
        else
        {
            rigbody.transform.position += dir * moveSpeed * Time.deltaTime;
            SetUnitState(UnitState.Trace);

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

    //void UnitVictory()
    //{
    //    if(GameManager.instance.State == GameState.GameVictory)
    //    {
    //        isRun = false;
    //        anim.SetBool("Run", isRun);
    //        isAtt = false;
    //        anim.SetBool("Attack", isAtt);
           
    //    }
    //}



    public override void AttackDelay()
    {
        if (state == UnitState.Die)
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

                if (monTarget != null)
                {
                    Vector3 vec = monTarget.gameObject.transform.position - this.transform.position;
                    traceDistance = vec.sqrMagnitude;
                    if (traceDistance < attackRange * attackRange)
                        SetUnitState(UnitState.Attack);
                    else
                        SetUnitState(UnitState.Run);
                }
                else
                {
                    if (towerDist < attackRange * attackRange)
                        SetUnitState(UnitState.Attack);
                    else
                        SetUnitState(UnitState.Run);
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
