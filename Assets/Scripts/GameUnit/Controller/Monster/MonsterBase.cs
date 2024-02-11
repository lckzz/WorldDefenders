using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.Burst.CompilerServices;
using UnityEngine;
using static Define;

public abstract class MonsterBase : Unit, IObserver
{
    protected List<Unit> unitCtrls = new List<Unit>();
    [SerializeField] protected Unit unitTarget;
    [SerializeField] protected PlayerTower playerTowerCtrl;
    protected MonsterStat monStat;



    protected DropItem dropItem;

    protected DebuffCreator debuffCreator;
    protected Debuff debuff;

    public int DropCost { get; protected set; }

    public Debuff Debuff { get { return debuff; } }

    public Unit UnitCtrl { get { return unitTarget; } }
    public PlayerTower PlayerTowerCtrl { get { return playerTowerCtrl; } }



    public void Notified(int att, float speed)
    {
        this.att = att;
        this.moveSpeed = speed;
    }

    public override void OnEnable()
    {
        if (sp != null && myColl != null)
        {
            //������Ʈ Ǯ���� �����Ǹ� �ʱ�ȭ ���������
            isDie = false;
            isRun = false;
            hp = maxHp;
            sp.color = new Color32(255, 255, 255, 255);
            myColl.enabled = true;
            unitTarget = null;
            playerTowerCtrl = null;
        }
    }

    public override void Init()
    {
        base.Init();
        monStat = new MonsterStat();

        TryGetComponent(out myColl);
        TryGetComponent(out dropItem);
        TryGetComponent(out debuffCreator);


        debuff = debuffCreator.AddDebuffComponent(Managers.Game.CurPlayerEquipSkill);


        if (Debuff is WeaknessDebuff weaknessDebuff)
            weaknessDebuff.AddObserver(this);           //������� �ɷ�ġ��ȭ���� �޾ƿ������� ����
    }

    public override void EnemySensor()
    {
        UnitSense();
        UnitDistanceAsending();
    }

    public override void AttackDelay() { }

    public override void OnAttack() { }


    public override void OnDamage(int att, int knockBack = 0, bool criticalCheck = false) { }


    WaitForSeconds wfs = new WaitForSeconds(1.5f);
    protected IEnumerator MonsterDieDropText()
    {
        yield return wfs;           //�ð� ����� �ؽ�Ʈ

        Managers.Sound.Play("Effect/Get_Cost");
        //�Ϲ� ���ʹ� ������ �ڽ�Ʈ�� ��´�.
        int randItem = Random.Range(0, 2);
        unitHUDHp?.ItemHudInit((int)DropItemType.Cost);

        unitHUDHp?.SpawnHUDText(DropCost.ToString(), (int)UnitDamageType.Item);
        Managers.Game.Cost += DropCost;
        Managers.UI.GetSceneUI<UI_GamePlay>().UpdateCost(Managers.Game.Cost);




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

    public override void CriticalAttack(Unit uniCtrl, string soundPath, string criticalSoundPath, string hitPath)
    {
        if (CriticalCheck())//true�� ũ��Ƽ�õ����� false�� �Ϲݵ�����
        {
            int attack = att * 2;
            uniCtrl.OnDamage(attack, monStat.knockBackForce, true);      //ũ��Ƽ���̸� ������2�迡 �˹����
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

    protected void TowerSensor()
    {
        towerColl = Physics2D.OverlapBox(pos.position, boxSize, 0, LayerMask.GetMask("Tower"));
        //Debug.Log(towerColl?.name);
        if (towerColl != null)
            towerColl.TryGetComponent(out playerTowerCtrl);
    }


    protected abstract void UnitSense();
    protected abstract void UnitDistanceAsending();
}
