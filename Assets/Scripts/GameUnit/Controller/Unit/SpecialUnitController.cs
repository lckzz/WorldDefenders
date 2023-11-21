using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static Define;

public class SpecialUnitController : Unit
{
    [SerializeField]
    protected UnitClass unitClass;
    [SerializeField]
    protected Define.SpecialUnitState state = Define.SpecialUnitState.Run;

    protected bool isSkil = false;
    protected bool isAttacking = false;
    [SerializeField] protected bool skillOn = false;     //��ų �ߵ��Ǵ�
    [SerializeField] protected string[] skilldialogs;
    protected int randomIdx;
    protected int dialogCount = 2;

    protected List<Unit> monCtrls = new List<Unit>();  //�����ȿ� ���� ������ �������� ��Ƶ�
    [SerializeField] protected Unit monTarget;  //���͵��� �������߿��� ���� ���ְ� ����� ���������� �޾ƿ�
    [SerializeField] protected MonsterPortal monsterPortal;
    protected List<Unit> skillMonList = new List<Unit>();

    protected UnitStat unitStat;

    protected float coolTime = 20.0f;

    protected readonly string warriorHitSound = "WarriorAttack";
    protected readonly string warriorCriticalSound = "CriticalSound";
    protected readonly string warriorHitEff = "HitEff";

    [SerializeField] protected GameObject appearDust;



    public SkillBook Skills { get; protected set; }
    public Unit Monctrl { get { return monTarget; } }

    public MonsterPortal MonsterPortal { get { return monsterPortal; } }

    public SpecialUnitState UniState { get { return state; } }

    Coroutine startCoolTime;

    public override void OnEnable()
    {
        if (sp != null && myColl != null)
        {
            //������Ʈ Ǯ���� �����Ǹ� �ʱ�ȭ ���������
            isDie = false;
            hp = maxHp;
            SetUnitState(SpecialUnitState.Run);
            sp.color = new Color32(255, 255, 255, 255);
            myColl.enabled = true;
            appearDust?.SetActive(true);

        }

    }

    public override void Init()
    {
        base.Init();
        spawnPosX = -9.2f;

        Skills = gameObject.GetComponent<SkillBook>();


        SetUnitState(SpecialUnitState.Run);

        startCoolTime = StartCoroutine(UnitSKillCoolTime(coolTime));
        appearDust?.SetActive(true);

    }



    public override void EnemySensor()      //������
    {
        //Debug.Log(isTargeting);
        //Debug.Log($"Ÿ����{isTageting}");
        #region Ÿ�ٱ���
        if (state == SpecialUnitState.Attack || state == SpecialUnitState.Skill)
            return;

        UnitSense();
        UnitDistanceAsending();






        #endregion

    }
    //���ֵ��� ����
    void UnitSense()
    {
        monCtrls.Clear();
        skillMonList.Clear();
        enemyColls2D = Physics2D.OverlapBoxAll(pos.position, boxSize, 0, LayerMask.GetMask("Monster") | LayerMask.GetMask("EliteMonster"));
        if (enemyColls2D != null)
        {
            if (enemyColls2D.Length <= 0)
            {
                TowerSensor();

                //�ڽ��� �ݶ��̴��� �ƹ��͵� ������
                if (monTarget != null)  //������ ���� Ÿ������ ���������
                {
                    monTarget = null;
                    return;
                }
            }



            //üũ�ڽ��ȿ� ���� �ݶ��̴��߿��� ���� ���ְ��� �Ÿ��� ���� ����� ���� ��󳻱�
            for (int ii = 0; ii < enemyColls2D.Length; ii++)
            {
                if (enemyColls2D[ii].gameObject.layer == LayerMask.NameToLayer("Monster"))
                {
                    MonsterController monctrl;
                    enemyColls2D[ii].TryGetComponent(out monctrl);
                    monCtrls.Add(monctrl);

                }
                else if (enemyColls2D[ii].gameObject.layer == LayerMask.NameToLayer("EliteMonster"))
                {
                    EliteMonsterController elite;
                    enemyColls2D[ii].TryGetComponent(out elite);
                    monCtrls.Add(elite);


                }
            }


        }
    }

    void UnitDistanceAsending()
    {
        if (monCtrls.Count > 0)
        {
            float disMin = 0;
            int min = 0;


            if (monCtrls.Count > 1)
            {

                for (int i = 0; i < monCtrls.Count; i++)
                {

                    if (i < 3)
                        skillMonList.Add(monCtrls[i]);


                    if (i == 0 && monCtrls.Count > 1)
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

                    else if (i < monCtrls.Count - 1)
                    {
                        float distB = (monCtrls[i + 1].transform.position - this.transform.position).sqrMagnitude;

                        if (disMin > distB * distB)
                        {
                            disMin = distB * distB;
                            min = i + 1;


                        }


                    }

                }
            }

            if (monCtrls.Count == 1)
            {
                skillMonList.Add(monCtrls[0]);

            }

            for (int ii = 0; ii < skillMonList.Count; ii++)
            {
                if (skillMonList[ii].IsDie)
                    skillMonList.RemoveAt(ii);
            }


            if (monCtrls.Count != 0)
            {
                monTarget = monCtrls[min];
                //skillMonList.Add(monTarget);

            }


        }

    }


    public void TowerSensor()
    {
        towerColl = Physics2D.OverlapBox(pos.position, boxSize, 0, LayerMask.GetMask("MonsterPortal"));
        if (towerColl != null)
            towerColl.TryGetComponent(out monsterPortal);
    }

 
    public void UnitStateCheck()
    {
        switch (state)
        {
            case SpecialUnitState.Idle:
                {
                    UnitIdle();
                    break;
                }
            case SpecialUnitState.Run:
                {
                    UnitMove();
                    break;
                }
            case SpecialUnitState.Trace:
                {
                    UnitTrace();
                    break;
                }
            case SpecialUnitState.Attack:
                {
                    UnitAttack();
                    break;
                }
            case SpecialUnitState.Skill:
                {
                    UnitSkill();   
                    break;
                }
            case SpecialUnitState.KnockBack:
                {
                    ApplyKnockBack(new Vector2(-1.0f, 1.0f), damageKnockBack);
                    break;
                }

            case SpecialUnitState.Die:
                {
                    UnitDie();
                    break;
                }

        }

    }

    protected void SetUnitState(SpecialUnitState state)
    {

        this.state = state;

        switch (this.state)
        {
            case SpecialUnitState.Idle:
                {
                    if (isAtt)
                    {
                        isAtt = false;
                        anim.SetBool("Attack", isAtt);
                    }
                    if (isRun)
                    {
                        isRun = false;
                        anim.SetBool("Run", isRun);
                    }
                    if (isSkil)
                    {
                        isSkil = false;
                        anim.SetBool("SkillAttack", isSkil);
                    }
                    break;
                }
            case SpecialUnitState.Run:
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
                    if (isSkil)
                    {
                        isSkil = false;
                        anim.SetBool("SkillAttack", isSkil);
                    }
                    break;
                }
            case SpecialUnitState.Attack:
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
                    if (isSkil)
                    {
                        isSkil = false;
                        anim.SetBool("SkillAttack", isSkil);
                    }

                    if (monCtrls.Count > 0)
                    {
                        if (skillOn)      //��ųOn�̸�
                        {
                            SetUnitState(SpecialUnitState.Skill);
                            return;
                        }
                    }

                    break;
                }
            case SpecialUnitState.Skill:
                {

                    if (isRun)
                    {
                        isRun = false;
                        anim.SetBool("Run", isRun);

                    }
                    if (isAtt)
                    {
                        isAtt = false;
                        anim.SetBool("Attack", isAtt);
                    }
                    if (!isSkil)
                    {

                        isSkil = true;
                        anim.SetBool("SkillAttack", isSkil);
                    }
                    break;
                }
            case SpecialUnitState.KnockBack:
                {
                    if (isRun)
                    {
                        isRun = false;
                        anim.SetBool("Run", isRun);

                    }
                    if (isAtt)
                    {
                        isAtt = false;
                        anim.SetBool("Attack", isAtt);

                    }
                    if (isSkil)
                    {
                        isSkil = false;
                        anim.SetBool("SkillAttack", isSkil);
                    }
                    break;
                }
            case SpecialUnitState.Die:
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

        if (monTarget != null || monsterPortal != null)
            SetUnitState(SpecialUnitState.Trace);



        if (IsTargetOn())
            return;

        rigbody.transform.position += Vector3.right * moveSpeed * Time.fixedDeltaTime;




    }

    void UnitTrace()
    {
        if (!IsTargetOn())
        {
            SetUnitState(SpecialUnitState.Run);
            return;
        }

        if (monTarget != null)
            Trace(monTarget);

        else if (monsterPortal != null)
            Trace(monsterPortal);
    }

    bool IsTargetOn()
    {
        if (monTarget == null && monsterPortal == null)
            return false;


        if (monTarget != null)
        {
            if (monTarget.gameObject.layer == LayerMask.NameToLayer("Monster") && monTarget is MonsterController monsterCtrl)
            {
                if (monsterCtrl.MonState == MonsterState.Die)
                    return false;

                if (!monTarget.gameObject.activeInHierarchy)
                    return false;
            }

            else if (monTarget.gameObject.layer == LayerMask.NameToLayer("EliteMonster") && monTarget is EliteMonsterController elite)
            {
                if (elite.MonState == Define.EliteMonsterState.Die)
                    return false;

                if (!monTarget.gameObject.activeInHierarchy)
                    return false;
            }
        }



        return true;
    }

    void UnitAttack()
    {


        if (anim.GetCurrentAnimatorStateInfo(0).IsName("NormalAttack"))
        {

            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)
            { 
                SetUnitState(SpecialUnitState.Idle);
                attackCoolTime = 1.0f;
            }


        }
    }


    public virtual void UnitSkill()
    {
        //���⼭ ������ ���ֵ��� ��ų���� �����ϸ� �ʹ� ����Ǵ� �޸𸮰� ����
        //�ֳĸ� �����ϱ� ���� ���ӿ�����Ʈ���� �̸� �����ϱ� ����
        //��Ÿ���� ���� ��ų�Ŵ������� �ش� ���ֿ� �´� ��ų�� �����´�.
        //�������� �� ���ݹڽ��� �ִ� ���ֵ��� �����;���
        //Ÿ�����ָ���Ʈ�� �Ű������� �ִ´�.
        //Managers.Skill.ActiveSkillUse(skillMonList,);



    }

    void UnitDie()
    {
        if (myColl.enabled)
        {
            myColl.enabled = false;
            StartCoroutine(Util.DestroyTime(gameObject, 5.0f));

            SetUnitState(SpecialUnitState.Die);

        }
    }

    public override void OnHeal(int heal)
    {
        if (hp > 0)
        {
            unitHUDHp?.SpawnHUDText(heal.ToString(), (int)Define.UnitDamageType.Team);
            hp += heal;

        }


        if (hp >= maxHp)
            hp = maxHp;

    }


    public override void OnDamage(int att, int knockBack = 0, bool criticalCheck = false)
    {

        if (hp > 0)
        {
            hp -= att;
            //�˹��� �����ϴ� ���� �ִٸ� �˹��ġ�� 0���� ������ش�.
            if (NoKnockBackValid())
                knockBack = 0;

            if (att > 0)   //���� �������� 0���� Ŭ���� ������ ǥ��
            {
                if (criticalCheck)
                    unitHUDHp?.SpawnHUDText(att.ToString(), (int)Define.UnitDamageType.Critical);
                else
                    unitHUDHp?.SpawnHUDText(att.ToString(), (int)Define.UnitDamageType.Enemy);
            }


            if (knockBack > 0)
            {
                SetUnitState(SpecialUnitState.KnockBack);
                damageKnockBack = knockBack;
            }


            if (hp <= 0)
            {
                hp = 0;
                SetUnitState(SpecialUnitState.Die);
            }
        }
    }







    public override void OnAttack()    //�ִϸ��̼� �̺�Ʈ �Լ�
    {

    }


    public override bool CriticalCheck()
    {
        //���ְ��ݷ��� �޾Ƽ� ũ��Ƽ��Ȯ���� �޾Ƽ� Ȯ���� ������ ũ������
        //�ƴϸ� �Ϲ� ����
        int rand = UnityEngine.Random.Range(0, 101);
        if (rand <= unitStat.criticalRate)
            return true;

        return false;


    }


    public override void CriticalAttack(Unit monCtrl, string soundPath, string criticlaSoundPath, string hitPath)
    {
        if (CriticalCheck())//true�� ũ��Ƽ�õ����� false�� �Ϲݵ�����
        {
            int attack = att * 2;
            monCtrl.OnDamage(attack, unitStat.knockBackForce,true);      //ũ��Ƽ���̸� ������2�迡 �˹����
            Managers.Resource.ResourceEffectAndSound(monTarget.transform.position,warriorCriticalSound,warriorHitEff);

        }
        else  //��ũ��Ƽ���̸� �Ϲݰ���
        {

            monCtrl.OnDamage(att);        //�˹��� ����
            Managers.Resource.ResourceEffectAndSound(monTarget.transform.position, warriorHitSound, warriorHitEff);

        }
    }

    public override void CriticalAttack(Tower monPortal, string soundPath,string criticlaSoundPath, string hitPath)
    {
        if (CriticalCheck())//true�� ũ��Ƽ�õ����� false�� �Ϲݵ�����
        {
            int attack = att * 2;
            monPortal.TowerDamage(attack);      //ũ��Ƽ���̸� ������2�� Ÿ���� 2�踸
            Managers.Resource.ResourceEffectAndSound(monPortal.transform.position,  warriorCriticalSound, warriorHitEff);

        }
        else  //��ũ��Ƽ���̸� �Ϲݰ���
        {
            monPortal.TowerDamage(att);        //�˹��� ����
            Managers.Resource.ResourceEffectAndSound(monPortal.transform.position, warriorHitSound, warriorHitEff);

        }
    }



    #region �˹�
    void ApplyKnockBack(Vector2 dir, float force)
    {
        if(transform.position.x < spawnPosX)
        {
            SetUnitState(SpecialUnitState.Idle);
            attackCoolTime = 1.0f;

            return;

        }

        if (!knockbackStart)
        {
            dir.y = 0;
            knockbackStart = true;
            StartCoroutine(RestoreGravityAfterKnockback(force));

        }
    }


    IEnumerator RestoreGravityAfterKnockback(float force)
    {
        WaitForSeconds wfs = new WaitForSeconds(knockbackDuration);
        float knockBackSpeed = 0.0f;
        float knockBackAccleration = force * 0.5f;            //��

        float knockbackTime = 0.0f;
        float maxKnockBackTime = 0.3f;

        while (knockbackTime < maxKnockBackTime)  //�ӵ� ����
        {
            knockBackSpeed += knockBackAccleration * Time.fixedDeltaTime;
            rigbody.velocity = new Vector2(-1, 0) * knockBackSpeed;
            knockbackTime += Time.fixedDeltaTime;
            yield return null;
        }

        //knockBackAccleration = 10.0f;

        while (knockBackSpeed > 0.0f)   //�ӵ� ����
        {
            knockBackSpeed -= (knockBackAccleration * 0.5f) * Time.fixedDeltaTime;

            Vector2 velo = new Vector2(-knockBackSpeed, rigbody.velocity.y);
            rigbody.velocity = velo;
            yield return null;
        }
        //rigbody.velocity *= 0.5f;

        yield return wfs; // �˹� ���� �ð�

        SetUnitState(SpecialUnitState.Idle);
        attackCoolTime = 0.5f;
        knockbackStart = false;



    }

    #endregion

    void Trace<T>(T obj) where T : UnityEngine.Component
    {
        Vector3 vec = obj.gameObject.transform.position - this.transform.position;
        traceDistance = vec.sqrMagnitude;
        Vector3 dir = vec.normalized;
        if (unitClass == UnitClass.Magician)
        {
            if (traceDistance < attackRange * attackRange)
            {
                SetUnitState(SpecialUnitState.Attack);
            }
            else
            {
                rigbody.transform.position += dir * moveSpeed * Time.fixedDeltaTime;
                SetUnitState(SpecialUnitState.Trace);
            }

        }

        else if (unitClass == UnitClass.Cavalry)
        {
            if (traceDistance < attackRange * attackRange)
            {

                SetUnitState(SpecialUnitState.Attack);
            }
            else
            {
                rigbody.transform.position += dir * moveSpeed * Time.fixedDeltaTime;
                SetUnitState(SpecialUnitState.Trace);
            }

        }

    }

    public override void AttackDelay()
    {
        if (state == SpecialUnitState.Die)
            return;

        if (attackCoolTime > 0.0f)
        {
            attackCoolTime -= Time.fixedDeltaTime;
            if (attackCoolTime <= .0f)
            {
                if(monTarget != null)
                {
                    Vector3 vec = monTarget.gameObject.transform.position - this.transform.position;
                    traceDistance = vec.sqrMagnitude;
                    if (traceDistance < attackRange * attackRange)
                    {
                        SetUnitState(SpecialUnitState.Attack);
                    }
                    else
                        SetUnitState(SpecialUnitState.Run);
                }
                else if (monsterPortal != null)
                {
                    Vector3 vec = monsterPortal.gameObject.transform.position - this.transform.position;
                    traceDistance = vec.sqrMagnitude;

                    if (traceDistance < attackRange * attackRange)
                        SetUnitState(SpecialUnitState.Attack);
                    else
                        SetUnitState(SpecialUnitState.Run);
                }
                else
                    SetUnitState(SpecialUnitState.Run);





            }

        }
    }

    

    IEnumerator UnitSKillCoolTime(float coolTime)
    {
        WaitForSeconds wfs = new WaitForSeconds(coolTime);

        while(true)
        {
            if(!skillOn)     //��ų�� �ȵ��Ҵٸ�
            {
                yield return wfs; //��Ÿ�� ���

                skillOn = true;      //��ų ��밡��!  ��ų����ϸ� �ٽ� false��
                Debug.Log("��ų��!@!@");

            }


            yield return null;

        }


    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(pos.position, boxSize);
    }

}
