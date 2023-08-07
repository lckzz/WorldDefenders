using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


enum UnitClass
{
    Warrior,
    Archer,
    Spear,
    Count
}


enum UnitState
{
    Idle,
    Run,
    Trace,
    Attack,
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

    Rigidbody2D rig;
    MonsterController[] monCtrls;  //�����ȿ� ���� ������ �������� ��Ƶ�
    MonsterController monCtrl;  //���͵��� �������߿��� ���� ���ְ� ����� ���������� �޾ƿ�
    Collider2D unitColl2d;
    MonsterPortal monsterPortal;


    public bool IsDie { get { return isDie; } }
    public int Att { get { return att; } }
    public bool IsTargering { get { return isTargeting; } }
    public MonsterController Monctrl { get { return monCtrl; } }

    public void IsTargetingSet(bool value) => isTargeting = value;
    public MonsterPortal MonsterPortal { get { return monsterPortal; } }


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
        TryGetComponent<Rigidbody2D>(out rig);
        TryGetComponent<Animator>(out anim);
        TryGetComponent<Collider2D>(out unitColl2d);
        if (unitClass == UnitClass.Archer)
            arrowPos = transform.Find("ArrowPos");
        else
            arrowPos = null;


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
        UnitMovement();
        AttackDelay();
        UnitVictory();

        //������ ������ 
    }



    public override void EnemySensor()      //������
    {

        //Debug.Log(isTargeting);
        //Debug.Log($"Ÿ����{isTageting}");
        if (!isTargeting)      //�ݶ��̴� ������ ������(���Ŀ� ���� �������� ���Ͱ� ������)
        {
            coll2d = Physics2D.OverlapBoxAll(pos.position, boxSize, 0, LayerMask.GetMask("Monster"));
            towerColl = Physics2D.OverlapBox(pos.position, boxSize, 0, LayerMask.GetMask("MonsterPortal"));
            if (coll2d != null)
            {
                monCtrls = new MonsterController[coll2d.Length];
                //üũ�ڽ��ȿ� ���� �ݶ��̴��߿��� ���� ���ְ��� �Ÿ��� ���� ����� ���� ��󳻱�
                for(int ii = 0; ii < coll2d.Length;ii++)
                {
                    coll2d[ii].TryGetComponent<MonsterController>(out monCtrls[ii]);
                   // monCtrls[ii] = coll2d[ii].GetComponent<MonsterController>();
                }
               
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
                            float distA = (monCtrls[i].transform.position - this.transform.position).magnitude;
                            float distB = (monCtrls[i + 1].transform.position - this.transform.position).magnitude;

                            if (distA > distB)
                            {
                                disMin = distB;
                                min = i + 1;
                            }
                            else
                            {
                                disMin = distA;
                                min = i;
                            }
                        }

                        else if (i < monCtrls.Length - 1)
                        {
                            float distB = (monCtrls[i + 1].transform.position - this.transform.position).magnitude;

                            if (disMin > distB)
                            {
                                disMin = distB;
                                min = i + 1;
                            }


                        }

                    }
                }
                

                if (monCtrls.Length != 0)
                {
                    isTargeting = true;
                    isUnitTarget = true;
                    monCtrl = monCtrls[min];
                }


            }

            else
            {
                //�����ȿ� ������ �������� �ʰ� Ÿ���� �����ϸ�
                if (towerColl != null)
                {

                    //Debug.Log("�� Ÿ������");
                    isTargeting = true;     //Ÿ������ ����ְ�
                    isTowerTarger = true;
                    towerColl.gameObject.TryGetComponent<MonsterPortal>(out monsterPortal);  //�÷��̾�Ÿ���� ������ �޾ƿ�

                }
            }
        }

    }


  


    void UnitMovement()
    {
        switch (state)
        {
            case UnitState.Run:
                if (isTargeting)
                {
                    state = UnitState.Trace;
                    break;
                }

                if (isAtt)
                {
                    isAtt = false;
                    anim.SetBool("Attack", isAtt);
                }

                if (!isRun)
                {
                    isRun = true;
                    anim.SetBool("Run", isRun);

                }
                transform.position += Vector3.right * moveSpeed * Time.deltaTime;



                break;

            case UnitState.Trace:
                #region ���ָ� ����������
                //if (isTargeting == false)
                //{
                //    state = UnitState.Run;
                //    monCtrl = null;
                //}

                //if(monCtrl != null)
                //{
                //    Vector3 vec = monCtrl.gameObject.transform.position - this.transform.position;
                //    float distance = vec.magnitude;
                //    Vector3 dir = vec.normalized;
                //    transform.position += dir * moveSpeed * Time.deltaTime;
                //    //Debug.Log(distance);
                //    if (unitClass == UnitClass.Archer)
                //    {

                //        if (distance < archerAttDis)
                //        {
                //            isRun = false;
                //            anim.SetBool("Run", isRun);
                //            state = UnitState.Attack;
                //        }
                //    }
                //    else if (unitClass == UnitClass.Warrior)
                //    {
                //        //Debug.Log(hit.distance);

                //        if (distance < 1.5f)
                //        {
                //            isRun = false;
                //            anim.SetBool("Run", isRun);
                //            state = UnitState.Attack;
                //        }
                //    }
                //}

                //else  //�߰ݻ����ε� ���������� ���ٸ�
                //{
                //    isTargeting = false;
                //    state = UnitState.Run;
                //    monCtrl = null;
                //}
                #endregion
                if (isUnitTarget)        //������ Ÿ�����̵Ǹ�
                    Trace(monCtrl);
                else if (isTowerTarger)
                    Trace(monsterPortal);

                break;
            case UnitState.Attack:
                if (attackCoolTime > .0f)
                    return;

                if (!isAtt)
                {
                    isAtt = true;
                    anim.SetBool("Attack", isAtt);

                }
                if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
                {
                    if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)
                    {
                        isAtt = false;
                        anim.SetBool("Attack", isAtt);

                        attackCoolTime = .5f;

                        //������ ������ ��� Ÿ������ �ٽ� ��´�.
                        isTargeting = false;
                        isUnitTarget = false;
                        isTowerTarger = false;

                        state = UnitState.Run;
                        isRun = false;

                        isAtt = false;
                        anim.SetBool("Attack", isAtt);
                        isRun = true;
                        anim.SetBool("Run", isRun);
                    }

                    
                }




                break;

            case UnitState.Die:
                if(!isDie)
                {
                    isDie = true;
                    anim.SetTrigger("Die");
                   
                }

                break;


        }// �ڽ��ȿ� ���� ������ ������ �޾ƿ´�.
    }

    public void OnDamage(int att)
    {
        if (monCtrl == null)
            return;



        if (hp > 0)
        {
            hp -= att;
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
            if (monCtrl != null)
            {
                monCtrl.OnDamage(att);      //�������� �ش�.

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


    void Trace<T>(T obj) where T : UnityEngine.Component 
    {

        if (!isTargeting)  //Ÿ������ ���������� 
        {
            state = UnitState.Run;   //�޸�����·� ����
            isRun = false;
            return;

        }


        Vector3 vec = obj.gameObject.transform.position - this.transform.position;
        float distance = vec.magnitude;
        Vector3 dir = vec.normalized;
        if (unitClass == UnitClass.Archer)
        {

            if (distance < archerAttDis)
            {
                isRun = false;
                anim.SetBool("Run", isRun);
                state = UnitState.Attack;
            }
            else
            {
                transform.position += dir * moveSpeed * Time.deltaTime;
                isRun = true;
                anim.SetBool("Run", isRun);
            }

        }
        else if (unitClass == UnitClass.Warrior)
        {

            if (distance < 1.5f)
            {
                isRun = false;
                anim.SetBool("Run", isRun);
                state = UnitState.Attack;
            }
            else
            {
                transform.position += dir * moveSpeed * Time.deltaTime;
                isRun = true;
                anim.SetBool("Run", isRun);
            }

        }

        isTargeting = false;  //Ÿ���� ����ؼ� Ÿ������ Ǯ�� ������ ���� ������ �ִ��� Ȯ���Ѵ�.


        


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
