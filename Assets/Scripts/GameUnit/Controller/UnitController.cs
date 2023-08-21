using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public enum UnitClass
{
    Warrior,
    Archer,
    Spear,
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
    private Animator anim;

    [SerializeField]
    private UnitClass unitClass;
    [SerializeField]
    private UnitState state = UnitState.Run;        //�׽�Ʈ���̶� �� 


    MonsterController[] monCtrls;  //�����ȿ� ���� ������ �������� ��Ƶ�
    [SerializeField] MonsterController monTarget;  //���͵��� �������߿��� ���� ���ְ� ����� ���������� �޾ƿ�
    Collider2D unitColl2d;
    [SerializeField] MonsterPortal monsterPortal;

    Rigidbody2D rigbody;

    public bool IsDie { get { return isDie; } }
    
    public int Att { get { return att; } }
    public int KnockBackForce { get { return knockbackForce; } }

    public MonsterController Monctrl { get { return monTarget; } }

    public void IsTargetingSet(bool value) => isTargeting = value;
    public MonsterPortal MonsterPortal { get { return monsterPortal; } }

    public UnitState UniState { get { return state; } }


    //��ó ����
    Transform arrowPos;

    //��ó ����

    UnitStat unitStat;

    public float hpPercent()
    {
        return hp / maxHp;
    }

    void Init()
    {
        unitStat  = new UnitStat();

        if (unitClass == UnitClass.Warrior)
            unitStat = Managers.Data.warriorDict[GlobalData.g_UnitWarriorLv];
        else if (unitClass == UnitClass.Archer)
            unitStat = Managers.Data.archerDict[GlobalData.g_UnitArcherLv];
        else if (unitClass == UnitClass.Spear)
            unitStat = Managers.Data.spearDict[GlobalData.g_UnitSpearLv];
        


        hp = unitStat.hp;
        att = unitStat.att;
        knockbackForce = unitStat.knockBackForce;
        moveSpeed = 2.5f;
        archerAttDis = 5.0f;
        maxHp = hp;


        //rig = this.GetComponent<Rigidbody2D>();
        //anim = GetComponent<Animator>();
        monsterPortal = GameObject.FindObjectOfType<MonsterPortal>();

        TryGetComponent<Animator>(out anim);
        TryGetComponent<Collider2D>(out unitColl2d);
        TryGetComponent<Rigidbody2D>(out rigbody);

        if (unitClass == UnitClass.Archer)
            arrowPos = transform.Find("ArrowPos");
        else
            arrowPos = null;

        SetUnitState(UnitState.Run);

    }


    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        //UnitVictory();
        TowerSensor();
        EnemySensor();
        UnitStateCheck();
        
        UnitVictory();

        if (Input.GetKeyDown(KeyCode.Q))
        {
            state = UnitState.KnockBack;

        }


        //������ ������ 
    }




    public override void EnemySensor()      //������
    {
        //Debug.Log(isTargeting);
        //Debug.Log($"Ÿ����{isTageting}");
        #region Ÿ�ٱ���

        coll2d = Physics2D.OverlapBoxAll(pos.position, boxSize, 0, LayerMask.GetMask("Monster"));
        if (coll2d != null)
        {

            if (coll2d.Length <= 0)
            {
                //�ڽ��� �ݶ��̴��� �ƹ��͵� ������
                if (monTarget != null)  //������ ���� Ÿ������ ���������
                {
                    monTarget = null;
                    return;
                }
            }

            monCtrls = new MonsterController[coll2d.Length];
            //üũ�ڽ��ȿ� ���� �ݶ��̴��߿��� ���� ���ְ��� �Ÿ��� ���� ����� ���� ��󳻱�
            for(int ii = 0; ii < coll2d.Length;ii++)
                coll2d[ii].TryGetComponent<MonsterController>(out monCtrls[ii]);
            
               
        }


        if(monCtrls.Length > 0)
        {
            float disMin = 0;
            int min = 0;


            if(monCtrls.Length > 1)
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

                        if (disMin > distB *  distB)
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
        //Ÿ���� �ֿ켱������ Ÿ���ϰ� �Ÿ��� ����ؼ� ����ؼ� �����Ÿ��ȿ� ������ Ÿ�� ����
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
                SetUnitState(UnitState.Trace);
        

            }

            if(unitClass == UnitClass.Warrior)
            {

                TowerAttackRange(1.5f);
            }

            else if(unitClass == UnitClass.Archer)
            {
                TowerAttackRange(6.5f);
            }

            else if (unitClass == UnitClass.Spear)
            {
                TowerAttackRange(2.0f);
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


    void SetUnitState(UnitState state)
    {
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


    void UnitMove()
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


    void UnitTrace()
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

    bool IsTargetOn()
    {
        if (monTarget == null && towerTrace == false)
            return false;


        if(monTarget != null)
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
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)
            {
                attackCoolTime = 1.0f;
                SetUnitState(UnitState.Idle);
                if (towerAttack)
                    towerAttack = false;
            }


        }
    }

    void UnitDie()
    {
        if (unitColl2d.enabled)
        {
            state = UnitState.Die;
            unitColl2d.enabled = false;
            GameObject.Destroy(gameObject, 5.0f);

        }
    }
   

    public void OnDamage(int att,int knockBack = 0)
    {

        if (hp > 0)
        {
            hp -= att;
            SetUnitState(UnitState.KnockBack);
            damageKnockBack = knockBack;

            if (hp <= 0)
            {
                hp = 0;
                SetUnitState(UnitState.Die);
            }
        }
    }


    float randomX = 0;
    float randomY = 0;

    Vector2 RandomPosSetting(Vector3 pos )
    {
        randomX = UnityEngine.Random.Range(-0.5f, 0.5f);
        randomY = UnityEngine.Random.Range(-0.5f, 0.5f);
        Vector2 randomPos = pos;
        randomPos.x += randomX;
        randomPos.y += randomY;

        return randomPos;
    }


    public void OnAttack()    //�ִϸ��̼� �̺�Ʈ �Լ�
    {
        if(unitClass == UnitClass.Warrior)
        {
            if(!towerAttack)
            {
                if (monTarget != null)
                {
                    monTarget.OnDamage(att, unitStat.knockBackForce);      //�������� �ش�.
                    Managers.Sound.Play("Sounds/Effect/WarriorAttack");
                    GameObject eff = Managers.Resource.Load<GameObject>("Prefabs/Effect/HitEff");
                    Vector2 randomPos = RandomPosSetting(monTarget.transform.position);
                    if (eff != null)
                        Instantiate(eff, randomPos, Quaternion.identity);

                }
            }
            else
            {
                monsterPortal.TowerDamage(att);
                Managers.Sound.Play("Sounds/Effect/WarriorAttack");
                GameObject eff = Managers.Resource.Load<GameObject>("Prefabs/Effect/HitEff");
                Vector2 randomPos = RandomPosSetting(monsterPortal.transform.position);
                if (eff != null)
                    Instantiate(eff, randomPos, Quaternion.identity);
            }

        }

        else if(unitClass == UnitClass.Archer)
        {

            if(!towerAttack)
            {
                if(monTarget != null)
                {
                    GameObject obj = Resources.Load<GameObject>("Prefabs/Weapon/UnitArrow");

                    if (obj != null)
                    {
                        Managers.Sound.Play("Sounds/Effect/Bow");
                        GameObject arrow = Instantiate(obj, arrowPos.position, Quaternion.identity, this.transform);
                        arrow.TryGetComponent(out ArrowCtrl arrowCtrl);
                        arrowCtrl.SetType(monTarget, null);
                    }
                }
            }
            else
            {
                GameObject obj = Resources.Load<GameObject>("Prefabs/Weapon/UnitArrow");

                if (obj != null)
                {
                    Managers.Sound.Play("Sounds/Effect/Bow");
                    GameObject arrow = Instantiate(obj, arrowPos.position, Quaternion.identity, this.transform);
                    arrow.TryGetComponent(out ArrowCtrl arrowCtrl);
                    arrowCtrl.SetType(null, monsterPortal);
                }
            }

            


        
        }

        if (unitClass == UnitClass.Spear)
        {
            if (!towerAttack)
            {
                if (monTarget != null)
                {
                    monTarget.OnDamage(att, unitStat.knockBackForce);      //�������� �ش�.
                    Managers.Sound.Play("Sounds/Effect/WarriorAttack");
                    GameObject eff = Managers.Resource.Load<GameObject>("Prefabs/Effect/HitEff");
                    Vector2 randomPos = RandomPosSetting(monTarget.transform.position);

                    if (eff != null)
                        Instantiate(eff, randomPos, Quaternion.identity);

                }
            }
            else
            {
                monsterPortal.TowerDamage(att);
                Managers.Sound.Play("Sounds/Effect/WarriorAttack");
                GameObject eff = Managers.Resource.Load<GameObject>("Prefabs/Effect/HitEff");
                Vector2 randomPos = RandomPosSetting(monsterPortal.transform.position);

                if (eff != null)
                    Instantiate(eff, monsterPortal.transform.position, Quaternion.identity);
            }

        }

    }

    void ApplyKnockBack(Vector2 dir , float force)
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
       
        yield return new WaitForSeconds(knockbackDuration); // �˹� ���� �ð�


        while (rigbody.velocity.magnitude > 0.1f)
        {
            rigbody.velocity *= 0.96f;
            yield return null;
        }
        //rigbody.velocity *= 0.5f;


        state = UnitState.Run;
    }

    void Trace<T>(T obj) where T : UnityEngine.Component 
    {

        Vector3 vec = obj.gameObject.transform.position - this.transform.position;
        float distance = vec.magnitude;
        Vector3 dir = vec.normalized;
        if (unitClass == UnitClass.Archer)
        {

            if (distance < archerAttDis)
            {

                SetUnitState(UnitState.Attack);
            }
            else
            {
                rigbody.transform.position += dir * moveSpeed * Time.deltaTime;
                SetUnitState(UnitState.Trace);
            }

        }
        else if (unitClass == UnitClass.Warrior)
        {

            if (distance < 1.5f)
            {
                SetUnitState(UnitState.Attack);
            }
            else
            {
                rigbody.transform.position += dir * moveSpeed * Time.deltaTime;
                SetUnitState(UnitState.Trace);

            }

        }

        else if (unitClass == UnitClass.Spear)
        {

            if (distance < 2.0f)
            {
                SetUnitState(UnitState.Attack);
            }
            else
            {
                rigbody.transform.position += dir * moveSpeed * Time.deltaTime;
                SetUnitState(UnitState.Trace);

            }

        }







    }


    void UnitVictory()
    {
        if(Managers.Game.State == GameState.GameVictory)
        {
            isRun = false;
            anim.SetBool("Run", isRun);
            isAtt = false;
            anim.SetBool("Attack", isAtt);
            isTargeting = false;
        }
    }



    public override void AttackDelay()
    {
        if(attackCoolTime > 0.0f)
        {
            attackCoolTime -= Time.deltaTime;
            if (attackCoolTime <= .0f)
            {
                SetUnitState(UnitState.Run);
            }

        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(pos.position, boxSize);
    }
}
