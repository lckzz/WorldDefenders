using System;
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
    protected SpriteRenderer sp;

    [SerializeField]
    protected float attackCoolTime = 1.5f;
    protected bool attackTime = true;      //공격쿨이 다돈 상태

 
    protected Collider2D[] enemyColls2D;
    protected Collider2D towerColl;
    protected Collider2D unitColl;
    protected Collider2D myColl;     //나 자신의 콜라이더를 받는 변수

    protected float unitDestroyTime = .0f;
    protected float traceDistance;


    [SerializeField]
    protected Transform pos;
    [SerializeField]
    protected Vector2 boxSize;

    protected UnitHp unitHUDHp;


    //넉백 관련 변수
    protected bool knockbackStart = false;
    protected float knockbackDuration = 0.25f;
    protected bool isNoKnockBack = false;           //넉백이 안되는 구간에 있다면

    //HUDUI
    private Transform parentTr;
    private GameObject hudPrefab;

    public bool IsDie { get { return isDie; } }
    public int Att { get { return att; } }
    public float Hp { get { return hp; } }
    public float MaxHp { get { return maxHp; } }
    public bool IsNoKnockBack { get { return isNoKnockBack; } set { isNoKnockBack = value; } }



    public virtual void Init()
    {
        TryGetComponent<Animator>(out anim);
        TryGetComponent<Rigidbody2D>(out rigbody);
        TryGetComponent<Collider2D>(out myColl);
        TryGetComponent<SpriteRenderer>(out sp);

        TryGetComponent<UnitHp>(out unitHUDHp);
        if (unitHUDHp == null)
            this.gameObject.AddComponent<UnitHp>().TryGetComponent(out unitHUDHp);


        parentTr = GameObject.Find("HUD_Canvas").transform;
        hudPrefab = Managers.Resource.Load<GameObject>("Prefabs/HUDDamage/DamageTxt");
        unitHUDHp.Init(parentTr, hudPrefab);

        Vector3 vec = this.gameObject.transform.position;
        vec.z = 0.0f;
        gameObject.transform.position = vec;        //혹시라도 설정된 z축이 0이 아닐때를 대비해서 0으로 넣어줌
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

    public bool NoKnockBackValid()
    {
        if(isNoKnockBack == false)
            return false;

        return true;
    }

    public void UnitDebuff(float debuffIdx, float durationTime, Action<bool> debuffOnOffAction)
    {
        StartCoroutine(StartDebuff(debuffIdx, durationTime, debuffOnOffAction));
    }

    protected IEnumerator StartDebuff(float debuffIdx,float durationTime,Action<bool> debuffOnOffAction)
    {
        WaitForSeconds wfs = new WaitForSeconds(durationTime);

        //기본 속도랑 공격력을 저장하고
        float defalutMoveSpeed = moveSpeed;
        int defalutAtt = att;   

        //디버프당하면 속도랑 공격력이 낮아짐
        moveSpeed -= moveSpeed *(debuffIdx / 100);
        att -= (int)(att * (debuffIdx / 100));

        yield return wfs;       //시간초만큼 대기하고 다시 원래대로 돌려줌

        moveSpeed = defalutMoveSpeed;
        att = defalutAtt;

        debuffOnOffAction(false);

        yield return null;
        
    }


    public virtual void OnHeal(int heal) { }

    public abstract void OnEnable();
    public abstract void EnemySensor(); //적감지 센서

    public abstract void AttackDelay();

    public abstract void OnAttack();

    public abstract void OnDamage(int att, int knockBack = 0, bool criticalCheck = false);

    public abstract bool CriticalCheck();

    public abstract void CriticalAttack(Unit ctrl,string soundPath,string criticalSoundPath,string hitPath);
    public abstract void CriticalAttack(Tower ctrl, string soundPath, string criticalSoundPath, string hitPath);









}
