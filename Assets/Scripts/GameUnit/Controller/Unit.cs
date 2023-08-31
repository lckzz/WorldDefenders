using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : MonoBehaviour,ISensor
{
    // Start is called before the first frame update
    [SerializeField]  protected float hp = 0;
    protected float maxHp = 0;
    [SerializeField]
    protected int att = 0;
    protected int knockbackForce = 0;
    protected int damageKnockBack = 0;
    protected float attackRange = .0f;
    protected float spawnPosX = 0;       //��ȯ����x�� ���Ϸδ� �˹����������


    protected float randomX = 0;
    protected float randomY = 0;

    protected float moveSpeed = .0f;
    protected bool isRun = false;
    protected bool isAtt = false;
    protected bool isDie = false;
    protected bool isIdle = false;

    [SerializeField]
    protected Animator anim; 
    protected Rigidbody2D rigbody;

    [SerializeField]
    protected float attackCoolTime = 1.5f;
    protected bool attackTime = true;      //�������� �ٵ� ����

    [SerializeField]
    protected Collider2D[] enemyColls2D;
    protected Collider2D tower;
    protected Collider2D unitColl;
    protected Collider2D myColl;     //�� �ڽ��� �ݶ��̴��� �޴� ����

    protected float unitDestroyTime = .0f;
    protected float traceDistance;



    //Ÿ�� ���� ����
    protected Vector2 towerVec;
    protected Vector2 towerDir;
    protected float towerDist;
    protected bool towerTrace = false;
    protected bool towerAttack = false;       //Ÿ���� ��������� 
    protected float towerAttackRange = 0.0f;

    //Ÿ�� ���� ����

    [SerializeField]
    protected Transform pos;
    [SerializeField]
    protected Vector2 boxSize;


    //�˹� ���� ����
    protected bool knockbackStart = false;
    protected float knockbackDuration = 0.25f;

    public bool IsDie { get { return isDie; } }
    public int Att { get { return att; } }

    public virtual void Init()
    {
        TryGetComponent<Animator>(out anim);
        TryGetComponent<Rigidbody2D>(out rigbody);
        TryGetComponent<Collider2D>(out myColl);
    }

    public float hpPercent()
    {
        return hp / maxHp;
    }

    public virtual void UnitDead()
    {
        if(unitDestroyTime > 0.0f)
        {
            unitDestroyTime -= Time.deltaTime;
            if(unitDestroyTime < 0.0f)
            {
                unitDestroyTime = .0f;

            }
        }
    }



    public abstract void EnemySensor(); //������ ����

    public abstract void AttackDelay();

    public abstract void OnAttack();

    public abstract void OnDamage(int att, int knockBack = 0);

    public abstract bool CriticalCheck();

    public abstract void CriticalAttack(Unit ctrl);
    public abstract void CriticalAttack(Tower ctrl);









}
