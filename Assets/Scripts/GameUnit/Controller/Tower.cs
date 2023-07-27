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

    public abstract void TowerDamage(int att);      //�������Դ� �Լ�����
    public abstract float hpPercent();
    protected abstract void TowerDestroy();     //�ı����·� �����ϴ� �Լ�����

}
