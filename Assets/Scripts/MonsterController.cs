using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.Burst.CompilerServices;
using UnityEngine;

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

public class MonsterController : MonoBehaviour
{

    private float hp = 100;       //���߿� json���ϵ����ͷ� ���� �޾ƿ� ���� ������ �ϵ��ڵ�(�׽�Ʈ��)
    private float Maxhp;      //���߿� json���ϵ����ͷ� ���� �޾ƿ� ���� ������ �ϵ��ڵ�(�׽�Ʈ��)

    private int att = 25;

    [SerializeField]
    private MonsterClass monsterClass;
    private MonsterState state = MonsterState.Run;        //�׽�Ʈ���̶� �� 

    float moveSpeed = 1.0f;
    bool isRun = false;
    bool isAtt = false;
    
    bool isDie = false;

    public bool IsDie { get { return isDie; } }
    public float Hp { get { return hp; } }

    float warriorRay = 1.0f;
    float archerRay = 1.0f;

    Rigidbody2D rig;
    RaycastHit2D hit;
    float archerAttDis = 8.5f;
    [SerializeField]
    private Animator anim;


    Collider2D coll2d;
    Collider2D monsterColl;

    [SerializeField]
    Transform pos;
    [SerializeField]
    Vector2 boxSize;

    UnitController unitctrl;

    // Start is called before the first frame update
    void Start()
    {

        Maxhp = hp;
        rig = this.GetComponent<Rigidbody2D>();
        monsterColl = this.GetComponent<Collider2D>();

    }

    // Update is called once per frame
    void Update()
    {
        EnemySensor();

        switch (state)
        {
            case MonsterState.Run:
                if (!isRun)
                {
                    isRun = true;
                    anim.SetBool("Run", isRun);

                }
                transform.position += Vector3.left * moveSpeed * Time.deltaTime;
                if (coll2d != null)
                    state = MonsterState.Trace;
           
                break;

            case MonsterState.Trace:
                if (coll2d == null || unitctrl == null)
                {
                    state = MonsterState.Run;
                    coll2d = null;
                    unitctrl = null;
                }

                Vector3 vec = unitctrl.gameObject.transform.position - this.transform.position;
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


                break;
            case MonsterState.Attack:
                if (!isAtt)
                {
                    isAtt = true;
                    anim.SetBool("Attack", isAtt);

                }
                if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)
                {
                    if(isAtt)
                        unitctrl.OnDamage(att);
                    isAtt = false;
                    anim.SetBool("Attack", isAtt);
                }

                break;


            case MonsterState.Die:
                if(!isDie)
                {
                    isDie = true;
                    anim.SetTrigger("Die");
                }
                

                break;

        }
    }

    public float hpPercent()
    {
        Debug.Log(hp / Maxhp);
        return hp / Maxhp;
    }

    public void OnDamage(int att)
    {
        if (unitctrl == null)
            return;


        if(hp > 0)
        {
            hp -= att;
            if(hp < 0)
            {
                hp = 0;
                state = MonsterState.Die;
                OnDead();

            }
        }
    }


    public void OnDead()
    {
        monsterColl.enabled = false;
        GameObject.Destroy(gameObject, 5.0f);
    }

    void EnemySensor()      //������
    {
        if (coll2d == null)      //�ݶ��̴� ������ ������
        {
            coll2d = Physics2D.OverlapBox(pos.position, boxSize, 0, LayerMask.GetMask("Unit"));
            if (coll2d != null)
            {
                unitctrl = coll2d.GetComponent<UnitController>();     //���� �����ϸ� ���� ������ �޾ƿ´�.
            }
        }

    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(pos.position, boxSize);
    }
}
