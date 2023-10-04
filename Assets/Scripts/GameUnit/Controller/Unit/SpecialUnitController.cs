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
    [SerializeField] protected GameObject speechBubbleObj;           //��ǳ��
    [SerializeField] protected SpeechBubbleCtrl speechBBCtrl;
    [SerializeField] protected string[] skilldialogs;
    protected int randomIdx;
    protected int dialogCount = 2;

    protected Unit[] monCtrls;  //�����ȿ� ���� ������ �������� ��Ƶ�
    [SerializeField] protected Unit monTarget;  //���͵��� �������߿��� ���� ���ְ� ����� ���������� �޾ƿ�
    [SerializeField] protected MonsterPortal monsterPortal;
    protected List<Unit> skillMonList = new List<Unit>();

    protected UnitStat unitStat;

    protected float coolTime = 20.0f;

    protected string warriorHitSound = "WarriorAttack";
    protected string warriorCriticalSound = "CriticalSound";
    protected string warriorHitEff = "HitEff";


    public SkillBook Skills { get; protected set; }
    public Unit Monctrl { get { return monTarget; } }

    public MonsterPortal MonsterPortal { get { return monsterPortal; } }

    public SpecialUnitState UniState { get { return state; } }

    Coroutine startCoolTime;

    public override void Init()
    {
        base.Init();
        spawnPosX = -9.2f;


        GameObject canvas = this.gameObject.transform.Find("Canvas").gameObject;
        if (canvas != null)
        {
            speechBubbleObj = canvas.gameObject.transform.Find("SpeechBubble").gameObject;
            speechBubbleObj.TryGetComponent(out speechBBCtrl);
        }
        //rig = this.GetComponent<Rigidbody2D>();
        //anim = GetComponent<Animator>();
        monsterPortal = GameObject.FindObjectOfType<MonsterPortal>();
        Skills = gameObject.GetComponent<SkillBook>();


        SetUnitState(SpecialUnitState.Run);

        startCoolTime = StartCoroutine(UnitSKillCoolTime(coolTime));

    }

    // Start is called before the first frame update
    //void Start()
    //{
    //    Init();
    //}

    //// Update is called once per frame
    //void Update()
    //{

    //}

    public override void EnemySensor()      //������
    {
        //Debug.Log(isTargeting);
        //Debug.Log($"Ÿ����{isTageting}");
        #region Ÿ�ٱ���
        if (state == SpecialUnitState.Attack || state == SpecialUnitState.Skill)
            return;


        skillMonList.Clear();
        enemyColls2D = Physics2D.OverlapBoxAll(pos.position, boxSize, 0, LayerMask.GetMask("Monster") | LayerMask.GetMask("EliteMonster"));
        if (enemyColls2D != null)
        {
            if (enemyColls2D.Length <= 0)
            {
                //�ڽ��� �ݶ��̴��� �ƹ��͵� ������
                if (monTarget != null)  //������ ���� Ÿ������ ���������
                {
                    monTarget = null;
                    return;
                }
            }

            monCtrls = new Unit[enemyColls2D.Length];

            //üũ�ڽ��ȿ� ���� �ݶ��̴��߿��� ���� ���ְ��� �Ÿ��� ���� ����� ���� ��󳻱�
            for (int ii = 0; ii < enemyColls2D.Length; ii++)
            {
                if (enemyColls2D[ii].gameObject.layer == LayerMask.NameToLayer("Monster"))
                {
                    MonsterController monctrl;
                    enemyColls2D[ii].TryGetComponent<MonsterController>(out monctrl);
                    monCtrls[ii] = monctrl;

                }
                else if (enemyColls2D[ii].gameObject.layer == LayerMask.NameToLayer("EliteMonster"))
                {
                    EliteMonsterController elite;
                    enemyColls2D[ii].TryGetComponent<EliteMonsterController>(out elite);
                    monCtrls[ii] = elite;

                }
            }
            

        }


        if (monCtrls.Length > 0)
        {
            float disMin = 0;
            int min = 0;


            if (monCtrls.Length > 1)
            {

                for (int i = 0; i < monCtrls.Length; i++)
                {
                    
                    if(i < 3)
                        skillMonList.Add(monCtrls[i]);


                    if (i == 0 && monCtrls.Length > 1)
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

                    else if (i < monCtrls.Length - 1)
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

            if(monCtrls.Length == 1)
            {
                skillMonList.Add(monCtrls[0]);

            }

            for (int ii = 0; ii < skillMonList.Count; ii++)
            {
                if (skillMonList[ii].IsDie)
                    skillMonList.RemoveAt(ii);
            }


            if (monCtrls.Length != 0)
            {
                monTarget = monCtrls[min];
                //skillMonList.Add(monTarget);

            }


        }





        #endregion

    }


    public void TowerSensor()
    {
        //Ÿ���� �ֿ켱������ Ÿ���ϰ� �Ÿ��� ����ؼ� ����ؼ� �����Ÿ��ȿ� ������ Ÿ�� ����
        if (monsterPortal == null)
            return;

        towerVec = monsterPortal.gameObject.transform.position - this.transform.position;
        towerDist = towerVec.sqrMagnitude;
        towerDir = towerVec.normalized;

        if (towerDist < 15.0f * 15.0f)
        {
            if (!towerTrace)
            {
                towerTrace = true;
                SetUnitState(SpecialUnitState.Trace);


            }

            if (unitClass == UnitClass.Magician)
            {

                TowerAttackRange(6.5f);
            }

            if (unitClass == UnitClass.Cavalry)
            {

                TowerAttackRange(2.0f);
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
                SetUnitState(SpecialUnitState.Attack);
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

                    if (monCtrls.Length > 0)
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

        if (monTarget != null)
            SetUnitState(SpecialUnitState.Trace);

        if (monTarget == null)
            if (towerTrace)
                towerTrace = false;

        if (IsTargetOn())
            return;

        rigbody.transform.position += Vector3.right * moveSpeed * Time.deltaTime;




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

        else if (monTarget == null)
            Trace(monsterPortal);
    }

    bool IsTargetOn()
    {
        if (monTarget == null && towerTrace == false)
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
            StartCoroutine(Util.UnitDieTime(gameObject, 5.0f));

            SetUnitState(SpecialUnitState.Die);

        }
    }

    public override void OnHeal(int heal)
    {
        if (hp > 0)
            hp += heal;


        if (hp >= maxHp)
            hp = maxHp;

    }


    public override void OnDamage(int att, int knockBack = 0)
    {

        if (hp > 0)
        {
            hp -= att;
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
            monCtrl.OnDamage(attack, unitStat.knockBackForce);      //ũ��Ƽ���̸� ������2�迡 �˹����
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


    //void UnitEffectAndSound(Vector3 pos, string soundPath, string effPath)
    //{
    //    Managers.Sound.Play($"Sounds/Effect/{soundPath}");
    //    GameObject eff = Managers.Resource.Load<GameObject>($"Prefabs/Effect/{effPath}");
    //    Vector2 randomPos = RandomPosSetting(pos);

    //    if (eff != null)
    //        Instantiate(eff, randomPos, Quaternion.identity);
    //}


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
        force = force * 0.5f;               //����������� �˹��� ���� �ݰ���
        float knockBackAccleration = 25.0f;            //��

        float knockbackTime = 0.0f;
        float maxKnockBackTime = 0.3f;

        while (knockbackTime < maxKnockBackTime)  //�ӵ� ����
        {
            knockBackSpeed += knockBackAccleration * Time.deltaTime;
            rigbody.velocity = new Vector2(-1, 0) * knockBackSpeed;
            knockbackTime += Time.deltaTime;
            yield return null;
        }

        //knockBackAccleration = 10.0f;

        while (knockBackSpeed > 0.0f)   //�ӵ� ����
        {
            knockBackSpeed -= (knockBackAccleration * 0.5f) * Time.deltaTime;

            Vector2 velo = new Vector2(-knockBackSpeed, rigbody.velocity.y);
            rigbody.velocity = velo;
            yield return null;
        }
        //rigbody.velocity *= 0.5f;

        yield return wfs; // �˹� ���� �ð�

        SetUnitState(SpecialUnitState.Run);
        knockbackStart = false;



    }

    #endregion

    void Trace<T>(T obj) where T : UnityEngine.Component
    {

        Vector3 vec = obj.gameObject.transform.position - this.transform.position;
        traceDistance = vec.magnitude;
        Vector3 dir = vec.normalized;
        if (unitClass == UnitClass.Magician)
        {
            Debug.Log(traceDistance);
            Debug.Log(attackRange);
            if (traceDistance < attackRange)
            {

                SetUnitState(SpecialUnitState.Attack);
            }
            else
            {
                rigbody.transform.position += dir * moveSpeed * Time.deltaTime;
                SetUnitState(SpecialUnitState.Trace);
            }

        }

        else if (unitClass == UnitClass.Cavalry)
        {
            if (traceDistance < attackRange)
            {

                SetUnitState(SpecialUnitState.Attack);
            }
            else
            {
                rigbody.transform.position += dir * moveSpeed * Time.deltaTime;
                SetUnitState(SpecialUnitState.Trace);
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

    void UnitVictory()
    {
        if (GameManager.instance.State == GameState.GameVictory)
        {
            isRun = false;
            anim.SetBool("Run", isRun);
            isAtt = false;
            anim.SetBool("Attack", isAtt);

        }
    }



    public override void AttackDelay()
    {
        if (state == SpecialUnitState.Die)
            return;

        if (attackCoolTime > 0.0f)
        {
            attackCoolTime -= Time.deltaTime;
            if (attackCoolTime <= .0f)
            {

                //if(monTarget != null)
                //{
                //    distance = (monTarget.transform.position - this.transform.position).sqrMagnitude;
                //    if (distance < attackRange * attackRange)
                //        SetUnitState(UnitState.Attack);
                //    else
                //        SetUnitState(UnitState.Run);
                //}
                //else
                //    SetUnitState(UnitState.Run);

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
                else
                {
                    if (towerDist < attackRange * attackRange)
                        SetUnitState(SpecialUnitState.Attack);
                    else
                        SetUnitState(SpecialUnitState.Run);
                }

   
                if (towerAttack)
                    towerAttack = false;

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
