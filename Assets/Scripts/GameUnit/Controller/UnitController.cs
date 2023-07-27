using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


enum UnitClass
{
    Warrior,
    Archer
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
    private UnitState state = UnitState.Run;        //테스트용이라 런 

    Rigidbody2D rig;
    MonsterController[] monCtrls;  //범위안에 들어온 몬스터의 정보들을 모아둠
    MonsterController monCtrl;  //몬스터들의 정보들중에서 제일 유닛과 가까운 몬스터정보를 받아옴
    Collider2D unitColl2d;
    MonsterPortal monsterPortal;


    public bool IsDie { get { return isDie; } }
    public int Att { get { return att; } }
    public bool IsTargering { get { return isTargeting; } }
    public MonsterController Monctrl { get { return monCtrl; } }

    public void IsTargetingSet(bool value) => isTargeting = value;
    public MonsterPortal MonsterPortal { get { return monsterPortal; } }


    //아처 전용
    Transform arrowPos;

    //아처 전용



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


        UnitMovement();
        EnemySensor();



        //앞으로 움직임 
    }



    public override void EnemySensor()      //적감지
    {

        //Debug.Log(isTargeting);
        //Debug.Log($"타겟팅{isTageting}");
        if (!isTargeting || monCtrl == null)      //콜라이더 정보가 없을때(추후에 현재 추적중인 몬스터가 없으면)
        {
            coll2d = Physics2D.OverlapBoxAll(pos.position, boxSize, 0, LayerMask.GetMask("Monster"));
            towerColl = Physics2D.OverlapBox(pos.position, boxSize, 0, LayerMask.GetMask("MonsterPortal"));
            if (coll2d != null)
            {
                monCtrls = new MonsterController[coll2d.Length];
                //체크박스안에 들어온 콜라이더중에서 현재 유닛과의 거리가 제일 가까운 것을 골라내기
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

                    else if(i < monCtrls.Length - 1)
                    {
                        float distB = (monCtrls[i + 1].transform.position - this.transform.position).magnitude;

                        if (disMin > distB)
                        {
                            disMin = distB;
                            min = i + 1;
                        }
                      

                    }

                }

                if (monCtrls.Length != 0)
                {
                    isTargeting = true;
                    monCtrl = monCtrls[min];
                    monsterPortal = null;
                }


            }

            else
            {
                //범위안에 유닛이 존재하지 않고 타워가 존재하면
                if (towerColl != null)
                {

                    //Debug.Log("헉 타워감지");
                    isTargeting = true;     //타겟팅을 잡아주고
                    towerColl.gameObject.TryGetComponent<MonsterPortal>(out monsterPortal);  //플레이어타워의 정보를 받아옴

                }
            }
        }

    }


  


    void UnitMovement()
    {
        switch (state)
        {
            case UnitState.Run:
                if(isAtt)
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


                if (isTargeting)
                    state = UnitState.Trace;
                break;

            case UnitState.Trace:
                #region 유닛만 구현했을때
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

                //else  //추격상태인데 몬스터정보가 없다면
                //{
                //    isTargeting = false;
                //    state = UnitState.Run;
                //    monCtrl = null;
                //}
                #endregion
                Trace(monCtrl, monsterPortal);

                break;
            case UnitState.Attack:

                //if (!isTargeting || monCtrl == null)
                //{
                //    isRun = false;
                //    state = UnitState.Run;
                //    monCtrl = null;

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
                }

                break;

            case UnitState.Die:
                if(!isDie)
                {
                    isDie = true;
                    anim.SetTrigger("Die");
                   
                }
                if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                    OnDead();

                break;


        }// 박스안에 들어온 몬스터의 정보를 받아온다.
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
                state = UnitState.Die;
            }
        }
    }

    public void OnAttack()    //애니메이션 이벤트 함수
    {
        if(unitClass == UnitClass.Warrior)
        {
            if (monCtrl != null)
            {
                monCtrl.OnDamage(att);
                if (monCtrl.IsDie)
                {
                    //몬스터가 죽었다면
                    isTargeting = false;
                    isRun = false;
                    state = UnitState.Run;
                    monCtrl = null;
                }

            }

            else if (monsterPortal != null)
            {
                Debug.Log("공격!");
                monsterPortal.TowerDamage(att);
            }
        }

        else if(unitClass == UnitClass.Archer)
        {
            if (monCtrl != null)
            {
                if (monCtrl.IsDie)
                {
                    //몬스터가 죽었다면
                    isTargeting = false;
                    isRun = false;
                    state = UnitState.Run;
                    monCtrl = null;
                }

            }


            if (isTargeting)
            {
                GameObject obj = Resources.Load<GameObject>("Prefab/Weapon/UnitArrow");

                if (obj != null)
                {
                    GameObject arrow = Instantiate(obj, arrowPos.position, Quaternion.identity, this.transform);
                }
            }


        
        }

    }


    void Trace<T, T1>(T mon, T1 tower) where T : MonsterController where T1 : MonsterPortal
    {
        if (mon != null)  //유닛이 있다면
        {
            if (!isTargeting)
            {
                state = UnitState.Run;
                mon = null;
            }

            if (mon != null)
            {
                Vector3 vec = mon.gameObject.transform.position - this.transform.position;
                float distance = vec.magnitude;
                Vector3 dir = vec.normalized;
                transform.position += dir * moveSpeed * Time.deltaTime;
                if (unitClass == UnitClass.Archer)
                {

                    if (distance < archerAttDis)
                    {
                        isRun = false;
                        anim.SetBool("Run", isRun);
                        state = UnitState.Attack;
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
                }
            }
            else  //추격상태인데 몬스터정보가 없다면
            {
                isTargeting = false;
                state = UnitState.Run;
                mon = null;
            }
        }

        else if (tower != null)
        {
            if (!isTargeting)
            {
                state = UnitState.Run;
                tower = null;
            }

            if (tower != null)
            {
                Vector3 vec = tower.gameObject.transform.position - this.transform.position;
                float distance = vec.magnitude;
                Vector3 dir = vec.normalized;
                transform.position += dir * moveSpeed * Time.deltaTime;
                if (unitClass == UnitClass.Archer)
                {

                    if (distance < archerAttDis)
                    {
                        isRun = false;
                        anim.SetBool("Run", isRun);
                        state = UnitState.Attack;
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
                }
            }
            //else  //추격상태인데 몬스터정보가 없다면
            //{
            //    isTargeting = false;
            //    state = MonsterState.Run;
            //    controller = null;
            //}
            isTargeting = false;  //타워는 계속해서 타겟팅을 풀고 센서를 통해 유닛이 있는지 확인한다.


        }


    }


    void UnitVictory()
    {
        if(GameManager.instance.state == GameState.GameVictory)
        {
            isAtt = false;
            anim.SetBool("Attack", isAtt);
            isTargeting = false;
        }
    }

    public void OnDead()
    {
        if (unitColl2d.enabled)
        {
            unitColl2d.enabled = false;
            GameObject.Destroy(gameObject, 5.0f);

        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(pos.position, boxSize);
    }
}
