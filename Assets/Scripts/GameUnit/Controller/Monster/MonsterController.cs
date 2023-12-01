using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.Burst.CompilerServices;
using UnityEditor;
using UnityEngine;
using static Define;

public enum MonsterClass
{
    Warrior,
    Archer,
    EliteWarrior,
    EliteCavalry,
    EliteShaman,
    Count
}

public enum MonsterState
{
    Idle,
    Run,
    Trace,
    Attack,
    KnockBack,
    Die
}

public class MonsterController : Unit,IObserver
{

    [SerializeField]
    private MonsterState state = MonsterState.Run;        //�׽�Ʈ���̶� �� 



    List<Unit> unitCtrls = new List<Unit>();
    [SerializeField] protected Unit unitTarget;
    [SerializeField] protected PlayerTower playerTowerCtrl;

    protected MonsterStat monStat;

    DropItem dropItem;

    [SerializeField] DebuffCreator debuffCreator;
    Debuff debuff;



    //public float Hp { get { return hp; } }

    public int DropGold { get ; protected set; }
    public int DropCost { get; protected set; }

    public int KnockBackForce { get { return knockbackForce; } }


    public Debuff Debuff { get { return debuff; } }
    public Unit UnitCtrl { get { return unitTarget; } }
    public PlayerTower PlayerTowerCtrl { get { return playerTowerCtrl; } }
    public MonsterState MonState { get { return state; } }



    protected readonly string appearTitleKey = "monsterAppearDialog";
    protected readonly string dieTitleKey = "unitDieDialog";
    private readonly string dieDialogSubKey = "monsterDie";

    protected readonly int appearProbability = 25;       //25����Ȯ���� �����ϸ鼭 ��ǳ��
    protected readonly int dieProbability = 25;


    public override void Init()
    {
        base.Init();

        monStat = new MonsterStat();
        spawnPosX = 18.0f;

        TryGetComponent(out myColl);
        TryGetComponent(out dropItem);
        TryGetComponent(out debuffCreator);

        debuff = debuffCreator.AddDebuffComponent(Managers.Game.CurPlayerEquipSkill);


        if(Debuff is WeaknessDebuff weaknessDebuff)
            weaknessDebuff.AddObserver(this);           //������� �ɷ�ġ��ȭ���� �޾ƿ������� ����

    }

    public override void OnEnable()
    {
        if (sp != null && myColl != null)
        {
            //������Ʈ Ǯ���� �����Ǹ� �ʱ�ȭ ���������
            isDie = false;
            isRun = false;
            hp = maxHp;
            SetMonsterState(MonsterState.Run);
            sp.color = new Color32(255, 255, 255, 255);
            myColl.enabled = true;
            unitTarget = null;
            playerTowerCtrl = null;
        }

    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if (Managers.Game.GameEndResult())       //������ �������� ����
            return;

        EnemySensor();
        MonsterStateCheck();

    }

    public override void EnemySensor()      //������
    {
        #region Ÿ�ٱ���

        UnitSense();
        UnitDistanceAsending();

        #endregion

    }

    //���ֵ��� ����
    void UnitSense()
    {
        unitCtrls.Clear();
        enemyColls2D = Physics2D.OverlapBoxAll(pos.position, boxSize, 0, LayerMask.GetMask("Unit") | LayerMask.GetMask("SpecialUnit"));


        if (enemyColls2D != null)
        {

            if (enemyColls2D.Length <= 0)
            {
                TowerSensor();   //���Ͱ� �ƹ��� ���ٸ� Ÿ�������� Ų��.


                //�ڽ��� �ݶ��̴��� �ƹ��͵� ������
                if (unitTarget != null)  //������ ���� Ÿ������ ���������
                {
                    unitTarget = null;
                    return;
                }
            }
            else
            {
                playerTowerCtrl = null;

            }

            //üũ�ڽ��ȿ� ���� �ݶ��̴��߿��� ���� ���ְ��� �Ÿ��� ���� ����� ���� ��󳻱�
            for (int ii = 0; ii < enemyColls2D.Length; ii++)
            {
                if (enemyColls2D[ii].gameObject.layer == LayerMask.NameToLayer("Unit"))
                {
                    UnitController unitctrl;
                    enemyColls2D[ii].TryGetComponent<UnitController>(out unitctrl);
                    unitCtrls.Add(unitctrl);


                }
                else if (enemyColls2D[ii].gameObject.layer == LayerMask.NameToLayer("SpecialUnit"))
                {
                    SpecialUnitController specialUnit;
                    enemyColls2D[ii].TryGetComponent<SpecialUnitController>(out specialUnit);
                    unitCtrls.Add(specialUnit);


                }
            }


        }
    }
    //������ ���ֵ��� �ش� ĳ���Ϳ��� �Ÿ� ��������
    void UnitDistanceAsending()
    {
        if (unitCtrls.Count > 0)
        {
            float disMin = 0;
            int min = 0;


            if (unitCtrls.Count > 1)
            {
                for (int i = 0; i < unitCtrls.Count; i++)
                {
                    if (i == 0 && unitCtrls.Count > 1)
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

                    else if (i < unitCtrls.Count - 1)
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


            if (unitCtrls.Count != 0)
            {
                unitTarget = unitCtrls[min];
            }


        }


    }


    void TowerSensor()
    {
        //Ÿ���� ������ ���ٸ� �׶� �������ϰ� �����߰��̳� ������ �� �� �ִ�.

        towerColl = Physics2D.OverlapBox(pos.position, boxSize, 0, LayerMask.GetMask("Tower"));


        if (towerColl != null)
            towerColl.TryGetComponent(out playerTowerCtrl);
        
    }





    void MonsterStateCheck()
    {
        switch (state)
        {
            case MonsterState.Idle:
                {
                    MonsterIdle();
                    break;
                }
            case MonsterState.Run:
                {
                    MonsterMove();
                    break;
                }
            case MonsterState.Trace:
                {
                    MonsterTrace();
                    break;
                }
            case MonsterState.Attack:
                {
                    

                    UnitAttack();
                    break;
                }
            case MonsterState.KnockBack:
                {
                    ApplyKnockBack(new Vector2(1.0f, 1.0f), damageKnockBack);
                    break;
                }
            case MonsterState.Die:
                {
                    MonsterDie();
                    break;
                }

        }

    }


    protected void SetMonsterState(MonsterState state)
    {
        if (isDie)
            return;

        this.state = state;
        switch (this.state)
        {
            case MonsterState.Idle:
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
                    break;
                }
            case MonsterState.Run:
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
                    break;
                }
            case MonsterState.Attack:
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
                    break;
                }
            case MonsterState.KnockBack:
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
                    break;
                }
            case MonsterState.Die:
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
        if (unitTarget != null || playerTowerCtrl != null)
            SetMonsterState(MonsterState.Trace);

        if (IsTargetOn())
            return;


        rigbody.transform.position += Vector3.left * moveSpeed * Time.fixedDeltaTime;




    }


    void MonsterTrace()
    {
        if (!IsTargetOn())
        {
            SetMonsterState(MonsterState.Run);
            return;
        }


        if (unitTarget != null)
            Trace(unitTarget);

        else if (playerTowerCtrl != null)
            Trace(playerTowerCtrl);
    }

    bool IsTargetOn()
    {
        if (unitTarget == null && playerTowerCtrl == null)
            return false;


        if(unitTarget != null)
        {
            if (unitTarget.gameObject.layer == LayerMask.NameToLayer("Unit"))
            {
                if(unitTarget is UnitController unitCtrl)
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
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {

            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)
            {
                SetMonsterState(MonsterState.Idle);

                attackCoolTime = 1.0f;

            }


        }
    }


    void MonsterDie()
    {
        if (myColl.enabled)
        {
            speechBubble.SpeechBubbuleOn(dieTitleKey, dieDialogSubKey, dieProbability);

            SetMonsterState(MonsterState.Die);
            myColl.enabled = false;
            if(Debuff is FireDebuff fireDebuff)
                fireDebuff.DebuffOnOff(false);


            dropItem?.Drop(this.gameObject.transform.position);
            StartCoroutine(Util.DestroyTime(gameObject, 3.0f));
            StartCoroutine(MonsterDieDropText());

        }
    }

    public override void OnDamage(int att, int knockBack = 0, bool criticalCheck = false)
    {

        if (hp > 0)
        {
            hp -= att;

            //�˹��� �����ϴ� ���� �ִٸ� �˹��ġ�� 0���� ������ش�.
            if (NoKnockBackValid())
                knockBack = 0;


            if(att > 0)
            {
                if (criticalCheck)
                    unitHUDHp?.SpawnHUDText(att.ToString(), (int)Define.UnitDamageType.Critical);
                else
                    unitHUDHp?.SpawnHUDText(att.ToString(), (int)Define.UnitDamageType.Enemy);
            }
  


            if (knockBack > 0)
            {
                SetMonsterState(MonsterState.KnockBack);
                damageKnockBack = knockBack;
            }


            if (hp <= 0)
            {
                hp = 0;
                SetMonsterState(MonsterState.Die);

            }
        }
    }


    public override void OnAttack() { }


    public override bool CriticalCheck()
    {
        //���ְ��ݷ��� �޾Ƽ� ũ��Ƽ��Ȯ���� �޾Ƽ� Ȯ���� ������ ũ������
        //�ƴϸ� �Ϲ� ����
        int rand = UnityEngine.Random.Range(0, 101);
        if (rand <= monStat.criticalRate)
            return true;

        return false;


    }


    public override void CriticalAttack(Unit uniCtrl, string soundPath, string criticalSoundPath,  string hitPath)
    {
        if (CriticalCheck())//true�� ũ��Ƽ�õ����� false�� �Ϲݵ�����
        {
            int attack = att * 2;
            uniCtrl.OnDamage(attack, monStat.knockBackForce,true);      //ũ��Ƽ���̸� ������2�迡 �˹����
            Managers.Resource.ResourceEffectAndSound(unitTarget.transform.position, criticalSoundPath, hitPath);

        }
        else  //��ũ��Ƽ���̸� �Ϲݰ���
        {


            uniCtrl.OnDamage(att);        //�˹��� ����
            Managers.Resource.ResourceEffectAndSound(unitTarget.transform.position, soundPath, hitPath);

        }
    }

    public override void CriticalAttack(Tower tower, string soundPath, string criticalSoundPath, string hitPath)
    {
        if (CriticalCheck())//true�� ũ��Ƽ�õ����� false�� �Ϲݵ�����
        {
            int attack = att * 2;
            tower.TowerDamage(attack);      //ũ��Ƽ���̸� ������2�� Ÿ���� 2�踸
            Managers.Resource.ResourceEffectAndSound(tower.transform.position, criticalSoundPath, hitPath);

        }
        else  //��ũ��Ƽ���̸� �Ϲݰ���
        {

            tower.TowerDamage(att);        //�˹��� ����
            Managers.Resource.ResourceEffectAndSound(tower.transform.position, soundPath, hitPath);

        }
    }



    void ApplyKnockBack(Vector2 dir, float force)
    {
        if(transform.position.x >= spawnPosX)
        {
            SetMonsterState(MonsterState.Idle);
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
        Debug.Log(force);
        float knockBackAccleration = force;            //��

        float knockbackTime = 0.0f;
        float maxKnockBackTime = 0.3f;

        while (knockbackTime < maxKnockBackTime)  //�ӵ� ����
        {
            knockBackSpeed += knockBackAccleration * Time.fixedDeltaTime;
            rigbody.velocity = new Vector2(1, 0) * knockBackSpeed;
            knockbackTime += Time.fixedDeltaTime;
            yield return null;
        }

        //knockBackAccleration = 10.0f;
        
        while(knockBackSpeed > 0.0f)   //�ӵ� ����
        {

            knockBackSpeed -= (knockBackAccleration * 0.25f) * Time.fixedDeltaTime;

            Vector2 velo = new Vector2(knockBackSpeed, rigbody.velocity.y);
            rigbody.velocity = velo;
            yield return null;
        }
        //rigbody.velocity *= 0.5f;

        yield return wfs; // �˹� ���� �ð�

        SetMonsterState(MonsterState.Idle);
        attackCoolTime = 0.5f;

        knockbackStart = false;



    }


    WaitForSeconds wfs = new WaitForSeconds(1.5f);
    IEnumerator MonsterDieDropText()
    {
        yield return wfs;           //�ð� ����� �ؽ�Ʈ

        //�Ϲ� ���ʹ� ������ �ڽ�Ʈ�� ��´�.
        int randItem = UnityEngine.Random.Range(0, 2);
        unitHUDHp?.ItemHudInit((int)DropItemType.Cost);

        unitHUDHp?.SpawnHUDText(DropCost.ToString(), (int)UnitDamageType.Item);
        Managers.Game.Cost += DropCost;
        Managers.UI.GetSceneUI<UI_GamePlay>().UpdateCost(Managers.Game.Cost);

        
        

    }



    void Trace<T>(T obj) where T : UnityEngine.Component
    {

        Vector2 vec = obj.gameObject.transform.position - this.transform.position;
        float distance = vec.sqrMagnitude;
        Vector3 dir = vec.normalized;

        if (distance < attackRange * attackRange)
        {
            SetMonsterState(MonsterState.Attack);
        }
        else
        {
            rigbody.transform.position += dir * moveSpeed * Time.fixedDeltaTime;
            SetMonsterState(MonsterState.Trace);
        }

    }




    public override void AttackDelay()
    {
        if (state == MonsterState.Die)
            return;


        if (attackCoolTime > 0.0f)
        {
            attackCoolTime -= Time.fixedDeltaTime;
            if (attackCoolTime <= .0f)
            {
                if (unitTarget != null)
                {
                    Vector2 vec = unitTarget.gameObject.transform.position - this.transform.position;
                    traceDistance = vec.sqrMagnitude;

                    if (traceDistance < attackRange * attackRange)
                        SetMonsterState(MonsterState.Attack);
                    else
                        SetMonsterState(MonsterState.Run);
                }
                else if(playerTowerCtrl != null)
                {

                    Vector2 vec = playerTowerCtrl.gameObject.transform.position - this.transform.position;
                    traceDistance = vec.sqrMagnitude;

                    if (traceDistance < attackRange * attackRange)
                        SetMonsterState(MonsterState.Attack);
                    else
                        SetMonsterState(MonsterState.Run);
                }
                else  //Ÿ���� ����Ÿ�ٿ� �ƹ��͵� ������ �ʾҴٸ�
                    SetMonsterState(MonsterState.Run);
                


            }

        }
    }


    public void Notified(int att, float speed)
    {
        this.att = att;
        this.moveSpeed = speed;

    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(pos.position, boxSize);
    }


}
