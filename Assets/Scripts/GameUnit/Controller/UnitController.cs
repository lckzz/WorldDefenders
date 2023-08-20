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
    public bool IsTargering { get { return isTargeting; } }
    public MonsterController Monctrl { get { return monTarget; } }

    public void IsTargetingSet(bool value) => isTargeting = value;
    public MonsterPortal MonsterPortal { get { return monsterPortal; } }

    public UnitState UniState { get { return state; } }


    //��ó ����
    Transform arrowPos;

    //��ó ����



    public float hpPercent()
    {
        return hp / maxHp;
    }

    void Init()
    {
        hp = 100;
        att = 15;
        moveSpeed = 2.5f;
        archerAttDis = 5.0f;
        maxHp = hp;
        isTargeting = false;
        isUnitTarget = false;
        isTowerTarger = false;

        //rig = this.GetComponent<Rigidbody2D>();
        //anim = GetComponent<Animator>();
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
        EnemySensor();
        UnitStateCheck();
        AttackDelay();
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
        towerColl = Physics2D.OverlapBox(pos.position, boxSize, 0, LayerMask.GetMask("MonsterPortal"));
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

        else
        {
            //�����ȿ� ������ �������� �ʰ� Ÿ���� �����ϸ�
            if (towerColl != null)
            {

                //Debug.Log("�� Ÿ������");
                towerColl.gameObject.TryGetComponent<MonsterPortal>(out monsterPortal);  //�÷��̾�Ÿ���� ������ �޾ƿ�

            }
        }
        


#endregion

    }


    void UnitStateCheck()
    {
        switch (state)
        {
            case UnitState.Idle:
                {

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
                    ApplyKnockBack(new Vector2(-1.0f,1.0f),5.5f);
                    break;
                }
            case UnitState.Die:
                {

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
                    if (!isIdle)
                    {

                        isIdle = true;
                        anim.SetBool("Idle", isIdle);
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


    void UnitMove()
    {
        if (monTarget != null || towerColl != null)
            SetUnitState(UnitState.Trace);



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

        else if(towerColl != null)
            Trace(towerColl);
    }

    bool IsTargetOn()
    {
        if (monTarget == null)
            return false;

        if (monTarget.MonState == MonsterState.Die)
            return false;

        if(!monTarget.gameObject.activeInHierarchy)
            return false;


        return true;
    }


    void UnitAttack()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)
            {
                SetUnitState(UnitState.Run);
            }


        }
    }


   

    public void OnDamage(int att)
    {
        if (monTarget == null)
            return;



        if (hp > 0)
        {
            hp -= att;
            SetUnitState(UnitState.KnockBack);

            if (hp <= 0)
            {
                Debug.Log(hp);

                hp = 0;
                OnDead();
            }
        }
    }

    public void OnAttack()    //�ִϸ��̼� �̺�Ʈ �Լ�
    {
        if(unitClass == UnitClass.Warrior)
        {
            if (monTarget != null)
            {
                monTarget.OnDamage(att);      //�������� �ش�.

            }

            if (monsterPortal != null)
            {
                Debug.Log("����!");
                monsterPortal.TowerDamage(att);
            }
        }

        else if(unitClass == UnitClass.Archer)
        {

            if (isTargeting)
            {
                GameObject obj = Resources.Load<GameObject>("Prefabs/Weapon/UnitArrow");

                if (obj != null)
                {
                    GameObject arrow = Instantiate(obj, arrowPos.position, Quaternion.identity, this.transform);
                }
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

    public void OnDead()
    {
        if (unitColl2d.enabled)
        {
            state = UnitState.Die;
            unitColl2d.enabled = false;
            GameObject.Destroy(gameObject, 5.0f);

        }
    }

    public override void AttackDelay()
    {
        if(attackCoolTime > 0.0f)
        {
            attackCoolTime -= Time.deltaTime;
            if (attackCoolTime <= .0f)
                attackCoolTime = .0f;

        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(pos.position, boxSize);
    }
}
