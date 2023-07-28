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

enum MonsterState
{
    Idle,
    Run,
    Trace,
    Attack,
    Die
}

public class MonsterController : Unit
{
    [SerializeField]
    private Animator anim;
    [SerializeField]
    private MonsterClass monsterClass;
    [SerializeField]
    private MonsterState state = MonsterState.Run;        //�׽�Ʈ���̶� �� 
    Rigidbody2D rig;


    UnitController[] unitctrls;
    UnitController unitctrl;
    PlayerTower playerTowerCtrl;
    

    Collider2D monsterColl;     //�׾��� �� �ݶ��̴� �������� ����

    //��ó ����
    Transform arrowPos;

    //��ó ����




    public bool IsDie { get { return isDie; } }
    public float Hp { get { return hp; } }
    public int Att { get { return att; } }

    public bool IsTargering { get { return isTargeting; } }

    public UnitController UnitCtrl { get { return unitctrl; } }
    public PlayerTower PlayerTowerCtrl { get { return playerTowerCtrl; } }


    void Init()
    {
        if(att == 0)
            att = 20;

        hp = 100;
        moveSpeed = 2.0f;
        archerAttDis = 5.0f;
        maxHp = hp;

        TryGetComponent<Rigidbody2D>(out rig);
        TryGetComponent<Animator>(out anim);
        TryGetComponent<Collider2D>(out monsterColl);
        if (monsterClass == MonsterClass.Archer)
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
        if(GameManager.instance.state == GameState.GamePlaying)
        {
            EnemySensor();
            MonsterMovement();

        }

        MonsterVictory();

    }



    public float hpPercent()
    {
        return hp / maxHp;
    }

    public void OnDamage(int att)
    {

        if(hp > 0)
        {
            hp -= att;
            if(hp <= 0)
            {
                hp = 0;
                state = MonsterState.Die;

            }
        }
    }


    public void OnDead()
    {
        if(monsterColl.enabled)
        {
            monsterColl.enabled = false;
            GameObject.Destroy(gameObject, 5.0f);

        }


    }

    public override void EnemySensor()      //������
    {
        if (!isTargeting || unitctrl == null)      //�ݶ��̴� ������ ������(���Ŀ� ���� �������� ���Ͱ� ������)
        {
            coll2d = Physics2D.OverlapBoxAll(pos.position, boxSize, 0, LayerMask.GetMask("Unit"));
            towerColl = Physics2D.OverlapBox(pos.position, boxSize, 0, LayerMask.GetMask("Tower"));
            if (coll2d != null)     //���ֵ��� �����ߴٸ�
            {
                unitctrls = new UnitController[coll2d.Length];
                //üũ�ڽ��ȿ� ���� �ݶ��̴��߿��� ���� ���ְ��� �Ÿ��� ���� ����� ���� ��󳻱�
                for (int ii = 0; ii < coll2d.Length; ii++)
                {
                    coll2d[ii].TryGetComponent<UnitController>(out unitctrls[ii]);
                    // monCtrls[ii] = coll2d[ii].GetComponent<MonsterController>();
                }

            }

            if (unitctrls.Length > 0)   //(�����ȿ� ������ �����ϸ�)�����ȿ� ���� ��ü�߿��� ���� ����� �� ����
            {

                float disMin = 0;
                int min = 0;

                for (int i = 0; i < unitctrls.Length; i++)
                {
                    if (i == 0 && unitctrls.Length > 1)
                    {
                        float distA = (unitctrls[i].transform.position - this.transform.position).magnitude;
                        float distB = (unitctrls[i + 1].transform.position - this.transform.position).magnitude;

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

                    else if (i < unitctrls.Length - 1)
                    {
                        float distB = (unitctrls[i + 1].transform.position - this.transform.position).magnitude;

                        if (disMin > distB)
                        {
                            disMin = distB;
                            min = i + 1;
                        }


                    }

                }

                if (unitctrls.Length != 0)  //���� ����� ������ Ÿ������ ��´�.
                {
                    isTargeting = true;
                    unitctrl = unitctrls[min];
                    playerTowerCtrl = null;
                }


            }

            else
            {
                //�����ȿ� ������ �������� �ʰ� Ÿ���� �����ϸ�
                if(towerColl != null)
                {
                    
                    //Debug.Log("�� Ÿ������");
                    isTargeting = true;     //Ÿ������ ����ְ�
                    towerColl.gameObject.TryGetComponent<PlayerTower>(out playerTowerCtrl);  //�÷��̾�Ÿ���� ������ �޾ƿ�
                }
            }
        }

    }


    private void MonsterMovement()
    {
        switch (state)
        {
            case MonsterState.Run:
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
                transform.position += Vector3.left * moveSpeed * Time.deltaTime;

                if (isTargeting)
                    state = MonsterState.Trace;
                break;

            case MonsterState.Trace:
                #region ���ָ� �߰��� �ڵ�
                //if (!isTargeting)
                //{
                //    state = MonsterState.Run;
                //    unitctrl = null;
                //}

                //if (unitctrl != null)
                //{
                //    Vector3 vec = unitctrl.gameObject.transform.position - this.transform.position;
                //    float distance = vec.magnitude;
                //    Vector3 dir = vec.normalized;
                //    transform.position += dir * moveSpeed * Time.deltaTime;
                //    if (monsterClass == MonsterClass.Archer)
                //    {

                //        if (distance < archerAttDis)
                //        {
                //            isRun = false;
                //            anim.SetBool("Run", isRun);
                //            state = MonsterState.Attack;
                //        }
                //    }
                //    else if (monsterClass == MonsterClass.Warrior)
                //    {

                //        if (distance < 1.5f)
                //        {
                //            isRun = false;
                //            anim.SetBool("Run", isRun);
                //            state = MonsterState.Attack;
                //        }
                //    }
                //}
                //else  //�߰ݻ����ε� ���������� ���ٸ�
                //{
                //    isTargeting = false;
                //    state = MonsterState.Run;
                //    unitctrl = null;
                //}

                #endregion
                Trace(unitctrl, playerTowerCtrl);

                break;
            case MonsterState.Attack:
                //if (!isTargeting)
                //{
                //    isRun = false;
                //    state = MonsterState.Run;
                //    unitctrl = null;

                //    break;
                //}

                if (!isAtt)
                {
                    isAtt = true;
                    anim.SetBool("Attack", isAtt);

                }
                if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)
                {
                    isAtt = false;
                    anim.SetBool("Attack", isAtt);


                    //if (playerTowerCtrl != null || unitctrl == null)
                    //{
                    //    isTargeting = false;
                    //    playerTowerCtrl = null;
                    //    unitctrl = null;
                    //}
                }


                break;


            case MonsterState.Die:
                if (!isDie)
                {
                    isDie = true;
                    anim.SetTrigger("Die");
                }
                if(anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                    OnDead();


                break;

        }
    }


    public void OnAttack()
    {

        if (monsterClass == MonsterClass.Warrior)
        {
            if (unitctrl != null)
            {
                unitctrl.OnDamage(att);
                if (unitctrl.IsDie)
                {
                    //���Ͱ� �׾��ٸ�
                    isTargeting = false;
                    isRun = false;
                    state = MonsterState.Run;
                    unitctrl = null;
                }

            }
            else if(playerTowerCtrl != null)
            {
 
                playerTowerCtrl.TowerDamage(att);
            }
        }

        else if (monsterClass == MonsterClass.Archer)
        {
            if (unitctrl != null)
            {
                if (unitctrl.IsDie)
                {
                    //���Ͱ� �׾��ٸ�
                    isTargeting = false;
                    isRun = false;
                    state = MonsterState.Run;
                    unitctrl = null;
                }

            }

            if (isTargeting)
            {
                GameObject obj = Resources.Load<GameObject>("Prefab/Weapon/MonsterArrow");

                if (obj != null)
                {
                    GameObject arrow = Instantiate(obj, arrowPos.position, Quaternion.identity, this.transform);
                }
            }



        }

    }



    void Trace<T,T1>(T unit, T1 tower) where T : UnitController where T1 : PlayerTower
    {
        if (unit != null)  //������ �ִٸ�
        {
            if (!isTargeting)
            {
                state = MonsterState.Run;
                unit = null;
            }

            if (unit != null)
            {
                Vector3 vec = unit.gameObject.transform.position - this.transform.position;
                float distance = vec.magnitude;
                Vector3 dir = vec.normalized;
                transform.position += dir * moveSpeed * Time.deltaTime;
                if (monsterClass == MonsterClass.Archer)
                {

                    if (distance < archerAttDis)
                    {
                        isRun = false;
                        anim.SetBool("Run", isRun);
                        state = MonsterState.Attack;
                    }
                }
                else if (monsterClass == MonsterClass.Warrior)
                {

                    if (distance < 1.5f)
                    {
                        isRun = false;
                        anim.SetBool("Run", isRun);
                        state = MonsterState.Attack;
                    }
                }
            }
            else  //�߰ݻ����ε� ���������� ���ٸ�
            {
                isTargeting = false;
                state = MonsterState.Run;
                unit = null;
            }
        }

        else if(tower != null)
        {
            if (!isTargeting)
            {
                state = MonsterState.Run;
                tower = null;
            }

            if (tower != null)
            {
                Vector3 vec = tower.gameObject.transform.position - this.transform.position;
                float distance = vec.magnitude;
                Vector3 dir = vec.normalized;
                transform.position += dir * moveSpeed * Time.deltaTime;
                if (monsterClass == MonsterClass.Archer)
                {

                    if (distance < archerAttDis)
                    {
                        isRun = false;
                        anim.SetBool("Run", isRun);
                        state = MonsterState.Attack;
                    }
                }
                else if (monsterClass == MonsterClass.Warrior)
                {

                    if (distance < 1.5f)
                    {
                        isRun = false;
                        anim.SetBool("Run", isRun);
                        state = MonsterState.Attack;
                    }
                }
            }
            //else  //�߰ݻ����ε� ���������� ���ٸ�
            //{
            //    isTargeting = false;
            //    state = MonsterState.Run;
            //    controller = null;
            //}
            isTargeting = false;  //Ÿ���� ����ؼ� Ÿ������ Ǯ�� ������ ���� ������ �ִ��� Ȯ���Ѵ�.


        }


    }


    void MonsterVictory()
    {
        if(GameManager.instance.state == GameState.GameFail)
        {
            isRun = true;
            anim.SetBool("Run", isRun);
            isAtt = false;
            anim.SetBool("Attack", isAtt);
            isTargeting = false;

            state = MonsterState.Run;

            transform.position += Vector3.left * moveSpeed * Time.deltaTime;

        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(pos.position, boxSize);
    }
}
