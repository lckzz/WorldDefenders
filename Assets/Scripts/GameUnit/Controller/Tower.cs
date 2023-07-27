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


public abstract class Tower : MonoBehaviour
{
    protected float hp;
    protected float maxHp;
    protected int level;

    protected virtual void Init(float _hp , int _level)
    {
        hp = _hp;
        maxHp = _hp;
        level = _level;
    }

    public abstract void TowerDamage(int att);      //데미지입는 함수구현
    public abstract float hpPercent();
    protected abstract void TowerDestroy();     //파괴상태로 돌입하는 함수구현

}
