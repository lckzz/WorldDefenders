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
    protected float spawnPosX = 0;       //소환지점x축 이하로는 넉백당하지않음


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
    protected bool attackTime = true;      //공격쿨이 다돈 상태

 
    protected Collider2D[] enemyColls2D;
    protected Collider2D tower;
    protected Collider2D unitColl;
    protected Collider2D myColl;     //나 자신의 콜라이더를 받는 변수

    protected float unitDestroyTime = .0f;
    protected float traceDistance;



    //타워 관련 변수
    protected Vector2 towerVec;
    protected Vector2 towerDir;
    protected float towerDist;
    protected bool towerTrace = false;
    protected bool towerAttack = false;       //타워에 가까워지면 
    protected float towerAttackRange = 0.0f;

    //타워 관련 변수

    [SerializeField]
    protected Transform pos;
    [SerializeField]
    protected Vector2 boxSize;

    protected UnitHp unitHUDHp;


    //넉백 관련 변수
    protected bool knockbackStart = false;
    protected float knockbackDuration = 0.25f;

    //HUDUI
    private Transform parentTr;
    private GameObject hudPrefab;

    public bool IsDie { get { return isDie; } }
    public int Att { get { return att; } }
    public float Hp { get { return hp; } }
    public float MaxHp { get { return maxHp; } }



    public virtual void Init()
    {
        TryGetComponent<Animator>(out anim);
        TryGetComponent<Rigidbody2D>(out rigbody);
        TryGetComponent<Collider2D>(out myColl);
        TryGetComponent<UnitHp>(out unitHUDHp);
        if (unitHUDHp == null)
            this.gameObject.AddComponent<UnitHp>().TryGetComponent(out unitHUDHp);


        parentTr = GameObject.Find("HUD_Canvas").transform;
        hudPrefab = Managers.Resource.Load<GameObject>("Prefabs/HUDDamage/DamageTxt");
        unitHUDHp.Init(parentTr,hudPrefab);
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

    public virtual void OnHeal(int heal) { }

    public abstract void EnemySensor(); //적감지 센서

    public abstract void AttackDelay();

    public abstract void OnAttack();

    public abstract void OnDamage(int att, int knockBack = 0, bool criticalCheck = false);

    public abstract bool CriticalCheck();

    public abstract void CriticalAttack(Unit ctrl,string soundPath,string criticalSoundPath,string hitPath);
    public abstract void CriticalAttack(Tower ctrl, string soundPath, string criticalSoundPath, string hitPath);









}
