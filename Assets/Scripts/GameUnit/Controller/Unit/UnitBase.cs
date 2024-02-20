using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitBase : Unit
{
    [SerializeField] protected UnitClass unitClass;
    protected UnitStat unitStat;
    protected List<Unit> monCtrls = new List<Unit>();  //�����ȿ� ���� ������ �������� ��Ƶ�
    [SerializeField] protected Unit monTarget;  //���͵��� �������߿��� ���� ���ְ� ����� ���������� �޾ƿ�
    [SerializeField] protected MonsterPortal monsterPortal;
    protected Dictionary<UnitClass, UnitStat> unitStatDict;


    protected readonly string appearTitleKey = "unitAppearDialog";
    protected readonly string dieTitleKey = "unitDieDialog";
    protected readonly string dieDialogSubKey = "specialUnitDie";


    public Unit Monctrl { get { return monTarget; } }
    public MonsterPortal MonsterPortal { get { return monsterPortal; } }
    // Start is called before the first frame update


    public override void OnEnable()
    {

    }


    public override void Init()
    {
        base.Init();
        unitStatDict = new Dictionary<UnitClass, UnitStat>
        {
            { UnitClass.Warrior , Managers.Data.warriorDict[Managers.Game.UnitWarriorLv] },
            { UnitClass.Archer , Managers.Data.archerDict[Managers.Game.UnitArcherLv] },
            { UnitClass.Spear , Managers.Data.spearDict[Managers.Game.UnitSpearLv] },
            { UnitClass.Priest , Managers.Data.spearDict[Managers.Game.UnitPriestLv] },
            { UnitClass.Magician , Managers.Data.magicDict[Managers.Game.UnitMagicianLv] },
            { UnitClass.Cavalry , Managers.Data.cavarlyDict[Managers.Game.UnitCavalryLv] },

        };


        unitStat = new UnitStat();
        unitStat = unitStatDict[unitClass];

        hp = unitStat.hp;
        att = unitStat.att;
        knockbackForce = unitStat.knockBackForce;
        attackRange = unitStat.attackRange;

        moveSpeed = 2.5f;
        maxHp = hp;


    }

    protected virtual void UnitSense()
    {
        //Ÿ�ٸ���Ʈ �ʱ�ȭ����
        monCtrls.Clear();
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
            else
            {
                monsterPortal = null;
            }


            //üũ�ڽ��ȿ� ���� �ݶ��̴��߿��� ���� ���ְ��� �Ÿ��� ���� ����� ���� ��󳻱�
            for (int ii = 0; ii < enemyColls2D.Length; ii++)
            {
                if (enemyColls2D[ii].gameObject.layer == LayerMask.NameToLayer("Monster"))
                {
                    MonsterController monctrl;
                    enemyColls2D[ii].TryGetComponent<MonsterController>(out monctrl);
                    monCtrls.Add(monctrl);

                }
                else if (enemyColls2D[ii].gameObject.layer == LayerMask.NameToLayer("EliteMonster"))
                {
                    EliteMonsterController elite;
                    enemyColls2D[ii].TryGetComponent<EliteMonsterController>(out elite);
                    monCtrls.Add(elite);

                }
            }


        }
    }

    //������ ���ֵ��� �ش� ĳ���Ϳ��� �Ÿ� ��������
    protected virtual void UnitDistanceAsending()
    {
        if (monCtrls.Count > 0)
        {
            float disMin = 0;
            int min = 0;


            if (monCtrls.Count > 1)
            {
                for (int i = 0; i < monCtrls.Count; i++)
                {
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


            if (monCtrls.Count != 0)
            {
                monTarget = monCtrls[min];
            }

        }
    }


    void TowerSensor()
    {
        towerColl = Physics2D.OverlapBox(pos.position, boxSize, 0, LayerMask.GetMask("MonsterPortal"));
        if (towerColl != null)
            towerColl.TryGetComponent(out monsterPortal);
    }

    void UnitIdle()
    {
        AttackDelay();

    }

    protected virtual bool IsTargetOn()
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


    public override void OnHeal(int heal)
    {
        if (hp > 0)
        {
            unitHUDHp?.SpawnHUDText(heal.ToString(), (int)Define.UnitDamageType.Team);
            hp += heal;
            NotifyToHpObserver();       //ü���� �ٲ� �������鿡�� ü���� �ٲ��ٴ°� �˸��� ������

        }

        if (hp >= maxHp)
            hp = maxHp;
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

    public override void CriticalAttack(Unit monCtrl, string soundPath, string criticalSoundPath, string hitPath)
    {
        if (CriticalCheck())//true�� ũ��Ƽ�õ����� false�� �Ϲݵ�����
        {
            int attack = att * 2;
            monCtrl.OnDamage(attack, unitStat.knockBackForce, true);      //ũ��Ƽ���̸� ������2�迡 �˹����
            Managers.Resource.ResourceEffectAndSound(monTarget.transform.position, criticalSoundPath, hitPath);

        }
        else  //��ũ��Ƽ���̸� �Ϲݰ���
        {

            monCtrl.OnDamage(att);        //�˹��� ����
            Managers.Resource.ResourceEffectAndSound(monTarget.transform.position, soundPath, hitPath);

        }
    }

    public override void CriticalAttack(Tower monPortal, string soundPath, string criticalSoundPath, string hitPath)
    {
        if (CriticalCheck())//true�� ũ��Ƽ�õ����� false�� �Ϲݵ�����
        {
            int attack = att * 2;
            monPortal.TowerDamage(attack);      //ũ��Ƽ���̸� ������2�� Ÿ���� 2�踸
            Managers.Resource.ResourceEffectAndSound(monPortal.transform.position, criticalSoundPath, hitPath);

        }
        else  //��ũ��Ƽ���̸� �Ϲݰ���
        {
            monPortal.TowerDamage(att);        //�˹��� ����
            Managers.Resource.ResourceEffectAndSound(monPortal.transform.position, soundPath, hitPath);

        }
    }


    public void SpeechBubbleOn(string speechTitleKey, string speechSubKey, int probaility)
    {
        speechBubble.SpeechBubbleOn(speechTitleKey, speechSubKey, probaility);


    }

    public override void EnemySensor()
    {
    }

    public override void AttackDelay()
    {

    }

    public override void OnDamage(int att, int knockBack = 0, bool criticalCheck = false)
    {

    }
}
