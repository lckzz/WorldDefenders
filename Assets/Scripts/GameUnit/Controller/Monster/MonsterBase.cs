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
            //오브젝트 풀에서 생성되면 초기화 시켜줘야함
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
            weaknessDebuff.AddObserver(this);           //디버프의 능력치변화값을 받아오기위한 구독
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
        yield return wfs;           //시간 대기후 텍스트

        Managers.Sound.Play("Effect/Get_Cost");
        //일반 몬스터는 죽으면 코스트를 뱉는다.
        int randItem = Random.Range(0, 2);
        unitHUDHp?.ItemHudInit((int)DropItemType.Cost);

        unitHUDHp?.SpawnHUDText(DropCost.ToString(), (int)UnitDamageType.Item);
        Managers.Game.Cost += DropCost;
        Managers.UI.GetSceneUI<UI_GamePlay>().UpdateCost(Managers.Game.Cost);




    }


    public override bool CriticalCheck()
    {
        //유닛공격력을 받아서 크리티컬확률을 받아서 확률에 맞으면 크리공격
        //아니면 일반 공격
        int rand = UnityEngine.Random.Range(0, 101);
        if (rand <= monStat.criticalRate)
            return true;

        return false;
    }

    public override void CriticalAttack(Unit uniCtrl, string soundPath, string criticalSoundPath, string hitPath)
    {
        if (CriticalCheck())//true면 크리티컬데미지 false면 일반데미지
        {
            int attack = att * 2;
            uniCtrl.OnDamage(attack, monStat.knockBackForce, true);      //크리티컬이면 데미지2배에 넉백까지
            Managers.Resource.ResourceEffectAndSound(unitTarget.transform.position, criticalSoundPath, hitPath);

        }
        else  //노크리티컬이면 일반공격
        {
            uniCtrl.OnDamage(att);        //넉백은 없이
            Managers.Resource.ResourceEffectAndSound(unitTarget.transform.position, soundPath, hitPath);

        }
    }

    public override void CriticalAttack(Tower tower, string soundPath, string criticalSoundPath, string hitPath)
    {
        if (CriticalCheck())//true면 크리티컬데미지 false면 일반데미지
        {
            int attack = att * 2;
            tower.TowerDamage(attack);      //크리티컬이면 데미지2배 타워는 2배만
            Managers.Resource.ResourceEffectAndSound(tower.transform.position, criticalSoundPath, hitPath);

        }
        else  //노크리티컬이면 일반공격
        {
            tower.TowerDamage(att);        //넉백은 없이
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
