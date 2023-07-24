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

public class UnitController : Unit,ISensor
{
    [SerializeField]
    private Animator anim;

    [SerializeField]
    private UnitClass unitClass;

    private UnitState state = UnitState.Run;        //테스트용이라 런 

    float warriorRay = 0.7f;
    float archerRay = 3.0f;


    Rigidbody2D rig;
    
    RaycastHit2D hit;
    AnimatorStateInfo animState;
    MonsterController[] monCtrls;  //범위안에 들어온 몬스터의 정보들을 모아둠
    MonsterController monCtrl;  //몬스터들의 정보들중에서 제일 유닛과 가까운 몬스터정보를 받아옴
    public MonsterController MonCtrl { get { return monCtrl; } set { monCtrl = value; } }

    public float hpPercent()
    {
        return hp / MaxHp;
    }

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        //if(unitClass == UnitClass.Warrior)
        //{
        //    hit = Physics2D.Raycast(rig.position, Vector3.right, warriorRay, LayerMask.GetMask("Monster"));
        //    Debug.DrawRay(rig.position, Vector3.right * warriorRay, Color.red);

        //}
        //else if(unitClass == UnitClass.Archer)
        //{
        //    hit = Physics2D.Raycast(rig.position, Vector3.right, archerRay, LayerMask.GetMask("Monster"));
        //    Debug.DrawRay(rig.position, Vector3.right * archerRay, Color.red);

        //}

        UnitMovement();
        EnemySensor();
       

        //앞으로 움직임 
    }

    void Init()
    {
        hp = 100;
        att = 15;
        moveSpeed = 2.5f;
        archerAttDis = 5.0f;

        rig = this.GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }


    public void EnemySensor()      //적감지
    {

        Debug.Log($"타겟팅{isTageting}");
        if (!isTageting)      //콜라이더 정보가 없을때(추후에 현재 추적중인 몬스터가 없으면)
        {
            coll2d = Physics2D.OverlapBoxAll(pos.position, boxSize, 0, LayerMask.GetMask("Monster"));
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

            if(monCtrls != null)
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
                    isTageting = true;
                    monCtrl = monCtrls[min];
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


                if (isTageting)
                    state = UnitState.Trace;
                break;

            case UnitState.Trace:
                if (isTageting == false)
                {
                    state = UnitState.Run;
                    monCtrl = null;
                }

                if(monCtrl != null)
                {
                    Vector3 vec = monCtrl.gameObject.transform.position - this.transform.position;
                    float distance = vec.magnitude;
                    Vector3 dir = vec.normalized;
                    transform.position += dir * moveSpeed * Time.deltaTime;
                    //Debug.Log(distance);
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
                        //Debug.Log(hit.distance);

                        if (distance < 1.5f)
                        {
                            isRun = false;
                            anim.SetBool("Run", isRun);
                            state = UnitState.Attack;
                        }
                    }
                }
                break;
            case UnitState.Attack:

                if (!isTageting || monCtrl == null)
                {
                    isRun = false;
                    state = UnitState.Run;
                    monCtrl = null;
                }

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
                    anim.SetTrigger("Die");
                   
                }
                isDie = true;
                break;


        }// 박스안에 들어온 몬스터의 정보를 받아온다.
    }

    public void OnDamage(int att)
    {
        if (monCtrl == null)
            return;

        if(hp > 0)
        {
            hp -= att;
            if (hp < 0)
            {
                hp = 0;
                state = UnitState.Die;
            }
        }
    }

    public void OnAttack()    //애니메이션 이벤트 함수
    {
        if (monCtrl != null)
        {
            monCtrl.OnDamage(att);
            if(monCtrl.IsDie)
            {
                //몬스터가 죽었다면
                isTageting = false;
            }

        }
    }


    public void OnDead()
    {
        if(state == UnitState.Die)
        {

        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(pos.position, boxSize);
    }
}
