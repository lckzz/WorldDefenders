using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TowerState
{
    Idle,
    Destroy
}

public enum MonsterPortalState
{
    Idle,
    Destroy
}


public abstract class Tower : MonoBehaviour,IHpSubject
{
    [SerializeField]
    protected float hp;
    protected float maxHp;
    protected int level;

    public Action onDead = null;
    private List<IHpObserver> hpObserverList = new List<IHpObserver>();

    protected float hpPer;
    protected virtual void Init(float _hp , int _level)
    {
        hp = _hp;
        maxHp = _hp;
        level = _level;
    }

    public abstract void TowerDamage(int att);      //데미지입는 함수구현
    protected abstract void TowerDestroy();     //파괴상태로 돌입하는 함수구현

    public void AddHpObserver(IHpObserver observer)
    {
        for(int ii = 0; ii  < hpObserverList.Count; ii++)       //옵저버리스트를 봐서
        {
            if (hpObserverList[ii] == observer)      //리스트안에 이미 옵저버로 넣었다면 리턴
                return;
        }

        hpObserverList.Add(observer);
    }

    public void RemoveHpObserver(IHpObserver observer)
    {
        hpObserverList.Remove(observer);
    }

    public void NotifyToHpObserver()
    {
        hpPer = hp / maxHp;

        foreach(IHpObserver observer in hpObserverList)
        {
            observer.Notified(hpPer);
        }
    }
}
