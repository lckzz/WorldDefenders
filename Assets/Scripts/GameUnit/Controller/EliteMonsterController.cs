using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class EliteMonsterController : Unit
{
    [SerializeField]
    private MonsterClass monsterClass;
    [SerializeField]
    private EliteMonsterState state = EliteMonsterState.Run;
    private bool isSkil = false;
    [SerializeField] protected bool skillOn = false;     //��ų �ߵ��Ǵ�


    Unit[] unitCtrls;
    [SerializeField]
    Unit unitTarget;
    [SerializeField]
    PlayerTower playerTowerCtrl;
    protected List<Unit> skillenemyList = new List<Unit>();



    protected MonsterStat monStat;

    protected float coolTime = 20.0f;


    public float Hp { get { return hp; } }

    public SkillBook Skills { get; protected set; }

    public Unit UnitCtrl { get { return unitTarget; } }
    public PlayerTower PlayerTowerCtrl { get { return playerTowerCtrl; } }
    public EliteMonsterState MonState { get { return state; } }

    Coroutine startCoolTime;


    public override void Init()
    {
        base.Init();



        playerTowerCtrl = GameObject.FindObjectOfType<PlayerTower>();
        Skills = gameObject.GetComponent<SkillBook>();

        TryGetComponent<Collider2D>(out myColl);



        SetMonsterState(EliteMonsterState.Run);
        startCoolTime = StartCoroutine(UnitSKillCoolTime(coolTime));

    }


    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    //void Update()
    //{


    //}





    public override void EnemySensor()      //������
    {
        //Debug.Log(isTargeting);
        //Debug.Log($"Ÿ����{isTageting}");
        #region Ÿ�ٱ���

        enemyColls2D = Physics2D.OverlapBoxAll(pos.position, boxSize, 0, LayerMask.GetMask("Unit") | LayerMask.GetMask("SpecialUnit"));


        if (enemyColls2D != null)
        {

            if (enemyColls2D.Length <= 0)
            {
                //�ڽ��� �ݶ��̴��� �ƹ��͵� ������
                if (unitTarget != null)  //������ ���� Ÿ������ ���������
                {
                    unitTarget = null;
                    return;
                }
            }

            unitCtrls = new Unit[enemyColls2D.Length];

            //üũ�ڽ��ȿ� ���� �ݶ��̴��߿��� ���� ���ְ��� �Ÿ��� ���� ����� ���� ��󳻱�
            for (int ii = 0; ii < enemyColls2D.Length; ii++)
            {
                if (enemyColls2D[ii].gameObject.layer == LayerMask.NameToLayer("Unit"))
                {
                    UnitController unitctrl;
                    enemyColls2D[ii].TryGetComponent<UnitController>(out unitctrl);
                    unitCtrls[ii] = unitctrl;

                }
                else if (enemyColls2D[ii].gameObject.layer == LayerMask.NameToLayer("SpecialUnit"))
                {
                    SpecialUnitController specialUnit;
                    enemyColls2D[ii].TryGetComponent<SpecialUnitController>(out specialUnit);
                    unitCtrls[ii] = specialUnit;

                }
            }


        }

        if (unitCtrls.Length > 0)
        {
            float disMin = 0;
            int min = 0;


            if (unitCtrls.Length > 1)
            {
                for (int i = 0; i < unitCtrls.Length; i++)
                {
                    if (i == 0 && unitCtrls.Length > 1)
                    {
                        float distA = (unitCtrls[i].transform.position - this.transform.position).sqrMagnitude;
                        float distB = (unitCtrls[i + 1].transform.position - this.transform.position).sqrMagnitude;

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

                    else if (i < unitCtrls.Length - 1)
                    {
                        float distB = (unitCtrls[i + 1].transform.position - this.transform.position).sqrMagnitude;

                        if (disMin > distB * distB)
                        {
                            disMin = distB * distB;
                            min = i + 1;
                        }


                    }

                }
            }


            if (unitCtrls.Length != 0)
            {
                unitTarget = unitCtrls[min];
            }


        }





        #endregion

    }

    protected void TowerSensor()
    {
        //Ÿ���� �ֿ켱������ Ÿ���ϰ� �Ÿ��� ����ؼ� ����ؼ� �����Ÿ��ȿ� ������ Ÿ�� ����

        towerVec = playerTowerCtrl.gameObject.transform.position - this.transform.position;
        towerDist = towerVec.sqrMagnitude;
        towerDir = towerVec.normalized;

        if (towerDist < 15.0f * 15.0f)
        {
            if (!towerTrace)
            {
                towerTrace = true;
                SetMonsterState(EliteMonsterState.Trace);

            }

            if (monsterClass == MonsterClass.EliteWarrior)
            {
                TowerAttackRange(monStat.attackRange);
            }

        }
        else
        {
            if (towerTrace)
            {
                towerTrace = false;

            }
        }
    }

    void TowerAttackRange(float distance)
    {
        if (towerDist < distance * distance)
        {

            if (!towerAttack)
            {
                towerAttack = true;
                SetMonsterState(EliteMonsterState.Attack);
            }
        }
        else
        {

            if (towerAttack)
            {
                towerAttack = false;
            }
        }
    }


    protected void MonsterStateCheck()
    {
        switch (state)
        {
            case EliteMonsterState.Idle:
                {
                    MonsterIdle();
                    break;
                }
            case EliteMonsterState.Run:
                {
                    MonsterMove();
                    break;
                }
            case EliteMonsterState.Trace:
                {
                    MonsterTrace();
                    break;
                }
            case EliteMonsterState.Attack:
                {


                    UnitAttack();
                    break;
                }
            case EliteMonsterState.Skill:
                {
                    MonsterSkill();
                    break;
                }
            case EliteMonsterState.KnockBack:
                {
                    ApplyKnockBack(new Vector2(1.0f, 1.0f), damageKnockBack);
                    break;
                }
            case EliteMonsterState.Die:
                {
                    MonsterDie();
                    break;
                }

        }

    }


    protected void SetMonsterState(EliteMonsterState state)
    {
        if (isDie)
            return;

        this.state = state;
        switch (this.state)
        {
            case EliteMonsterState.Idle:
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
            case EliteMonsterState.Run:
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
            case EliteMonsterState.Attack:
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
                    break;
                }
            case EliteMonsterState.Skill:
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
            case EliteMonsterState.KnockBack:
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
            case EliteMonsterState.Die:
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


    void MonsterIdle()
    {
        AttackDelay();

    }


    void MonsterMove()
    {
        if (unitTarget != null)
            SetMonsterState(EliteMonsterState.Trace);

        if (unitTarget == null)
            if (towerTrace)
                towerTrace = false;


        if (IsTargetOn())
            return;


        rigbody.transform.position += Vector3.left * moveSpeed * Time.deltaTime;




    }


    void MonsterTrace()
    {
        if (!IsTargetOn())
        {
            SetMonsterState(EliteMonsterState.Run);
            return;
        }

        if (unitTarget != null)
            Trace(unitTarget);

        else if (unitTarget == null)
            Trace(playerTowerCtrl);
    }

    bool IsTargetOn()
    {



        if (unitTarget == null && towerTrace == false)
            return false;


        if (unitTarget != null)
        {
            if (unitTarget.gameObject.layer == LayerMask.NameToLayer("Unit"))
            {
                if (unitTarget is UnitController unitCtrl)
                {
                    if (unitCtrl.UniState == UnitState.Die)
                        return false;

                    if (!unitTarget.gameObject.activeInHierarchy)
                        return false;
                }

            }
            else if (unitTarget.gameObject.layer == LayerMask.NameToLayer("SpecialUnit"))
            {
                if (unitTarget is SpecialUnitController specialUnit)
                {
                    if (specialUnit.UniState == Define.SpecialUnitState.Die)
                        return false;

                    if (!unitTarget.gameObject.activeInHierarchy)
                        return false;
                }

            }
        }



        return true;
    }


    void UnitAttack()
    {
        if (skillOn)      //��ųOn�̸�
            SetMonsterState(EliteMonsterState.Skill);

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("NormalAttack"))
        {

            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)
            {
                SetMonsterState(EliteMonsterState.Idle);

                attackCoolTime = 1.0f;

            }


        }
    }


    public virtual void MonsterSkill()
    {

    }

    void MonsterDie()
    {
        if (myColl.enabled)
        {
            SetMonsterState(EliteMonsterState.Die);
            myColl.enabled = false;
            GameObject.Destroy(gameObject, 3.0f);

        }
    }

    public override void OnDamage(int att, int knockBack = 0)
    {

        if (hp > 0)
        {
            hp -= att;

            if (knockBack > 0)
            {
                SetMonsterState(EliteMonsterState.KnockBack);
                damageKnockBack = knockBack;
            }


            if (hp <= 0)
            {
                hp = 0;
                SetMonsterState(EliteMonsterState.Die);

            }
        }
    }


    public override void OnAttack()
    {

        if (monsterClass == MonsterClass.EliteWarrior)
        {

            if (!towerAttack)
            {

                if (unitTarget != null)
                    CriticalAttack(unitTarget);

            }
            else
                CriticalAttack(playerTowerCtrl);


        }

        
        
    }

    public override bool CriticalCheck()
    {
        //���ְ��ݷ��� �޾Ƽ� ũ��Ƽ��Ȯ���� �޾Ƽ� Ȯ���� ������ ũ������
        //�ƴϸ� �Ϲ� ����
        int rand = UnityEngine.Random.Range(0, 101);
        if (rand <= monStat.criticalRate)
            return true;

        return false;


    }


    public override void CriticalAttack(Unit uniCtrl)
    {
        if (CriticalCheck())//true�� ũ��Ƽ�õ����� false�� �Ϲݵ�����
        {
            Debug.Log("ũ��Ƽ��!!!!");
            int attack = att * 2;
            uniCtrl.OnDamage(attack, monStat.knockBackForce);      //ũ��Ƽ���̸� ������2�迡 �˹����
            MeleeUnitEffectAndSound(unitTarget.transform.position, "CriticalSound", "HitEff");

        }
        else  //��ũ��Ƽ���̸� �Ϲݰ���
        {
            Debug.Log("�Ϲݰ���...");

            uniCtrl.OnDamage(att);        //�˹��� ����
            MeleeUnitEffectAndSound(unitTarget.transform.position, "WarriorAttack", "HitEff");

        }
    }

    public override void CriticalAttack(Tower tower)
    {
        if (CriticalCheck())//true�� ũ��Ƽ�õ����� false�� �Ϲݵ�����
        {
            int attack = att * 2;
            tower.TowerDamage(attack);      //ũ��Ƽ���̸� ������2�� Ÿ���� 2�踸
            MeleeUnitEffectAndSound(tower.transform.position, "CriticalSound", "HitEff");

        }
        else  //��ũ��Ƽ���̸� �Ϲݰ���
        {
            tower.TowerDamage(att);        //�˹��� ����
            MeleeUnitEffectAndSound(tower.transform.position, "WarriorAttack", "HitEff");

        }
    }


    void MeleeUnitEffectAndSound(Vector3 pos, string soundPath, string effPath)
    {
        Managers.Sound.Play($"Sounds/Effect/{soundPath}");
        GameObject eff = Managers.Resource.Load<GameObject>($"Prefabs/Effect/{effPath}");
        Vector2 randomPos = RandomPosSetting(pos);

        if (eff != null)
            Instantiate(eff, randomPos, Quaternion.identity);
    }


    void ApplyKnockBack(Vector2 dir, float force)
    {
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
        float knockBackAccleration = 25.0f;            //��

        float knockbackTime = 0.0f;
        float maxKnockBackTime = 0.3f;

        while (knockbackTime < maxKnockBackTime)  //�ӵ� ����
        {
            knockBackSpeed += knockBackAccleration * Time.deltaTime;
            rigbody.velocity = new Vector2(1, 0) * knockBackSpeed;
            knockbackTime += Time.deltaTime;
            yield return null;
        }

        //knockBackAccleration = 10.0f;

        while (knockBackSpeed > 0.0f)   //�ӵ� ����
        {
            knockBackSpeed -= (knockBackAccleration * 0.5f) * Time.deltaTime;

            Vector2 velo = new Vector2(knockBackSpeed, rigbody.velocity.y);
            rigbody.velocity = velo;
            yield return null;
        }
        //rigbody.velocity *= 0.5f;

        yield return wfs; // �˹� ���� �ð�

        SetMonsterState(EliteMonsterState.Run);
        knockbackStart = false;



    }



    void Trace<T>(T obj) where T : UnityEngine.Component
    {
        Vector3 vec = obj.gameObject.transform.position - this.transform.position;
        float distance = vec.magnitude;
        Vector3 dir = vec.normalized;

        if (monsterClass == MonsterClass.EliteWarrior)
        {

            if (distance < attackRange)
            {
                SetMonsterState(EliteMonsterState.Attack);
            }
            else
            {
                rigbody.transform.position += dir * moveSpeed * Time.deltaTime;
                SetMonsterState(EliteMonsterState.Trace);

            }

        }







    }


    Vector2 RandomPosSetting(Vector3 pos)
    {
        randomX = UnityEngine.Random.Range(-0.5f, 0.5f);
        randomY = UnityEngine.Random.Range(-0.5f, 0.5f);
        Vector2 randomPos = pos;
        randomPos.x += randomX;
        randomPos.y += randomY;

        return randomPos;
    }

    protected void MonsterVictory()
    {
        //Managers.Game.state = GameState.GameFail;


        if (Managers.Game.State == GameState.GameFail)
        {
            isRun = false;
            anim.SetBool("Run", isRun);
            isAtt = false;
            anim.SetBool("Attack", isAtt);

            state = EliteMonsterState.Idle;

            transform.position = transform.position;

        }
    }


    public override void AttackDelay()
    {
        if (state == EliteMonsterState.Die)
            return;


        if (attackCoolTime > 0.0f)
        {
            attackCoolTime -= Time.deltaTime;
            if (attackCoolTime <= .0f)
            {
                if (unitTarget != null)
                {
                    Vector3 vec = unitTarget.gameObject.transform.position - this.transform.position;
                    traceDistance = vec.sqrMagnitude;

                    if (traceDistance < attackRange * attackRange)
                        SetMonsterState(EliteMonsterState.Attack);
                    else
                        SetMonsterState(EliteMonsterState.Run);
                }
                else
                {

                    if (towerDist < attackRange * attackRange)
                        SetMonsterState(EliteMonsterState.Attack);
                    else
                    {
                        SetMonsterState(EliteMonsterState.Run);
                        
                    }
                }



                if (towerAttack)
                    towerAttack = false;
            }

        }
    }

    IEnumerator UnitSKillCoolTime(float coolTime)
    {
        WaitForSeconds wfs = new WaitForSeconds(coolTime);

        while (true)
        {
            if (!skillOn)     //��ų�� �ȵ��Ҵٸ�
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
